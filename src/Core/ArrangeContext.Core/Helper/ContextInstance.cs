using System;

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
    }
}
