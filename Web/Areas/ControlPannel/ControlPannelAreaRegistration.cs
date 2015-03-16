using System.Web.Mvc;

namespace Web.Areas.ControlPannel
{
    public class ControlPannelAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ControlPannel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ControlPannel_default",
                "ControlPannel/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
