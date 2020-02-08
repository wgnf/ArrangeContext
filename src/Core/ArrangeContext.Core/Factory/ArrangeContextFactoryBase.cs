using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using ArrangeContext.Core.Factory.Contracts;
using ArrangeContext.Core.Helper;
using ArrangeContext.Core.Helper.Contracts;
using ArgumentException = System.ArgumentException;

namespace ArrangeContext.Core.Factory
{
    public abstract class ArrangeContextFactoryBase : IArrangeContextFactory
    {
        private static IArrangeContextFactory _instance;
        private readonly IReflectionHelper _reflectionHelper;

        protected ArrangeContextFactoryBase(IReflectionHelper reflectionHelper = null)
        {
            _reflectionHelper = reflectionHelper ?? new ReflectionHelper();
        }

        public IArrangeContext<T> CreateArrangeContext<T>(bool mockOptionalParameters) where T : class
        {
            var constructor = _reflectionHelper.GetConstructor<T>();
            var parameters = _reflectionHelper.GetParametersFor(constructor);
            var contextParameters = ConvertToContextParameters(parameters, mockOptionalParameters).ToList();

            return new ArrangeContextImplementation<T>(contextParameters);
        }

        protected virtual bool CanMockParameter(ParameterInfo parameter)
        {
            return false;
        }

        protected virtual object MockParameter(ParameterInfo parameter)
        {
            return null;
        }

        public static Func<IArrangeContextFactory> FactoryInstanceCreator { get; set; } = null;

        internal static IArrangeContextFactory GetFactory()
        {
            if (_instance != null)
                return _instance;
            if (FactoryInstanceCreator == null)
                throw new ArgumentException($"No '{nameof(FactoryInstanceCreator)}' was specified!");

            _instance = FactoryInstanceCreator();
            return _instance;
        }

        private IEnumerable<ContextParameter> ConvertToContextParameters(
            IEnumerable<ParameterInfo> parameters, bool mockOptionalParameters)
        {
            return parameters.Select(p =>
                new ContextParameter(
                    p.ParameterType,
                    p.Name,
                    CreateMockedInstance(p, mockOptionalParameters)));
        }

        private object CreateMockedInstance(ParameterInfo parameter, bool mockOptionalParameters)
        {
            try
            {
                // if the DefaultValue of an optional parameter is null, and we don't want to substitute it, we keep null 
                if (parameter.IsOptional && parameter.DefaultValue == null && !mockOptionalParameters) return null;

                // if the parameter is a ValueType, we can ask the Activator to do it's job for us
                if (parameter.ParameterType.IsValueType)
                    return Activator.CreateInstance(parameter.ParameterType);

                // in other cases ask the "registered" factory, if it's able to create a mock for us -- or we use the fallback
                return CanMockParameter(parameter)
                    ? MockParameter(parameter)
                    : CreateMockedInstanceFallback(parameter, mockOptionalParameters);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // no clue how to test this yet..
        [ExcludeFromCodeCoverage]
        private object CreateMockedInstanceFallback(ParameterInfo parameter, bool mockOptionalParameters)
        {
            try
            {
                var constructor = _reflectionHelper.GetConstructor(parameter.ParameterType);
                var parameters = _reflectionHelper.GetParametersFor(constructor);

                return Activator.CreateInstance(parameter.ParameterType,
                    parameters.Select(p => CreateMockedInstance(p, mockOptionalParameters)));
            }
            catch (Exception)
            {
                return Activator.CreateInstance(parameter.ParameterType);
            }
        }
    }
}