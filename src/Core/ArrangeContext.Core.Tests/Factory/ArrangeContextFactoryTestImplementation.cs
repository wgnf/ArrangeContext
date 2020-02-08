using System.Collections.Generic;
using System.Reflection;
using ArrangeContext.Core.Factory;
using ArrangeContext.Core.Helper.Contracts;

namespace ArrangeContext.Core.Tests.Factory
{
    internal class ArrangeContextFactoryTestImplementation : ArrangeContextFactoryBase
    {
        public IList<ParameterInfo> MockedParameters { get; }

        public ArrangeContextFactoryTestImplementation(IReflectionHelper reflectionHelper) : base(reflectionHelper)
        {
            MockedParameters = new List<ParameterInfo>();
        }

        protected override bool CanMockParameter(ParameterInfo parameter) => true;

        protected override object MockParameter(ParameterInfo parameter)
        {
            MockedParameters.Add(parameter);
            return null;
        }
    }
}