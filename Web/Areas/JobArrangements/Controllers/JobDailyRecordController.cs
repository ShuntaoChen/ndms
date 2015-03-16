using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;
using Web.Controllers;

namespace Web.Areas.JobArrangements.Controllers
{
    public class JobDailyRecordController : MyBaseController
    {
        //值班列表打开画面
        // GET: /JobArrangements/JobDailyRecord/

        public ActionResult Index()
        {
            return View();
        }

        //值班列表打开详细画面
        // GET: /JobArrangements/JobDailyRecord/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }

        //值班列表添加打开画面
        // GET: /JobArrangements/JobDailyRecord/Create

        public ActionResult Create()
        {
            return View();
        }

        //值班列表添加保存画面
        // POST: /JobArrangements/JobDailyRecord/Create

        [HttpPost]
        public ActionResult DoCreate(FormCollection collection)
        {
            return SuccessMsg("JobArrangementsJobDailyRecordIndex", "添加成功", "closeCurrent");

        }

        //值班列表修改打开画面
        // GET: /JobArrangements/JobDailyRecord/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();

        }

        //值班列表修改保存画面
        // POST: /JobArrangements/JobDailyRecord/Edit/5

        [HttpPost]
        public ActionResult DoEdit(int id, FormCollection collection)
        {
            return SuccessMsg("JobArrangementsJobDailyRecordIndex", "修改成功", "closeCurrent");

        }

        //值班列表删除画面
        // GET: /JobArrangements/JobDailyRecord/Delete/5
 
        public ActionResult Delete(int id)
        {
            return SuccessMsg("JobArrangementsJobDailyRecordIndex", "删除成功", "closeCurrent");
        }

    }
}
