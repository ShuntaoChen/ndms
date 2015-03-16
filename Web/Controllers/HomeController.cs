using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using JiYun.Common.Sessions;
using JiYun.Modules.Core.Models;
using JiYun.Modules.Core.Services;
using JiYun.Plugins.Highcharts;
using JiYun.Web.Filters;
using JiYun.Web.Models;
using JiYun.Web.Services;
using log4net;
using System.Web;
using JiYun.Common.Utils;
using System.Security.Cryptography;
using System.Text;
using NPOI.SS.Formula.Functions;

using JiYun.Plugins.Highcharts.Options;
using JiYun.Plugins.Highcharts.Helpers;
namespace Web.Controllers
{
    [CompressFilter]
    [LogFilter]
    public class HomeController : Controller
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;
        private readonly IMasterService _masterService;
        private readonly IDeptService _deptService;
      

        public HomeController( IPermissionService permissionService, IUserService userService, IMasterService masterService, IDeptService deptService)
        {
            _permissionService = permissionService;
            _userService = userService;
            _masterService = masterService;
            _deptService = deptService;
          

        }

        public ActionResult Index()
        {
            var userSession = Session[SessionUtil.LoginUserKey] as UserSession;

            //没有登录，跳转到登录页面，由于这一页特殊性，所以单独处理，其他页面用AuthFilter处理。
            if (userSession == null)
            {
                return RedirectToAction("Login", "Home");
            }


            ViewBag.Permissions = userSession.Permissions;
            ViewBag.Name = userSession.Name;
            ViewBag.LastLoginTime = userSession.LastLoginTime;

            var user = _userService.GetById(userSession.UserID);
            if (user != null)
            {
                ViewBag.S_Role = string.Join(" ", user.S_Role.Take(3).Select(r => r.Name).ToArray());

            }
         
            return View();
        }

        public ActionResult Main()
        {
            var userSession = Session[SessionUtil.LoginUserKey] as UserSession;

            #region 1.待办任务

       
            #endregion
           
            return View();
        }

