using System;
using System.Collections.Generic;
using GuideMVC_.Models;

namespace GuideMVC_.ViewModels
{
    public class AddUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PassportNumber { get; set; }
        public string PassportSeries { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Homeland { get; set; }
        public DateTime? BirthDate { get; set; }
        public List<Gender> Genders { get; set; }
        public int GenderId { get; set; }
    }
}