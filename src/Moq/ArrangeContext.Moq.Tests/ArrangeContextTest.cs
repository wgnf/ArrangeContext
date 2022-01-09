using ArrangeContext.Core;
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
            var context = ArrangeContext<TestClass>.Create();
            var instance = context.Build();

            const string expected = "something";

            context.For<ISomeInterface>().Setup(x => x.DoSomething()).Returns(expected);

            var result = instance.Property3.DoSomething();
            result.Should().Be(expected);
        }
        
        [Test]
        // c.f.: https://github.com/wgnf/ArrangeContext/issues/16
        public void Should_Work_With_Sealed_Dependencies()
        {
            var contextThatShouldFailBuild = new ArrangeContext<SystemUnderTestWithSealedDependency>();
            Assert.Throws<InstanceCreationFailedException>(() => contextThatShouldFailBuild.Build());
            
            var contextThatShouldFailFor = new ArrangeContext<SystemUnderTestWithSealedDependency>();
            Assert.Throws<InstanceCreationFailedException>(() => contextThatShouldFailFor.For<SealedDependency>());
            
            var contextThatShouldNotFail = new ArrangeContext<SystemUnderTestWithSealedDependency>();

            var newInstance = new SealedDependency();
            Assert.DoesNotThrow(() => contextThatShouldNotFail.Use(newInstance));

            var sut = contextThatShouldNotFail.Build();
            sut
                .Should()
                .NotBeNull();
        }
    }
}
