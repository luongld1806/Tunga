using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TungaRestaurant.Data;
using TungaRestaurant.Models;

namespace TungaRestaurant.Controllers
{
    public class TableController : Controller
    {
        private readonly TungaRestaurantDbContext _context;
        private readonly UserManager<UserInfo> _userManager;

        public TableController(TungaRestaurantDbContext context,UserManager<UserInfo> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Branchs = await _context.Branch.ToListAsync();
            ViewBag.Food = await _context.Foods.ToListAsync();
            return View("~/Views/Home/TableReservation.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookATable([Bind("Name,date,time,time_to,phone,type,numberOfGuest,message,TableId")] TableBookInfor tableBookInfor)
        {
            
            if (ModelState.IsValid)
            {
                DateTime revDate = DateTime.ParseExact(tableBookInfor.date + " " + tableBookInfor.time, "M/d/yyyy h:mmtt", CultureInfo.InvariantCulture);
                DateTime revEnd = DateTime.ParseExact(tableBookInfor.date + " " + tableBookInfor.time_to, "M/d/yyyy h:mmtt", CultureInfo.InvariantCulture);
                if (revEnd<=revDate)
                {
                    TempData["bookingValue"] = JsonConvert.SerializeObject(tableBookInfor);
                    TempData["Message"] = "Time End must after Time Start";
                    return RedirectToAction("Index", "Home");
                }
                
                Reservation reservation = new Reservation();
                reservation.CreatedAt = DateTime.Now;
                reservation.NumberOfGuest = tableBookInfor.numberOfGuest;
                reservation.ReservationAt = revDate;
                reservation.ReservationEnd = revEnd;
                reservation.Status = ReservationStatus.USING;
                reservation.TableId = tableBookInfor.TableId;
                var token = "";
                if (User != null)
                {
                    UserInfo loginUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    reservation.UserId = loginUser.Id;
                    token = Regex.Replace(loginUser.Id, @"[^0-9\._]", "");
                    reservation.Token = token;
                }
                
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                TempData["Message"] = "You had booking table at "+revDate+" to "+revEnd+" with token "+token;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["bookingValue"] = JsonConvert.SerializeObject(tableBookInfor); 
                TempData["Message"] = "Please select right and all status";
                return RedirectToAction("Index", "Home");
            }
            
           
        }
    }
}
