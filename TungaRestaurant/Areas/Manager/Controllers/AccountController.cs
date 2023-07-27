using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TungaRestaurant.Data;
using TungaRestaurant.Models;

namespace TungaRestaurant.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly TungaRestaurantDbContext tungaRestaurantDbContext;
        private readonly UserManager<UserInfo> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<UserInfo> userManager, RoleManager<IdentityRole> roleManager,TungaRestaurantDbContext tungaRestaurantDbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tungaRestaurantDbContext = tungaRestaurantDbContext;
        }
        public IActionResult Index()
        {
            List<UserInfo> users = userManager.Users.Where(u=>!u.Id.Equals(userManager.GetUserId(User))).ToList();
            return View(users);
        }
        public IActionResult Create()
        {
            List<IdentityRole> roles = roleManager.Roles.ToList();
            List<Branch> branches = tungaRestaurantDbContext.Branch.ToList();
            ViewBag.Roles = roles;
            ViewBag.Branches = branches;
            ViewBag.StatusList = Enum.GetValues(typeof(UserStatus));
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Email,DisplayName,PhoneNumber,Sex,Address")] UserInfo user,[FromForm] string RoleName,[FromForm] string Password)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            IdentityRole role = await roleManager.FindByNameAsync(RoleName);
            if (role == null)
            {
                return RedirectToAction(nameof(Index));
            }
            user.UserName = user.Email;
            var Result = await userManager.CreateAsync(user,Password);
            var addRoleResult = await userManager.AddToRoleAsync(user, role.Name);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(string id)
        {
            UserInfo user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var roles = roleManager.Roles.ToList();
            List<Branch> branches = tungaRestaurantDbContext.Branch.ToList();
            ViewBag.Roles = roles;
            ViewBag.Branches = branches;
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Email,DisplayName,PhoneNumber,Sex,Address,BranchId,Status")] UserInfo user, [FromForm] string RoleName, [FromForm] string Password)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            IdentityRole role = await roleManager.FindByNameAsync(RoleName);
            if (role == null)
            {
                return RedirectToAction(nameof(Index));
            }
            UserInfo userFromDb = userManager.Users.Where(u => u.Id.Equals(user.Id)).First();
            userFromDb.DisplayName = user.DisplayName;
            userFromDb.PhoneNumber = user.PhoneNumber;
            userFromDb.Sex = user.Sex;
            userFromDb.Address = user.Address;

            var currentRoles = await userManager.GetRolesAsync(user);
            string currentRole = (currentRoles.Count > 0) ? currentRoles[0] : "";
            userFromDb.BranchId = user.BranchId;
            if (!currentRole.Equals(role))
            {
                try
                {
                    var removeFromRole = await userManager.RemoveFromRoleAsync(user,currentRole);
                }
                catch
                {

                }
                var addRoleResult = await userManager.AddToRoleAsync(user, role.Name);
            }
            if(Password != null)
            {
                userFromDb.PasswordHash = userManager.PasswordHasher.HashPassword(userFromDb, Password);
            }
            var updateResl = await userManager.UpdateAsync(userFromDb);
            return RedirectToAction(nameof(Index));
        }
    }
}
