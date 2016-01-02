using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Haiku.Data.Entities
{
    [Table("Haikus")]
    public class HaikuEntity : TEntity<int>
    {
        public HaikuEntity()
        {
            this.Ratings = new HashSet<HaikuRating>();
            this.Reports = new HashSet<Report>();
        }

        [Required]
        [MinLength(10)]
        public string Text { get; set; }

        public DateTime DatePublished { get; set; }

        public int UserId { get; set; }

        public double? Rating { get; set; }

        public int RatingsSum { get; set; }

        public int RatingsCount { get; set; }
        
        public virtual User User { get; set; }

        public virtual ICollection<HaikuRating> Ratings { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}
