# ArrangeContext
.NET C#: Simple Tool to automatically initialize your system-under-test with mocked instances.

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

## Supported mocking Frameworks:

- [Moq](https://github.com/moq/moq4)
- [NSubstitute](https://github.com/nsubstitute/NSubstitute)

---

## Features

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
