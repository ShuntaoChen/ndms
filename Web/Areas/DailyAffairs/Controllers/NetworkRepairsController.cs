using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;

namespace Web.Areas.DailyAffairs.Controllers
{
    public class NetworkRepairsController : BaseController
    {
        //网络报修列表打开页面
        // GET: /DailyAffairs/NetworkRepairs/

        public ActionResult Index()
        {
            return View();
        }
        //领导审批列表打开页面
        // GET: /DailyAffairs/NetworkRepairs/
        public ActionResult LeaderIndex()
        {
            #region 1.页面处理
            // 影藏页面工具条
            ViewData["ToolBar"] = "1";
            #endregion
            return View();
        }
        

        //网络报修详细打开页面
        // GET: /DailyAffairs/NetworkRepairs/Details/5

        public ActionResult Detail(int id)
        {
            return View();
        }
         //领导审批打开页面
        // GET: /DailyAffairs/NetworkRepairs/LeaderDetail/5

        public ActionResult LeaderDetail(int id)
        {
            return View();
        }

        //领导审批打开页面
        // GET: /DailyAffairs/NetworkRepairs/LeaderDetail/5

        public ActionResult DoLeaderDetail(int id)
        {
            return SuccessMsg("DailyAffairsNetworkRepairsLeaderIndex", "审核成功", "closeCurrent");
        }
        //网络报修添加打开页面
        // GET: /DailyAffairs/NetworkRepairs/Create

        public ActionResult Create()
        {


            return View();
        }

        //网络报修添加保存页面
        // POST: /DailyAffairs/NetworkRepairs/Create

        [HttpPost]
        public ActionResult DoCreate(FormCollection collection)
        {
            return SuccessMsg("DailyAffairsNetworkRepairsIndex", "添加成功", "closeCurrent");

        }

        //网络报修修改打开页面
        // GET: /DailyAffairs/NetworkRepairs/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //网络报修修改保存页面
        // POST: /DailyAffairs/NetworkRepairs/Edit/5

        [HttpPost]
        public ActionResult DoEdit(int id, FormCollection collection)
        {
            return SuccessMsg("DailyAffairsNetworkRepairsIndex", "修改成功", "closeCurrent");

        }

        //网络报修删除
        // GET: /DailyAffairs/NetworkRepairs/Delete/5
 
        public ActionResult Delete(int id)
        {
            return SuccessMsg("DailyAffairsNetworkRepairsIndex", "删除成功", "closeCurrent");
        }



      
    }
}
