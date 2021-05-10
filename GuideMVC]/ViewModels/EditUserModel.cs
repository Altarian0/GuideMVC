using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GuideMVC_.Models;

namespace GuideMVC_.ViewModels
{
    public class EditUserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PassportNumber { get; set; }
        public string PassportSeries { get; set; }
        public DateTime? BirthDate { get; set; }
        public List<Gender> Genders { get; set; }
        public int GenderId { get; set; }
    }
}
