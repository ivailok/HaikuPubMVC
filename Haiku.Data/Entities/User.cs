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
    [Table("Users")]
    public class User : TEntity<int>
    {
        public User()
        {
            this.Haikus = new HashSet<HaikuEntity>();
        }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string Nickname { get; set; }

        [Required]
        [ExactStringLength(88, "Password")]
        public string Password { get; set; }

        [Required]
        [ExactStringLength(24, "Salt")]
        public string Salt { get; set; }

        public double? Rating { get; set; }

        public double HaikusRatingSum { get; set; }
        
        public int HaikusCount { get; set; }

        public virtual ICollection<HaikuEntity> Haikus { get; set; }
    }
}
