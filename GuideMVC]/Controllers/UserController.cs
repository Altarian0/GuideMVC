using System;
using System.Linq;
using System.Threading.Tasks;
using GuideMVC_.Helpers;
using GuideMVC_.Interfaces;
using GuideMVC_.Models;
using GuideMVC_.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuideMVC_.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GuideDBContext _db;
        private IRelativeService _relativeService;

        public UserController(UserManager<ApplicationUser> userManager, GuideDBContext db,
            IRelativeService relativeService)
        {
            _userManager = userManager;
            _db = db;
            _relativeService = relativeService;
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

            ProfileViewModel profile = new ProfileViewModel();

            profile.Person = person;
            profile.Father = _relativeService.GetFather(person.Id);
            profile.Mother = _relativeService.GetMother(person.Id);
            profile.Grandfather = _relativeService.GetFather(_relativeService.GetFather(person.Id).Id);
            profile.Grandmother = _relativeService.GetMother(_relativeService.GetFather(person.Id).Id);
            profile.Spouse = _relativeService.GetSpouse(person.Id);
            profile.Siblings = _relativeService.GetSiblings(person.Id);
            profile.Grandchildren = _relativeService.GetGrandchildren(person.Id);
            profile.Children = _relativeService.GetChildrenViews(person.Id);
            profile.Marriage = _relativeService.GetMarriage(person.Id);
            profile.Marriages = _relativeService.GetMarriages(person.Id);

            ViewBag.UserRoles = await _userManager.GetRolesAsync(person.ApplicationUser);
            return View(profile);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddUser()
        {
            AddUserModel addUserModel = new AddUserModel();
            addUserModel.Genders = _db.Genders.ToList();
            return View(addUserModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserModel addUserModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                    {Email = addUserModel.Email, UserName = addUserModel.Email};

                var operation = await _userManager.CreateAsync(user, addUserModel.Password);

                if (operation.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    Person person = new Person()
                    {
                        Firstname = addUserModel.FirstName,
                        Lastname = addUserModel.LastName,
                        Middlename = addUserModel.MiddleName,
                        Birthdate = addUserModel.BirthDate,
                        PassportNumber = addUserModel.PassportNumber,
                        PassportSeries = addUserModel.PassportSeries,
                        Address = addUserModel.Address,
                        Phone = addUserModel.Phone,
                        Homeland = addUserModel.Homeland,

                        UserId = user.Id,
                        GenderId = addUserModel.GenderId
                    };
                    await _db.Persons.AddAsync(person);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Profile", new {id = person.Id});
                }
                else
                {
                    foreach (var error in operation.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View();
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
                Address = person.Address,
                Phone = person.Phone,
                Homeland = person.Homeland,
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
            person.Address = editUserModel.Address;
            person.Phone = editUserModel.Phone;
            person.Homeland = editUserModel.Homeland;
            person.GenderId = editUserModel.GenderId;

            _db.Entry(person).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return RedirectToAction("Profile", "User");
        }

        [HttpGet]
        public IActionResult AddRelative(int toPersonId, int fromPersonId, int relativeTypeId, int gender, int marriageId)
        {
            AddRelativeModel addRelativeModel = new AddRelativeModel()
            {
                ToPersonId = toPersonId,
                FromPersonId = fromPersonId,
                RelativeTypeId = relativeTypeId,
                RelativeTypes = _db.RelativeTypes.ToList(),
                FromPersons = relativeTypeId == 1? _db.Persons.Where(n=>n.GenderId == gender).ToList() : _db.Persons.ToList(),
                ToPersons = _db.Persons.ToList(),
                MarriageId = marriageId
            };
            return View(addRelativeModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRelative(AddRelativeModel addRelativeModel)
        {
            if (addRelativeModel.RelativeTypeId == 1)
            {
                var toPerson = _db.Persons.FirstOrDefault(n => n.Id == addRelativeModel.ToPersonId);
                var fromPerson = _db.Persons.FirstOrDefault(n => n.Id == addRelativeModel.FromPersonId);

                if (toPerson is null || fromPerson is null)
                    return RedirectToAction("Profile");
                
                Marriage marriage = new Marriage()
                {
                    WeddDate = addRelativeModel.WeddDate,
                    IsDivorced = addRelativeModel.IsDivorced,
                    DivorceDate = addRelativeModel.DivorceDate,
                    Description = $"{toPerson.Firstname} и {fromPerson.Firstname}"
                };
                
                try
                {
                    await _db.Marriages.AddAsync(marriage);
                    await _db.SaveChangesAsync();
                    addRelativeModel.MarriageId = marriage.Id;
                }
                catch(InvalidOperationException ex)
                {
                    return RedirectToAction("Profile");
                }
            }
            
            var relative = new UserRelative()
            {
                ToUserId = addRelativeModel.ToPersonId,
                FromUserId = addRelativeModel.FromPersonId,
                RelativeTypeId = addRelativeModel.RelativeTypeId,
                MarriageId = addRelativeModel.MarriageId
            };
            
            try
            {
                await _db.UserRelatives.AddAsync(relative);
                await _db.SaveChangesAsync();
            }
            catch(InvalidOperationException ex)
            {
                return RedirectToAction("Profile");
            }
            return RedirectToAction("Profile", "User", new {id = addRelativeModel.ToPersonId});
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
            return RedirectToAction("Profile", new {id = toPersonId});
        }

        public IActionResult EditRelative(int toPersonId, int fromPersonId, int relativeTypeId, int parentGender)
        {
            EditRelativeModel editRelativeModel = new EditRelativeModel()
            {
                ToPersonId = toPersonId,
                FromPersonId = fromPersonId,
                RelativeTypeId = relativeTypeId,
                ParentGender = parentGender,
                RelativeTypes = _db.RelativeTypes.ToList(),
                FromPersons = _db.Persons.ToList(),
                ToPersons = _db.Persons.ToList(),
            };

            return View(editRelativeModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRelative(EditRelativeModel editRelativeModel)
        {
            if (editRelativeModel.FromPersonId != 0)
            {
                var relative = _db.UserRelatives.FirstOrDefault(n =>
                                   n.ToUserId == editRelativeModel.ToPersonId &&
                                   n.FromUserId == editRelativeModel.FromPersonId &&
                                   n.RelativeTypeId == editRelativeModel.RelativeTypeId) ??
                               _db.UserRelatives.FirstOrDefault(n =>
                                   n.ToUserId == editRelativeModel.FromPersonId &&
                                   n.FromUserId == editRelativeModel.ToPersonId &&
                                   n.RelativeTypeId == editRelativeModel.RelativeTypeId);
                if (relative is null)
                    return RedirectToAction("Profile");
                _db.UserRelatives.Remove(relative);
                await _db.SaveChangesAsync();
            }

            UserRelative userRelative = new UserRelative()
            {
                ToUserId = editRelativeModel.ToPersonId,
                FromUserId = editRelativeModel.NewFromPersonId,
                RelativeTypeId = editRelativeModel.RelativeTypeId
            };
            await _db.UserRelatives.AddAsync(userRelative);
            await _db.SaveChangesAsync();


            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> DeleteMarriage(int toPersonId, int fromPersonId, int relativeTypeId)
        {
            var relative = _db.UserRelatives.Include(n => n.Marriage)
                               .FirstOrDefault(n =>
                                   n.ToUserId == toPersonId &&
                                   n.FromUserId == fromPersonId &&
                                   n.RelativeTypeId == relativeTypeId) ??
                           _db.UserRelatives
                               .FirstOrDefault(n =>
                                   n.ToUserId == fromPersonId &&
                                   n.FromUserId == toPersonId &&
                                   n.RelativeTypeId == relativeTypeId);
            if (relative is null)
                return RedirectToAction("Profile");
            var marriage = relative.Marriage;
            marriage.DivorceDate = DateTime.Now;
            marriage.IsDivorced = true;
            await _db.SaveChangesAsync();

            return RedirectToAction("Profile");
        }
    }
}