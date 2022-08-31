using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Entidades
{
    [DataContract]
    public  class Genre
    {
        [DataMember]
        public int GenreId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Movie> Movies { get; set; }

    }
}
