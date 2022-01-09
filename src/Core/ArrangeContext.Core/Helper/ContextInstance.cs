namespace ArrangeContext.Core.Helper
{
    /// <summary>
    ///     Container that holds both an instance of parameter and a mocked instance (i.e. Mock{Something})
    /// </summary>
    public class ContextInstance
    {
        /// <summary>
        ///     Creates a new instance of <see cref="ContextInstance"/>
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <param name="mockedInstance">The mocked instance</param>
        public ContextInstance(
            object instance,
            object mockedInstance)
        {
            Instance = instance;
            MockedInstance = mockedInstance;
        }

        /// <summary>
        ///     The instance
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        ///     The mocked instance
        /// </summary>
        public object MockedInstance { get; set; }

        /// <summary>
        ///     Determines if the <see cref="ContextInstance"/> has been initialized
        /// </summary>
        /// <returns>Whether or not the <see cref="ContextInstance"/> has been initialized</returns>
        public bool IsInitialized()
        {
            return Instance != null && MockedInstance != null;
        }
    }
}
