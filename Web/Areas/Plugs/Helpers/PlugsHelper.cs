using System.Web.Mvc;
using System.Web;
using System;
using System.Text;
using System.ComponentModel;
using PagedList;
using JiYun.Modules.Core.Services;
using Web.Areas.Plugs.Controllers;
using System.IO;
using JiYun.Common.Utils;
using System.Collections.Generic;


namespace JiYun.Web.Helpers
{


    public static class PlugsHelper
    {

        #region 上传下载控件
        /// <summary>
        /// 上传控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controlname"></param>
        /// <returns></returns>
        public static MvcHtmlString UploadFile(this HtmlHelper helper, string controlname = "AttachmentID", string attid = "", int width = 80, bool showUpLoad = true, bool showDel = true)
        {
            string exitFile = "";
            if (attid != "")
            {

                exitFile = GetAttas(attid, showDel == true ? "" : "1", controlname);
            }

            string div_id = Guid.NewGuid().ToString();

            StringBuilder builder = new StringBuilder();
            builder.Append("<div id=" + div_id + " style=\"float:left;width:" + width + "px\">");
            builder.Append("<input type='hidden' name='" + controlname + "' ID='" + controlname + "' value='" + attid + "'/>");
            if (showUpLoad)
            {
                if (attid == "" || attid == null)
                {

                    attid = Guid.NewGuid().ToString();
                }

                builder.Append("<a href=\"javascript:\" onclick=\"JUpload('" + attid + "','" + div_id + "','" + controlname +"')\" >");
                builder.Append("<img title=\"点击上传附件\" style=\" cursor:pointer;float:left\" src=\"/Plugs/JUpload/Content/img/attachment.png\" />");

            }
            builder.Append("</a>" + exitFile + "</div>");

            return new MvcHtmlString(builder.ToString());
        }


        /// <summary>
        /// 显示已上传的附件
        /// </summary>
        /// <param name="guid">附件编号</param>
        /// <param name="sign">是否显示删除按钮，1 不显示 </param>
        /// <returns></returns>
        public static string GetAttas(string guid, string sign, string controlid)
        {
            string result = "<div class=\"jfile\" >";
            if (!string.IsNullOrEmpty(guid))
            {
                AttachmentService att = new AttachmentService();
                var list = att.GetByAttachmentID(guid);
                foreach (var item in list)
                {
                    //if (System.IO.File.Exists(Server.MapPath(item.Path)))
                    //{
                    string img = "<img style=\"cursor:pointer;height:16px;float: left;\" src=\"/Plugs/JUpload/Content/img/filetype/" + item.FileName.Substring(item.FileName.LastIndexOf('.') + 1) + ".gif\"/>";
                    if (item.FileName.ToLower().EndsWith("jpg") || item.FileName.ToLower().EndsWith("jpeg") || item.FileName.ToLower().EndsWith("gif") || item.FileName.ToLower().EndsWith("png") || item.FileName.ToLower().EndsWith("icon") || item.FileName.ToLower().EndsWith("bmp"))
                    {
                        img = "<a class=\"viewImg\" href=\"" + item.Path.Substring(1) + "\" style=\"float: left; \"><img style=\"cursor:pointer;width:16px;height:16px;\" src=\"" + item.Path.Substring(1) + "\"/></a>";
                    }
                    result += "<div style='width:65%;border-bottom: solid 1px #d0d0d0;height:30px;margin-bottom: 2px;'>" + img + "<a title=\"下载\" href=\"#\" onclick=\"DownSignlFile(" + item.ID + ")\" style=\"float: left; color:blue;line-height: 18px;\">" + item.FileName + "</a>&nbsp;";
                    if (sign != "1")
                    {
                        result += "<a style='float: right;' name='hidNameSign' onclick=\"deleteFile(this,'" + item.ID + "','" + controlid + "')\" href=\"javascript:void(0)\"><img title=\"删除\" src=\"/Plugs/JUpload/Content/img/deleteFile.png\"/></a>";
                    }
                    result += "</div>";
                    // }
                }
            }
            return (result + "</div>");
        }



        /// <summary>
        /// 下载控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="attid"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static MvcHtmlString DownLoadFile(this HtmlHelper helper, string attid = "", string option = "")
        {

            StringBuilder builder = new StringBuilder();

            if (attid == null || attid == "")
            {
                return new MvcHtmlString("");
            }
            builder.Append("<a href=\"/Plugs/Upload/DownLoadList?attachment=" + attid + "\" rel=\"FileDownLoadList\"");
            builder.Append("target=\"dialog\" mask=\"true\" title=\"附件列表\" style=\"float:" + option + "; margin-left:5px;\">");
            builder.Append("<img alt=\"\" src=\"/Plugs/JUpload/Content/img/download.png\" />");
            builder.Append("</a>");


            return new MvcHtmlString(builder.ToString());
        }
        #endregion

