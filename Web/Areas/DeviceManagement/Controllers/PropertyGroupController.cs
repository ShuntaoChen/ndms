using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;

namespace Web.Areas.DeviceManagement.Controllers
{
    public class PropertyGroupController : BaseController
    {
        //
        // GET: /DeviceManagement/PropertyGroup/

        public ActionResult Index()
        {

            return View();
        }

        //
        // GET: /DeviceManagement/PropertyGroup/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }

        //
        // GET: /DeviceManagement/PropertyGroup/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /DeviceManagement/PropertyGroup/Create

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
        // GET: /DeviceManagement/PropertyGroup/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /DeviceManagement/PropertyGroup/Edit/5

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
        // GET: /DeviceManagement/PropertyGroup/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DeviceManagement/PropertyGroup/Delete/5

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
