namespace ArrangeContext.NSubstitute
{
    public static class NSubstituteArrangeContextExtensions
    {
        public static void Use<T>(
            this Core.ArrangeContext context, 
            object mockedInstance)
            where T : class
        {
            var factory = new NSubstituteArrangeContextFactory(context);

            var parameter = factory.GetParameter<T>();
            factory.ReplaceInstance(parameter, mockedInstance, mockedInstance);
        }

        public static void Use<T, TContext>(
            this Core.ArrangeContext<TContext> context,
            object mockedInstance,
            string parameterName)
            where T : class
            where TContext : class
        {
            var factory = new NSubstituteArrangeContextFactory(context);

            var parameter = factory.GetParameter(parameterName);
            factory.ReplaceInstance(parameter, mockedInstance, mockedInstance);
        }

        public static T For<T>(
            this Core.ArrangeContext context)
            where T : class
        {
            var factory = new NSubstituteArrangeContextFactory(context);

            var parameter = factory.GetParameter<T>();
            return (T)parameter.Instance.MockedInstance;
        }

        public static T For<T, TContext>(
            this Core.ArrangeContext<TContext> context, 
            string parameterName)
            where T : class
            where TContext : class
        {
            var factory = new NSubstituteArrangeContextFactory(context);

            var parameter = factory.GetParameter(parameterName);
            return (T)parameter.Instance.MockedInstance;
        }

        public static TContext Build<TContext>(
            this Core.ArrangeContext<TContext> context) where TContext : class
        {
            var factory = new NSubstituteArrangeContextFactory(context);
            return (TContext)factory.Build();
        }
    }
}
