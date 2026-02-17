using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Domain.Utilities
{
    public class ValidationException : Exception
    {

        public IDictionary<string, string[]> _errors;
        public ValidationException(string message) : base(message)
        {
            _errors = new Dictionary<string, string[]>();
        }

        public ValidationException() : base("One or more validation failures have occurred")
        {
            _errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IDictionary<string, string[]> errors):this()
        {
            _errors = errors;
        }

    }
}
