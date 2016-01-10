using Haiku.Data.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Data.Entities
{
    [Table("Sessions")]
    public class Session : TEntity<int>
    {
        [Required]
        [Index(IsUnique = true)]
        [ExactStringLength(88, "Session token")]
        public string Token { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string Nickname { get; set; }

        [Required]
        public DateTime From { get; set; }
    }
}
