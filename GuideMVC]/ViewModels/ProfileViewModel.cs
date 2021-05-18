using System.Collections.Generic;
using System.Linq;
using GuideMVC_.Helpers;
using GuideMVC_.Interfaces;
using GuideMVC_.Models;
using GuideMVC_.Services;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GuideMVC_.ViewModels
{
    public class ProfileViewModel
    {
        public Person Person { get; set; }
        public Person Father { get; set; }
        public Person Mother { get; set; }
        public Person Grandfather { get; set; }
        public Person Grandmother { get; set; }
        public Person Spouse { get; set; }
        public List<PersonView> Siblings { get; set; }

        public List<Person> Grandchildren { get; set; }
        public List<PersonView> Children { get; set; }
    }
}