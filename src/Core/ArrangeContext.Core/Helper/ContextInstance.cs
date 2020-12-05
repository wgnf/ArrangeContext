using System;

namespace ArrangeContext.Core.Helper
{
    public class ContextInstance
    {
        public ContextInstance(
            object instance,
            object mockedInstance)
        {
            Instance = instance;
            MockedInstance = mockedInstance;
        }

        public object Instance { get; set; }

        public object MockedInstance { get; set; }
    }
}
