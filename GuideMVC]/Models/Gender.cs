using System;
using System.Collections.Generic;

#nullable disable

namespace GuideMVC_.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Users = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Person> Users { get; set; }
    }
}
