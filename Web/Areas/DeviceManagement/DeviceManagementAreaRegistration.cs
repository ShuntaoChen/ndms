using System.Web.Mvc;

namespace Web.Areas.DeviceManagement
{
    public class DeviceManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DeviceManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DeviceManagement_default",
                "DeviceManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
