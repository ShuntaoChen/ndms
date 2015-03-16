using System.Web.Mvc;

namespace Web.Areas.Plugs
{
    public class PlugsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Plugs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Plugs_default",
                "Plugs/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
