using System;
using ArrangeContext.Core.Helper;
using ArrangeContext.Core.Helper.Contracts;
using FluentAssertions;
using NUnit.Framework;

namespace ArrangeContext.Core.Tests.Helper
{
    [TestFixture]
    internal class ReflectionHelperTests
    {
        [Test]
        public void Fails_When_No_Public_Constructor()
        {
            var sut = new ReflectionHelper();

            Assert.Throws<ArgumentException>(() => sut.GetConstructor<ClassWithoutPublicConstructor>());
        }

        [Test]
        public void Properly_Configured()
        {
            var sut = new ReflectionHelper();

            sut.Should().NotBeNull();
            sut.Should().BeAssignableTo<IReflectionHelper>();
        }

        [Test]
        public void Returns_The_Constructor_With_The_Most_Parameters()
        {
            var sut = new ReflectionHelper();

            var ctor = sut.GetConstructor<ClassWithMultipleConstructors>();
            ctor.GetParameters().Should().ContainSingle(p => p.Name == "iHaveTheMostParams");
        }

        [Test]
        public void Returns_The_Only_One_Accessible_Constructor()
        {
            var sut = new ReflectionHelper();

            var ctor = sut.GetConstructor<ClassWithOnlyOneConstructor>();
            ctor.GetParameters().Should().OnlyContain(p => p.Name == "iAmAParameter");
        }

        #region Helpers

        // ReSharper disable once ClassNeverInstantiated.Local
#pragma warning disable S3453 // Classes should not have only "private" constructors
        private class ClassWithoutPublicConstructor
#pragma warning restore S3453 // Classes should not have only "private" constructors
        {
            private ClassWithoutPublicConstructor()
            {
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class ClassWithOnlyOneConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            public ClassWithOnlyOneConstructor(string iAmAParameter)
            {
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class ClassWithMultipleConstructors
        {
            // ReSharper disable once UnusedMember.Global
            // ReSharper disable once UnusedMember.Local
            public ClassWithMultipleConstructors()
            {
            }

            // ReSharper disable once UnusedMember.Global
            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once UnusedParameter.Local
            public ClassWithMultipleConstructors(string someParameter)
            {
            }

            // ReSharper disable once UnusedMember.Global
            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once UnusedParameter.Local

            public ClassWithMultipleConstructors(
                string someParameter,
                // ReSharper disable once UnusedParameter.Local
                string iHaveTheMostParams)
            {
            }
        }

        #endregion
    }
}