using System;
using System.Linq;
using System.Threading.Tasks;
using GuideMVC_.Helpers;
using GuideMVC_.Models;
using GuideMVC_.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuideMVC_.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GuideDBContext _db;

        public UserController(UserManager<ApplicationUser> userManager, GuideDBContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Profile(int id)
        {
            Person person = new Person();
            if (id == 0)
                person = await _db.Persons.Include(n => n.ApplicationUser)
                    .FirstOrDefaultAsync(n => n.UserId == _userManager.GetUserId(User));
            else
                person = await _db.Persons.Include(n => n.ApplicationUser)
                    .FirstOrDefaultAsync(n => n.Id == id);

            var userRelatives = _db.UserRelatives
                .Include(n => n.FromPerson)
                .Include(n => n.ToPerson)
                .ToList();
            ProfileViewModel profile = new ProfileViewModel()
            {
                Person = person,
                Relatives = userRelatives,
            };
            ViewBag.UserRoles = await _userManager.GetRolesAsync(person.ApplicationUser);
            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            var person = await _db.Persons.Include(n => n.ApplicationUser)
                .FirstOrDefaultAsync(n => n.UserId == applicationUser.Id);
            EditUserModel editUserModel = new EditUserModel
            {
                Id = person.Id,
                BirthDate = person.Birthdate,
                FirstName = person.Firstname,
                LastName = person.Lastname,
                MiddleName = person.Middlename,
                PassportNumber = person.PassportNumber,
                PassportSeries = person.PassportSeries,
                GenderId = person.GenderId,
                Genders = _db.Genders.ToList()
            };

            return View(editUserModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserModel editUserModel)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            var person = await _db.Persons.Include(n => n.ApplicationUser)
                .FirstOrDefaultAsync(n => n.UserId == applicationUser.Id);

            if (!ModelState.IsValid)
            {
                return View(editUserModel);
            }

            person.Firstname = editUserModel.FirstName;
            person.Lastname = editUserModel.LastName;
            person.Middlename = editUserModel.MiddleName;
            person.Birthdate = editUserModel.BirthDate;
            person.PassportNumber = editUserModel.PassportNumber;
            person.PassportSeries = editUserModel.PassportSeries;
            person.GenderId = editUserModel.GenderId;

            _db.Entry(person).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return RedirectToAction("Profile", "User");
        }

        [HttpGet]
        public IActionResult AddRelative(int toPersonId, int fromPersonId, int relativeTypeId)
        {
            AddRelativeModel addRelativeModel = new AddRelativeModel()
            {
                ToPersonId = toPersonId,
                FromPersonId = fromPersonId,
                RelativeTypeId = relativeTypeId,
                RelativeTypes = _db.RelativeTypes.ToList(),
                FromPersons = _db.Persons.ToList(),
                ToPersons = _db.Persons.ToList(),
            };
            return View(addRelativeModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRelative(AddRelativeModel addRelativeModel)
        {
            var relative = new UserRelative()
            {
                ToUserId = addRelativeModel.ToPersonId,
                FromUserId = addRelativeModel.FromPersonId,
                RelativeTypeId = addRelativeModel.RelativeTypeId
            };

            _db.UserRelatives.Add(relative);
            await _db.SaveChangesAsync();
            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> DeleteRelative(int toPersonId, int fromPersonId, int relativeTypeId)
        {
            var relative = _db.UserRelatives.FirstOrDefault(n =>
                               n.ToUserId == toPersonId &&
                               n.FromUserId == fromPersonId &&
                               n.RelativeTypeId == relativeTypeId) ??
                           _db.UserRelatives.FirstOrDefault(n =>
                               n.ToUserId == fromPersonId &&
                               n.FromUserId == toPersonId &&
                               n.RelativeTypeId == relativeTypeId);
            if (relative is null)
                return RedirectToAction("Profile");
            _db.UserRelatives.Remove(relative);
            await _db.SaveChangesAsync();
            return RedirectToAction("Profile");
        }
    }
}