        #region 百家姓
        /// <summary>
        /// 百家姓
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controlid">隐藏域控件名称</param>
        /// <param name="current">当前值</param>
        /// <param name="type">0：横向 1：纵向</param>
        /// <param name="width">宽度或高度</param>
        /// <param name="callback">回调JS方法</param>
        /// <returns></returns>
        public static MvcHtmlString Surnames(this HtmlHelper helper, string controlid = "Letter", string current = "", string callback = "", int type = 0, int width = 600)
        {

            StringBuilder builder = new StringBuilder();

            string strcolor1 = (current == "" || current == null) ? "" : "blue";
            string strcolor2 = (current == "" || current==null) ? "blue" : "";

            for (int a = 90; a > 64; a--)
            {
                var letter = char.ConvertFromUtf32(a).ToUpper();
                strcolor1 = letter == current ? "blue" : "";
                builder.Append("<li style=\"float: right; cursor: pointer; margin-right:5px;\">");
                builder.Append("<a style=\"font-weight: 700; color:" + strcolor1 + "\"" + " onclick=\"letterCallback('" + controlid + "', '" + letter + "','" + callback + "')\">");
                builder.Append("<span style=\"background-position: 12px -796px; padding: 0;\" class=\"cls_letter\">" +letter+ "</span>");
                builder.Append("</a></li>");
                   
            }
            builder.Append("<li style=\"float: right; cursor: pointer; margin-right:5px;\">");
            builder.Append("<a style=\"font-weight: 700; color:" + strcolor2 + "\"" + " onclick=\"letterCallback('" + controlid + "','','" + callback + "')\">");
            builder.Append("<span style=\"background-position: 32px -796px; padding: 0;\" class=\"cls_letter\">全部</span></a></li>");

            builder.Append("<input type=\"hidden\" name=\"" + controlid + "\" value=\"" + current + "\" />");

            return new MvcHtmlString(builder.ToString());
        }
        #endregion

        #region 图片上传

        /// <summary>
        /// 单图片上传
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="Controlid">控件ID</param>
        /// <param name="Path">图片路径</param>
        /// <returns></returns>
        public static MvcHtmlString ImgUpload(this HtmlHelper helper, string Controlid = "ImgUpload", string Path = null)
        {
            string guid = DateTime.Now.Ticks.ToString(); //随机字符串
            string result = "";

            result = "<div class='UploadButton'>";
            result += "<a href='#'> <input type='file'  name='" + Controlid + "' id='" + Controlid + "' accept='image/* ' value='选择图片'  /></a>";
            result += "</div>";

            //DIV用于显示
            if (Path != null)
            {
                result += "<div id='Disp_" + Controlid + "'>";
                result += "<a class='viewImg' href='" + Path + "'>";
                result += "<img style='cursor:pointer;width:150px; border: solid 5px #DDE0E6;' src='" + Path + "'/></a>";
                result += "</div>";
            }



            return new MvcHtmlString(result);
        }



        /// <summary>
        /// 多图片上传
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="dir">图片保存的文件夹</param>
        /// <param name="showListId">图片展示DIVID，尽量复杂不重复</param>
        /// <param name="Controlid">控件ID</param>
        /// <param name="Path">图片路径</param>
        /// <param name="initType">初始化方法</param>
        /// <param name="dragable">是否支持拖拽</param>
        /// <returns></returns>
        public static MvcHtmlString ImgUploadMutil(this HtmlHelper helper, string dir, string showListId, string Controlid = "ImgUpload", string HControlid = "ImagePath", List<string> pathList = null, string initType = "InitSWFUpload", bool dragable = true)
        {
            if (string.IsNullOrEmpty(dir))
            {
                dir = "default";
            }
            string guid = DateTime.Now.Ticks.ToString(); //随机字符串
            string result = "";

            //缓存图片路径
            result = " <input type='hidden' name='" + HControlid + "' id='" + HControlid + "' /> ";
            result += " <div id='show' style='display: none;'></div>";
            //显示原有图片（更新时用）
            result += "<div id='" + showListId + "' class='show_list'>";
            result += "<ul>";
            if (pathList != null)
            {

                for (int i = 0; i < pathList.Count; i++)
                {
                    result += "<li>";
                    result += "<div class='img_box'>";
                    result += "<a href='" + pathList[i] + "' class='viewImg'>";
                    result += "<img src='" + pathList[i] + "'/></a> <a href='javascript:;' onclick='remove_img(this);' class='deletea'>";
                    result += "<img src='../../../../Content/Images/ToolBarButton/no.png'></a>";
                    result += "</div>";
                    result += "</li>";
                }


            }
            result += "</ul>";
            result += "</div>";
            result += "<div class='upload_btn TipImp' title='建议单张图片小于2M'>";
            result += "<span id='upload'></span>";
            result += "</div>";

            //脚本区域
            result += "<script>";
            result += " $(function () {";
            result += initType + "('/Upload/_SwfUpload2/?dir=" + dir + "', 'Filedata', '2046KB', '../../Scripts/swfupload/swfupload.swf','" + showListId + "');";
            result += "$('.img_box').bind('mouseover', function () {";
            result += "    $(this).children('a.deletea').show();";
            result += "});";
            result += "$('.img_box').bind('mouseout', function () {";
            result += "     $(this).children('a.deletea').hide();";
            result += "});";
            result += "afterDocumnetLoad();";
            result += "});";

            //删除已上传图片
            result += " function delUpFile(filepath) {";
            result += " $.ajax({";
            result += " type: 'POST',";
            result += " dataType: 'json',";
            result += " url: 'Upload/_DeleteUpFile',";
            result += " data: { 'filepath': filepath },";
            result += " success: function (result) {";
            result += " }";
            result += " });";
            result += "}";

            result += " function remove_img(obj) {";
            result += " $(obj).parent().parent().remove();";
            result += " }";

            if (dragable)
            {
                result += " $('.show_list ul').sortable();";
            }
            result += "</script>";

            return new MvcHtmlString(result);
        }


        #endregion
    }
}
