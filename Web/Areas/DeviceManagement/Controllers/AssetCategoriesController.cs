using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;

namespace Web.Areas.DeviceManagement.Controllers
{
    public class AssetCategoriesController : BaseController
    {
        //
        // GET: /DeviceManagement/AssetCategories/

        public ActionResult Index()
        {
            ViewData["Header"] = "1";
            return View();
        }

        //
        // GET: /DeviceManagement/AssetCategories/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }

        //
        // GET: /DeviceManagement/AssetCategories/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /DeviceManagement/AssetCategories/Create

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
        // GET: /DeviceManagement/AssetCategories/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /DeviceManagement/AssetCategories/Edit/5

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
        // GET: /DeviceManagement/AssetCategories/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DeviceManagement/AssetCategories/Delete/5

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
