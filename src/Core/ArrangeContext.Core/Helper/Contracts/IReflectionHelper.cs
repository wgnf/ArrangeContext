using System;
using System.Collections.Generic;
using System.Reflection;

namespace ArrangeContext.Core.Helper.Contracts
{
    internal interface IReflectionHelper
    {
        ConstructorInfo GetConstructor<T>() where T : class;
        IEnumerable<ParameterInfo> GetParametersFor(ConstructorInfo constructor);
        ConstructorInfo GetConstructor(Type type);
    }
}