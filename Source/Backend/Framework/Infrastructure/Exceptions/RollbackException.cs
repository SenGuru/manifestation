using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Framework.Infrastructure.Models.Result;

namespace Framework.Infrastructure.Exceptions
{
    [Serializable]
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class RollbackException : Exception
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public RollbackException(string message, object problemObjectDetails = null)
            : base(message)
        {
            Error = null;
            ProblemObjectDetails = problemObjectDetails;
        }

        public RollbackException(ReturnError error)
            : base()
        {
            Error = error;
        }

        public RollbackException(List<ReturnErrorItem> error)
            : base()
        {
            ProblemObjectDetails = error;
        }

        public RollbackException(IReturnModel model)
            : base()
        {
            Error = model?.ErrorHolder;
        }

        public RollbackException()
            : base()
        {
            Error = null;
        }

        public RollbackException(string message, Exception innerException)
            : base(message, innerException)
        {
            Error = null;
        }

        protected RollbackException(SerializationInfo info, StreamingContext context)
#pragma warning disable SYSLIB0051 // Type or member is obsolete
            : base(info, context)
#pragma warning restore SYSLIB0051 // Type or member is obsolete
        {
            // Deserialize custom properties here if needed
        }

        public ReturnError Error { get; set; }

        public object ProblemObjectDetails { get; set; }
    }
}
