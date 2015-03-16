using System.Web.Mvc;

namespace Web.Areas.DutyLogs
{
    public class DutyLogsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DutyLogs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DutyLogs_default",
                "DutyLogs/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
