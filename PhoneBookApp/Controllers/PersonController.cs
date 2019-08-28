using PhoneBookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneBookApp.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        SourceManager sourceManager = new SourceManager();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Table(int id = 1, string filter = "", int pagination = 5)
        {
            ViewBag.Id = id;
            ViewBag.Filter = filter;
            ViewBag.Pagination = pagination;
            return PartialView("_Table", sourceManager.GetPhoneBookList(filter));
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(PersonModel personModel)
        {
            if (ModelState.IsValid)
            {
                sourceManager.Add(personModel);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            return View(sourceManager.GetById(id));
        }
        [HttpPost]
        public ActionResult Edit(PersonModel personModel)
        {
            if (ModelState.IsValid)
            {
                sourceManager.Update(personModel);
                //TempData["Update"] = $"{personModel.FirstName} {personModel.LastName}";
                 TempData["Update"] =personModel.FirstName +" "+ personModel.LastName;
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Remove(long id)
        {
            return View(sourceManager.GetById(id));
        }
        [HttpPost]
        public ActionResult Remove(PersonModel personModel)
        {
           // TempData["Remove"] = $"{personModel.FirstName} {personModel.LastName}";
             TempData["Remove"] =  "{personModel.FirstName} {personModel.LastName}";
            sourceManager.Delete(personModel.ID);
            return RedirectToAction("Index");
        }
        
    }
}