using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Data.Validation
{
    public class ExactStringLengthAttribute : StringLengthAttribute
    {
        public int Length { get; set; }

        public ExactStringLengthAttribute(int length, string propName)
            : base(length)
        {
            this.MinimumLength = length;
            this.Length = length;
            this.ErrorMessage = string.Format("{0} must be exactly {1} symbols.", propName, this.Length);
        }

        public override bool IsValid(object value)
        {
            string strValue = value as string;
            if (strValue != null)
            {
                return strValue.Length == this.Length;
            }
            return false;
        }
    }
}
