namespace ArrangeContext.Core
{
    public interface IArrangeContext<out TContext> where TContext : class
    {
        TContext Build();
        T For<T>();
        T For<T>(string parameterName);
        T Use<T>(T instance);
        T Use<T>(T instance, string parameterName);
    }
}