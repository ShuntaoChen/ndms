using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;

namespace Web.Areas.DeviceManagement.Controllers
{
    public class PropertiesController : BaseController
    {
        //
        // GET: /DeviceManagement/Properties/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /DeviceManagement/Properties/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }

        //
        // GET: /DeviceManagement/Properties/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /DeviceManagement/Properties/Create

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
        // GET: /DeviceManagement/Properties/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /DeviceManagement/Properties/Edit/5

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
        // GET: /DeviceManagement/Properties/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DeviceManagement/Properties/Delete/5

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
