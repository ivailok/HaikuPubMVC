using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.DTO.Exceptions
{
    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException(string message = null)
            : base (message)
        {
        }

        public UnauthorizedAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
