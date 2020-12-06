using System;
using System.Diagnostics.CodeAnalysis;

namespace ArrangeContext.Core.Helper
{
    /// <summary>
    ///     Container that holds information about a Constructor-Parmater for an ArrangeContext
    /// </summary>
    [ExcludeFromCodeCoverage] // simple POCO
    public class ContextParameter
    {
        /// <summary>
        ///     Creates a new instance of <see cref="ContextParameter"/>
        /// </summary>
        /// <param name="parameterType">The type of the parameter</param>
        /// <param name="name">The name of the parameter</param>
        /// <param name="instance">The <see cref="ContextInstance"/> for that parameter</param>
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

        /// <summary>
        ///     The Parameter-Type
        /// </summary>
        public Type Type { get; }

        /// <summary>
        ///     The Parameter-Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The <see cref="ContextInstance"/> for that Parameter
        /// </summary>
        public ContextInstance Instance { get; set; }
    }
}