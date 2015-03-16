using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;
using Web.Areas.Mes.Services;
using Web.Controllers;
using JiYun.Modules.Core.Services;
using Web.Areas.Mes.Models;

namespace Web.Areas.Mes.Controllers
{
    public class BulletinController : MyBaseController
    {
        private readonly IDeptService _deptService;
        private readonly IUserService _userService;
        private readonly INoticeService _noticeService;
        private readonly IAttachmentService _attachmentService;
        public BulletinController(IDeptService deptService, INoticeService noticeService,IAttachmentService attachmentService,
            IUserService IUserService)
        {
            _deptService = deptService;
            _noticeService = noticeService;
            _attachmentService = attachmentService;
            _userService = IUserService;
        }

        public ActionResult Index(FormCollection forms)
        {
            ViewData.Model = _noticeService.GetList(forms);

            return View();
        }

        public ActionResult Create()
        {
            ViewData["Depts"] = _deptService.GetAllDepts().OrderBy(p => p.Parent_ID).ToList();
            ViewData["Users"] = _userService.GetAllUsers().ToList();
           
            return View();
        }
        [HttpPost]
        public JsonResult DoCreate(X_Notice model, string[] ledids)
        {
            model.Sender = GetUserInfo().Name;
            model.CreateOn = DateTime.Now;
            _noticeService.Create(model);

            if (ledids.Any())
            {
                var list = new List<X_NoticeDetail>();
                var userList = _userService.GetAllUsers().ToList();
                foreach (var item in ledids)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    int id = int.Parse(item);
                    var user = userList.FirstOrDefault(u => u.ID == id);
                    if (user == null) continue;
                    var not = new X_NoticeDetail();
                    not.UserID = user.ID;
                    not.UName = user.Name;
                    not.IsRead = false;
                    not.X_Notice_ID = model.ID;
                    if (user.S_DeptS_User.Any())
                    {
                        not.DeptID = user.S_DeptS_User.First().ID;
                        not.DeptName = user.S_DeptS_User.First().S_Dept.Name;
                    }
                    list.Add(not);

                }
                _noticeService.Create(list);
            }

            return SuccessMsg("MesBulletinIndex");
        }

        public ActionResult Edit(int id)
        {
            var model = _noticeService.GetById(id);
            ViewData.Model = model;
            ViewData["Depts"] = _deptService.GetAllDepts().OrderBy(p => p.Parent_ID).ToList();
            ViewData["Users"] = _userService.GetAllUsers().ToList();

            return View();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ledids"></param>
        /// <param name="DeleteIds">删除的明细</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DoEdit(X_Notice model, string[] ledids, string DeleteIds)
        {
            model.Sender = GetUserInfo().Name;
            _noticeService.Update(model);
            var ndIds = _noticeService.GetByIdNoticeDetail(model.ID);
            //全删
            List<string> delelist= DeleteIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(); //里面保存的用户编号
            List<int> deleleidlist = ndIds.Where(u => delelist.Contains(u.UserID.ToString())).Select(u => u.ID).ToList();
            _noticeService.DeleteListDetail(deleleidlist);

            //重新插入
            if (ledids.Any())
            {
                var list = new List<X_NoticeDetail>();
                var userList = _userService.GetAllUsers().ToList();
                foreach (var item in ledids)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    int id = int.Parse(item);
                    if (ndIds.Any(u => u.UserID == id) == false)
                    {
                        var user = userList.FirstOrDefault(u => u.ID == id);
                        if (user == null) continue;
                        var not = new X_NoticeDetail();
                        not.UserID = user.ID;
                        not.UName = user.Name;
                        not.IsRead = false;
                        not.X_Notice_ID = model.ID;
                        if (user.S_DeptS_User.Any())
                        {
                            not.DeptID = user.S_DeptS_User.First().S_Dept_ID;
                            not.DeptName = user.S_DeptS_User.First().S_Dept.Name;
                        }
                        list.Add(not);
                    }
                }
                _noticeService.Create(list);
            }

            return SuccessMsg("MesBulletinIndex");
        }

        public JsonResult Delete(string ids)
        {
            var pIds = ids.Split(',').ToList();
            var idlist = new List<int>();
            pIds.ForEach(f => idlist.Add(int.Parse(f)));
            _noticeService.DeleteList(idlist);

            return SuccessMsg("MesBulletinIndex");
        }
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">区分是单独的查看画面还是签收画面</param>
        /// <returns></returns>
        public ActionResult Detail(int id, string type)
        {
            var model = _noticeService.GetById(id);
            ViewData.Model = model;
            ViewBag.AttachmentIDs = _attachmentService.GetByAttachmentID(model.AttachmentID).ToList();
            ViewBag.Type = type == null ? "" : type;
            ViewBag.Content = "";
            if (type == "1")
            {
                var details = model.X_NoticeDetail.Where(u => u.UserID == GetCurrentUserID());
                if (details.Any())
                {
                    ViewBag.Content = details.First().ReadRemark;
                    foreach (var detail in details)
                    {
                        detail.IsRead = true;
                        detail.ReadTime = DateTime.Now;
                        _noticeService.Update(detail);
                    }
                }
            }
            return View();
        }
        /// <summary>
        /// 我的通知公告
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        public ActionResult NoticIndex(FormCollection forms)
        {
            ViewData.Model = _noticeService.GetList(forms,GetCurrentUserID());
            ViewBag.UserID = GetCurrentUserID();
            return View();
        }
        /// <summary>
        /// 签收
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult Sign(int id, string Content)
        {
            var model = _noticeService.GetById(id);
            var details = model.X_NoticeDetail.Where(u => u.UserID == GetCurrentUserID());
            if (details.Any())
            {
                foreach (var detail in details)
                {
                    detail.IsRead = true;
                    detail.ReadRemark = Content;
                    _noticeService.Update(detail);
                }
            }
            return SuccessMsg("MesBulletinDetail");
        }

        public ActionResult SignStatus(int id)
        {
            ViewData.Model = _noticeService.GetById(id);

            return View();
        }
        /// <summary>
        /// 删除当前登录用户的
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteDetail(string ids)
        {
            string[] arr = ids.Split(',');
            foreach (string str in arr)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    int _id = int.Parse(str);
                    var model = _noticeService.GetById(_id);
                    var details = model.X_NoticeDetail.Where(u => u.UserID == GetCurrentUserID());
                    if (details.Any())
                    {
                        foreach (var detail in details)
                        {
                            detail.IsDelete = true;
                            _noticeService.Update(detail);
                        }
                    }
                }
            }
            return SuccessMsg("MesBulletinNoticIndex");
        }
    }
}