        public ActionResult SidebarMenu(int id)
        {
            var userSession = Session[SessionUtil.LoginUserKey] as UserSession;
            ViewBag.Permissions = userSession.Permissions;
            ViewData.Model = userSession.Permissions.Where(p => p.Parent_ID == id && (p.Type == "Controller" || p.Type == "Folder")).ToList();
            return View();
        }
        public ActionResult Login()
        {
            //是否启用验证码
            ViewBag.ValidateCode = ConfigHelper.GetSystemConfig("validateCode");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //防止外部提交表单。
        public ActionResult DoLogin(string username, string password, string validateCode)
        {
            try
            {

                if (ConfigHelper.GetSystemConfig("validateCode") == "1")
                {
                    string code = TempData["SecurityCode"] as string;
                    if (!code.Equals(validateCode, StringComparison.OrdinalIgnoreCase) && !code.Equals("nothing"))
                    {
                        TempData["msg"] = "登录失败！验证码错误！";
                        return RedirectToAction("Login", "Home");
                    }
                }

                DateTime lastLoginTime;
                var user = _userService.Login(username, password, out lastLoginTime);

                if (user == null)
                {
                    _logger.Error("[" + username + "]登陆失败！");
                    TempData["msg"] = "登录失败！用户名密码错误！";
                    return RedirectToAction("Login", "Home");
                }
                if (!user.Status)
                {
                    _logger.Error("[" + username + "]停用账户登陆！");
                    TempData["msg"] = "对不起！账户已停用！";
                    return RedirectToAction("Login", "Home");
                }
                var us = new UserSession
                    {
                        SessionID = Session.SessionID,
                        UserID = user.ID,
                        Name = user.Name,
                        Number = user.Account,
                        LastLoginTime = lastLoginTime,
                        Roles = user.S_Role.Select(t => t.Name).ToArray(),
                        Permissions = new List<Permission>()
                    };

                if (user.Account == "root")
                {
                    us.UserType = "super";
                    var permissions =
                        _permissionService.GetAllPermissions().ToList();
                    foreach (var perm in permissions)
                    {
                        us.Permissions.Add(
                            Mapper.Map<S_Permission, Permission>(perm));
                    }

                }
                else
                {
                    us.UserType = "farmer";
                    var roles = user.S_Role.ToList();
                    var permissions =
                        new List<S_Permission>();
                    permissions = roles.Aggregate(permissions, (current, role) => current.Union(role.S_Permission).ToList());
                    foreach (var perm in permissions)
                    {
                        us.Permissions.Add(
                            Mapper.Map<S_Permission, Permission>(perm));
                    }
                }

                Session[SessionUtil.LoginUserKey] = us; //用户信息
                Session[SessionUtil.LoginUserIdKey] = us.UserID;
                Session[SessionUtil.UsersKey] = _userService.GetAllUsersWithRoot().ToDictionary(k => k.ID, v => v.Name);

                //刷新在线用户
                UpdateOnlineUser();

                _logger.Info("【" + us.Name + " " + us.Number + "】登陆成功！");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                TempData["msg"] = "对不起！出现异常，请联系管理员！";
                return RedirectToAction("Login", "Home");
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            var session = Session[SessionUtil.LoginUserKey] as UserSession;
            if (session != null)
            {
                var onlineUsers = HttpRuntime.Cache["onlineUsers"] as List<List<string>>;

                //移除用户
                for (int i = 0; i < onlineUsers.Count; i++)
                {
                    if (session.UserID.ToString() == onlineUsers[i][0])
                    {
                        onlineUsers.Remove(onlineUsers[i]);
                        break;
                    }
                }

                _logger.Info("【" + session.Name + " " + session.Number + "】退出！");
            }
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <returns></returns>
        public ActionResult Config()
        {
            var session = Session[SessionUtil.LoginUserKey] as UserSession;
            if (session != null)
            {
                var user = _userService.GetById(session.UserID);
                ViewData.Model = user;
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public JsonResult Config(S_User user, string oldPassword, string newPassword)
        {
            var model = _userService.GetById(user.ID);

            if (model == null)
            {
                return Json(
                    new
                    {
                        statusCode = "200",
                        message = "操作失败！用户不存在！"
                    }
                );
            }

            if (!string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(newPassword))
            {
                if (JiYun.Common.Utils.Utils.MD5Decrypt(model.Password) != oldPassword)
                {
                    return Json(
                        new
                        {
                            statusCode = "200",
                            message = "操作失败！原密码错误！"
                        }
                    );
                }

                model.Password = JiYun.Common.Utils.Utils.MD5Encrypt(newPassword);

            }
            //chy 20140904 del  user为空，把Name清空了，再次修改的时候会报错。
            //model.Name = user.Name;

            _userService.Update(model);
            return Json(
                new
                {
                    statusCode = "200",
                    message = "操作成功！"
                }
            );
        }

        #region 在线人员
        //检索最新在线人数
        public JsonResult OnlineCount()
        {

            return Json(UpdateOnlineUser());
        }


        /// <summary>
        /// 在线人员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineList(FormCollection formCollection)
        {
            UserService us = new UserService();

            var list = HttpRuntime.Cache["onlineUsers"] as List<List<string>>;
            List<int> uids = new List<int>();
            foreach (var item in list.Select(l => l[0]).ToList())
            {
                uids.Add(int.Parse(item));
            }
            ViewData.Model = us.GetOnlineList(formCollection, uids);
            return View();
        }


        /// <summary>
        /// 更新在线用户
        /// </summary>
        private int UpdateOnlineUser()
        {
            var onlineUsers = HttpRuntime.Cache["onlineUsers"] as List<List<string>>;
            var userSession = Session[SessionUtil.LoginUserKey] as UserSession;


            if (userSession != null)
            {

                if (onlineUsers == null)
                {
                    onlineUsers = new List<List<string>> { };
                }

                //移除掉线的用户（判断依据时间大于20秒）
                for (int i = 0; i < onlineUsers.Count; i++)
                {

                    if (Utils.StrDateDiffSeconds(onlineUsers[i][2].ToString(), 0) > 30)
                    {
                        onlineUsers.Remove(onlineUsers[i]);
                    }
                }

                if (onlineUsers.Where(o => o[0] == userSession.UserID.ToString()).Count() == 0)
                {
                    onlineUsers.Add(new List<string> { userSession.UserID.ToString(), userSession.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });

                    HttpRuntime.Cache.Insert("onlineUsers", onlineUsers, null, DateTime.MaxValue, TimeSpan.Zero);
                }

                return onlineUsers.Count();

            }

            return 0;
        }
        #endregion

        #region 验证码
        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult SecurityCode()
        {
            string oldcode = TempData["SecurityCode"] as string;
            string code = Utils.CreateRandomCode(4);
            TempData["SecurityCode"] = code;
            return File(Utils.CreateValidateGraphic(code), "image/Jpeg");
        }
        #endregion

        #region 代办任务
        /// <summary>
        /// 代办任务
        /// </summary>

        public ActionResult IndexExaminePersonList()
        {


            //1.1 根据登录人的角色显示



            return View();
        }

        #endregion
        


    }
}
