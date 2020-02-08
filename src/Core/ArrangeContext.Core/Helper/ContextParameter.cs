using System;
using System.Diagnostics.CodeAnalysis;

namespace ArrangeContext.Core.Helper
{
    // simple POCO
    [ExcludeFromCodeCoverage]
    internal class ContextParameter
    {
        public ContextParameter(Type parameterType, string name, object instance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            Type = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            Name = name;
            Instance = instance;
        }

        public Type Type { get; }

        public string Name { get; }

        public object Instance { get; set; }
    }
}