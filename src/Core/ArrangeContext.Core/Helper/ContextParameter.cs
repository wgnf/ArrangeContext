using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ArrangeContext.Core.Helper
{
    /// <summary>
    ///     Container that holds information about a Constructor-Parameter for an ArrangeContext
    /// </summary>
    [ExcludeFromCodeCoverage] // simple POCO
    public class ContextParameter
    {
        /// <summary>
        ///     Creates a new instance of <see cref="ContextParameter"/>
        /// </summary>
        /// <param name="parameterInfo">The <see cref="ParameterInfo"/> that is the "base" for that parameter</param>
        /// <param name="instance">The <see cref="Helper.ContextInstance"/> for that parameter</param>
        public ContextParameter(
            ParameterInfo parameterInfo,
            ContextInstance instance)
        {
            ContextInstance = instance ?? throw new ArgumentNullException(nameof(instance));
            ParameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterInfo));

            Type = parameterInfo.ParameterType;
            Name = parameterInfo.Name;
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
        ///     The <see cref="ParameterInfo"/> that is the base for this Parameter
        /// </summary>
        public ParameterInfo ParameterInfo { get; }

        /// <summary>
        ///     The <see cref="Helper.ContextInstance"/> for that Parameter
        /// </summary>
        public ContextInstance ContextInstance { get; }
    }
}