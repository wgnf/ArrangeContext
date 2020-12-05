using ArrangeContext.Core.Helper;
using System;
using System.Linq;
using System.Reflection;

namespace ArrangeContext.Core
{
    public abstract class ArrangeContextFactoryBase
    {
        public ArrangeContextFactoryBase(
            ArrangeContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ArrangeContext Context { get; }

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

        public ContextParameter GetParameter<T>()
        {
            ConsiderInitialization();

            var parameter = Context.ContextParameters.FirstOrDefault(p => p.Type == typeof(T));
            if (parameter == null)
                throw new ArgumentException(
                    $"The specified Type '{typeof(T).Name}' is not a constructor parameter of '{Context.ContextType.Name}'");
            return parameter;
        }

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

                return GetMockedInstanceFromProvider(parameter, mockOptionalParameters);
            }
            catch (Exception)
            {
                // TODO: Or should we better throw here?
                return new ContextInstance(null, null);
            }
        }

        protected abstract ContextInstance GetMockedInstanceFromProvider(
            ParameterInfo parameter,
            bool mockOptionalParameters);
    }
}
