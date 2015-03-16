using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;
using NPOI.SS.Formula.Functions;

namespace Web.Areas.DailyAffairs.Controllers
{
    public class PlanListController : BaseController
    {
        //计划管理列表打开画面
        // GET: /DailyAffairs/PlanList/
        
        public ActionResult Index()
        {
            return View();
        }

        //计划管理详细打开画面
        // GET: /DailyAffairs/PlanList/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }

        //计划管理添加打开画面
        // GET: /DailyAffairs/PlanList/Create

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult DoCreate()
        {
            return SuccessMsg("");
        }

        //计划管理添加保存画面
        // POST: /DailyAffairs/PlanList/Create

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

        //计划管理修改打开画面
        // GET: /DailyAffairs/PlanList/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }
        /// <summary>
        /// 计划管理修改
        /// </summary>
        /// <returns></returns>
        public ActionResult DoEdit()
        {
            return SuccessMsg("");
        }
        //计划管理修改保存画面
        // POST: /DailyAffairs/PlanList/Edit/5

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

        //计划管理删除
        // GET: /DailyAffairs/PlanList/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }


        public ActionResult SelectDoer()
        {
            return View();
        }

        public ActionResult SelectSupervisor()
        {
            return View();
        }
    }
}
