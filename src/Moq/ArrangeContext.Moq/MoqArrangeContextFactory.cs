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

        protected override ContextInstance GetMockedInstanceFromProvider(
            ParameterInfo parameter, 
            bool mockOptionalParameters)
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
            // TODO: Information about inaccessible things
            // with fix: internals visible to DynamicProxyGenAssembly2
            catch (Exception)
            {
                return new ContextInstance(null, null);
            }
        }
    }
}
