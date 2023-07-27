using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TungaRestaurant.Data;
using TungaRestaurant.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TungaRestaurant.Controllers
{
    public class CartController : Controller
    {
        private readonly TungaRestaurantDbContext _dbContext;
        private readonly UserManager<UserInfo> _userManager;
        public CartController(TungaRestaurantDbContext context,UserManager<UserInfo> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
        }
        [Authorize]
        // GET: Cart
        public async Task<IActionResult> Index()
        {
            string userId = _userManager.GetUserId(User);

            List<UserInfo> userList = new List<UserInfo>();
            UserInfo user = userList.FirstOrDefault(u => u.Id == userId);
            var cartDbContext = await _dbContext.Carts.Include(c=>c.Food).Include(c=>c.UserInfo).OrderByDescending(c => c.Id).ToListAsync();
            return View(cartDbContext);
        }
        // GET: Cart/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Cart = await _dbContext.Carts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Cart == null)
            {
                return NotFound();
            }

            return View(Cart);
        }
        // POST: Cart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FoodId,UserInfoId,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cart.UserInfoId = _userManager.GetUserId(User);
                    Cart cartThatHasExistedFood = await _dbContext.Carts.FirstOrDefaultAsync(c => c.FoodId == cart.FoodId);
                    if (cartThatHasExistedFood != null)
                    {
                        cartThatHasExistedFood.Quantity += cart.Quantity;
                        _dbContext.Update(cartThatHasExistedFood);
                    }
                    else
                    {
                        _dbContext.Add(cart);
                    }
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ViewBag.error = "This item has exist in your cart. Please add or reduce it by go to Cart page!";
                    return View(cart);
                }
            }
            return View(cart);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FoodId,UserInfoId,Quantity")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(cart);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            return View(cart);
        }

        

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm]int id)
        {
            var Cart = await _dbContext.Carts.FindAsync(id);
            _dbContext.Carts.Remove(Cart);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _dbContext.Carts.Any(e => e.Id == id);
        }
    }
}
