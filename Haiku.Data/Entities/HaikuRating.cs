using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Data.Entities
{
    [Table("Ratings")]
    public class HaikuRating
    {
        public int Value { get; set; }

        public DateTime DateCreated { get; set; }

        [ForeignKey("Haiku")]
        public int HaikuId { get; set; }

        public virtual HaikuEntity Haiku { get; set; }
        
        public int UserId { get; set; }
    }
}
