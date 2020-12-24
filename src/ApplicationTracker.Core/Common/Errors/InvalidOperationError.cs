using FluentResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationTracker.Core.Common.Errors
{
    public class InvalidOperationError : Error
    {
        public InvalidOperationError(string description) : base(description)
        { }
    }
}
