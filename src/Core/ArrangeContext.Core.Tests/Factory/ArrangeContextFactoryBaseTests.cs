using System.Linq;
using System.Reflection;
using ArrangeContext.Core.Factory;
using ArrangeContext.Core.Helper.Contracts;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace ArrangeContext.Core.Tests.Factory
{
    [TestFixture]
    internal class ArrangeContextFactoryBaseTests
    {
        [Test]
        public void Creates_Arrange_Context_From_Class_With_Default_Param()
        {
            var reflectionHelper = Substitute.For<IReflectionHelper>();
            var sut = new ArrangeContextFactoryTestImplementation(reflectionHelper);

            var ctor = Substitute.For<ConstructorInfo>();
            reflectionHelper.GetConstructor<object>().Returns(ctor);
            var param = GetOptionalParameter();
            reflectionHelper.GetParametersFor(ctor).Returns(new[] {param});

            var result = sut.CreateArrangeContext<object>(false);
            result.Should().NotBeNull();
            sut.MockedParameters.Should().BeEmpty();

            result = sut.CreateArrangeContext<object>(true);
            result.Should().NotBeNull();
            sut.MockedParameters.Should().OnlyContain(p => p.Name == "optionalParameter");
        }

        [Test]
        public void Creates_Arrange_Context_From_Class_With_Interface_Param()
        {
            var reflectionHelper = Substitute.For<IReflectionHelper>();
            var sut = new ArrangeContextFactoryTestImplementation(reflectionHelper);

            var ctor = Substitute.For<ConstructorInfo>();
            reflectionHelper.GetConstructor<object>().Returns(ctor);
            var param = GetInterfaceParameter();
            reflectionHelper.GetParametersFor(ctor).Returns(new[] {param});

            var result = sut.CreateArrangeContext<object>(false);
            result.Should().NotBeNull();

            sut.MockedParameters.Should().OnlyContain(p => p.Name == "someParameter");
        }

        [Test]
        public void Creates_Arrange_Context_From_Class_Without_Ctor_Params()
        {
            var reflectionHelper = Substitute.For<IReflectionHelper>();
            var sut = new ArrangeContextFactoryTestImplementation(reflectionHelper);

            var ctor = Substitute.For<ConstructorInfo>();
            reflectionHelper.GetConstructor<object>().Returns(ctor);
            reflectionHelper.GetParametersFor(ctor).Returns(Enumerable.Empty<ParameterInfo>());

            var result = sut.CreateArrangeContext<object>(true);
            result.Should().NotBeNull();

            sut.MockedParameters.Should().BeEmpty();
        }

        [Test]
        public void Creates_Arrange_Context_From_Class_With_Value_Type_Param()
        {
            var reflectionHelper = Substitute.For<IReflectionHelper>();
            var sut = new ArrangeContextFactoryTestImplementation(reflectionHelper);

            var ctor = Substitute.For<ConstructorInfo>();
            reflectionHelper.GetConstructor<object>().Returns(ctor);
            var param = GetValueTypeParameter();
            reflectionHelper.GetParametersFor(ctor).Returns(new[] { param });

            var result = sut.CreateArrangeContext<object>(false);
            result.Should().NotBeNull();

            sut.MockedParameters.Should().BeEmpty("ValueType won't be mocked");
        }

        [Test]
        public void Provides_Factory_Instance_And_Caches_This_Instance()
        {
            var reflectionHelper = Substitute.For<IReflectionHelper>();
            ArrangeContextFactoryBase.FactoryInstanceCreator = () => new ArrangeContextFactoryTestImplementation(reflectionHelper);

            var instance = ArrangeContextFactoryBase.GetFactory();
            instance.Should().NotBeNull();

            var anotherInstance = ArrangeContextFactoryBase.GetFactory();
            anotherInstance.Should().NotBeNull();
            ReferenceEquals(instance, anotherInstance).Should().BeTrue();
        }

        #region Helper

        private static ParameterInfo GetOptionalParameter()
        {
            var ctor = typeof(SomeClassWithOptionalParameter).GetConstructors().FirstOrDefault();
            var optionalParameter = ctor?.GetParameters().FirstOrDefault();

            return optionalParameter;
        }

        private static ParameterInfo GetInterfaceParameter()
        {
            var ctor = typeof(SomeClassWithInterfaceParameter).GetConstructors().FirstOrDefault();
            var parameter = ctor?.GetParameters().FirstOrDefault();

            return parameter;
        }

        private static ParameterInfo GetValueTypeParameter()
        {
            var ctor = typeof(SomeClassWithValueTypeParameter).GetConstructors().FirstOrDefault();
            var parameter = ctor?.GetParameters().FirstOrDefault();

            return parameter;
        }

        private interface ISomeInterface { }

        private class SomeClassWithOptionalParameter
        {
            // ReSharper disable once UnusedParameter.Local
            public SomeClassWithOptionalParameter(ISomeInterface optionalParameter = null)
            {
            }
        }

        private class SomeClassWithInterfaceParameter
        {
            // ReSharper disable once UnusedParameter.Local
            public SomeClassWithInterfaceParameter(ISomeInterface someParameter)
            {
            }
        }

        private class SomeClassWithValueTypeParameter
        {
            // ReSharper disable once UnusedParameter.Local
            public SomeClassWithValueTypeParameter(SomeEnumTest valueTypeParameter)
            {
            }
        }

        private enum SomeEnumTest
        {
            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once UnusedMember.Global
            SomeValue,
            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once UnusedMember.Global
            AnotherValue
        }

        #endregion
    }
}