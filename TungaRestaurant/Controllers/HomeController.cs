using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TungaRestaurant.Data;
using TungaRestaurant.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TungaRestaurant.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TungaRestaurantDbContext _context;
        private readonly UserManager<UserInfo> _userManager;

        public HomeController(ILogger<HomeController> logger,TungaRestaurantDbContext tungaRestaurantDbContext,UserManager<UserInfo> userManager)
        {
            _logger = logger;
            _context = tungaRestaurantDbContext;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<JsonResult> ajaxRooms(int branch)
        {
            
            return Json(await _context.Rooms.Where(r=>r.BranchId==branch).ToListAsync());
        }
        public async Task<JsonResult> ajaxTables(int branch,int room,string date, string time,string time_to,int? numberOfGuest)
        {
            if (numberOfGuest == null)
            {
                numberOfGuest = 0;
            };

            List<Table> tables;


            if (!string.IsNullOrEmpty(date)&& !string.IsNullOrEmpty(time)&& !string.IsNullOrEmpty(time_to))
            {

                DateTime revDate = DateTime.ParseExact(date + " " + time, "M/d/yyyy h:mmtt", CultureInfo.InvariantCulture);
                DateTime revEnd = DateTime.ParseExact(date + " " + time_to, "M/d/yyyy h:mmtt", CultureInfo.InvariantCulture);
                tables =await _context.Table
                    .Where(t => t.RoomId == room)
                    .Where(t => t.Room.BranchId == branch)
                    .Where(t => t.NumberOfGuest >= numberOfGuest)
                    .Where(t =>
                      _context.Reservations
                            .Where(re => re.ReservationAt.Date == revDate.Date)
                            .Where(re => re.ReservationAt < revEnd && revDate < re.ReservationEnd)
                          .FirstOrDefault(re => re.TableId == t.Id) == null
                     ).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(time))
            {
                DateTime revDate = DateTime.ParseExact(date + " " + time, "M/d/yyyy h:mmtt", CultureInfo.InvariantCulture);
                tables =await _context.Table
                    .Where(t => t.RoomId == room)
                    .Where(t => t.Room.BranchId == branch)
                    .Where(t => t.NumberOfGuest >= numberOfGuest)
                    .Where(t =>
                      _context.Reservations
                            .Where(re => re.ReservationAt.Date == revDate.Date)
                            .Where(re => revDate < re.ReservationEnd)
                          .FirstOrDefault(re => re.TableId == t.Id) == null
                     ).ToListAsync();
            }else if (!string.IsNullOrEmpty(date))
            {
                DateTime revDate = DateTime.ParseExact(date, "M/d/yyyy", CultureInfo.InvariantCulture);
                tables =await _context.Table
                   .Where(t => t.RoomId == room)
                   .Where(t => t.Room.BranchId == branch)
                   .Where(t => t.NumberOfGuest >= numberOfGuest)
                    .Where(t =>
                      _context.Reservations
                            .Where(re => re.ReservationAt.Date == revDate.Date)   
                          .FirstOrDefault(re => re.TableId == t.Id) == null
                     ).ToListAsync();
            }
            else
            {
                 tables =await _context.Table
                   .Where(t => t.RoomId == room)
                   .Where(t => t.Room.BranchId == branch)
                   .Where(t => t.NumberOfGuest >= numberOfGuest).ToListAsync();
            }

            return Json(tables);
        }
        
        public async Task<IActionResult> Index()
        {
            if(TempData["Message"]!=null)
            {
                ViewBag.Message =TempData["Message"].ToString();
            }
            if (TempData["Notice"] != null)
            {
                ViewBag.Notice = TempData["Notice"].ToString();
            }
            if (TempData["Description"] != null)
            {
                ViewBag.Description = TempData["Description"].ToString();
            }
            ViewBag.ListTable = await _context.Table.Include(t=>t.Room).ToListAsync();
            ViewBag.MainCategorys = await _context.Categories.Include(c=>c.Foods).Take(3).ToListAsync();
            ViewBag.ListBranch = await _context.Branch.Where(b=>b.Status!=BranchStatus.CLOSE).ToListAsync();
            ViewBag.ListRoom = await _context.Rooms.ToListAsync();
            if (TempData["bookingValue"] != null)
            {
               
                return View(JsonConvert.DeserializeObject<TableBookInfor>((string)TempData["bookingValue"])); ;
            }
            return View();
        }
       
            public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Menu(int? branch,string isVegan,string search)
        {
            bool isVeganValue = (isVegan != null)? bool.Parse(isVegan):false;
            if (search == null) search = "";
            search.Trim();
            UserInfo user = null;
            List<Branch> branches = await _context.Branch.ToListAsync();
            ViewBag.Branches = branches;
            if (User.Identity.Name != null)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                
                if (branch == null) branch = user.PreferBranchId;
                if (isVegan == null) isVeganValue = user.IsVegan;
            }
            if (branch == null)
            {
                return View("Views/Home/SelectBranch.cshtml");
            }
            if(user != null)
            {
                user.PreferBranchId = branch;
                user.IsVegan = isVeganValue;
                await _userManager.UpdateAsync(user);
            }
            ViewBag.BranchId = branch;
            ViewBag.IsVegan = isVeganValue;
            // query by food
            //IQueryable<Food> foods = _context.Foods.Include(f => f.Category);
            //if (isVegan)
            //{
            //    foods.Where(f => f.IsVeganDish);
            //}
            //foods.Where(f => f.BranchId == null || f.BranchId == branch);
            //ViewBag.Foods = await foods.ToListAsync();

            //query by category
            ViewBag.Categories = _context.Categories.Include(c => c.Foods )
                .Where(categories=>categories
                    .Foods.Where(f=>
                        (f.BranchId==branch || f.BranchId == null))
                        .Where(f=>f.Name.Contains(search))
                        .Where(f=>f.IsVeganDish == isVeganValue)
                        .FirstOrDefault() != null)
                .ToList();
            return View();
        }

        public IActionResult Food(int id)
        {
            Food food = _context.Foods.Include(f=>f.Category).FirstOrDefault(f => f.Id == id);
            if (food == null)
            {
                TempData["msg"] = "Food not exist";
                return RedirectToAction(nameof(Menu));
            }
            List<Food> relateFood = new List<Food>();
            int relateOption = 0;
            do
            {
                if (relateFood.Count < 4)
                {
                    List<Food> tmpFood = new List<Food>();
                    switch (relateOption)
                    {
                        case 0:
                            tmpFood = _context.Foods.Where(f => f.IsVeganDish == food.IsVeganDish && f.CategoryId == food.CategoryId)
                                .Where(f => f.Id != food.Id)
                                .Where(f => f.BranchId == food.BranchId || f.BranchId == null)
                                .OrderBy(f => f.Id)
                                .Take(4).ToList();
                            break;
                        case 1:
                            tmpFood = _context.Foods.Where(f => f.Price > food.Price)
                                .Where(f => f.IsVeganDish == food.IsVeganDish)
                                .Where(f => f.Id != food.Id)
                                .Where(f => !relateFood.Contains(f))
                                .Where(f => f.BranchId == food.BranchId || f.BranchId == null)
                                .OrderBy(f => f.Id)
                                .Take(4 - relateFood.Count)
                                .ToList();
                            break;
                        case 2:
                            tmpFood = _context.Foods
                                .Where(f => f.IsVeganDish == food.IsVeganDish)
                                .Where(f => f.Id != food.Id)
                                .Where(f => !relateFood.Contains(f))
                                .Where(f => f.BranchId == food.BranchId || f.BranchId == null)
                                .OrderBy(f => f.Id)
                                .Take(4 - relateFood.Count)
                                .ToList();
                            break;
                        default:
                            relateOption = -1;
                            break;
                    }
                    relateFood.AddRange(tmpFood);
                    if(relateOption != -1)
                        relateOption++;

                }
            } while (relateFood.Count < 4 && relateOption != -1);

            ViewBag.Food = food;
            ViewBag.RelateFood = relateFood;
            return View();
        }
    }
}
