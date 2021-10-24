# :keyboard: ArrangeContext

[![GitHub license](https://img.shields.io/badge/License-Unlicense-blue.svg)](LICENSE)
[![GitHub stars](https://img.shields.io/github/stars/wgnf/ArrangeContext?style=social)](https://github.com/wgnf/ArrangeContext/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/wgnf/ArrangeContext?style=social)](https://github.com/wgnf/ArrangeContext/network/members)
[![GitHub watchers](https://img.shields.io/github/watchers/Owgnf/ArrangeContext?style=social)](https://github.com/wgnf/ArrangeContext/watchers)  
[![Latest Release Core](https://img.shields.io/nuget/v/ArrangeContext.Core?style=for-the-badge)](https://www.nuget.org/packages/ArrangeContext.Core/)
[![Downloads Core](https://img.shields.io/nuget/dt/ArrangeContext.Core?style=for-the-badge)](https://www.nuget.org/packages/ArrangeContext.Core/)

:keyboard: .NET C#: Simple Tool to automatically initialize your system-under-test with mocked instances.  
Inspired by [NEdifis](https://github.com/awesome-inc/NEdifis) and [AutoFixture](https://github.com/AutoFixture/AutoFixture)

This'll turn something horrible like this:

```cs

var mock1 = new Mock<ISomeService1>();
var mock2 = new Mock<ISomeService2>();
var mock3 = new Mock<ISomeService3>();
var mock4 = new Mock<ISomeService4>();

var systemUnderTest = new SystemUnderTest(mock1, mock2, mock3, mock4);
```

To an even easier call like:

```cs

var systemUnderTest = new ArrangeContext<SystemUnderTest>().Build();
```

Additionally giving you the extra comfort of not needing to update the test-classes when you add a new parameter to your System-Under-Test:  
If you add a new Parameter with `ISomeService5` to your `SystemUnderTest`, in the "default"-approach you'd have to add a new line `var mock5 = new Mock<ISomeService5>()` to the arrangement of your `SystemUnderTest`!

## 🌟 Supported mocking Frameworks

|Framework ||
|----------|---------|
|[Moq](https://github.com/moq/moq4)|[![Latest Release Moq](https://img.shields.io/nuget/v/ArrangeContext.Moq?style=for-the-badge)](https://www.nuget.org/packages/ArrangeContext.Moq/) [![Downloads Moq](https://img.shields.io/nuget/dt/ArrangeContext.Moq?style=for-the-badge)](https://www.nuget.org/packages/ArrangeContext.Moq/)|
|[NSubstitute](https://github.com/nsubstitute/NSubstitute)|[![Latest Release NSubstitute](https://img.shields.io/nuget/v/ArrangeContext.NSubstitute?style=for-the-badge)](https://www.nuget.org/packages/ArrangeContext.NSubstitute/) [![Downloads NSubstitute](https://img.shields.io/nuget/dt/ArrangeContext.NSubstitute?style=for-the-badge)](https://www.nuget.org/packages/ArrangeContext.NSubstitute/)|
|[RhinoMocks](https://github.com/hibernating-rhinos/rhino-mocks)|[![Latest Release Rhino Mocks](https://img.shields.io/nuget/v/ArrangeContext.RhinoMocks?style=for-the-badge)](https://www.nuget.org/packages/ArrangeContext.RhinoMocks/) [![Downloads Rhino Mocks](https://img.shields.io/nuget/dt/ArrangeContext.RhinoMocks?style=for-the-badge)](https://www.nuget.org/packages/ArrangeContext.RhinoMocks/)|
|[FakeItEasy](https://github.com/FakeItEasy/FakeItEasy)|[![Latest Release FakeItEasy](https://img.shields.io/nuget/v/ArrangeContext.FakeItEasy?style=for-the-badge)](https://www.nuget.org/packages/ArrangeContext.FakeItEasy/) [![Downloads FakeItEasy](https://img.shields.io/nuget/dt/ArrangeContext.FakeItEasy?style=for-the-badge)](https://www.nuget.org/packages/ArrangeContext.FakeItEasy/)|

---

## 🌐 Features

### Creating the Context

```cs

var context = new ArrangeContext<YourTestClass>();
```

Creating the `ArrangeContext` is just as easy as creating a new class, providing your System-Under-Test with as the generic type-parameter.

### Build

```cs

context.Build();
```

This'll build the System-Under-Test with automatically mocked Constructor parameters for you, to run all the test's on this instance.

### Retrieving the mocked parameters

```cs

var service1 = context.For<IService1>();
var service2 = context.For<IService2>("parameterName");
```

`For<T>` and `For<T>(string parameterName)` are used to retrieve the mocked instances from the `ArrangeContext` so you can tell them what to do and/or return when specific things are called (depending on the Framework you use obviously!).

### Replacing instances

```cs

var myInstance1 = new Service1();
var myInstance2 = new Service2();

context.Use<IService1>(myInstance1);
context.Use<IService2>(myInstance2, "parameterName");
```

You don't like the mocked instance that was created for you? No problem! Using `Use<T>()` and `Use<T>(string parameterName)` you can replace any instance on the `ArrangeContext` that you like!

## ⌨️ Developing

To develop and work with ArrangeContext you just need to clone this Repo somewhere on your PC and then open the Solution or the complete Source-Folder (under `src`) with your favorite IDE. No additional tools required.  
  
Before you can start, you should restore all NuGet-Packages using `dotnet restore` if that's not done for you by your IDE.

## 👋 Want to Contribute?

Cool! We're always welcoming anyone that wants to contribute to this project! Take a look at the [Contributing Guidelines](CONTRIBUTING.md), which helps you get started. You can also look at the [Open Issues](https://github.com/wgnf/ArrangeContext/issues) for getting more info about current or upcoming tasks.

## 💬 Want to discuss?

If you have any questions, doubts, ideas, problems or you simply want to present your opinions and views, feel free to hop into [Discussions](https://github.com/wgnf/ArrangeContext/discussions) and write about what you care about. We'd love to hear from you!

