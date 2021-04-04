using FluentAssertions;
using NUnit.Framework;

namespace ArrangeContext.Moq.Tests
{
    [TestFixture]
    internal class ArrangeContextTest
    {
        [Test]
        public void Usage_Test()
        {
            var context = new ArrangeContext<TestClass>();
            var instance = context.Build();

            const string expected = "something";

            context.For<ISomeInterface>().Setup(x => x.DoSomething()).Returns(expected);

            var result = instance.Property3.DoSomething();
            result.Should().Be(expected);
        }
    }
}
