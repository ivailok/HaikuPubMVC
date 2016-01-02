using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.DTO.Response
{
    [DataContract]
    public class PagingMetadata
    {
        [DataMember(Name = "totalCount")]
        public int TotalCount { get; set; }
    }
    
    [DataContract]
    public class PagingDto<T>
    {
        [DataMember(Name = "metadata", IsRequired = true)]
        public PagingMetadata Metadata { get; set; }

        [DataMember(Name = "results", IsRequired = true)]
        public IEnumerable<T> Results { get; set; }
    }
}
