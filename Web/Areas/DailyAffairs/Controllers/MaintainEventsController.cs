using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Modules.Core.Services;
using JiYun.Web.Controllers;

namespace Web.Areas.DailyAffairs.Controllers
{
    public class MaintainEventsController : BaseController
    {
        //基础配置表
        private readonly IMasterService _iMasterService;

        public MaintainEventsController(IMasterService iMasterService)
        {

            _iMasterService = iMasterService;
         
        }
        //维护事件列表打开
        // GET: /DailyAffairs/MaintainEvents/

        public ActionResult Index(FormCollection form)
        {
            ViewBag.SjType = new SelectList(_iMasterService.GetDetailsByMasterName("事件维护"), "Value", "Name", form["SjType"]);
            return View();
        }

        //维护事件详细打开
        // GET: /DailyAffairs/MaintainEvents/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }

        //维护事件添加打开
        // GET: /DailyAffairs/MaintainEvents/Create

        public ActionResult Create(FormCollection form)
        {
            ViewBag.SjType = new SelectList(_iMasterService.GetDetailsByMasterName("事件维护"), "Value", "Name", form["SjType"]);

            return View();
        }

        //维护事件添加保存
        // POST: /DailyAffairs/MaintainEvents/Create

        [HttpPost]
        public ActionResult DoCreate(FormCollection collection)
        {
            return SuccessMsg("DailyAffairsMaintainEventsIndex", "添加成功", "closeCurrent");

        }

        //维护事件修改打开
        // GET: /DailyAffairs/MaintainEvents/Edit/5

        public ActionResult Edit(int id,FormCollection form)
        {
            ViewBag.SjType = new SelectList(_iMasterService.GetDetailsByMasterName("事件维护"), "Value", "Name", form["SjType"]);

            return View();
        }

        //维护事件修改保存
        // POST: /DailyAffairs/MaintainEvents/Edit/5

        [HttpPost]
        public ActionResult DoEdit(int id)
        {
            return SuccessMsg("DailyAffairsMaintainEventsIndex", "修改成功", "closeCurrent");

        }
        //维护事件删除
        // GET: /DailyAffairs/MaintainEvents/Delete/5
        public ActionResult Delete(int id)
        {
            return SuccessMsg("DailyAffairsMaintainEventsIndex", "删除成功", "closeCurrent");
        }

    }
}
