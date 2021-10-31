using System;

namespace ArrangeContext.Core
{
    /// <summary>
    ///     Exception for when the creation of an Instance failed
    /// </summary>
    [Serializable]
    public class InstanceCreationFailedException : Exception
    {
        /// <summary>
        ///     Creates a new instance of <see cref="InstanceCreationFailedException"/>
        /// </summary>
        public InstanceCreationFailedException() { }

        /// <summary>
        ///     Creates a new instance of <see cref="InstanceCreationFailedException"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter for which the creation failed</param>
        public InstanceCreationFailedException(string parameterName) : base($"Instance-Creation failed for the Parameter {parameterName}.")
        {
            ParameterName = parameterName;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="InstanceCreationFailedException"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter for which the creation failed</param>
        /// <param name="inner">The Inner exception</param>
        public InstanceCreationFailedException(string parameterName, Exception inner) : base($"Instance-Creation failed for the Parameter {parameterName}. Please check the InnerException for more details.", inner)
        {
            ParameterName = parameterName;
        }

        /// <inheritdoc/>
        protected InstanceCreationFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     The Parameter for which the creation failed
        /// </summary>
        public string ParameterName { get; }
    }
}
