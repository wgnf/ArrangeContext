using System;

namespace ArrangeContext.Core
{
    /// <summary>
    ///     Exception for when the Mocking of a parameter failed
    /// </summary>
    [Serializable]
    public class MockingFailedException : Exception
    {
        /// <summary>
        ///     Creates a new instance of <see cref="MockingFailedException"/>
        /// </summary>
        public MockingFailedException() { }

        /// <summary>
        ///     Creates a new instance of <see cref="MockingFailedException"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter for which the mocking failed</param>
        public MockingFailedException(string parameterName) : base($"Mocking failed for the Parameter {parameterName}. Please check the InnerException for more details.")
        {
            ParameterName = parameterName;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="MockingFailedException"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter for which the mocking failed</param>
        /// <param name="inner">The Inner exception</param>
        public MockingFailedException(string parameterName, Exception inner) : base($"Mocking failed for the Parameter {parameterName}. Please check the InnerException for more details.", inner)
        {
            ParameterName = parameterName;
        }

        /// <inheritdoc/>
        protected MockingFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     The Parameter for which the Mocking failed
        /// </summary>
        public string ParameterName { get; }
    }
}
