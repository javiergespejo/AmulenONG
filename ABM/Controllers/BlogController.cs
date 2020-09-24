using ABM.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABM.Controllers
{
    public class BlogController : Controller
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Blog
        public ActionResult Index()
        {
            IEnumerable<Models.post> posts = unitOfWork.PostRepository.Get(includeProperties: "category");
            return View(posts.ToList());
        }

        // GET: Blog/Details/5
        public ActionResult Details(int id)
        {
            Models.post posts = unitOfWork.PostRepository.GetByID(id);
            return View(posts);
        }

        // GET: Blog/Create
        public ActionResult Create()
        {
            CategorysDropDownList();
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.post model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.PostRepository.Insert(model);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            CategorysDropDownList(model.id_category);
            return View(model);
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int id)
        {
            Models.post posts = unitOfWork.PostRepository.GetByID(id);
            CategorysDropDownList(posts.id_category);
            return View(posts);
        }

        // POST: Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.post posts)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.PostRepository.Update(posts);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            CategorysDropDownList(posts.id_category);
            return View(posts);
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int id)
        {
            Models.post posts = unitOfWork.PostRepository.GetByID(id);
            posts.is_active = false;
            unitOfWork.PostRepository.SoftDelete(id);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        // POST: Blog/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void CategorysDropDownList(object selectedCategory = null)
        {
            var categorysQuery = unitOfWork.CategoryRepository.Get(
                orderBy: q => q.OrderBy(d => d.name));
            ViewBag.DepartmentID = new SelectList(categorysQuery, "id", "name", selectedCategory);
        }
    
    }
}
