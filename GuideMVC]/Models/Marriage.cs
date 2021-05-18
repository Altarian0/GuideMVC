using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuideMVC_.Models
{
    public class Marriage
    {
        public Marriage()
        {
            Relatives = new HashSet<UserRelative>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime WeddDate { get; set; }
        public DateTime DivorceDate { get; set; }
        public bool IsDivorced { get; set; }
        public virtual ICollection<UserRelative> Relatives{ get; set; }
    }
}
