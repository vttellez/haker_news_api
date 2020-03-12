using System;
using System.Runtime.Serialization;

namespace HackerNews.Core.Proxy.Validation
{
    public class ValidationException : Exception
    {
        private const string ValidationResultKey = "ValidationResult";

        public ValidationException()
        {
        }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Adds result to exception data dictionary with a key of ValidationResult
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="result">Validation result</param>
        public ValidationException(string message, ValidationResult result)
            : base(message)
        {
            Data.Add(ValidationResultKey, result);
        }

        /// <summary>
        ///     Adds result to exception data dictionary with a key of ValidationResult
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Exception for the inner exception</param>
        /// <param name="result">Validation result</param>
        public ValidationException(string message, Exception innerException, ValidationResult result)
            : base(message, innerException)
        {
            Data.Add(ValidationResultKey, result);
        }

        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ValidationResult ValidationResult => Data[ValidationResultKey] as ValidationResult;
    }
}
