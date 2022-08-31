using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Entidades
{
    [DataContract]
    public class Movie
    {
        [DataMember]
        public int MovieId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Director { get; set; }

        [DataMember]
        public DateTime ReleaseDate { get; set; }

        [DataMember]
        public decimal Gross { get; set; }

        [DataMember]
        public double Rating { get; set; }

        [DataMember]
        public int GenreID { get; set; }

        [DataMember]
        public virtual Genre Genre { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<ActorMovie> Characters { get; set; }
    }

    
}
