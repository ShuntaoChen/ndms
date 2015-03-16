using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Modules.Core.Services;
using JiYun.Web.Controllers;

namespace Web.Areas.DailyAffairs.Controllers
{
    public class MaintenanceManagementController : BaseController
    {
          //基础配置表
        private readonly IMasterService _iMasterService;

        public MaintenanceManagementController(IMasterService iMasterService)
        {

            _iMasterService = iMasterService;
         
        }
        //维护管理打开页面
        // GET: /DailyAffairs/MaintenanceManagement/

        public ActionResult Index()
        {
            return View();
        }

        //维护管理详细打开页面
        // GET: /DailyAffairs/MaintenanceManagement/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }

        //维护管理添加打开页面
        // GET: /DailyAffairs/MaintenanceManagement/Create

        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult DoCrete()
        {
            return SuccessMsg("添加成功");
        }

        //维护管理添加保存页面
        // POST: /DailyAffairs/MaintenanceManagement/Create

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

        //维护管理修改打开页面
        // GET: /DailyAffairs/MaintenanceManagement/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }
        
        public ActionResult DoEdit()
        {
            return SuccessMsg("");
        }

        //维护管理修改保存页面
        // POST: /DailyAffairs/MaintenanceManagement/Edit/5

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

        //维护管理删除
        // GET: /DailyAffairs/MaintenanceManagement/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

     
    }
}
