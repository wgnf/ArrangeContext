using ArrangeContext.Core.Helper;
using Moq;
using System;
using System.Reflection;

namespace ArrangeContext.Moq
{
    internal class MoqArrangeContextFactory : Core.ArrangeContextFactoryBase
    {
        public MoqArrangeContextFactory(
            Core.ArrangeContext context) : base(context)
        {
        }

        protected override ContextInstance GetMockedInstanceFromProvider(ParameterInfo parameter)
        {
            try
            {
                // NOTE: We have to use some workaround with reflection here
                // because we're creating a Mock<T> at runtime and
                // the generic Type is not known at compile-time

                var classType = typeof(Mock<>);
                var typeParams = new Type[] { parameter.ParameterType };
                var constructedType = classType.MakeGenericType(typeParams);

                var mockedInstance = (Mock)Activator.CreateInstance(constructedType);
                return new ContextInstance(mockedInstance.Object, mockedInstance);
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
