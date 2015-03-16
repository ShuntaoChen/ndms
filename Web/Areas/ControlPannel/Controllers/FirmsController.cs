using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Modules.Core.Services;
using JiYun.Web.Controllers;

namespace Web.Areas.ControlPannel.Controllers
{
    public class FirmsController : BaseController
    {
                //基础配置表
        private readonly IMasterService _iMasterService;

        public FirmsController(IMasterService iMasterService)
        {

            _iMasterService = iMasterService;
         
        }

        //厂商管理列表打开画面
        // GET: /ControlPannel/Firms/

        public ActionResult Index(FormCollection form)
        {
            ViewBag.CsType = new SelectList(_iMasterService.GetDetailsByMasterName("厂商类型"), "Value", "Name", form["CsType"]);

            return View();
        }

        //厂商管理详细打开画面
        // GET: /ControlPannel/Firms/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }

        //厂商管理添加打开画面
        // GET: /ControlPannel/Firms/Create

        public ActionResult Create()
        {
            return View();
        }

        //厂商管理添加保存画面
        // POST: /ControlPannel/Firms/Create

        [HttpPost]
        public ActionResult DoCreate(FormCollection collection)
        {
            return SuccessMsg("ControlPannelFirmsIndex", "添加成功", "closeCurrent");
        }

        //厂商管理修改打开画面
        // GET: /ControlPannel/Firms/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //厂商管理修改保存画面
        // POST: /ControlPannel/Firms/Edit/5

        [HttpPost]
        public ActionResult DoEdit(int id, FormCollection collection)
        {
            return SuccessMsg("ControlPannelFirmsIndex", "修改成功", "closeCurrent");

        }

        //厂商管理删除画面
        // GET: /ControlPannel/Firms/Delete/5
 
        public ActionResult Delete(int id)
        {
            return SuccessMsg("ControlPannelFirmsIndex", "删除成功", "closeCurrent");
        }

    }
}
