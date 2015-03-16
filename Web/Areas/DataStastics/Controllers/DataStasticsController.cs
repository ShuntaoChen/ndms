using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Plugins.Highcharts;
using JiYun.Plugins.Highcharts.Helpers;
using JiYun.Plugins.Highcharts.Options;
using JiYun.Web.Controllers;

namespace Web.Areas.DataStastics.Controllers
{
    public class DataStasticsController : BaseController
    {
        //工作量统计
        // GET: /DataStastics/WorkloadStastics/

        public ActionResult WorkloadStastics()
        {
            string[] Categories = new[] { "ZY001", "ZY002", "枫泾管理员", "维护员", "维护员2" };

            var data = new[]
                {
                    new Series
                        {
                            Name = "人员",
                            Data =
                                new Data(new object[]
                                    {1,1,6,3,4})
                        }
                };

            var chart = new ColumnChart
            {
                Name = "PersonnelColumn",
                Title = "人员工作量统计",
                Categories = Categories,
                Data = data,
                Click = "LineClick(this)"
            };
            ViewData.Model = chart;
            return View(ViewData.Model);
        }

        //设备统计
        // GET: /DataStastics/DataStastics/DeviceStastics/5

        public ActionResult DeviceStastics()
        {
            string[] Categories = new[] { "交换机", "服务器", "磁盘阵列", "普通PC机", "防火墙" ,"托管服务器"};

            var data = new[]
                {
                    new Series
                        {
                            Name = "资产类型",
                            Data =
                                new Data(new object[]
                                    {0,7,4,0,16,29})
                        }
                };

            var chart = new ColumnChart
            {
                Name = "AssetsColumn",
                Title = "资产分类数量统计",
                Categories = Categories,
                Data = data,
                Click = "LineClick(this)"
            };
            ViewData.Model = chart;
            return View(ViewData.Model);
        }

        //维护统计
        // GET: /DataStastics/DataStastics/MaintainStastics

        public ActionResult MaintainStastics()
        {
            string[] Categories = new[] { "安全问题", "网络问题", "病毒问题", "硬件问题", "周期维护","终端问题","其他问题","软件数据库","服务器" };

            var data = new[]
                {
                    new Series
                        {
                            Name = "维护类型",
                            Data =
                                new Data(new object[]
                                    {2,4,0,0,1,2,0,0,0})
                        }
                };

            var chart = new ColumnChart
            {
                Name = "TypeColumn",
                Title = "维护类型统计",
                Categories = Categories,
                Data = data,
                Click = "LineClick(this)"
            };
            ViewData.Model = chart;
            return View(ViewData.Model);
        }



        //值班统计
        // GET: /DataStastics/DataStastics/DutyStastics/5

        public ActionResult DutyStastics()
        {
            string[] Categories = new[] { "ZY001", "ZY002", "枫泾管理员", "维护员", "维护员2" };

            var data = new[]
                {
                    new Series
                        {
                            Name = "人员",
                            Data =
                                new Data(new object[]
                                    {1,1,6,3,4})
                        }
                };

            var chart = new ColumnChart
            {
                Name = "JobColumn",
                Title = "人员值班统计",
                Categories = Categories,
                Data = data,
                Click = "LineClick(this)"
            };
            ViewData.Model = chart;
            return View(ViewData.Model);
        }

     
    }
}
