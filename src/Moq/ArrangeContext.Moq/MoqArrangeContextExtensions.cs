using ArrangeContext.Core.Helper;
using Moq;
using System;
using System.Linq;
using System.Reflection;

namespace ArrangeContext.Moq
{
    public static class MoqArrangeContextExtensions
    {
        public static void Use<T>(
            this Core.ArrangeContext context, 
            Mock<T> mockedInstance)
            where T : class
        {
            ConsiderInitialization(context);

            var parameter = GetParameter<T>(context);
            ReplaceInstance(parameter, mockedInstance, context);
        }

        public static void Use<T, TContext>(
            this Core.ArrangeContext<TContext> context,
            Mock<T> mockedInstance,
            string parameterName)
            where T : class
            where TContext : class
        {
            ConsiderInitialization(context);

            var parameter = GetParameter(parameterName, context);
            ReplaceInstance(parameter, mockedInstance, context);
        }

        // TODO:
        // Check if MockedInstance != null
        // else throw Exception, that this Type has not been mocked
        public static Mock<T> For<T>(
            this Core.ArrangeContext context)
            where T : class
        {
            ConsiderInitialization(context);

            var parameter = GetParameter<T>(context);
            return (Mock<T>)parameter.Instance.MockedInstance;
        }

        public static Mock<T> For<T, TContext>(
            this Core.ArrangeContext<TContext> context, 
            string parameterName)
            where T : class
            where TContext : class
        {
            ConsiderInitialization(context);

            var parameter = GetParameter(parameterName, context);
            return (Mock<T>)parameter.Instance.MockedInstance;
        }

        public static TContext Build<TContext>(
            this Core.ArrangeContext<TContext> context) where TContext : class
        {
            ConsiderInitialization(context);
            
            var instance = Activator.CreateInstance(typeof(TContext), context.ContextParameters.Select(p => p.Instance.Instance).ToArray());
            return (TContext)instance;
        }

        private static void ConsiderInitialization(
            Core.ArrangeContext context)
        {
            if (!context.Initialized)
            {
                context.Initialize();
                
                foreach (var parameter in context.Parameters)
                {
                    var instance = CreateMockedInstance(parameter, context.MockOptionalParameters);
                    var contextParameter = new ContextParameter(
                        parameter.ParameterType,
                        parameter.Name,
                        instance);
                    context.ContextParameters.Add(contextParameter);
                }
            }
        }

        private static ContextInstance CreateMockedInstance(ParameterInfo parameter, bool mockOptionalParameters)
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

                // in other cases we use Moq to create a Mocked instance
                // NOTE: We have to use some workaround with reflection here
                // because we're creating a Mock<T> at runtime and
                // the generic Type is not known at compile-time

                var classType = typeof(Mock<>);
                var typeParams = new Type[] { parameter.ParameterType };
                var constructedType = classType.MakeGenericType(typeParams);
                var mockedInstance = (Mock)Activator.CreateInstance(constructedType);
                return new ContextInstance(mockedInstance.Object, mockedInstance);
            }
            // TODO: Information about inaccessible things
            // with fix: internals visible to DynamicProxyGenAssembly2
            catch (Exception)
            {
                return new ContextInstance(null, null);
            }
        }

        private static ContextParameter GetParameter<T>(
            Core.ArrangeContext context)
            where T : class
        {
            var parameter = context.ContextParameters.FirstOrDefault(p => p.Type == typeof(T));
            if (parameter == null)
                throw new ArgumentException(
                    $"The specified Type '{typeof(T).Name}' is not a constructor parameter of '{context.ContextType.Name}'");
            return parameter;
        }

        private static ContextParameter GetParameter(
            string parameterName, 
            Core.ArrangeContext context)
        { 
            var parameter = context.ContextParameters.FirstOrDefault(p =>
                p.Name.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase));
            if (parameter == null)
                throw new ArgumentException(
                    $"The specified parameter with name '{parameterName}' is not a constructor parameter of '{context.ContextType.Name}'");
            return parameter;
        }

        private static void ReplaceInstance<T>(
            ContextParameter parameterToReplace, 
            Mock<T> mockedInstance,
            Core.ArrangeContext context)
            where T : class
        {
            var index = context.ContextParameters.IndexOf(parameterToReplace);
            context.ContextParameters.Remove(parameterToReplace);

            var newParameter = new ContextParameter(
                parameterToReplace.Type, 
                parameterToReplace.Name, 
                new ContextInstance(mockedInstance.Object, mockedInstance));
            context.ContextParameters.Insert(index, newParameter);
        }
    }
}
