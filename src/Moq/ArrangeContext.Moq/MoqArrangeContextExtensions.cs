using Moq;
using System;

namespace ArrangeContext.Moq
{
    /// <summary>
    ///     Extensions for ArrangeContext.Moq
    /// </summary>
    public static class MoqArrangeContextExtensions
    {
        /// <summary>
        ///     Replaces the previously generated parameter indicated by <typeparamref name="T"/> with the provided <paramref name="mockedInstance"/>
        /// </summary>
        /// <typeparam name="T">Indicates the parameter that should be replaced</typeparam>
        /// <param name="context">The <see cref="Core.ArrangeContext"/> to operate on</param>
        /// <param name="mockedInstance">The new instance for the parameter with the type <typeparamref name="T"/></param>
        public static void Use<T>(
            this Core.ArrangeContext context, 
            Mock<T> mockedInstance)
            where T : class
        {
            var factory = new MoqArrangeContextFactory(context);

            var parameter = factory.GetParameter<T>();
            factory.ReplaceInstance(parameter, mockedInstance.Object, mockedInstance);
        }

        /// <summary>
        ///     Replaces the previously generated parameter indicated by the <paramref name="parameterName"/> with the provided <paramref name="mockedInstance"/>
        /// </summary>
        /// <typeparam name="T">Indicates the type of the parameter that should be replaced</typeparam>
        /// <param name="context">The <see cref="Core.ArrangeContext"/> to operate on</param>
        /// <param name="parameterName">The name of the parameter that should be replaced</param>
        /// <param name="mockedInstance">The new instance for the parameter with the name <paramref name="parameterName"/></param>
        public static void Use<T>(
            this Core.ArrangeContext context,
            string parameterName,
            Mock<T> mockedInstance)
            where T : class
        {
            var factory = new MoqArrangeContextFactory(context);

            var parameter = factory.GetParameter(parameterName);
            factory.ReplaceInstance(parameter, mockedInstance.Object, mockedInstance);
        }

        /// <summary>
        ///     Returns the mocked instance for the parameter indicated by <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Indicates the type of the parameter that should be returned</typeparam>
        /// <param name="context">The <see cref="Core.ArrangeContext"/> to operate on</param>
        /// <returns>Returns the mocked instance for the parameter indicated by <typeparamref name="T"/></returns>
        public static Mock<T> For<T>(
            this Core.ArrangeContext context)
            where T : class
        {
            var factory = new MoqArrangeContextFactory(context);

            var parameter = factory.GetParameter<T>();
            var mockedInstance = parameter.Instance.MockedInstance;
            if (mockedInstance == null)
                throw new InvalidOperationException($"The instance for the parameter with the type {typeof(T).Name} has not been mocked!");

            return (Mock<T>)mockedInstance;
        }

        /// <summary>
        ///     Returns the mocked instance for the parameter indicated by the <paramref name="parameterName"/>
        /// </summary>
        /// <typeparam name="T">Indicates the type of the parameter that should be returned</typeparam>
        /// <param name="context">The <see cref="Core.ArrangeContext"/> to operate on</param>
        /// <param name="parameterName">Indicates the name of the parameter that should be returned</param>
        /// <returns>Returns the mocked instance for the parameter indicated by <typeparamref name="T"/> and <paramref name="parameterName"/></returns>
        public static Mock<T> For<T>(
            this Core.ArrangeContext context, 
            string parameterName)
            where T : class
        {
            var factory = new MoqArrangeContextFactory(context);

            var parameter = factory.GetParameter(parameterName);
            var mockedInstance = parameter.Instance.MockedInstance;
            if (mockedInstance == null)
                throw new InvalidOperationException($"The instance for the parameter with the type {typeof(T).Name} and name {parameterName} has not been mocked!");

            return (Mock<T>)mockedInstance;
        }

        /// <summary>
        ///     Creates the instance for the system-under-test with the type <typeparamref name="TContext"/>
        /// </summary>
        /// <typeparam name="TContext">Indicates the type of the system-under-test to create</typeparam>
        /// <param name="context">The <see cref="Core.ArrangeContext{TContext}"/> to operate on</param>
        /// <returns>Returns the constructed system-under-test with mocked instances</returns>
        public static TContext Build<TContext>(
            this Core.ArrangeContext<TContext> context) where TContext : class
        {
            var factory = new MoqArrangeContextFactory(context);
            return (TContext)factory.Build();
        }


    }
}
