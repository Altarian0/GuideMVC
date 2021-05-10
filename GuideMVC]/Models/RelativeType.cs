using System;
using System.Collections.Generic;

#nullable disable

namespace GuideMVC_.Models
{
    public partial class RelativeType
    {
        public RelativeType()
        {
            UserRelatives = new HashSet<UserRelative>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRelative> UserRelatives { get; set; }
    }
}
