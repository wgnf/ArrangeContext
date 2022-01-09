using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArrangeContext.Core.Helper;

namespace ArrangeContext.Core
{
    /// <summary>
    ///     The base-class for everything that is ArrangeContext
    /// </summary>
    /// <typeparam name="TContext">The Context that is worked with</typeparam>
    public abstract class ArrangeContextBase<TContext> where TContext : class
    {
        private readonly IList<ContextParameter> _contextParameters;

        private readonly bool _includeOptionalParameters;

        /// <summary>
        ///     Creates a new instance of the Arrange-Context
        /// </summary>
        /// <param name="includeOptionalParameters">If optional parameters should be considered or not</param>
        protected ArrangeContextBase(bool includeOptionalParameters)
        {
            _includeOptionalParameters = includeOptionalParameters;

            _contextParameters = new List<ContextParameter>();
            var reflectionHelper = new ReflectionHelper();

            var constructor = reflectionHelper.GetConstructor<TContext>();
            var constructorParameters = reflectionHelper.GetParametersFor(constructor);

            /*
             * NOTE:
             * We're initializing the context-parameters here so that we have a "blueprint" of all parameters
             * that we need in the correct order (needed for creating the instance of 'TContext' in 'Build()'),
             * leaving the Properties of 'ContextInstance' null, so we can figure out which missing parameters have
             * to be mocked
             */
            foreach (var constructorParameter in constructorParameters)
                _contextParameters.Add(new ContextParameter(
                    constructorParameter,
                    new ContextInstance(null, null)));
        }

        /// <summary>
        ///     Builds the Context with substituted ctor-Parameters
        /// </summary>
        /// <returns>A new instance of <typeparamref name="TContext" /> with substituted ctor-Parameters</returns>
        public TContext Build()
        {
            InitializeMissingParameters();

            var parameterArray = _contextParameters
                .Select(p => p.ContextInstance.Instance)
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
            var parameter = _contextParameters.FirstOrDefault(p =>
                p.Name.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase));
            if (parameter == null)
                throw new ArgumentException(
                    $"The specified parameter with name '{parameterName}' is not a constructor parameter of '{typeof(TContext).Name}'");
            return parameter;
        }

        /// <summary>
        ///     Replaces the <paramref name="parameterToReplace" /> with another provided instance
        /// </summary>
        /// <param name="parameterToReplace">The parameter to replace</param>
        /// <param name="instance">The instance to replace the current parameter with</param>
        /// <param name="mockedInstance">The mocked instance of the provided instance (i.e. for Mock&lt;Something&gt;)</param>
        protected void ReplaceInstance(
            ContextParameter parameterToReplace,
            object instance,
            object mockedInstance)
        {
            parameterToReplace.ContextInstance.Instance = instance;
            parameterToReplace.ContextInstance.MockedInstance = mockedInstance;
        }

        /// <summary>
        ///     The place to create the mocked instance with the used provider
        /// </summary>
        /// <param name="parameter">The parameter that should be mocked</param>
        /// <returns>The mocked instance</returns>
        protected abstract ContextInstance CreateMockedInstance(ParameterInfo parameter);
        
        /// <summary>
        ///     Initializes the provided <see cref="ContextParameter"/> with a mock
        /// </summary>
        /// <param name="contextParameter">The <see cref="ContextParameter"/> to initialize</param>
        protected void InitializeContextParameter(ContextParameter contextParameter)
        {
            var instance = CreateContextInstance(contextParameter.ParameterInfo);
            contextParameter.ContextInstance.Instance = instance.Instance;
            contextParameter.ContextInstance.MockedInstance = instance.MockedInstance;
        }

        private void InitializeMissingParameters()
        {
            foreach (var parameter in _contextParameters)
            {
                // we only want to initialize not initialized parameters here...
                if (parameter.ContextInstance?.Instance != null)
                    continue;

                InitializeContextParameter(parameter);
            }
        }

        private ContextInstance CreateContextInstance(ParameterInfo parameter)
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

        private static ContextInstance CreateInstanceFromActivator(Type type)
        {
            var instance = Activator.CreateInstance(type);
            return new ContextInstance(instance, null);
        }
    }
}