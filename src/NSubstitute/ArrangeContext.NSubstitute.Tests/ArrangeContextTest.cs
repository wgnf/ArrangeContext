using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace ArrangeContext.NSubstitute.Tests
{
    [TestFixture]
    internal class ArrangeContextTest
    {
        [Test]
        public void Usage_Test()
        {
            var context = ArrangeContext<TestClass>.Create();
            var instance = context.Build();

            const string expected = "something";

            context.For<ISomeInterface>().DoSomething().Returns(expected);

            var result = instance.Property3.DoSomething();
            result.Should().Be(expected);


            var someInterface = Substitute.For<ISomeInterface>();
            someInterface
                .DoSomething()
                .Returns(expected);
            
            var context2 = ArrangeContext<TestClass>.Create();
            context2.Use(someInterface);
            
            var instance2 = context.Build();

            result = instance2.Property3.DoSomething();
            result.Should().Be(expected);
        }
    }
}
