using ArrangeContext.Core.Helper;
using FakeItEasy;
using System;
using System.Reflection;
using System.Linq;

namespace ArrangeContext.FakeItEasy
{
    internal class FakeItEasyArrangeContextFactory : Core.ArrangeContextFactoryBase
    {
        public FakeItEasyArrangeContextFactory(
            Core.ArrangeContext context) : base(context)
        {
        }

        protected override ContextInstance GetMockedInstanceFromProvider(ParameterInfo parameter)
        {
            try
            {
                // NOTE: We have to use some workaround with reflection here
                // because we're calling a generic Method where
                // the generic Type is not known at compile-time

                var methods = typeof(A).GetMethods();
                var method = methods.FirstOrDefault(m => m.Name == nameof(A.Fake));

                var typeParams = new Type[] { parameter.ParameterType };
                var genericMethod = method.MakeGenericMethod(typeParams);
                var mockedInstance = genericMethod.Invoke(null, null);

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
