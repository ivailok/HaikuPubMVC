using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.DTO.Exceptions
{
    public class DuplicateUserNicknameException : Exception
    {
        public DuplicateUserNicknameException(string message = null)
            : base(message)
        {
        }

        public DuplicateUserNicknameException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
