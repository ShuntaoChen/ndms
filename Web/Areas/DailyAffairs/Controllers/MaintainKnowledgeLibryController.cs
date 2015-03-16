using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Modules.Core.Services;
using JiYun.Web.Controllers;
using WebDBService.Models;
using WebDBService.Services;

namespace Web.Areas.DailyAffairs.Controllers
{
    public class MaintainKnowledgeLibryController : BaseController
    {
        //基础配置表
        private readonly IMasterService _iMasterService;
        private readonly IMaintenanceKnowledgeService _iMaintenanceKnowledgeService;//知识库
        public MaintainKnowledgeLibryController(IMasterService iMasterService,IMaintenanceKnowledgeService iMaintenanceKnowledgeService)
        {
            _iMasterService = iMasterService;
            _iMaintenanceKnowledgeService = iMaintenanceKnowledgeService;
        }
        //知识管理列表打开画面
        // GET: /DailyAffairs/MaintainKnowledgeLibry/
        public ActionResult Index(FormCollection form)
        {
            ViewBag.ZsType = new SelectList(_iMasterService.GetDetailsByMasterName("知识库"), "Value", "Name", form["ZsType"]);
            ViewData.Model = _iMaintenanceKnowledgeService.GetList(form);
          //  ViewData.Model = _iMaintenanceKnowledgeService.GetAll();
            return View();
        }
         //知识管理转移打开画面
        // GET: /DailyAffairs/MaintainKnowledgeLibry/Details/5
        public ActionResult Transfer(string ids)
        {
            return View();
        }
        //知识管理转移保存画面
        // GET: /DailyAffairs/MaintainKnowledgeLibry/Details/5
        [HttpPost]
        public ActionResult DoTransfer(string ids)
        {
            return SuccessMsg("ControlPannelMotorRoomsIndex", "转移成功", "closeCurrent");
        }
        //知识管理详细打开画面
        // GET: /DailyAffairs/MaintainKnowledgeLibry/Details/5
        public ActionResult Detail(int id)
        {
            return View();
        }

        //知识管理添加打开画面
        // GET: /DailyAffairs/MaintainKnowledgeLibry/Create
        public ActionResult Create(FormCollection form)
        {
            ViewBag.ZsType = new SelectList(_iMasterService.GetDetailsByMasterName("知识库"), "Value", "Name", form["ZsType"]);

            return View();
        }
        //知识管理修改打开画面
        // GET: /DailyAffairs/MaintainKnowledgeLibry/Edit/5

        public ActionResult Edit(int id, FormCollection form)
        {
            ViewBag.ZsType = new SelectList(_iMasterService.GetDetailsByMasterName("知识库"), "Value", "Name", form["ZsType"]);

            return View();
        }
        //知识管理添加保存画面
        // POST: /DailyAffairs/MaintainKnowledgeLibry/Create

        [HttpPost]
        public ActionResult DoCreate(MaintenanceKnowledges Model)
        {
           // _iMaintenanceKnowledgeService.Create(Model);
            return SuccessMsg("ControlPannelMotorRoomsIndex", "添加成功", "closeCurrent");
        }



        //知识管理修改保存画面
        // POST: /DailyAffairs/MaintainKnowledgeLibry/Edit/5

        [HttpPost]
        public ActionResult DoEdit(int id)
        {
            return SuccessMsg("ControlPannelMotorRoomsIndex", "添加成功", "closeCurrent");

        }

        //知识管理删除画面
        // GET: /DailyAffairs/MaintainKnowledgeLibry/Delete/5
 
        public ActionResult Delete(int id)
        {
            return SuccessMsg("ControlPannelMotorRoomsIndex", "添加成功", "closeCurrent");
        }

        //
        // POST: /DailyAffairs/MaintainKnowledgeLibry/Delete/5

       
    }
}
