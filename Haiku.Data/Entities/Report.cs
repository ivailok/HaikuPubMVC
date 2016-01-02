using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Data.Entities
{
    [Table("Reports")]
    public class Report : TEntity<int>
    {
        public string Reason { get; set; }

        public DateTime DateSent { get; set; }

        public int HaikuId { get; set; }

        public virtual HaikuEntity Haiku { get; set; }
    }
}
