using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuideMVC_.Models;
using GuideMVC_.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GuideMVC_.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult UserList()
        {
            return View(_userManager.Users.ToList());
        }
        
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return NotFound();

            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            var allRoles = _roleManager.Roles.ToList();
            ChangeRoleModel changeRoleModel = new ChangeRoleModel()
            {
                UserId = userId,
                UserEmail = user.Email,
                UserRoles = userRoles,
                AllRoles = allRoles,
            };
            return View(changeRoleModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = roles.Except(userRoles);
            var rolesToRemove = userRoles.Except(roles);
            
            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            return RedirectToAction("UserList");
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        public async Task<IActionResult> Delete(string id)
        {
            var role =await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();
            
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (string.IsNullOrEmpty(name)) 
                return View(name);
            
            var result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(name);
        }
    }
}