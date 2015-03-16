using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;

namespace Web.Areas.DailyAffairs.Controllers
{
    public class MaintainCategoriesController : BaseController
    {
        //事件类型列表打开画面
        // GET: /DailyAffairs/MaintainCategories/

        public ActionResult Index()
        {
            return View();
        }

        //事件类型详细打开画面
        // GET: /DailyAffairs/MaintainCategories/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }

        //事件类型保存打开画面
        // GET: /DailyAffairs/MaintainCategories/Create

        public ActionResult Create()
        {
            return View();
        }

        //事件类型添加保存画面
        // POST: /DailyAffairs/MaintainCategories/Create

        [HttpPost]
        public ActionResult DoCreate(FormCollection collection)
        {
            return SuccessMsg("DailyAffairsMaintainCategoriesIndex", "添加成功", "closeCurrent");

        }

        //事件类型修改打开画面
        // GET: /DailyAffairs/MaintainCategories/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //事件类型修改保存画面
        // POST: /DailyAffairs/MaintainCategories/Edit/5

        [HttpPost]
        public ActionResult DoEdit(int id, FormCollection collection)
        {
            return SuccessMsg("DailyAffairsMaintainCategoriesIndex", "修改成功", "closeCurrent");

        }


        //事件类型删除保存
        // POST: /DailyAffairs/MaintainCategories/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            return SuccessMsg("DailyAffairsMaintainCategoriesIndex", "删除成功", "closeCurrent");

        }
    }
}
