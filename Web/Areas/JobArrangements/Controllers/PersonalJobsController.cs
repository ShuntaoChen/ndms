using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.JobArrangements.Controllers
{
    public class PersonalJobsController : Controller
    {
        //
        // GET: /JobArrangements/PersonalJobs/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 为了避免客户端Id冲突，临时采用此办法，看有没有更好的办法在实现的时候处理
        /// </summary>
        /// <returns></returns>
        public ActionResult SelfJob()
        {

            return View();
        }

        public ActionResult SelfJobList()
        {
            return View();
        }

    }
}
