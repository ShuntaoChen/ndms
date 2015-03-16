using System.Web.Mvc;

namespace Web.Areas.DataStastics
{
    public class DataStasticsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DataStastics";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DataStastics_default",
                "DataStastics/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
