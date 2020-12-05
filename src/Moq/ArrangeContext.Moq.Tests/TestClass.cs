namespace ArrangeContext.Moq.Tests
{
    public class TestClass
    {
        public TestClass()
        {

        }

        public TestClass(string param1, double param2, ISomeInterface param3)
        {
            Property1 = param1;
            Property2 = param2;
            Property3 = param3;
        }

        public string Property1 { get; set; }

        public double Property2 { get; set; }

        public ISomeInterface Property3 { get; set; }
    }

    public interface ISomeInterface
    {
        string DoSomething();
    }
}
