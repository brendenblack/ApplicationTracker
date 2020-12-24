using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain.ApplicationStatus
{
    public class ValidationResult
    {
        internal ValidationResult(bool isValid, string token)
        {
            IsValid = isValid;
            Token = token;
        }
        
        public ValidationResult()
        { }

        public bool IsValid { get; private set; }

        public string Token { get; private set; }
    }
}
