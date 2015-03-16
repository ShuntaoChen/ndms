using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Modules.Core.Services;
using JiYun.Web.Controllers;
using NPOI.SS.Formula.Functions;
using WebDBService.Services;

namespace Web.Areas.DailyAffairs.Controllers
{
    public class DailyJobController : BaseController
    {
           //基础配置表
        private readonly IMasterService _iMasterService;
       
        public DailyJobController(IMasterService iMasterService)
        {
            _iMasterService = iMasterService;
        }
        //日常工作列表打开
        // GET: /DailyAffairs/DailyJob/

        public ActionResult Index(FormCollection form)
        {
            ViewBag.RcType = new SelectList(_iMasterService.GetDetailsByMasterName("日常工作"), "Value", "Name", form["RcType"]);
            return View();
        }

        //日常工作详细打开
        // GET: /DailyAffairs/DailyJob/Details/5

        public ActionResult Detail(int id)
        {
            ViewBag.SjType = new SelectList(_iMasterService.GetDetailsByMasterName("事件维护"), "Value", "Name", Request["SjType"]);
            return View();
        }

        //日常工作添加打开
        // GET: /DailyAffairs/DailyJob/Create

        public ActionResult Create()
        {
            ViewBag.SjType = new SelectList(_iMasterService.GetDetailsByMasterName("事件维护"), "Value", "Name", "");//Request["SjType"]);
            return View();
        }
        //
        public ActionResult DoCreate()
        {
            return SuccessMsg("");
        }

        public ActionResult DoEndit()
        {
            return SuccessMsg("");
        }

        //日常工作添加保存
        // POST: /DailyAffairs/DailyJob/Create

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

        //日常工作修改打开
        // GET: /DailyAffairs/DailyJob/Edit/5
 
        public ActionResult Edit(int id)
        {
            ViewBag.SjType = new SelectList(_iMasterService.GetDetailsByMasterName("事件维护"), "Value", "Name", "");//Request["SjType"]);
            return View();
        }

        //日常工作修改保存
        // POST: /DailyAffairs/DailyJob/Edit/5

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

        //日常工作删除
        // GET: /DailyAffairs/DailyJob/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DailyAffairs/DailyJob/Delete/5


        public ActionResult SelectCompany()
        {
            ViewBag.RcType = new SelectList(_iMasterService.GetDetailsByMasterName("日常工作"), "Value", "Name", Request["RcType"]);

            return View();
        }
    }
}
