using System.Web.Mvc;
using JiYun.Web.Controllers;

namespace Web.Controllers
{
    public class UserInfoController : BaseController
    {
        public ActionResult ExpandInfo()
        {
            return View();
        }

        public ActionResult SystemLog()
        {
            return View();
        }

    }
}
