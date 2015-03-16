using System.Web.Mvc;

namespace Web.Areas.JobArrangements
{
    public class JobArrangementsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "JobArrangements";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "JobArrangements_default",
                "JobArrangements/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
