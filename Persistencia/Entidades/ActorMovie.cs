using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Entidades
{
    [DataContract]
    public class ActorMovie
    {
        [DataMember]
        public int ActorMovieId { get; set; }

        [DataMember]
        public String Character { get; set; }

        [DataMember]
        public int MovieId { get; set; }
        [IgnoreDataMember]
        public Movie Movie { get; set; }

        [DataMember]
        public int ActorId { get; set; }

        [IgnoreDataMember]
        public Actor Actor { get; set; }
    }
}
