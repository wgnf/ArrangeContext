using ArrangeContext.Core.Helper;
using ArrangeContext.Core.Helper.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArrangeContext.Core
{
    /// <summary>
    ///     The base-class for everything that is ArrangeContext
    /// </summary>
    /// <typeparam name="TContext">The Context that is worked with</typeparam>
    public abstract class ArrangeContextBase<TContext> where TContext : class
    {
        private readonly IReflectionHelper _reflectionHelper;

        private readonly IList<ContextParameter> _contextParameters;

        private readonly bool _includeOptionalParameters;
        private bool _initialized = false;

        /// <summary>
        ///     Creates a new instance of the Arrange-Context
        /// </summary>
        /// <param name="includeOptionalParameters">If optional parameters should be considered or not</param>
        protected ArrangeContextBase(bool includeOptionalParameters)
        {
            _includeOptionalParameters = includeOptionalParameters;

            _contextParameters = new List<ContextParameter>();
            _reflectionHelper = new ReflectionHelper();
        }

        /// <summary>
        ///     Builds the Context with substituted ctor-Parameters
        /// </summary>
        /// <returns>A new instance of <typeparamref name="TContext"/> with substituted ctor-Parameters</returns>
        public TContext Build()
        {
            ConsiderInitialization();

            var parameterArray = _contextParameters
                .Select(p => p.Instance.Instance)
                .ToArray();
            var instance = Activator.CreateInstance(typeof(TContext), parameterArray);
            return (TContext)instance;
        }

        /// <summary>
        ///     Gets a <see cref="ContextParameter"/> of the given Type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of the parameter to find</typeparam>
        /// <exception cref="ArgumentException">When the parameter for the type <typeparamref name="T"/> could not be found</exception>
        /// <returns>The found parameter</returns>
        protected ContextParameter GetParameter<T>()
        {
            ConsiderInitialization();

            var parameter = _contextParameters.FirstOrDefault(p => p.Type == typeof(T));
            if (parameter == null)
                throw new ArgumentException(
                    $"The specified Type '{typeof(T).Name}' is not a constructor parameter of '{typeof(TContext).Name}'");
            return parameter;
        }

        /// <summary>
        ///     Gets a <see cref="ContextParameter"/> with the given <paramref name="parameterName"/>
        /// </summary>
        /// <exception cref="ArgumentException">When the parameter for the <paramref name="parameterName"/> could not be found</exception>
        /// <returns>The found parameter</returns>
        protected ContextParameter GetParameter(string parameterName)
        {
            ConsiderInitialization();

            var parameter = _contextParameters.FirstOrDefault(p =>
                p.Name.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase));
            if (parameter == null)
                throw new ArgumentException(
                    $"The specified parameter with name '{parameterName}' is not a constructor parameter of '{typeof(TContext).Name}'");
            return parameter;
        }

        /// <summary>
        ///     Replaces the <paramref name="parameterToReplace"/> with another provided instance
        /// </summary>
        /// <param name="parameterToReplace">The parameter to replace</param>
        /// <param name="instance">The instance to replace the current parameter with</param>
        /// <param name="mockedInstance">The mocked instance of the provided instance (i.e. for Mock&lt;Something&gt;)</param>
        protected void ReplaceInstance(
            ContextParameter parameterToReplace,
            object instance,
            object mockedInstance)
        {
            ConsiderInitialization();

            var index = _contextParameters.IndexOf(parameterToReplace);
            _contextParameters.Remove(parameterToReplace);

            var newParameter = new ContextParameter(
                parameterToReplace.Type,
                parameterToReplace.Name,
                new ContextInstance(instance, mockedInstance));
            _contextParameters.Insert(index, newParameter);
        }

        /// <summary>
        ///     The place to create the mocked instance with the used provider
        /// </summary>
        /// <param name="parameter">The parameter that should be mocked</param>
        /// <returns>The mocked instance</returns>
        protected abstract ContextInstance CreateMockedInstance(ParameterInfo parameter);

        private void ConsiderInitialization()
        {
            if (_initialized) return;

            try
            {
                var parameters = GetParameters();
                InitializeContextParameters(parameters);
            }
            finally
            {
                _initialized = true;
            }
        }

        private IEnumerable<ParameterInfo> GetParameters()
        {
            var constructor = _reflectionHelper.GetConstructor<TContext>();
            var parameters = _reflectionHelper.GetParametersFor(constructor);

            return parameters;
        }

        private void InitializeContextParameters(IEnumerable<ParameterInfo> parameters)
        {
            foreach (var parameter in parameters)
            {
                var contextParameter = InitializeContextParameterFor(parameter);
                _contextParameters.Add(contextParameter);
            }
        }

        private ContextParameter InitializeContextParameterFor(ParameterInfo parameter)
        {
            var instance = CreateInstance(parameter);
            var contextParameter = new ContextParameter(
                parameter.ParameterType,
                parameter.Name,
                instance);

            return contextParameter;
        }

        private ContextInstance CreateInstance(ParameterInfo parameter)
        {
            try
            {
                if (parameter.IsOptional &&
                    parameter.DefaultValue == null &&
                    !_includeOptionalParameters)
                    return new ContextInstance(null, null);

                var parameterType = parameter.ParameterType;

                if (parameterType.IsPrimitive ||
                    parameterType.IsValueType)
                    return CreateInstanceFromActivator(parameterType);

                // because strings are _extra_
                if (parameterType == typeof(string))
                    return new ContextInstance(default(string), null);

                var instance = CreateMockedInstance(parameter);
                return instance;
            }
            catch (Exception ex)
            {
                throw new InstanceCreationFailedException(parameter.Name, ex);
            }
        }

        private ContextInstance CreateInstanceFromActivator(Type type)
        {
            var instance = Activator.CreateInstance(type);
            return new ContextInstance(instance, null);
        }
    }
}
