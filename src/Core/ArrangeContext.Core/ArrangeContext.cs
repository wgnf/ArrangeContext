using ArrangeContext.Core.Helper;
using ArrangeContext.Core.Helper.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ArrangeContext.Core
{
    /// <summary>
    ///     Context of the system-under-test to arrange
    /// </summary>
    public abstract class ArrangeContext
    {
        /// <summary>
        ///     The type of the system-under-test to create
        /// </summary>
        public Type ContextType { get; set; }

        /// <summary>
        ///     Value that indicates if optional Parameters should be mocked
        /// </summary>
        public bool MockOptionalParameters { get; set; }

        /// <summary>
        ///     Value that indicates if the Context was Initialized or not
        /// </summary>
        public bool Initialized { get; set; }

        /// <summary>
        ///     That Constructor that has been chosen to use to create the system-under-test
        /// </summary>
        public ConstructorInfo Constructor { get; set; }

        /// <summary>
        ///     The parameters that are present in the <see cref="Constructor"/>
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters { get; set; }

        /// <summary>
        ///     The <see cref="Parameters"/> enriched with more information and mocked instances
        /// </summary>
        public IList<ContextParameter> ContextParameters { get; set; }

        /// <summary>
        ///     Base-Method to Initialize the Context
        /// </summary>
        public virtual void Initialize()
        {
            Initialized = true;
        }
    }

    /// <summary>
    ///     Context of the system-under-test to arrange
    /// </summary>
    /// <typeparam name="TContext">The system-under-test to arrange</typeparam>
    public class ArrangeContext<TContext> : ArrangeContext where TContext : class
    {
        private readonly IReflectionHelper _reflectionHelper;

        /// <summary>
        ///     Creates a new instance of <see cref="ArrangeContext{TContext}"/>
        /// </summary>
        /// <param name="mockOptionalParameters">Value that indicates if optional parameters should be mocked</param>
        public ArrangeContext(bool mockOptionalParameters = true)
        {
            _reflectionHelper = new ReflectionHelper();

            MockOptionalParameters = mockOptionalParameters;
            Initialized = false;
            ContextType = typeof(TContext);
        }

        /// <summary>
        ///     Method to initialize the Context
        /// </summary>
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
