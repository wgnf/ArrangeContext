using System;
using System.Collections.Generic;
using System.Linq;
using ArrangeContext.Core.Helper;
using FluentAssertions;
using NUnit.Framework;

namespace ArrangeContext.Core.Tests
{
    [TestFixture]
    internal class ArrangeContextImplementationTests
    {
        [Test]
        public void Properly_Configured()
        {
            var sut = new ArrangeContextImplementation<object>(Enumerable.Empty<ContextParameter>().ToList());

            sut.Should().NotBeNull();
            sut.Should().BeAssignableTo<IArrangeContext<object>>();
        }

        [Test]
        public void Return_Parameter_By_Type()
        {
            const string stringParameterInstance = "someString";
            var parameters = new List<ContextParameter>
            {
                new ContextParameter(typeof(string), "someStringParameter", stringParameterInstance),
                new ContextParameter(typeof(object), "someObjectParameter", null)
            };
            var sut = new ArrangeContextImplementation<object>(parameters);

            sut.For<string>().Should().BeOfType<string>();
            sut.For<string>().Should().Be(stringParameterInstance);
        }

        [Test]
        public void Fail_On_Return_Parameter_By_Type()
        { 
            var sut = new ArrangeContextImplementation<object>(Enumerable.Empty<ContextParameter>().ToList());

            Assert.Throws<ArgumentException>(() => sut.For<object>());
        }

        [Test]
        public void Return_Parameter_By_Name()
        {
            const string stringParameterInstance = "someString";
            var parameters = new List<ContextParameter>
            {
                new ContextParameter(typeof(string), "someStringParameter", stringParameterInstance),
                new ContextParameter(typeof(object), "someObjectParameter", null)
            };
            var sut = new ArrangeContextImplementation<object>(parameters);

            var result = sut.For<string>("someStringParameter");
            result.Should().BeOfType<string>();
            result.Should().Be(stringParameterInstance);
        }

        [Test]
        public void Fail_On_Return_Parameter_By_Name()
        {
            var sut = new ArrangeContextImplementation<object>(Enumerable.Empty<ContextParameter>().ToList());

            Assert.Throws<ArgumentException>(() => sut.For<object>("someParameter"));
        }

        [Test]
        public void Replace_Parameter_With_New_Instance_By_Type()
        {
            var objInstance = new object();
            var parameters = new List<ContextParameter>
            {
                new ContextParameter(typeof(string), "someStringParameter", "123"),
                new ContextParameter(typeof(object), "someObjectParameter", null)
            };
            var sut = new ArrangeContextImplementation<object>(parameters);

            sut.For<object>().Should().BeNull();
            sut.Use(objInstance);
            sut.For<object>().Should().Be(objInstance);
        }

        [Test]
        public void Fail_On_Replace_Parameter_With_New_Instance_By_Type()
        {
            var sut = new ArrangeContextImplementation<object>(Enumerable.Empty<ContextParameter>().ToList());
            Assert.Throws<ArgumentException>(() => sut.Use(new object()));
        }

        [Test]
        public void Replace_Parameter_With_New_Instance_By_Name()
        {
            var objInstance = new object();
            var parameters = new List<ContextParameter>
            {
                new ContextParameter(typeof(string), "someStringParameter", "123"),
                new ContextParameter(typeof(object), "someObjectParameter", null)
            };
            var sut = new ArrangeContextImplementation<object>(parameters);

            sut.For<object>("someObjectParameter").Should().BeNull();
            sut.Use(objInstance, "someObjectParameter");
            sut.For<object>("someObjectParameter").Should().Be(objInstance);
        }

        [Test]
        public void Fail_On_Replace_Parameter_With_New_Instance_By_Name()
        {
            var sut = new ArrangeContextImplementation<object>(Enumerable.Empty<ContextParameter>().ToList());
            Assert.Throws<ArgumentException>(() => sut.Use(new object(), "someParameter"));
        }
    }
}