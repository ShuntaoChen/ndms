using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;
using Web.Areas.Mes.Services;
using Web.Controllers;
using Web.Areas.Mes.Models;
using JiYun.Modules.Core.Services;

namespace Web.Areas.Mes.Controllers
{
    /// <summary>
    /// 我的日历
    /// </summary>
    public class CalendarController : MyBaseController
    {
        //
        // GET: /Calendar/
        #region 引用申明
        private readonly IScheduleService ds;
        private UserService us = new UserService();
        #endregion
        public CalendarController(IScheduleService IScheduleService)
        {
            ds = IScheduleService;
        }

        public ActionResult Index()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult Create(DateTime starttime, DateTime endtime)
        {
            ViewData["starttime"] = starttime.ToString("yyyy-MM-dd HH:mm");
            ViewData["endtime"] = endtime.ToString("yyyy-MM-dd HH:mm");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DoCreate(X_Schedules sch, FormCollection form)
        {
            if (string.IsNullOrEmpty(form["IsRemind"]))
                sch.IsRemind = 0;
            else
                sch.IsRemind = 1;
            sch.Color = form["Title_COLOR"];
            sch.UserID = GetUserInfo().UserID;
            sch.CreateDate = DateTime.Now;
            sch.LastRemaindTime = DateTime.Now;
            X_ScheduleDetails Detail = new X_ScheduleDetails();
            Detail.UserID = sch.UserID;

            Detail.DisplayName = GetUserInfo().Name;
            sch.X_ScheduleDetails.Add(Detail);
            ds.CreateScheduler(sch, string.IsNullOrEmpty(form["RemindRecyle"]) ? 0 : int.Parse(form["RemindRecyle"]), string.IsNullOrEmpty(form["RemindRecyle"]) ? 0 : int.Parse(form["RemindRecyle"]));
            return SuccessMsg("MesCalendarIndex");
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            X_Schedules sch = ds.GetByID(id);
            //判断该条记录是否可以进行修改
            if (sch.EndTime < DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:00"))
            {
                ViewBag.IsEdit = 0;
            }
            else
                ViewBag.IsEdit = 1;
            ViewData.Model = sch;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DoEdit(X_Schedules scheduler, FormCollection form)
        {
            if (string.IsNullOrEmpty(form["IsRemind"]))
                scheduler.IsRemind = 0;
            else
                scheduler.IsRemind = 1;
            scheduler.Color = form["Title_COLOR"];
            ds.Update(scheduler);
            return SuccessMsg("MesCalendarIndex");
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SchDelete(int id)
        {
            string uid = GetUserInfo().UserID.ToString();
            ds.DeleteScheduler(int.Parse(uid), id);
            return SuccessMsg("MesCalendarIndex");
        }
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            return View();
        }
        /// <summary>
        /// 加载日历中数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Load()
        {
            string uid = GetUserInfo().UserID.ToString();

            //JY.Web.Services.LogService log = new Services.LogService();
            //log.Log(JY.Web.Helpers.FormatString.TimestampToDateTime(start).ToString());
            //log.Log(JY.Web.Helpers.FormatString.TimestampToDateTime(end).ToString());
            var schedulers = ds.SearchSchedulers(int.Parse(uid));

            var schs = new List<Sch>();
            foreach (var s in schedulers)
            {
                Sch sch = new Sch();
                sch.id = s.ID;
                sch.title = ds.SubString(s.Title, 40);
                sch.content = s.Contents;
                sch.start = ((DateTime)s.StartTime).ToString("yyyy-MM-dd HH:mm:ss");
                sch.end = ((DateTime)s.EndTime).ToString("yyyy-MM-dd HH:mm:ss");
                sch.color = s.Color;
                schs.Add(sch);

            }

            return Json(schs);
        }
        public class Sch
        {
            public int id { get; set; }

            public string title { get; set; }

            public string start { get; set; }

            public string end { get; set; }

            public string content { get; set; }
            public string color { get; set; }

        }


    }
}
