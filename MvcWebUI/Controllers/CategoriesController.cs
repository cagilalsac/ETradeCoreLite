#nullable disable
using DataAccess.Entities;
using DataAccess.Services.CRUD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _038_ETradeCoreLite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        // Add service injections here
        private readonly CategoryServiceBase _categoryService;

        public CategoriesController(CategoryServiceBase categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Category> categoryList = _categoryService.GetList();
            return View("List", categoryList);
        }

        // GET: Categories/Details/5
        public IActionResult Details(int id)
        {
            Category category = _categoryService.GetItem(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryService.Add(category);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int id)
        {
            Category category = _categoryService.GetItem(id);
            if (category == null)
            {
                return NotFound();
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(category);
        }

        // POST: Categories/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryService.Update(category);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            // Add get related items service logic here to set ViewData if necessary and update null parameter in SelectList with these items
            return View(category);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int id)
        {
            Category category = _categoryService.GetItem(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _categoryService.Delete(c => c.Id == id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
	}
}
