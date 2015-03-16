using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using JiYun.Modules.Core.Services;
using JiYun.Modules.Core.Models;

namespace Community.Web.JUpload.Upload
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {
        public IAttachmentService AttachmentService = new AttachmentService();
        private readonly JavaScriptSerializer js;
        private string StorageRoot
        {
            get 
            {
                string path = Path.Combine("~/Uploads/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/");
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(path)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
                }
                return path;
            } //Path should! always end with '/'
        }

        public UploadHandler()
        {
            js = new JavaScriptSerializer();
            js.MaxJsonLength = 41943040;
        }

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("Pragma", "no-cache");
            context.Response.AddHeader("Cache-Control", "private, no-cache");

            HandleMethod(context);
        }

        // Handle request based on method
        private void HandleMethod(HttpContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "HEAD":
                case "GET":
                    if (context.Request["type"] == "DT")
                    {
                        DeleteFile(context);
                    }
                    else
                    {
                        if (GivenFilename(context)) DeliverFile(context);
                        else ListCurrentFiles(context);
                    }
                    break;

                case "POST":
                case "PUT":
                    UploadFile(context);
                    break;

                case "DELETE":
                    DeleteFile(context);
                    break;

                case "OPTIONS":
                    ReturnOptions(context);
                    break;

                default:
                    context.Response.ClearHeaders();
                    context.Response.StatusCode = 405;
                    break;
            }
        }

        private static void ReturnOptions(HttpContext context)
        {
            context.Response.AddHeader("Allow", "DELETE,GET,HEAD,POST,PUT,OPTIONS");
            context.Response.StatusCode = 200;
        }

        // Delete file from the server
        private void DeleteFile(HttpContext context)
        {
            string filePath = "";
            string pt = (HttpRuntime.Cache["JUpload"] as List<string>)[0]; //附件编号

            filePath = HttpContext.Current.Server.MapPath(StorageRoot + context.Request["f"]);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                var ct = new JiYun.Modules.Core.Services.AttachmentService();
                var deletelist = ct.GetAllAttachment().Where(a => a.Path == filePath);
                if (deletelist.Count() > 0)
                {
                    ct.Delete(deletelist.First().ID);
                }
            }
        }

        // Upload file to the server
        private void UploadFile(HttpContext context)
        {
            var statuses = new List<FilesStatus>();
            var headers = context.Request.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadWholeFile(context, statuses);
            }
            else
            {
                UploadPartialFile(headers["X-File-Name"], context, statuses);
            }

            WriteJsonIframeSafe(context, statuses);
        }

        // Upload partial file
        private void UploadPartialFile(string fileName, HttpContext context, List<FilesStatus> statuses)
        {
            if (context.Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var inputStream = context.Request.Files[0].InputStream;
            var fullName = StorageRoot + Path.GetFileName(fileName);

            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            statuses.Add(new FilesStatus(new FileInfo(fullName)));
        }

        // Upload entire file
        private void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
        {
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                var file = context.Request.Files[i];
                string pt = ((HttpRuntime.Cache["JUpload"] as List<string>)[0]);
                string guid = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var fullPath = StorageRoot + guid;
                file.SaveAs(HttpContext.Current.Server.MapPath(fullPath));

                var atta = new S_Attachment();
                atta.AttachmentID = pt;
                atta.CreateOn = DateTime.Now;
                atta.FileName = Path.GetFileName(file.FileName);
                atta.Path = fullPath;
                atta.Size = file.ContentLength;
                AttachmentService.Insert(atta);
                statuses.Add(new FilesStatus(atta.FileName, file.ContentLength, fullPath));
            }
        }

        private void WriteJsonIframeSafe(HttpContext context, List<FilesStatus> statuses)
        {
            context.Response.AddHeader("Vary", "Accept");
            try
            {
                if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
                    context.Response.ContentType = "application/json";
                else
                    context.Response.ContentType = "text/plain";
            }
            catch
            {
                context.Response.ContentType = "text/plain";
            }

            var jsonObj = js.Serialize(statuses.ToArray());
            context.Response.Write(jsonObj);
        }

        private static bool GivenFilename(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Request["f"]);
        }

        private void DeliverFile(HttpContext context)
        {
            var filename = context.Request["f"];
            var filePath = HttpContext.Current.Server.MapPath(StorageRoot + filename);

            if (File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
            }
            else
                context.Response.StatusCode = 404;
        }

        private void ListCurrentFiles(HttpContext context)
        {
            var files =
                new DirectoryInfo(StorageRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select(f => new FilesStatus(f))
                    .ToArray();

            string jsonObj = js.Serialize(files);
            context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            context.Response.Write(jsonObj);
            context.Response.ContentType = "application/json";
        }

    }
}