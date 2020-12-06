using ArrangeContext.Core;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;

namespace ArrangeContext.RhinoMocks.Tests
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

            context.For<ISomeInterface>().Stub(x => x.DoSomething()).Return(expected);

            var result = instance.Property3.DoSomething();
            result.Should().Be(expected);
        }
    }
}
