using ArrangeContext.Core;
using ArrangeContext.Core.Helper;
using System;
using System.Reflection;
using Moq;

namespace ArrangeContext.Moq
{
    /// <summary>
    ///     The Arrange Context for <typeparamref name="TContext"/>
    /// </summary>
    /// <typeparam name="TContext">The Context to work on</typeparam>
    public class ArrangeContext<TContext> : ArrangeContextBase<TContext> where TContext : class
    {
        /// <summary>
        ///     Creates a new instance of <see cref="ArrangeContext{TContext}"/>
        /// </summary>
        /// <param name="includeOptionalParameters">If optional parameters should be considered or not</param>
        /// <returns>A new instance of <see cref="ArrangeContext{TContext}"/></returns>
        public static ArrangeContext<TContext> Create(bool includeOptionalParameters = true)
        {
            return new ArrangeContext<TContext>(includeOptionalParameters);
        }

        /// <inheritdoc/>
        public ArrangeContext(bool includeOptionalParameters = true)
            : base(includeOptionalParameters)
        {
        }

        /// <summary>
        ///     Replaces the previously generated parameter indicated by <typeparamref name="T"/> with the provided <paramref name="mockedInstance"/>
        /// </summary>
        /// <typeparam name="T">Indicates the parameter that should be replaced</typeparam>
        /// <param name="mockedInstance">The new instance for the parameter with the type <typeparamref name="T"/></param>
        public void Use<T>(Mock<T> mockedInstance) where T : class
        {
            var parameter = GetParameter<T>();
            ReplaceInstance(parameter, mockedInstance.Object, mockedInstance);
        }
        
        /// <summary>
        ///     Replaces the previously generated parameter indicated by <typeparamref name="T"/> with the provided <paramref name="mockedInstance"/>
        /// </summary>
        /// <typeparam name="T">Indicates the type of the parameter that should be replaced</typeparam>
        /// <param name="mockedInstance">The new instance for the parameter with the type <typeparamref name="T"/></param>
        public void Use<T>(T mockedInstance) where T : class
        {
            var parameter = GetParameter<T>();
            // hack: this is dirty, but it's fine 'for now'
            ReplaceInstance(parameter, mockedInstance, null);
        }

        /// <summary>
        ///     Replaces the previously generated parameter indicated by the <paramref name="parameterName"/> with the provided <paramref name="mockedInstance"/>
        /// </summary>
        /// <typeparam name="T">Indicates the type of the parameter that should be replaced</typeparam>
        /// <param name="parameterName">The name of the parameter that should be replaced</param>
        /// <param name="mockedInstance">The new instance for the parameter with the name <paramref name="parameterName"/></param>
        public void Use<T>(Mock<T> mockedInstance, string parameterName) where T : class
        {
            var parameter = GetParameter(parameterName);
            ReplaceInstance(parameter, mockedInstance.Object, mockedInstance);
        }

        /// <summary>
        ///     Replaces the previously generated parameter indicated by the <paramref name="parameterName"/> with the provided <paramref name="mockedInstance"/>
        /// </summary>
        /// <typeparam name="T">Indicates the type of the parameter that should be replaced</typeparam>
        /// <param name="mockedInstance">The new instance for the parameter with the name <paramref name="parameterName"/></param>
        /// <param name="parameterName">The name of the parameter that should be replaced</param>
        public void Use<T>(T mockedInstance, string parameterName) where T : class
        {
            var parameter = GetParameter(parameterName);
            // hack: this is dirty, but it's fine 'for now'
            ReplaceInstance(parameter, mockedInstance, null);
        }

        /// <summary>
        ///     Returns the mocked instance for the parameter indicated by <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Indicates the type of the parameter that should be returned</typeparam>
        /// <returns>Returns the mocked instance for the parameter indicated by <typeparamref name="T"/></returns>
        public Mock<T> For<T>() where T : class
        {
            var parameter = GetParameter<T>();
            if (!parameter.ContextInstance.IsInitialized())
                InitializeContextParameter(parameter);
            
            var determinedMockedInstance = parameter.ContextInstance.MockedInstance;
            return (Mock<T>)determinedMockedInstance;
        }

        /// <summary>
        ///     Returns the mocked instance for the parameter indicated by the <paramref name="parameterName"/>
        /// </summary>
        /// <typeparam name="T">Indicates the type of the parameter that should be returned</typeparam>
        /// <param name="parameterName">Indicates the name of the parameter that should be returned</param>
        /// <returns>Returns the mocked instance for the parameter indicated by <typeparamref name="T"/> and <paramref name="parameterName"/></returns>
        public Mock<T> For<T>(string parameterName) where T : class
        {
            var parameter = GetParameter(parameterName);
            if (!parameter.ContextInstance.IsInitialized())
                InitializeContextParameter(parameter);
            
            var determinedMockedInstance = parameter.ContextInstance.MockedInstance;
            return (Mock<T>)determinedMockedInstance;
        }

        /// <inheritdoc/>
        protected override ContextInstance CreateMockedInstance(ParameterInfo parameter)
        {
            try
            {
                // NOTE: We have to use some workaround with reflection here
                // because we're creating a Mock<T> at runtime and
                // the generic Type is not known at compile-time

                var classType = typeof(Mock<>);
                var typeParams = new[] { parameter.ParameterType };
                var constructedType = classType.MakeGenericType(typeParams);

                var mockedInstance = (Mock)Activator.CreateInstance(constructedType);
                return new ContextInstance(mockedInstance.Object, mockedInstance);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"There was an issue creating a mock for {parameter.ParameterType.Name}." +
                    "This is possibly due to the Type not being accessible. Consider making it internal/public and/or using [assembly:InternalsVisibleTo(\"DynamicProxyGenAssembly2\")]!" +
                    "For more information refer to the InnerException of this Exception!", ex);
            }
        }
    }
}
