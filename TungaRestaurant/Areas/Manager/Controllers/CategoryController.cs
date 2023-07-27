using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TungaRestaurant.Data;
using TungaRestaurant.Models;

namespace TungaRestaurant.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly TungaRestaurantDbContext _dbContext;
        public CategoryController(TungaRestaurantDbContext context)
        {
            _dbContext = context;
        }
        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Categories.ToListAsync());
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Status")] Category category)
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
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images"));
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            category.Image = fileName;
                        }
                    }
                    _dbContext.Add(category);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.error = "Mã danh mục hoặc tên danh mục đã tồn tại!" + ex;
                    return View(category);
                }
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Status")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //upload image
                    var files = HttpContext.Request.Form.Files;
                    if (files != null && files[0].Length > 0)
                    {
                        var file = files[0];
                        var fileName = file.Name;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            category.Image = fileName;
                        }
                    }
                    _dbContext.Update(category);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _dbContext.Categories.Any(e => e.Id == id);
        }
    }
}
