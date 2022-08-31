using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Entidades
{
    [DataContract]
    public class Actor
    {
        [DataMember]
        public int ActorId { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public DateTime DateBirth { get; set; }

        public virtual ICollection<ActorMovie> Characters { get; set; }

    }
}
