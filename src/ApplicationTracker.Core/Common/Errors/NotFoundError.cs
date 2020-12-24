using FluentResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationTracker.Core.Common.Errors
{
    public class NotFoundError : Error
    {
        public NotFoundError()
            : base()
        {
        }

        public NotFoundError(string message)
            : base(message)
        {
        }

        public NotFoundError(string message, Exception innerException)
            : base(message)
        {
            CausedBy(innerException);
        }

        public NotFoundError(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
