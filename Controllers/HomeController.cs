using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
     
        // here we can "inject" our context service into the constructorcopy
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            List<Dish> AllDishes = dbContext.Dishes.ToList();
            ViewBag.Dishes = AllDishes;
            
            foreach(var x in AllDishes)
            {
                Console.WriteLine(x.Name);
            }
            return View(AllDishes);
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            return View("Create");
        }

        [HttpPost("Create")]
        public IActionResult Create(Dish newDish)
        {
            dbContext.Add(newDish);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            Console.WriteLine(id);
            HttpContext.Session.SetInt32("id", id);
            Dish Dish = dbContext.Dishes.FirstOrDefault(dish => dish.DishId == id);
            ViewBag.Dish = Dish;
            Console.WriteLine(Dish.Name);
            return View("Show", Dish);
        }

        [HttpGet("update/{id}")]
        public IActionResult Update(int id)
        {
            Dish DishToUpdate = dbContext.Dishes.FirstOrDefault(dish => dish.DishId == id);
            
            return View("Edit", DishToUpdate);
        }

        [HttpPost("edit")]
        public IActionResult Edit(Dish UpdatedDish)
        {
            int? id = HttpContext.Session.GetInt32("id");
            Dish DishToUpdate = dbContext.Dishes.FirstOrDefault(dish => dish.DishId == id);
            // Then we may modify properties of this tracked model object
            DishToUpdate.Name = UpdatedDish.Name;
            DishToUpdate.Chef = UpdatedDish.Chef;
            DishToUpdate.Tastiness = UpdatedDish.Tastiness;
            DishToUpdate.Calories = UpdatedDish.Calories;
            DishToUpdate.Description = UpdatedDish.Description;
            DishToUpdate.UpdatedAt = DateTime.Now;
            dbContext.SaveChanges();
            return Redirect($"{DishToUpdate.DishId}");
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
           
            Dish DishToDelete = dbContext.Dishes.SingleOrDefault(dish => dish.DishId == id);
            dbContext.Dishes.Remove(DishToDelete);
            
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

