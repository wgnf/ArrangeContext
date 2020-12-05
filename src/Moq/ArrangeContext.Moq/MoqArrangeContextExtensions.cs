using Moq;

namespace ArrangeContext.Moq
{
    public static class MoqArrangeContextExtensions
    {
        public static void Use<T>(
            this Core.ArrangeContext context, 
            Mock<T> mockedInstance)
            where T : class
        {
            var factory = new MoqArrangeContextFactory(context);

            var parameter = factory.GetParameter<T>();
            factory.ReplaceInstance(parameter, mockedInstance.Object, mockedInstance);
        }

        public static void Use<T, TContext>(
            this Core.ArrangeContext<TContext> context,
            Mock<T> mockedInstance,
            string parameterName)
            where T : class
            where TContext : class
        {
            var factory = new MoqArrangeContextFactory(context);

            var parameter = factory.GetParameter(parameterName);
            factory.ReplaceInstance(parameter, mockedInstance.Object, mockedInstance);
        }

        // TODO:
        // Check if MockedInstance != null
        // else throw Exception, that this Type has not been mocked
        public static Mock<T> For<T>(
            this Core.ArrangeContext context)
            where T : class
        {
            var factory = new MoqArrangeContextFactory(context);

            var parameter = factory.GetParameter<T>();
            return (Mock<T>)parameter.Instance.MockedInstance;
        }

        // TODO:
        // Check if MockedInstance != null
        // else throw Exception, that this Type has not been mocked
        public static Mock<T> For<T, TContext>(
            this Core.ArrangeContext<TContext> context, 
            string parameterName)
            where T : class
            where TContext : class
        {
            var factory = new MoqArrangeContextFactory(context);

            var parameter = factory.GetParameter(parameterName);
            return (Mock<T>)parameter.Instance.MockedInstance;
        }

        public static TContext Build<TContext>(
            this Core.ArrangeContext<TContext> context) where TContext : class
        {
            var factory = new MoqArrangeContextFactory(context);
            return (TContext)factory.Build();
        }


    }
}
