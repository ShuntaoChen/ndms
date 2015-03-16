<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<script type="text/javascript">
function createCalendar(starttime,endtime) {
       
       $.pdialog.open("<%= Url.Action("Create", "Calendar") %>?starttime="+escape(starttime)+"&endtime="+escape(endtime), "CalendarCreate", "添加日程", {width:755, height:510, mask: true});
   }
</script>
<div class="pageContent" layouth="1">
	<div id="calendarr" style="width: 95%; margin: 0 auto; margin-top: 5px;"></div>
</div>
<link href="../../Scripts/fullCalendar/theme.css" rel="stylesheet" type="text/css" />
<script src="../../Scripts/fullCalendar/jquery-ui-1.8.11.custom.min.js" type="text/javascript"></script>
<link href="../../Scripts/fullCalendar/fullcalendar.css" rel="stylesheet" type="text/css" />
<script src="../../Scripts/fullCalendar/fullcalendar.js" type="text/javascript"></script>
<link href="../../Scripts/fullCalendar/fullcalendar.print.css" rel="stylesheet" media="print" type="text/css" />
<script type='text/javascript'>
    $(function () {
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        var calendar = $('#calendarr').fullCalendar({
            theme: true,
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            selectable: true,
            selectHelper: true,
            select: function (start, end, allDay) {
                var endtime="";
                if(allDay==false)
                    endtime=end.getFullYear() + "-" + (end.getMonth() + 1) + "-" + end.getDate() + " " + end.getHours() + ":" + end.getMinutes();
                else
                    endtime=end.getFullYear() + "-" + (end.getMonth() + 1) + "-" + end.getDate() + " 23:59";
                createCalendar(start.getFullYear() + "-" + (start.getMonth() + 1) + "-" + start.getDate() + " " + start.getHours() + ":" + start.getMinutes(), endtime);
                calendar.fullCalendar('unselect');
            },
            titleFormat: {
                month: 'yyyy年M月',
                week: "yyyy年M月d日 {'&#8212;' M月d日}",
                day: 'yyyy年M月d日  dddd'
            },
            columnFormat: {
                month: 'dddd',
                week: ' M/d dddd',
                day: 'M/d dddd'
            },
            timeFormat: {
                '': 'HH(:mm)t'
            },
            buttonText: {
                today: '今日',
                month: '月',
                week: '周',
                day: '日'
            },
            tt: function (d) {
                return d.getHours() < 12 ? '上午' : '下午'
            },
            dayNames: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
            defaultView: 'month',
            weekends:true,
            allDayDefault: false,
            allDaySlot: false,
                editable: false,
            weekMode: 'liquid',
            firstHour: 8,
            aspectRatio: 1.8,
            events: '<%= Url.Action("Load", "Calendar") %>',
            eventClick: function (calEvent, jsEvent, view) {
                $(".fc-event-title,.fc-event-time").css("color", "white");
                $(this).find(".fc-event-title").first().css("color", "red");
                $(this).find(".fc-event-time").first().css("color", "red");

                $.pdialog.open('<%= Url.Action("Edit", "Calendar") %>/' + calEvent.id, "CalendarEdit", "修改日程", { width: 755, height: 510, mask: true });

            },

            eventRender: function (event, element) {
                $('.fc-event-title').poshytip({      //小贴士  tip-yellowsimple  tip-yellow tip-green tip-violet
                    className: 'tip-yellowsimple',
                    showTimeout: 1,
                    alignTo: 'target',
                    alignX: 'center',
                    alignY: 'top',
                    offsetY: 5,
                    allowTipHover: false
                });
            }
        });
    });
//    $(function () {
//        $(".pageContent").css("overflow-y", "auto");
//    });
</script>
