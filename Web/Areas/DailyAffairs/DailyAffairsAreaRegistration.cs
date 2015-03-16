using System.Web.Mvc;

namespace Web.Areas.DailyAffairs
{
    public class DailyAffairsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DailyAffairs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DailyAffairs_default",
                "DailyAffairs/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
