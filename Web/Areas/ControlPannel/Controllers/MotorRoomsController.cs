using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using JiYun.Modules.Core.Services;
using JiYun.Web.Controllers;

namespace Web.Areas.ControlPannel.Controllers
{
    public class MotorRoomsController : BaseController
    {
         //基础配置表
        private readonly IMasterService _iMasterService;

        public MotorRoomsController(IMasterService iMasterService)
        {

            _iMasterService = iMasterService;
         
        }
        //机房管理列表打开画面
        // GET: /ControlPannel/MotorRooms/

        public ActionResult Index(FormCollection form)
        {
            ViewBag.JfType = new SelectList(_iMasterService.GetDetailsByMasterName("机房类型"), "Value", "Name", form["JfType"]);

            return View();
        }

        //机房管理详细打开画面
        // GET: /ControlPannel/MotorRooms/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }

        //机房管理添加打开画面
        // GET: /ControlPannel/MotorRooms/Create

        public ActionResult Create(FormCollection form)
        {
            ViewBag.JfType = new SelectList(_iMasterService.GetDetailsByMasterName("机房类型"), "Value", "Name", form["JfType"]);

            return View();
        }

        //机房管理添加保存画面
        // POST: /ControlPannel/MotorRooms/Create

        [HttpPost]
        public ActionResult DoCreate(FormCollection collection)
        {
            return SuccessMsg("ControlPannelMotorRoomsIndex", "添加成功", "closeCurrent");
        }

        //机房管理修改打开画面
        // GET: /ControlPannel/MotorRooms/Edit/5
 
        public ActionResult Edit(int id ,FormCollection form)
        {
            ViewBag.JfType = new SelectList(_iMasterService.GetDetailsByMasterName("机房类型"), "Value", "Name", form["JfType"]);

            return View();
        }

        //机房管理修改保存画面
        // POST: /ControlPannel/MotorRooms/Edit/5

        [HttpPost]
        public ActionResult DoEdit(int id, FormCollection collection)
        {
            return SuccessMsg("ControlPannelMotorRoomsIndex", "修改成功", "closeCurrent");
        }

        //机房管理删除画面
        // GET: /ControlPannel/MotorRooms/Delete/5
 
        public ActionResult Delete(int id)
        {
            return SuccessMsg("ControlPannelMotorRoomsIndex", "删除成功", "closeCurrent");
        }

    }
}
