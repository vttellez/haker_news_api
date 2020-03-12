using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace HackerNews.Core.Proxy.Validation
{
    [Serializable]
    public class ValidationResult
    {
        private const string InvalidResultKey = "ValidationResultErrors";
        private const string ValidResultKey = "ValidationResultIsValid";

        public ValidationResult()
        {
            ValidationResultErrors = new List<ValidationResultError>();
        }

        public ValidationResult(SerializationInfo info)
        {
            ValidationResultErrors = (List<ValidationResultError>)info.GetValue(InvalidResultKey, typeof(List<ValidationResultError>));
        }

        public List<ValidationResultError> ValidationResultErrors { get; set; }

        public bool ValidationResultIsValid => !ValidationResultErrors.ToList().Any();

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(InvalidResultKey, ValidationResultErrors, typeof(List<ValidationResultError>));
            info.AddValue(ValidResultKey, ValidationResultIsValid);
        }
    }
}
