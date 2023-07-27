using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TungaRestaurant.Data;
using TungaRestaurant.Models;

namespace TungaRestaurant.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Admin,Branch Manager")]
    public class OrderController : Controller
    {
        private readonly UserManager<UserInfo> _userManager;
        private readonly TungaRestaurantDbContext _context;
        public OrderController(UserManager<UserInfo> userManager, TungaRestaurantDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            List<Order> orders = _context.Orders.Include(o=>o.OrderDetail).Include(u=>u.User).OrderByDescending(o => o.Id).ToList();
            ViewBag.Orders = orders;
            return View(orders);
        }
        public IActionResult Detail(int id)
        {
            List<Order> order = _context.Orders.Include(o => o.OrderDetail).Where(o=>o.Id == id).ToList();
            ViewBag.Order = order;
            return View();
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Order = await _context.Orders.Include(o=>o.User).Where(o=>o.Id == id).FirstOrDefaultAsync();
            if (Order == null)
            {
                return NotFound();
            }
            return View(Order);
        }
        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,CreatedAt,ShipAddress,Status,UserInfoId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
