namespace ArrangeContext.Core
{
    /// <inheritdoc/>
    public abstract class ArrangeContextBaseWithBaseMethods<TContext> : ArrangeContextBase<TContext> where TContext : class
    {
        /// <inheritdoc/>
        protected ArrangeContextBaseWithBaseMethods(bool includeOptionalParameters)
            :base(includeOptionalParameters)
        {
        }

        /// <summary>
        ///     Replaces the previously generated parameter indicated by <typeparamref name="T"/> with the provided <paramref name="mockedInstance"/>
        /// </summary>
        /// <typeparam name="T">Indicates the parameter that should be replaced</typeparam>
        /// <param name="mockedInstance">The new instance for the parameter with the type <typeparamref name="T"/></param>
        public void Use<T>(T mockedInstance) where T : class
        {
            var parameter = GetParameter<T>();
            ReplaceInstance(parameter, mockedInstance, mockedInstance);
        }

        /// <summary>
        ///     Replaces the previously generated parameter indicated by the <paramref name="parameterName"/> with the provided <paramref name="mockedInstance"/>
        /// </summary>
        /// <typeparam name="T">Indicates the type of the parameter that should be replaced</typeparam>
        /// <param name="parameterName">The name of the parameter that should be replaced</param>
        /// <param name="mockedInstance">The new instance for the parameter with the name <paramref name="parameterName"/></param>
        public void Use<T>(T mockedInstance, string parameterName) where T : class
        {
            var parameter = GetParameter(parameterName);
            ReplaceInstance(parameter, mockedInstance, mockedInstance);
        }

        /// <summary>
        ///     Returns the mocked instance for the parameter indicated by <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Indicates the type of the parameter that should be returned</typeparam>
        /// <returns>Returns the mocked instance for the parameter indicated by <typeparamref name="T"/></returns>
        public T For<T>() where T : class
        {
            var parameter = GetParameter<T>();
            var determinedMockedInstance = parameter.Instance.MockedInstance;
            return (T)determinedMockedInstance;
        }

        /// <summary>
        ///     Returns the mocked instance for the parameter indicated by the <paramref name="parameterName"/>
        /// </summary>
        /// <typeparam name="T">Indicates the type of the parameter that should be returned</typeparam>
        /// <param name="parameterName">Indicates the name of the parameter that should be returned</param>
        /// <returns>Returns the mocked instance for the parameter indicated by <typeparamref name="T"/> and <paramref name="parameterName"/></returns>
        public T For<T>(string parameterName) where T : class
        {
            var parameter = GetParameter(parameterName);
            var determinedMockedInstance = parameter.Instance.MockedInstance;
            return (T)determinedMockedInstance;
        }
    }
}
