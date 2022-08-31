using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Entidades
{
    [DataContract]
    public class MovieDto
    {
        [DataMember(Name = "actor")]
        public Actor Actor { get; set; }

        [DataMember(Name = "movie")]
        public Movie Movie { get; set; }

        [DataMember(Name = "actorMovie")]
        public ActorMovie ActorMovie { get; set; }
    }
}
