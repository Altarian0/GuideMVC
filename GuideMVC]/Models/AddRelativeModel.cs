using System;
using System.Collections.Generic;

namespace GuideMVC_.Models
{
    public class AddRelativeModel
    {
        public int ToPersonId { get; set; }
        public int FromPersonId { get; set; }
        public int RelativeTypeId { get; set; }
        public int MarriageId { get; set; }
        public DateTime WeddDate { get; set; }
        public DateTime DivorceDate { get; set; }
        public bool IsDivorced { get; set; }
        
        public List<RelativeType> RelativeTypes { get; set; }
        public List<Person> FromPersons { get; set; }
        public List<Person> ToPersons { get; set; }
    }
}