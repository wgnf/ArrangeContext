using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace ArrangeContext.FakeItEasy.Tests
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

            A.CallTo(() => context.For<ISomeInterface>().DoSomething()).Returns(expected);

            var result = instance.Property3.DoSomething();
            result.Should().Be(expected);
        }
    }
}
