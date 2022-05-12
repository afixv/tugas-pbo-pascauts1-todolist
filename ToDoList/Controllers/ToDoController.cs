using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext contex;

        public ToDoController(ToDoContext contex)
        {
            this.contex = contex;
        }

        //Primary
         public async Task<ActionResult> Index()
        {
            IQueryable<TodoList> items = (IQueryable<TodoList>)(from i in contex.ToDoList orderby i.Id select i);
            List<TodoList> todoList = await items.ToListAsync();
            return View(todoList);
        }

        //Create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoList item)
        {
            if (ModelState.IsValid)
            {
                contex.Add(item);
                await contex.SaveChangesAsync();

                TempData["Success"] = "The item has been Added!";

                return RedirectToAction("Index");
            }
            return View(item);
        }

        //Edit
        public async Task<ActionResult> Edit(int id)
        {
            TodoList item = await contex.ToDoList.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                contex.Update(item);
                await contex.SaveChangesAsync();

                TempData["Success"] = "The item has been Updated!";

                return RedirectToAction("Index");
            }
            return View(item);
        }

        // Delete
        public async Task<ActionResult> Delete(int id)
        {
            TodoList item = await contex.ToDoList.FindAsync(id);
            if (item == null)
            {
                TempData["Error"] = "The item doesn't exist!";
            } 
            else
            {
                contex.ToDoList.Remove(item);
                await contex.SaveChangesAsync();

                TempData["Success"] = "The item has been deleted!";
            }
            return RedirectToAction("index");
        }
    }
}
