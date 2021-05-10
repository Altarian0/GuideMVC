using System;
using System.Collections.Generic;

#nullable disable

namespace GuideMVC_.Models
{
    public partial class UserRelative
    {
        public int ToUserId { get; set; }
        public int FromUserId { get; set; }
        public int RelativeTypeId { get; set; }

        public virtual Person FromPerson { get; set; }
        public virtual RelativeType RelativeType { get; set; }
        public virtual Person ToPerson { get; set; }
    }
}
