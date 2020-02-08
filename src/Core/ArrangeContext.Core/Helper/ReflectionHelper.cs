using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using ArrangeContext.Core.Helper.Contracts;

namespace ArrangeContext.Core.Helper
{
    internal class ReflectionHelper : IReflectionHelper
    {
        public ConstructorInfo GetConstructor<T>() where T : class
        {
            return GetConstructor(typeof(T));
        }

        public ConstructorInfo GetConstructor(Type type)
        {
            var constructors = type.GetConstructors();

            switch (constructors.Length)
            {
                case 0:
                    throw new ArgumentException($"Type '{type.Name}' has no publicly accessible constructors.");
                case 1:
                    return constructors.First();
                default:
                    return GetConstructorWithMostParameters(constructors);
            }
        }

        // wrapper -- no need to test this
        [ExcludeFromCodeCoverage]
        public IEnumerable<ParameterInfo> GetParametersFor(ConstructorInfo constructor)
        {
            return constructor.GetParameters();
        }

        private static ConstructorInfo GetConstructorWithMostParameters(IEnumerable<ConstructorInfo> constructors)
        {
            var constructorsList = constructors.ToList();

            var maxConstructorParameter = constructorsList.Max(c => c.GetParameters().Length);
            return constructorsList.First(c => c.GetParameters().Length == maxConstructorParameter);
        }
    }
}