using ArrangeContext.Core.Helper;
using NSubstitute;
using System;
using System.Reflection;

namespace ArrangeContext.NSubstitute
{
    internal class NSubstituteArrangeContextFactory : Core.ArrangeContextFactoryBase
    {
        public NSubstituteArrangeContextFactory(
            Core.ArrangeContext context) : base(context)
        {
        }

        protected override ContextInstance GetMockedInstanceFromProvider(ParameterInfo parameter)
        {
            try
            {
                var mockedInstance = Substitute.For(new[] { parameter.ParameterType }, Array.Empty<object>());
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
