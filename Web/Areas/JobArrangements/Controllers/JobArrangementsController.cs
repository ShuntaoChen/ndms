using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mdl = Web.Areas.JobArrangements.Models.JobArrangement;


namespace Web.Areas.JobArrangements.Controllers
{
    public class JobArrangementsController : Controller
    {
        //
        // GET: /JobArrangements/JobArrangements/


        public ActionResult Index()
        {
            Mdl m = new Mdl().Fill<Mdl>(Request);
            return View(m);
        }

        public ActionResult _Job()
        {
            return View();

        }

        
        public ActionResult _OnDuty()
        {
            return View();

        }

        //
        // GET: /JobArrangements/JobArrangements/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /JobArrangements/JobArrangements/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /JobArrangements/JobArrangements/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /JobArrangements/JobArrangements/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /JobArrangements/JobArrangements/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /JobArrangements/JobArrangements/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /JobArrangements/JobArrangements/Delete/5

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
    }
}
