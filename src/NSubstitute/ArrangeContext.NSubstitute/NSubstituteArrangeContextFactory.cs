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
            // TODO: Information about inaccessible things
            // with fix: internals visible to DynamicProxyGenAssembly2
            catch (Exception)
            {
                return new ContextInstance(null, null);
            }
        }
    }
}
