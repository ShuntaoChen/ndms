using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Modules.Core.Services;
using JiYun.Web.Controllers;


namespace Web.Areas.DeviceManagement.Controllers
{
    public class AssetsManagementController : BaseController
    {
        //基础配置表
        private readonly IMasterService _iMasterService;

        public AssetsManagementController(IMasterService service )
        {
            _iMasterService = service;
        }

        //
        // GET: /DeviceManagement/AssetsManagement/

        public ActionResult Index()
        {
            ViewBag.AssetCategory = new SelectList(_iMasterService.GetDetailsByMasterName("资产类型"), "Value", "Name", Request["AssetCategory"]);
            ViewBag.AssetStatus = new SelectList(_iMasterService.GetDetailsByMasterName("资产状态"), "Value", "Name", Request["AssetStatus"]);
            
            return View();
        }

        //
        // GET: /DeviceManagement/AssetsManagement/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }

        //
        // GET: /DeviceManagement/AssetsManagement/Create

        public ActionResult Create()
        {
            ViewBag.AssetCategory = new SelectList(_iMasterService.GetDetailsByMasterName("资产类型"), "Value", "Name", Request["AssetCategory"]);
            return View();
        } 

        //
        // POST: /DeviceManagement/AssetsManagement/Create

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
        // GET: /DeviceManagement/AssetsManagement/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /DeviceManagement/AssetsManagement/Edit/5

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
        // GET: /DeviceManagement/AssetsManagement/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DeviceManagement/AssetsManagement/Delete/5

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

        public ActionResult ResourceSharing()
        {
            return View();
        }
    }
}
