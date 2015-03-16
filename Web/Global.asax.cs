using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Reflection;
using AutoMapper;
using System.Data.Entity;
using Autofac;
using Autofac.Integration.Mvc;
using JiYun.Modules.Core.Data;
using JiYun.Web.Services;
using log4net;

namespace Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
                );

        }

        protected void Application_Start()
        {
            try
            {

                //log4net
                log4net.Config.XmlConfigurator.Configure();

                //autofac
                var builder = RegisterService();
                DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));

                //automapper
                Mapper.CreateMap<JiYun.Modules.Core.Models.S_Permission, JiYun.Web.Models.Permission>();

                //dbcontext
                Database.SetInitializer<CoreContext>(null);

                AreaRegistration.RegisterAllAreas();

                RegisterGlobalFilters(GlobalFilters.Filters);
                RegisterRoutes(RouteTable.Routes);

                //打包压缩js和css
                RegisterBundles(BundleTable.Bundles);

            }
            catch (Exception exception)
            {
                // 记录注入失败的详细错误
                var logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                if (exception is ReflectionTypeLoadException)
                {
                    var typeLoadException = exception as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                    logger.Error(new AggregateException(typeLoadException.Message, loaderExceptions));
                }
                logger.Error(exception);
            }
        }


        /// <summary>
        /// Autofac自动注入
        /// </summary>
        /// <returns></returns>
        private static ContainerBuilder RegisterService()
        {
            var builder = new ContainerBuilder();

            // 注入所有以Service结尾并继承IDependency的所有接口
            var baseType = typeof(IDependency);

            #region MyRegion

            /* 排除该程序集
             * 报System.TypeLoadException错: 
             * 程序集“Microsoft.Web.Mvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null”中的类型
             * “Microsoft.Web.Mvc.MvcDynamicSessionControllerFactory”的方法“GetControllerSessionBehavior”没有实现。 */
            //var exceptAssemblys = new List<Assembly>
            //    {
            //        Assembly.Load("Microsoft.Web.Mvc")
            //    };
            //var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToList().Except(exceptAssemblys).ToArray();
            #endregion

            var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToArray();
            builder.RegisterControllers(assemblys);
            builder.RegisterAssemblyTypes(assemblys)
                   .Where(type => baseType.IsAssignableFrom(type) && type != baseType && type.Name.EndsWith("Service"))
                   .AsImplementedInterfaces();
                   //.SingleInstance();

            return builder;
        }

        /// <summary>
        /// js/css资源打包
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterBundles(BundleCollection bundles)
        {
            /* 默认排除规则
             * bundles.IgnoreList 默认包含以下项目，以下项目添加之后自动被忽略；
             * Pattern    ".intellisense.js"    string
             * Pattern    "-vsdoc.js"    string
             * Pattern    ".debug.js"    string
             * Pattern    ".min.js"    string
             * Pattern    ".min.css"    string
             * 自定义
             * bundles.IgnoreList.Ignore("*-vsdoc.js");
             */
            // 清空默认排除规则
            bundles.IgnoreList.Clear();
            bundles.Clear();

            //打包文件夹下面的所有js文件
            //bundles.Add(new ScriptBundle("~/JYScripts").IncludeDirectory("~/Scripts/", "*.js", true));

            bundles.Add(new ScriptBundle("~/JYScripts").Include(
                "~/Scripts/jquery-1.7.2.min.js",
                "~/Scripts/jquery-ui.min.js",
                "~/Scripts/ParamQueryGrid/pqgrid.min.js",
                "~/Scripts/speedup.js",
                "~/Scripts/jquery.cookie.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.bgiframe.js",
                "~/Scripts/xheditor/xheditor-1.1.12-zh-cn.min.js",
                // 第三方jquery插件
                "~/Scripts/LigerUI/js/ligerui.min.js",
                "~/Scripts/select2/select2.js",
                "~/Scripts/Highcharts-2.3.5/highcharts.js",
                "~/Scripts/Highcharts-2.3.5/highcharts-more.js",
                "~/Scripts/My97DatePicker/WdatePicker.js",
                "~/Scripts/Ztree3.5/js/jquery.ztree.all-3.5.min.js",
                "~/Scripts/poshytip-1.1/jquery.poshytip.min.js",
                "~/Scripts/JY.Global.js",
                "~/Scripts/dwz.min.js",
                "~/Scripts/dwz.regional.zh.js",
                "~/Scripts/ToolBar.js",
                "~/Scripts/dwz.extend.js",
                "~/Scripts/private.js",
                "~/Scripts/swfupload/swfupload.js",
                "~/Scripts/swfupload/swfupload.handlers.js",
                "~/Scripts/raty/jquery.raty.js",
                "~/Scripts/raty/jquery.raty.min.js",
                //自己封装的控件
                "~/Plugs/JUpload/Scripts/JUpload.js",
                "~/Plugs/Plugs.js"
                            ));
        }
    }
}