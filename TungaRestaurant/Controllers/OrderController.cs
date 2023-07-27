using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TungaRestaurant.Models;
using TungaRestaurant.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TungaRestaurant.Areas.Manager.Controllers;
using TungaRestaurant.Ultils;

namespace TungaRestaurant.Controllers
{
    [Authorize]
    public class OrderController : Controller, IActionFilter
    {

        private List<OrderDetail> listOrderDetail = new List<OrderDetail>();

        private readonly TungaRestaurantDbContext _context;
        private readonly UserManager<UserInfo> _userManager;

        public OrderController(TungaRestaurantDbContext context, UserManager<UserInfo> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            //return ve trang checkout
            return RedirectToAction(actionName:"Index",controllerName:"Home");
        }

        // POST: Cart
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] string ShipAddress)
        {
            Order order = new Order();
            if (ModelState.IsValid)
            {
                try
                {
                    List<Cart> carts = _context.Carts.Include(c=>c.Food).Where(c => c.UserInfoId == _userManager.GetUserId(User)).ToList();
                    if (carts.Count == 0)
                    {
                        TempData["Notice"] = "Your cart is empty!";
                        TempData["Description"] = "Try to order some food first!";                        
                        return RedirectToAction("Index", "Home");
                    }
                    order.UserInfoId = _userManager.GetUserId(User);
                    //lay user
                    List<UserInfo> userList = new List<UserInfo>();
                    userList = _context.Users.Where(c => c.Id == _userManager.GetUserId(User)).ToList();
                    UserInfo user = userList.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
                    if (ShipAddress == null)
                    {
                        order.ShipAddress = user.Address;
                    }
                    else
                    {
                        order.ShipAddress = ShipAddress;
                    }
                    
                    //lay cart
                    order.Name = _userManager.GetUserName(User);
                    float totalPrice = 0;
                    foreach (var cart in carts)
                    {
                        totalPrice += cart.Quantity * cart.Food.Price;
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.FoodId = cart.FoodId;
                        orderDetail.Price = cart.Food.Price;
                        orderDetail.Quantity = cart.Quantity;
                        order.OrderDetail.Add(orderDetail);
                        _context.Carts.Remove(cart);
                    }
                    order.Price = totalPrice;
                    order.CreatedAt = DateTime.Now;
                    

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    TokenManager tokenManager = new TokenManager();
                    UserInfo userInfo = await _userManager.FindByNameAsync(User.Identity.Name);
                    ViewBag.OrderToken = tokenManager.GetOrderToken(userInfo,order.Id.ToString());
                    return View("/Views/Order/CheckOut.cshtml");

                }
                catch (Exception)
                {
                    ViewBag.error = "This item has exist in your Order. Please add or reduce it by go to Order page!";
                }
            }
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

    }
}