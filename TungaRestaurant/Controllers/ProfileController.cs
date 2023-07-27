using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TungaRestaurant.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TungaRestaurant.Data;
using Microsoft.AspNetCore.Mvc.Routing;

namespace TungaRestaurant.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<UserInfo> _userManager;
        private readonly TungaRestaurantDbContext _context;
        public ProfileController(UserManager<UserInfo> userManager,TungaRestaurantDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.Name == null) return RedirectToAction(controllerName:"Home",actionName:"Index");
            UserInfo user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Profile = user;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("DisplayName,Sex,Address,PhoneNumber")] UserInfo user,[FromForm] string oldPassword,[FromForm] string newPassword,[FromForm] string confirmPassword)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            var files = HttpContext.Request.Form.Files;
            var avata = (files.Count > 0) ? files[0] : null;
            UserInfo loginUser = await _userManager.FindByNameAsync(User.Identity.Name);
            loginUser.DisplayName = user.DisplayName;
            loginUser.Sex = user.Sex;
            loginUser.Address = user.Address;
            loginUser.PhoneNumber = user.PhoneNumber;
            if (await CheckNewPassword(oldPassword, newPassword, confirmPassword, loginUser))
            {
                loginUser.PasswordHash = _userManager.PasswordHasher.HashPassword(loginUser, newPassword);
            }
            if(avata != null)
            {
                try
                {
                    string time = DateTime.Now.Ticks + "";
                    var fileName = time + avata.FileName;
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"));
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        avata.CopyTo(stream);
                    }
                    if(loginUser.Avatar != null)
                    {
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images",loginUser.Avatar));
                    }
                    loginUser.Avatar = fileName;

                }
                catch
                {
                    TempData["ErrMsg"] = "Fail to upload image";
                }

            }
            await _userManager.UpdateAsync(loginUser);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CheckNewPassword(string old,string password,string confirm,UserInfo user)
        {
            if(old == null || password == null || confirm == null)
            {
                return false;
            }
            if (! await _userManager.CheckPasswordAsync(user, old))
            {
                return false;
            }
            if (!password.Equals(confirm))
            {
                return false;
            }
            return true;
        }
        public async Task<IActionResult> History()
        {
            UserInfo user = await _userManager.Users
                                        .Include(u => u.Orders)
                                            .ThenInclude(o=>o.OrderDetail)
                                                .ThenInclude(od=>od.Food)
                                        .Where(u=>u.UserName.Equals(User.Identity.Name))
                                        .FirstOrDefaultAsync();
            ViewBag.LoginUser = user;
            return View();
        }
        public async Task<IActionResult> Reservation()
        {
            string uid = _userManager.GetUserId(User);
            List<Reservation> reservations = await _context.Reservations
                                                     .Include(r=>r.Table)
                                                     .Where(r=>r.UserId.Equals(uid))
                                                     .OrderByDescending(r=>r.Id)
                                                     .ToListAsync();
            ViewBag.Reservations = reservations;
            return View();
        }
    }
}
