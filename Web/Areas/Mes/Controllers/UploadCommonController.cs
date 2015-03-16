using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;
using Web.Controllers;
using JY.Module.Message.Services;
using JiYun.Modules.Core.Services;
using JiYun.Modules.Core.Models;
using System.IO;
using System.Configuration;
using JiYun.Web.Results;

namespace Web.Areas.Mes.Controllers
{
    /// <summary>
    /// 附件相关Common
    /// </summary>
    public class UploadCommonController : MyBaseController
    {
        #region 引用声明
        private AttachmentService attsService = new AttachmentService();
        #endregion

        #region 附件相关
        public JsonResult DeleteAttachment(int id)
        {
            attsService.Delete(id);
            return SuccessMsg("删除成功");
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="ATTACHMENTID"></param>
        /// <returns></returns>
        public List<S_Attachment> GetAttachmentList(string attachment)
        {
            List<S_Attachment> listAtt = attsService.GetByAttachmentID(attachment).ToList();
            return listAtt;
        }

        /// <summary>
        /// 通用附件下载列表
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public ActionResult DownLoadList(string attachment)
        {
            ViewData.Model = GetAttachmentList(attachment);
            return View();
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult Download(int id)
        {
            var model = attsService.GetById(id);
            if (System.IO.File.Exists(Server.MapPath(model.Path)) == false)
            {
                //return ErrorMsg("未找到文件！", "");
                return Content("0");
            }
            else
            {
                return Content("1");
            }
        }
        [ValidateInput(false)]
        public ActionResult Download2(int id)
        {
            var model = attsService.GetById(id);
            return new DownloadResult
            {
                VirtualPath = model.Path,
                FileDownloadName = Url.Encode(model.FileName)
            };
        }

        /// <summary>
        /// 执行上传附件
        /// </summary>
        /// <param name="FileData"></param>
        /// <param name="folder"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult DoUploadify(HttpPostedFileBase FileData, string guid, string parentID, string userID, string pub)
        {
            string folder = ConfigurationManager.AppSettings["UploadPath"].ToString();
            //以每天创建一个文件夹的形式保存文件
            folder = string.Format("{0}/{1}/{2}/{3}/", folder, DateTime.Now.ToString("yyyy年"), DateTime.Now.ToString("MM月"), DateTime.Now.ToString("dd日"));
            //判断是否存在该文件夹，如果不存在就创建文件夹
            if (!Directory.Exists(Server.MapPath(folder)))
            {
                Directory.CreateDirectory(Server.MapPath(folder));
            }
            string result = "";
            if (FileData != null)
            {
                try
                {
                    string newguid = System.Guid.NewGuid().ToString();
                    result = Path.GetFileName(FileData.FileName);
                    int size = FileData.ContentLength;
                    string ext = Path.GetExtension(FileData.FileName);
                    string saveName = newguid + ext;

                    S_Attachment atta = new S_Attachment();
                    atta.FileName = result;
                    atta.Path = folder + saveName;
                    atta.Size = size;
                    atta.CreateOn = System.DateTime.Now;
                    atta.AttachmentID = guid;
                    atta.CreatorId = int.Parse(userID);
                    //保存至数据库
                    attsService.Insert(atta);
                    //保存至服务器
                    FileData.SaveAs(Server.MapPath(folder + saveName));
                }
                catch
                {
                    result = "";
                }
            }
            return Content(result);
        }
        /// <summary>
        /// 修改附件时，删除单个文件
        /// </summary>
        /// <param name="FileID"></param>
        /// <returns></returns>
        public JsonResult DeleteFile(int FileID)
        {

            attsService.Delete(FileID);
            //成功为1
            var json = new
            {
                Status = 1
            };
            return Json(json);
        }



        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult DoBasewnload(int id)
        {
            var model = attsService.GetById(id); ;
            if (System.IO.File.Exists(Server.MapPath(model.Path)) == false)
            {
                return ErrorMsg("未找到文件！", "");
            }
            else
            {
                return new DownloadResult
                {
                    VirtualPath = model.Path,
                    FileDownloadName = Url.Encode(model.FileName)
                };
            }
        }

        #endregion

    }
}
