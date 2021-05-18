using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

#nullable disable

namespace GuideMVC_.Models
{
    public partial class Person 
    {
        public Person()
        {
            UserRelativeFromUsers = new HashSet<UserRelative>();
            UserRelativeToUsers = new HashSet<UserRelative>();
        }

        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public DateTime? Birthdate { get; set; }
        public string PassportNumber { get; set; }
        public string PassportSeries { get; set; }
        public int GenderId { get; set; } 
        public string UserId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Homeland { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual ICollection<UserRelative> UserRelativeFromUsers { get; set; }
        public virtual ICollection<UserRelative> UserRelativeToUsers { get; set; }
        public virtual Marriage WifeMarriage{ get; set; }
        public virtual Marriage HusbandMarriage { get; set; }

    }
}
