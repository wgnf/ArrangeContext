using ArrangeContext.Core.Helper;
using Rhino.Mocks;
using System;
using System.Reflection;

namespace ArrangeContext.RhinoMocks
{
    internal class RhinoMocksArrangeContextFactory : Core.ArrangeContextFactoryBase
    {
        public RhinoMocksArrangeContextFactory(
            Core.ArrangeContext context) : base(context)
        {
        }

        protected override ContextInstance GetMockedInstanceFromProvider(ParameterInfo parameter)
        {
            try
            {
                var mockedInstance = MockRepository.GenerateMock(parameter.ParameterType, Array.Empty<Type>());
                return new ContextInstance(mockedInstance, mockedInstance);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"There was an issue creating a mock for {parameter.ParameterType.Name}." +
                    "This is possibly due to the Type not being accessible. Consider making it internal/public and/or using [assembly:InternalsVisibleTo(\"DynamicProxyGenAssembly2\")]!" +
                    "For more information refer to the InnerException of this Exception!", ex);
            }
        }
    }
}
