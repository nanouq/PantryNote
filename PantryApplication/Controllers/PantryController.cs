using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryApplication.Data;
using PantryApplication.Models;

namespace PantryApplication.Controllers
{
    [Authorize]
    public class PantryController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        public PantryController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var userPantry = await _db.Pantry.FirstOrDefaultAsync(p => p.UserId == userId);

            var userItems = await _db.Item
                .Where(i => i.PantryId == userPantry.Id)
                .ToListAsync();

            return View(userItems);
        }

        public IActionResult Add() 
        {
            return View();
        }

        public IActionResult Perishable()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Perishable(PerishableItem obj)
        {
            var userId = _userManager.GetUserId(User);
            var pantry = await _db.Pantry.FirstOrDefaultAsync(p => p.UserId == userId);

            obj.PantryId = pantry.Id;
            obj.DateAdded = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.Item.Add(obj);
                _db.SaveChanges();
                TempData["successful"] = "New perishable item created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult NonPerishable()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NonPerishable(NonPerishableItem obj)
        {
            var userId = _userManager.GetUserId(User);
            var pantry = await _db.Pantry.FirstOrDefaultAsync(p => p.UserId == userId);

            obj.PantryId = pantry.Id;
            obj.DateAdded = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.Item.Add(obj);
                _db.SaveChanges();
                TempData["successful"] = "New non-perishable item created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var item = _db.Item.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            //check item type
            if (item is PerishableItem)
            {
                return RedirectToAction("EditPerishable", new { id = item.Id });
            }
            else if (item is NonPerishableItem)
            {
                return RedirectToAction("EditNonPerishable", new {id  = item.Id});
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditPerishable(int id)
        {
            var item = _db.Item.OfType<PerishableItem>().FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        public IActionResult EditPerishable(PerishableItem perishableItem)
        {
            if (ModelState.IsValid)
            {
                _db.Item.Update(perishableItem);
                _db.SaveChanges();
                TempData["successful"] = "Perishable item updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        [HttpGet]
        public IActionResult EditNonPerishable(int id)
        {
            var item = _db.Item.OfType<NonPerishableItem>().FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        public IActionResult EditNonPerishable(NonPerishableItem nonPerishableItem)
        {
            if (ModelState.IsValid)
            {
                _db.Item.Update(nonPerishableItem);
                _db.SaveChanges();
                TempData["successful"] = "Non-perishable item updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.Item.Find(id);
            if (item != null)
            {
                _db.Item.Remove(item);
                _db.SaveChanges();
                TempData["successful"] = "Item deleted successfully";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Search(string search)
        {
            var results = _db.Item
                .Where(item => item.Name.Contains(search))
                .ToList();

            ViewBag.Search = search;
            return View("SearchResults", results);
        }

        [HttpPost]
        public IActionResult Report(string reportType)
        {
            var userId = _userManager.GetUserId(User);
            var pantryId = _db.Pantry.FirstOrDefault(p => p.UserId ==  userId)?.Id;
            var today = DateTime.Now;
            var title = "";         
            if (pantryId == null)
            {
                return NotFound("Pantry not found for current user");
            }

            switch (reportType)
            {
                //items expiring within 30 days
                case "1":
                    var thirtyDaysFromNow = today.AddDays(30);

                    var expiringItems = _db.Item
                        .Where(item => item.PantryId == pantryId
                        && item.ExpirationDate != null
                        && item.ExpirationDate.Value >= today
                        && item.ExpirationDate.Value <= thirtyDaysFromNow)
                        .OrderBy(item => item.ExpirationDate)
                        .ToList();
                    title = "Items Expiring Soon Report";
                    var expiringColumns = new List<string> { "Item Name", "Quantity", "Unit", "Expiration Date" };
                    var expiringRows = expiringItems
                        .Select(item => new List<string>
                        {
                            item.Name,
                            item.GetFormattedQuantity(),
                            item.Unit,
                            item.ExpirationDate?.ToString("MM-dd-yyyy")
                        }).ToList();
                    return View(Tuple.Create(title, expiringColumns, expiringRows));
                //Count by category
                case "2":
                    var categoryCount = _db.Item
                        .Where(item => item.PantryId == pantryId)
                        .GroupBy(item => item.Category)
                        .Select(group => new
                        {
                            Category = group.Key,
                            NumberOfItems = group.Count(),
                            LastItemAdded = group.Max(item => item.DateAdded)
                        }).ToList();
                    title = "Category Count Report";
                    var categoryColumns = new List<string> { "Category", "Number of Items", "Date of Last Item Added" };
                    var categoryRows = categoryCount
                        .Select(group => new List<string>
                        {
                            group.Category,
                            group.NumberOfItems.ToString(),
                            group.LastItemAdded.ToString("MM-dd-yyyy")
                        }).ToList();
                    return View(Tuple.Create(title, categoryColumns, categoryRows));
                default:
                    TempData["error"] = "Invalid report type selected";
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        
        public IActionResult Details(int id)
        {
            var item = _db.Item.Find(id);

            if (item == null)
            {
                return NotFound();
            }   
            
            return View(item);
        }

    }
}
