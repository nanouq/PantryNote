using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryApplication.Data;
using PantryApplication.Models;

namespace PantryApplication.Controllers
{
    [Authorize]
    public class PantryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PantryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //Test database => on screen functionality
            //List<Pantry> objPantryList = _db.Pantry.ToList();
            List<Item> objItemList = _db.Item.ToList();
            return View(objItemList);
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
        public IActionResult Perishable(PerishableItem obj)
        {

            //hard codes pantry id as 1 for testing, remove this once user login is implemented
            obj.PantryId = 1;
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
        public IActionResult NonPerishable(NonPerishableItem obj)
        {
            //hard codes pantry id as 1 for testing, remove this once user login is implemented
            obj.PantryId = 1;
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
        public IActionResult EditNonPerishable(PerishableItem nonPerishableItem)
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
    }
}
