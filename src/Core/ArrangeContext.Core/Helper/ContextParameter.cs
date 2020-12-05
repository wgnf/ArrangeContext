using System;
using System.Diagnostics.CodeAnalysis;

namespace ArrangeContext.Core.Helper
{
    // simple POCO
    [ExcludeFromCodeCoverage]
    public class ContextParameter
    {
        public ContextParameter(
            Type parameterType, 
            string name, 
            ContextInstance instance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            Name = name;

            Type = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public Type Type { get; }

        public string Name { get; }

        public ContextInstance Instance { get; set; }
    }
}