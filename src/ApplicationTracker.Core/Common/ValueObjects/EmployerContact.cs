using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Common.ValueObjects
{
    /// <summary>
    /// Represents a contact from an employer, providing facilities to serialize and deserialize it.
    /// </summary>
    public class EmployerContact
    {
        public EmployerContact() { }

        public EmployerContact(string note)
        {
            var pattern = @"\[(.*)\] \[(.*)\] (.*)";
            
            Match match = Regex.Match(note, pattern);
            if (match.Success)
            {
                Person = match.Groups[1].Value;
                Subject = match.Groups[2].Value;
                Contents = match.Groups[3].Value;
            }
            else
            {
                Person = "Unknown";
                Subject = "Re:";
                Contents = note;
            }

        }

        public EmployerContact(string person, string subject, string contents)
        {
            Person = person;
            Subject = subject;
            Contents = contents;
        }

        /// <summary>
        /// The contact person from the organization.
        /// </summary>
        public string Person { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The body of the contact.
        /// </summary>
        public string Contents { get; set; }

        public override string ToString()
        {
            return $"[{Person}] [{Subject}] {Contents}";
        }
    }
}
