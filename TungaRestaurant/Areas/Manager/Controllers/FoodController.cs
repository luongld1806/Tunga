using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TungaRestaurant.Data;
using TungaRestaurant.Models;

namespace TungaRestaurant.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Admin,Branch Manager")]
    public class FoodController:Controller
    {
        private readonly TungaRestaurantDbContext _dbContext;
        public FoodController(TungaRestaurantDbContext context)
        {
            _dbContext = context;
        }
        // GET: Food
        public async Task<IActionResult> Index()
        {
            var foodDbContext = await _dbContext.Foods.Include(f => f.Category).Include(f=>f.Branch).OrderByDescending(f => f.Id).ToListAsync();
            return View(foodDbContext);
        }

        // GET: Food/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Food = await _dbContext.Foods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Food == null)
            {
                return NotFound();
            }

            return View(Food);
        }

        // GET: Food/Create
        public IActionResult Create()
        {
            List<Branch> branches = _dbContext.Branch.ToList();
            var emptyBranch = new Branch();
            emptyBranch.Id = 0;
            emptyBranch.Name = "All Branch";
            branches.Insert(0,emptyBranch);
            ViewData["CateId"] = new SelectList(_dbContext.Categories, "Id", "Name");
            ViewData["BranchId"] = new SelectList(branches , "Id", "Name");
            return View();
        }

        // POST: Food/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Price,CookDuration,BranchId,CategoryId,ServeUnit,Status,IsVeganDish")] Food Food)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //upload image
                    var files = HttpContext.Request.Form.Files;
                    if (files != null && files[0].Length > 0)
                    {
                        var file = files[0];
                        string time = DateTime.Now.Ticks + "";
                        var fileName = time + file.FileName;
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"));
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            Food.Image = fileName;
                        }
                    }
                    if (Food.BranchId == 0) Food.BranchId = null;
                    _dbContext.Add(Food);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.error = "This food has existed! Please contact with admin for more infor." + ex;
                }
            }
            ViewData["CateId"] = new SelectList(_dbContext.Categories, "CateId", "CateId", Food.Category);
            ViewData["BranchId"] = new SelectList(_dbContext.Branch, "BranchId", "BranchId", Food.BranchId);
            return View(Food);

        }

        // GET: Food/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Food = await _dbContext.Foods.FindAsync(id);
            if (Food == null)
            {
                return NotFound();
            }

            List<Branch> branches = _dbContext.Branch.ToList();
            var emptyBranch = new Branch();
            emptyBranch.Id = 0;
            emptyBranch.Name = "All Branch";
            branches.Insert(0, emptyBranch);
            ViewData["CateId"] = new SelectList(_dbContext.Categories, "Id", "Name");
            ViewData["BranchId"] = new SelectList(branches, "Id", "Name",Food.BranchId);
            return View(Food);
        }

        // POST: Food/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Price,CookDuration,BranchId,ServeUnit,Status,CategoryId,IsVeganDish")] Food Food)
        {
            if (id != Food.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //upload image
                    var files = HttpContext.Request.Form.Files;
                    if (files != null && files.Count > 0)
                    {
                        var file = files[0];
                        string time = DateTime.Now.Ticks + "";
                        var fileName = time + file.FileName;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            Food.Image = fileName;
                        }
                    }
                    if (Food.BranchId == 0) Food.BranchId = null;
                    _dbContext.Update(Food);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(Food.Id))
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
            ViewData["CateId"] = new SelectList(_dbContext.Categories, "CateId", "CateId", Food.Category);
            ViewData["ProducerId"] = new SelectList(_dbContext.Branch, "BranchId", "BranchId", Food.BranchId);
            
            return View(Food);
        }

        // GET: Food/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Food = await _dbContext.Foods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Food == null)
            {
                return NotFound();
            }

            return View(Food);
        }

        // POST: Food/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Food = await _dbContext.Foods.FindAsync(id);
            _dbContext.Foods.Remove(Food);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return _dbContext.Foods.Any(e => e.Id == id);
        }
    }
}
