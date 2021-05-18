using System.Collections.Generic;

namespace GuideMVC_.Models
{
    public class EditRelativeModel
    {
        public int ToPersonId { get; set; }
        public int FromPersonId { get; set; }
        public int NewFromPersonId { get; set; }
        public int RelativeTypeId { get; set; }
        public int ParentGender { get; set; }
        public List<RelativeType> RelativeTypes { get; set; }
        public List<Person> FromPersons { get; set; }
        public List<Person> ToPersons { get; set; }
    }
}