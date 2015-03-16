using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Modules.Core.Services;
using JiYun.Web.Controllers;

namespace Web.Areas.DailyAffairs.Controllers
{
    public class ExecutePlanController : BaseController
    {
        private IMasterService _iMasterService;
        public ExecutePlanController(IMasterService iMasterService)
        {
            _iMasterService = iMasterService;
        }

        //维护记录列表 打开画面
        // GET: /DailyAffairs/ExecutePlan/

        public ActionResult Index()
        {
            return View();
        }

        //维护记录详细打开画面
        // GET: /DailyAffairs/ExecutePlan/Details/5

        public ActionResult Detail(int id)
        {
            ViewBag.SjType = new SelectList(_iMasterService.GetDetailsByMasterName("事件维护"), "Value", "Name", Request["SjType"]);

            return View();
        }

        //维护记录添加打开画面
        // GET: /DailyAffairs/ExecutePlan/Create

        public ActionResult Create()
        {
            ViewBag.SjType = new SelectList(_iMasterService.GetDetailsByMasterName("事件维护"), "Value", "Name", Request["SjType"]);
            return View();
        }

        //维护记录添加保存画面
        // POST: /DailyAffairs/ExecutePlan/Create

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

        //维护记录修改打开画面
        // GET: /DailyAffairs/ExecutePlan/Edit/5

        public ActionResult Edit(int id)
        {
            ViewBag.SjType = new SelectList(_iMasterService.GetDetailsByMasterName("事件维护"), "Value", "Name", Request["SjType"]);

            return View();
        }

        public ActionResult DoEdit()
        {
            return SuccessMsg("");
        }
        //维护记录修改保存画面
        // POST: /DailyAffairs/ExecutePlan/Edit/5

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

        //维护记录删除
        // GET: /DailyAffairs/ExecutePlan/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }


    }
}
