namespace ArrangeContext.Core.Factory.Contracts
{
    public interface IArrangeContextFactory
    {
        IArrangeContext<T> CreateArrangeContext<T>(bool mockOptionalParameters) where T : class;
    }
}