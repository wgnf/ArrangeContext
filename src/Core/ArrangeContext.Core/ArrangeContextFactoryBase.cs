using ArrangeContext.Core.Helper;
using System;
using System.Linq;
using System.Reflection;

namespace ArrangeContext.Core
{
    /// <summary>
    ///     Base-Class for an Arrange-Factory
    /// </summary>
    public abstract class ArrangeContextFactoryBase
    {
        /// <summary>
        ///     Creates a new instance of <see cref="ArrangeContextFactoryBase"/> accepting the <see cref="ArrangeContext"/> to use
        /// </summary>
        /// <param name="context">The <see cref="ArrangeContext"/> to use</param>
        public ArrangeContextFactoryBase(
            ArrangeContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        ///     The underlying <see cref="ArrangeContext"/> for this Factory
        /// </summary>
        public ArrangeContext Context { get; }

        /// <summary>
        ///     Uses the underlying information of <see cref="ArrangeContext.ContextParameters"/> to create the instance of the system-under-test
        /// </summary>
        /// <returns>The instance of the system-under-test</returns>
        public object Build()
        {
            ConsiderInitialization();

            var instance = Activator
                .CreateInstance(Context.ContextType, Context
                    .ContextParameters
                    .Select(p => p.Instance.Instance)
                    .ToArray());

            return instance;
        }

        /// <summary>
        ///     Gets a parameter indicated by <typeparamref name="T"/> from the underlying <see cref="Context"/>
        /// </summary>
        /// <typeparam name="T">Type that indicates the parameter to get</typeparam>
        /// <returns>The <see cref="ContextParameter"/> that was found using <typeparamref name="T"/></returns>
        public ContextParameter GetParameter<T>()
        {
            ConsiderInitialization();

            var parameter = Context.ContextParameters.FirstOrDefault(p => p.Type == typeof(T));
            if (parameter == null)
                throw new ArgumentException(
                    $"The specified Type '{typeof(T).Name}' is not a constructor parameter of '{Context.ContextType.Name}'");
            return parameter;
        }

        /// <summary>
        ///     Gets a parameter by name
        /// </summary>
        /// <param name="parameterName">The name for the parameter to find</param>
        /// <returns>The <see cref="ContextParameter"/> that was found using the <paramref name="parameterName"/></returns>
        public ContextParameter GetParameter(string parameterName)
        {
            ConsiderInitialization();

            var parameter = Context.ContextParameters.FirstOrDefault(p =>
                p.Name.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase));
            if (parameter == null)
                throw new ArgumentException(
                    $"The specified parameter with name '{parameterName}' is not a constructor parameter of '{Context.ContextType.Name}'");
            return parameter;
        }

        /// <summary>
        ///     Replaces an already existing instance with another instance
        /// </summary>
        /// <param name="parameterToReplace"><see cref="ContextParameter"/> that should be replaced</param>
        /// <param name="instance">The new instance for the <see cref="ContextParameter"/></param>
        /// <param name="mockedInstance">The new mocked instance for the <see cref="ContextParameter"/></param>
        public void ReplaceInstance(
            ContextParameter parameterToReplace,
            object instance,
            object mockedInstance)
        {
            ConsiderInitialization();

            var index = Context.ContextParameters.IndexOf(parameterToReplace);
            Context.ContextParameters.Remove(parameterToReplace);

            var newParameter = new ContextParameter(
                parameterToReplace.Type,
                parameterToReplace.Name,
                new ContextInstance(instance, mockedInstance));
            Context.ContextParameters.Insert(index, newParameter);
        }

        /// <summary>
        ///     Abstract Method that gets the mocked instance of a <see cref="ParameterInfo"/> from different Mock-Frameworks
        /// </summary>
        /// <param name="parameter">The <see cref="ParameterInfo"/> to get the mocked instance for</param>
        /// <returns>A <see cref="ContextInstance"/> containing an instance and a mocked instance</returns>
        protected abstract ContextInstance GetMockedInstanceFromProvider(ParameterInfo parameter);

        private void ConsiderInitialization()
        {
            if (Context.Initialized) return;

            Context.Initialize();

            foreach (var parameter in Context.Parameters)
            {
                var instance = CreateMockedInstance(parameter, Context.MockOptionalParameters);
                var contextParameter = new ContextParameter(
                    parameter.ParameterType,
                    parameter.Name,
                    instance);
                Context.ContextParameters.Add(contextParameter);
            }
        }

        private ContextInstance CreateMockedInstance(
            ParameterInfo parameter,
            bool mockOptionalParameters)
        {
            try
            {
                // if the DefaultValue of an optional parameter is null, and we don't want to substitute it, we keep null 
                if (parameter.IsOptional && parameter.DefaultValue == null && !mockOptionalParameters)
                    return new ContextInstance(null, null);

                // if the parameter is a primitive type, we can ask the Activator to do it's job for us
                if (parameter.ParameterType.IsPrimitive)
                {
                    var instance = Activator.CreateInstance(parameter.ParameterType);
                    return new ContextInstance(instance, null);
                }

                // just some special treatment for strings - because they're that extra
                if (parameter.ParameterType == typeof(string))
                {
                    var instance = default(string);
                    return new ContextInstance(instance, null);
                }

                return GetMockedInstanceFromProvider(parameter);
            }
            catch (Exception ex)
            {
                throw new MockingFailedException(parameter.Name, ex);
            }
        }
    }
}
