using ArrangeContext.Core;
using ArrangeContext.Core.Helper;
using Rhino.Mocks;
using System;
using System.Reflection;

namespace ArrangeContext.RhinoMocks
{
    /// <summary>
    ///     The Arrange Context for <typeparamref name="TContext"/>
    /// </summary>
    /// <typeparam name="TContext">The Context to work on</typeparam>
    public class ArrangeContext<TContext> : ArrangeContextBaseWithBaseMethods<TContext> where TContext : class
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

        /// <inheritdoc/>
        protected override ContextInstance CreateMockedInstance(ParameterInfo parameter)
        {
            try
            {
                var mockedInstance = MockRepository.GenerateMock(parameter.ParameterType, Array.Empty<Type>());
                return new ContextInstance(mockedInstance, mockedInstance);
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
