using System.Diagnostics.CodeAnalysis;
using ArrangeContext.Core.Factory;

namespace ArrangeContext.Core
{
    // simple delegation to the Factory
    [ExcludeFromCodeCoverage]
    public static class ArrangeContext
    {
        public static IArrangeContext<TContext> For<TContext>(bool mockOptionalParameters = true) where TContext : class
        {
            return ArrangeContextFactoryBase.GetFactory().CreateArrangeContext<TContext>(mockOptionalParameters);
        }
    }
}