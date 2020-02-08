using System;
using System.Collections.Generic;
using System.Linq;
using ArrangeContext.Core.Helper;

namespace ArrangeContext.Core
{
    internal sealed class ArrangeContextImplementation<TContext> : IArrangeContext<TContext> where TContext : class
    {
        private readonly IList<ContextParameter> _contextParameters;

        public ArrangeContextImplementation(IList<ContextParameter> contextParameters)
        {
            _contextParameters = contextParameters ?? throw new ArgumentNullException(nameof(contextParameters));
        }

        public TContext Build()
        {
            return (TContext) Activator.CreateInstance(typeof(TContext), _contextParameters.Select(p => p.Instance));
        }

        public T For<T>()
        {
            return (T) GetParameter<T>().Instance;
        }

        public T For<T>(string parameterName)
        {
            return (T)GetParameter(parameterName).Instance;
        }

        public T Use<T>(T instance)
        {
            var parameter = GetParameter<T>();
            ReplaceInstance(parameter, instance);
            return instance;
        }

        public T Use<T>(T instance, string parameterName)
        {
            var parameter = GetParameter(parameterName);
            ReplaceInstance(parameter, instance);
            return instance;
        }

        #region Parameter-Helper

        private ContextParameter GetParameter<T>()
        {
            var parameter = _contextParameters.FirstOrDefault(p => p.Type == typeof(T));
            if (parameter == null)
                throw new ArgumentException(
                    $"The specified Type '{typeof(T).Name}' is not a constructor parameter of '{typeof(TContext).Name}'");
            return parameter;
        }

        private ContextParameter GetParameter(string parameterName)
        {
            var parameter = _contextParameters.FirstOrDefault(p =>
                string.Compare(p.Name, parameterName, StringComparison.OrdinalIgnoreCase) == 0);
            if (parameter == null)
                throw new ArgumentException(
                    $"The specified parameter with name '{parameterName}' is not a constructor parameter of '{typeof(TContext).Name}'");
            return parameter;
        }

        private void ReplaceInstance<T>(ContextParameter parameter, T instance)
        {
            var index = _contextParameters.IndexOf(parameter);
            _contextParameters.Remove(parameter);
            var newParameter = new ContextParameter(parameter.Type, parameter.Name, instance);
            _contextParameters.Insert(index, newParameter);
        }

        #endregion
    }
}