using ArrangeContext.Core.Helper;
using ArrangeContext.Core.Helper.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ArrangeContext.Core
{
    public abstract class ArrangeContext
    {
        public Type ContextType { get; set; }

        public bool MockOptionalParameters { get; set; }

        public bool Initialized { get; set; }

        public ConstructorInfo Constructor { get; set; }

        public IEnumerable<ParameterInfo> Parameters { get; set; }

        public IList<ContextParameter> ContextParameters { get; set; }

        public virtual void Initialize()
        {
            Initialized = true;
        }
    }

    public class ArrangeContext<TContext> : ArrangeContext where TContext : class
    {
        private readonly IReflectionHelper _reflectionHelper;

        public ArrangeContext(bool mockOptionalParameters = true)
        {
            _reflectionHelper = new ReflectionHelper();

            MockOptionalParameters = mockOptionalParameters;
            Initialized = false;
            ContextType = typeof(TContext);
        }

        public override void Initialize()
        {
            if (Initialized) return;

            try
            {
                Constructor = _reflectionHelper.GetConstructor<TContext>();
                Parameters = _reflectionHelper.GetParametersFor(Constructor);
                ContextParameters = new List<ContextParameter>();
            }
            finally
            {
                Initialized = true;
            }
        }
    }
}
