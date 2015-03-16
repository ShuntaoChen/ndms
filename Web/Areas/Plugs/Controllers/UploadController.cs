using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Modules.Core.Services;
using JiYun.Modules.Core.Models;
using JiYun.Web.Results;
using JiYun.Web.Controllers;
using System.Drawing;
using System.Collections;
using JiYun.Common.Utils;
using System.Globalization;
using System.Xml;

namespace Web.Areas.Plugs.Controllers
{
    /// <summary>
    /// 主要封装了上传的一些Action
    /// </summary>
    public class UploadController : BaseController
    {
        public IAttachmentService AttachmentService;
        public UploadController(IAttachmentService attachmentService)
        {
            AttachmentService = attachmentService;
        }

        private string StorageRoot
        {
            get 
            {
                string path = Path.Combine("~/Uploads/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/");
                if (!Directory.Exists(Server.MapPath(path)))
                {
                    Directory.CreateDirectory(Server.MapPath(path));
                }
                return path;
            }
        }

        #region 附件上传
        /// <summary>
        /// 显示已上传的附件
        /// </summary>
        /// <param name="guid">附件编号</param>
        /// <param name="sign">是否显示删除按钮，1 不显示 </param>
        /// <returns></returns>
        public JsonResult GetAttas(string guid,string sign, string controlid)
        {
            string result = "<div class=\"jfile\" >";
            if (!string.IsNullOrEmpty(guid))
            {
                var list = AttachmentService.GetByAttachmentID(guid);
                foreach (var item in list)
                {
                    if (System.IO.File.Exists(Server.MapPath(item.Path)))
                    {
                        string img = "<img style=\"cursor:pointer;height:16px;float: left;\" src=\"/Plugs/JUpload/Content/img/filetype/" + item.FileName.Substring(item.FileName.LastIndexOf('.') + 1) + ".gif\"/>";
                        if (item.FileName.ToLower().EndsWith("jpg") || item.FileName.ToLower().EndsWith("jpeg") || item.FileName.ToLower().EndsWith("gif") || item.FileName.ToLower().EndsWith("png") || item.FileName.ToLower().EndsWith("icon") || item.FileName.ToLower().EndsWith("bmp"))
                        {
                            img = "<a class=\"viewImg\" href=\"" + item.Path.Substring(1) + "\" style=\"float: left;\"><img style=\"cursor:pointer;width:16px;height:16px;\" src=\"" + item.Path.Substring(1) + "\"/></a>";
                        }
                        result += "<div style='width:65%;border-bottom: solid 1px #d0d0d0;height:30px;margin-bottom: 2px;'>" + img + "<a title=\"下载\" href=\"/Upload/Download?id=" + Path.GetFileName(item.Path) + "\" style=\"float: left;line-height: 18px;\">" + item.FileName + "</a>&nbsp;";
                        if(sign != "1"){
                            result += "<a style='float: right;' name='hidNameSign' onclick=\"deleteFile(this,'" + item.ID + "','" + controlid + "')\" href=\"javascript:void(0)\"><img title=\"删除\" src=\"/Plugs/JUpload/Content/img/deleteFile.png\"/></a>";
                        }
                        result += "</div>";
                    }
                }
            }
            return Json(result + "</div>");
        }

        public ActionResult JUpload(string guid, string navTabID, string controlid)
        {
            ViewBag.nid = navTabID;
            ViewBag.guid = guid;
            ViewBag.controlid = controlid;
            var list = new List<string>();
            list.Add(guid);
            HttpRuntime.Cache.Insert("JUpload", list, null, DateTime.MaxValue, TimeSpan.Zero);
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpGet]
        public void Delete(string id)
        {
            int fileid = int.Parse(id);
            
            var deletelist = AttachmentService.GetAllAttachment().Where(a => a.ID == fileid);
            if (deletelist.Count() > 0)
            {
                if (System.IO.File.Exists(Server.MapPath(deletelist.First().Path)))
                {
                    System.IO.File.Delete(Server.MapPath(deletelist.First().Path));
                }

                AttachmentService.Delete(deletelist.First().ID);
            }
           
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpGet]
        public void Download(string id)
        {
            var filename = id;
            var filePath = Path.Combine(Server.MapPath(StorageRoot), filename);

            var context = HttpContext;

            if (System.IO.File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
            }
            else
                context.Response.StatusCode = 404;
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpPost]
        public ActionResult UploadFiles()
        {
            var r = new List<ViewDataUploadFilesResult>();

            foreach (string file in Request.Files)
            {
                var statuses = new List<ViewDataUploadFilesResult>();
                var headers = Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    UploadWholeFile(Request, statuses);
                }
                else
                {
                    UploadPartialFile(headers["X-File-Name"], Request, statuses);
                }

                JsonResult result = Json(statuses);
                result.ContentType = "text/plain";

                return result;
            }

            return Json(r);
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadPartialFile(string fileName, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            var inputStream = file.InputStream;

            var fullName = Path.Combine(StorageRoot, Path.GetFileName(fileName));

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
            statuses.Add(new ViewDataUploadFilesResult()
            {
                name = fileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = "/Upload/Download/" + fileName,
                delete_url = "/Upload/Delete/" + fileName,
                thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
                delete_type = "GET",
            });
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                string guid = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var fullPath = StorageRoot + guid;
                file.SaveAs(Server.MapPath(fullPath));

                var atta = new JiYun.Modules.Core.Models.S_Attachment();
                atta.AttachmentID = (HttpRuntime.Cache["JUpload"] as List<string>)[0];
                atta.CreateOn = DateTime.Now;

                atta.FileName = Path.GetFileName(file.FileName);
                atta.Path = fullPath;
                atta.Size = file.ContentLength;
                AttachmentService.Insert(atta);

                statuses.Add(new ViewDataUploadFilesResult()
                {
                    name = file.FileName,
                    size = file.ContentLength,
                    type = file.ContentType,
                    url = "/Upload/Download/" + guid,
                    delete_url = "/Upload/Delete/" + guid,
                    thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath),
                    delete_type = "GET",
                });
            }
        }

        #endregion

        #region 附件下载


        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="ATTACHMENTID"></param>
        /// <returns></returns>
        public List<S_Attachment> GetAttachmentList(string attachment)
        {
            List<S_Attachment> listAtt = AttachmentService.GetByAttachmentID(attachment).ToList();
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
        /// <param name="id">附件表ID</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult Download(int id)
        {
            var model = AttachmentService.GetById(id);
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
            var model = AttachmentService.GetById(id);
            return new DownloadResult
            {
                VirtualPath = model.Path,
                FileDownloadName = Url.Encode(model.FileName)
            };
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="id">附件表ID</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult DoBasewnload(int id)
        {
            var model = AttachmentService.GetById(id); ;
            if (System.IO.File.Exists(Server.MapPath(model.Path)) == false)
            {
                return ShowErrorMessage("未找到文件！");
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

        #region 图片上传

        [HttpPost]
        public ActionResult _SwfUpload2(string dir = "default")
        {
            string savePath = "/Uploads/" + dir + "/";
            string saveUrl = "/Uploads/" + dir + "/";
            const string fileTypes = "gif,jpg,jpeg,png,bmp";
            const int maxSize = 1000000;

            Hashtable hash;

            HttpPostedFileBase file = Request.Files["Filedata"];
            if (file == null)
            {
                hash = new Hashtable();
                hash["msg"] = 0;
                hash["msgbox"] = "请选择文件";
                return Json(hash);
            }

            string dirPath = Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                hash = new Hashtable();
                hash["msg"] = 0;
                hash["msgbox"] = "上传目录不存在,已帮你重新创建目录，请重新上传！";
                Directory.CreateDirectory(dirPath);
                //return Json(hash);
            }

            string fileName = file.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();

            ArrayList fileTypeList = ArrayList.Adapter(fileTypes.Split(','));

            if (file.InputStream == null || file.InputStream.Length > maxSize)
            {
                hash = new Hashtable();
                hash["msg"] = 0;
                hash["msgbox"] = "上传文件大小超过限制";
                return Json(hash);
            }

            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                hash = new Hashtable();
                hash["msg"] = 0;
                hash["msgbox"] = "上传文件扩展名是不允许的扩展名";
                return Json(hash);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff", DateTimeFormatInfo.InvariantInfo);
            dirPath += newFileName + "\\";
            Directory.CreateDirectory(dirPath);
            SaveImage(dir, file, dirPath, newFileName, fileExt);


            string fileUrl = saveUrl + newFileName + "/" + newFileName + fileExt;

            hash = new Hashtable();
            hash["msg"] = 1;
            hash["msgbox"] = fileUrl;

            return Json(hash, "text/html;charset=UTF-8"); ;
        }

        /// <summary>
        /// 删除上传的图片
        /// </summary>
        /// <param name="filepath"></param>
        public void _DeleteUpFile(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                return;
            }
            string fullpath = GetMapPath(filepath);
            string fullpathDirectory = fullpath.Substring(0, fullpath.LastIndexOf("\\"));
            if (Directory.Exists(fullpathDirectory))
            {
                Directory.Delete(fullpathDirectory, true);
            }

        }

        /// <summary>
        /// 删除上传的图片
        /// </summary>
        /// <param name="filepath"></param>
        public static void DeleteUpFile(string filepath)
        {
            try
            {
                if (string.IsNullOrEmpty(filepath))
                {
                    return;
                }
                filepath = filepath.Substring(filepath.IndexOf(@"/Uploads"), filepath.Length - filepath.IndexOf(@"/Uploads"));
                string fullpath = GetMapPath(filepath);
                string fullpathDirectory = fullpath.Substring(0, fullpath.LastIndexOf("\\"));
                if (Directory.Exists(fullpathDirectory))
                {
                    Directory.Delete(fullpathDirectory, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (System.Web.HttpContext.Current != null)
            {
                return System.Web.HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        /// <summary>
        /// 保存图片并生成缩略图
        /// </summary>
        /// <param name="saveDir">图片分类</param>
        /// <param name="file"></param>
        /// <param name="dirPath"></param>
        /// <param name="fileName"></param>
        /// <param name="fileExt"></param>
        public static void SaveImage(string saveDir, HttpPostedFileBase file, string dirPath, string fileName, string fileExt)
        {
            //保存原图
            string filePath = dirPath + fileName + fileExt;
            file.SaveAs(filePath);

            XmlDocument xml = new XmlDocument();
            xml.Load(GetMapPath("/Config/") + "ImageSetting.xml");
            XmlNode imageSetting = xml.SelectSingleNode("imageSetting");
            XmlNodeList xnl = imageSetting.ChildNodes;

            //获取备用的文字水印设置
            XmlNode waterNode = imageSetting.SelectSingleNode("watermark");
            string waterStr = waterNode.SelectSingleNode("str").InnerText;
            string waterFont = waterNode.SelectSingleNode("font").InnerText;
            string waterSize = waterNode.SelectSingleNode("size").InnerText;

            foreach (XmlElement imageTypeNode in xnl)
            {
                if (imageTypeNode.GetAttribute("dir") == saveDir)
                {
                    XmlNodeList advList = imageTypeNode.ChildNodes;
                    foreach (XmlNode node in advList)
                    {
                        try
                        {
                            string tail = node.SelectSingleNode("tail").InnerText;
                            string type = node.SelectSingleNode("type").InnerText;
                            string width = node.SelectSingleNode("width").InnerText;
                            string height = node.SelectSingleNode("height").InnerText;
                            string waterMarkType = node.SelectSingleNode("watermark").InnerText;

                            if (string.IsNullOrEmpty(tail) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(width) ||
                                string.IsNullOrEmpty(height))
                            {
                                continue;
                            }

                            string thumPath = dirPath + fileName + tail + fileExt;

                            MakeThumNail(filePath, thumPath, Int32.Parse(width), Int32.Parse(height), type, waterMarkType, waterStr, waterFont, waterSize);
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 保存图片并生成缩略图
        /// </summary>
        /// <param name="saveDir"></param>
        /// <param name="file"></param>
        /// <param name="dirPath"></param>
        /// <param name="fileName"></param>
        /// <param name="fileExt"></param>
        public static void SaveImage(string saveDir, Image file, string dirPath, string fileName, string fileExt)
        {
            //保存原图
            string filePath = dirPath + fileName + fileExt;
            file.Save(filePath);

            XmlDocument xml = new XmlDocument();
            xml.Load(GetMapPath("/Config/") + "ImageSetting.xml");
            XmlNode imageSetting = xml.SelectSingleNode("imageSetting");

            //获取备用的文字水印设置
            XmlNode waterNode = imageSetting.SelectSingleNode("watermark");
            string waterStr = waterNode.SelectSingleNode("str").InnerText;
            string waterFont = waterNode.SelectSingleNode("font").InnerText;
            string waterSize = waterNode.SelectSingleNode("size").InnerText;

            XmlNodeList xnl = imageSetting.ChildNodes;
            foreach (XmlElement imageTypeNode in xnl)
            {
                if (imageTypeNode.GetAttribute("dir") == saveDir)
                {
                    XmlNodeList advList = imageTypeNode.ChildNodes;
                    foreach (XmlNode node in advList)
                    {
                        try
                        {
                            string tail = node.SelectSingleNode("tail").InnerText;
                            string type = node.SelectSingleNode("type").InnerText;
                            string width = node.SelectSingleNode("width").InnerText;
                            string height = node.SelectSingleNode("height").InnerText;
                            string waterMarkType = node.SelectSingleNode("watermark").InnerText;

                            if (string.IsNullOrEmpty(tail) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(width) ||
                                string.IsNullOrEmpty(height))
                            {
                                continue;
                            }

                            string thumPath = dirPath + fileName + tail + fileExt;

                            MakeThumNail(filePath, thumPath, Int32.Parse(width), Int32.Parse(height), type, waterMarkType, waterStr, waterFont, waterSize);
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 保存图片并生成缩略图
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string SaveImage(HttpPostedFileBase file)
        {
            return SaveImage("default", file);
        }

        /// <summary>
        /// 将文件保存在/Uploads/dir下
        /// </summary>
        /// <param name="file">文件流</param>
        /// <returns>原图保存路径</returns>
        public static string SaveImage(string dir, HttpPostedFileBase file)
        {
            string savePath = "/Uploads/" + dir + "/";
            string saveUrl = "/Uploads/" + dir + "/";
            const string fileTypes = "gif,jpg,jpeg,png,bmp";

            Hashtable hash;

            if (file == null)
            {
                return "文件不存在";
            }

            //创建目录
            string dirPath = GetMapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string fileName = file.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();
            //判断扩展名
            if (string.IsNullOrEmpty(fileExt) ||
                Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return "扩展名不正确";
            }

            //生成文件名
            System.Threading.Thread.Sleep(10);  //防止时间重复
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff", DateTimeFormatInfo.InvariantInfo);
            dirPath += newFileName + "\\";
            Directory.CreateDirectory(dirPath);
            SaveImage(dir, file, dirPath, newFileName, fileExt);

            string fileUrl = saveUrl + newFileName + "/" + newFileName + fileExt;

            return fileUrl;
        }

        /// <summary>
        /// 保存图片并生成缩略图
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string SaveImage(Image file)
        {
            return SaveImage("default", file);
        }


        /// <summary>
        /// 保存图片并生成缩略图
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string SaveImage(string dir, Image file)
        {
            string savePath = "/Uploads/" + dir + "/";
            string saveUrl = "/Uploads/" + dir + "/";

            if (file == null)
            {
                return "文件不存在";
            }

            //创建目录
            string dirPath = GetMapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            //生成文件名
            System.Threading.Thread.Sleep(10);  //防止时间重复
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff", DateTimeFormatInfo.InvariantInfo);
            dirPath += newFileName + "\\";
            Directory.CreateDirectory(dirPath);
            SaveImage(dir, file, dirPath, newFileName, ".png");

            string fileUrl = saveUrl + newFileName + "/" + newFileName + ".png";

            return fileUrl;
        }

        #region 生成缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">原图片地址</param>
        /// <param name="thumNailPath">缩略图地址</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="model">生成缩略的模式，HW-拉伸，W-指定宽度，H-指定高度，Cut-剪裁</param>
        /// <param name="waterMarkType">插入水印类型，0=不插入，1=插入图片，2=插入文字</param>
        /// <param name="waterStr">水印文字</param>
        /// <param name="waterFont">水印字体</param>
        /// <param name="waterSize">文字大小</param>
        public static void MakeThumNail(string originalImagePath, string thumNailPath, int width, int height, string model, string waterMarkType = "0", string waterStr = "", string waterFont = "", string waterSize = "")
        {

            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int thumWidth = width;      //缩略图的宽度
            int thumHeight = height;    //缩略图的高度

            int x = 0;
            int y = 0;

            int originalWidth = originalImage.Width;    //原始图片的宽度
            int originalHeight = originalImage.Height;  //原始图片的高度

            switch (model)
            {
                case "HW":      //指定高宽缩放,可能变形
                    break;
                case "W":       //指定宽度,高度按照比例缩放
                    thumHeight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":       //指定高度,宽度按照等比例缩放
                    thumWidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut":
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)thumWidth / (double)thumHeight)
                    {
                        originalHeight = originalImage.Height;
                        originalWidth = originalImage.Height * thumWidth / thumHeight;
                        y = 0;
                        x = (originalImage.Width - originalWidth) / 2;
                    }
                    else
                    {
                        originalWidth = originalImage.Width;
                        originalHeight = originalWidth * height / thumWidth;
                        x = 0;
                        y = (originalImage.Height - originalHeight) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(thumWidth, thumHeight);

            //新建一个画板
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量查值法
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量，低速度呈现平滑程度
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            //清空画布并以透明背景色填充
            graphic.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            graphic.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, thumWidth, thumHeight), new System.Drawing.Rectangle(x, y, originalWidth, originalHeight), System.Drawing.GraphicsUnit.Pixel);

            if (waterMarkType != "0")
            {
                string watermark = GetMapPath(@"/Uploads/watermark/") + "watermark.png";
                if (waterMarkType == "1" && System.IO.File.Exists(watermark)) //如果文件存在添加图片水印
                {
                    Image waterImage = Image.FromFile(watermark);
                    int markWidth = waterImage.Width;
                    int markHeight = waterImage.Height;
                    int drawHeight = height - markHeight - 10;
                    int drawWidth = width - markWidth - 10;
                    if (drawHeight > 0 && drawWidth > 0) //对于过小的图片不添加水印
                    {
                        // ARGB矩阵转换
                        //           ImageAttributes imageAttributes = new ImageAttributes();
                        //           ColorMap colorMap = new ColorMap();

                        //           ColorMap[] remapTable = { colorMap };
                        //           imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
                        //           float[][] colorMatrixElements = {   
                        //             new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f}, // red红色  
                        //             new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f}, //green绿色  
                        //             new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f}, //blue蓝色         
                        //             new float[] {0.0f,  0.0f,  0.0f,  1.0f, 0.0f}, //透明度       
                        //             new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};//  
                        //           ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                        //           imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                        //           ColorAdjustType.Bitmap);

                        graphic.DrawImage(waterImage, new Rectangle(drawWidth, drawHeight, markWidth, markHeight), 0, 0, markWidth, markHeight, GraphicsUnit.Pixel);
                        //graphic.DrawImage(waterImage, new Rectangle(drawWidth, drawHeight, markWidth, markHeight), 0, 0, markWidth, markHeight, GraphicsUnit.Pixel, imageAttributes);
                        //graphic.DrawImage(waterImage, new Point(drawWidth, drawHeight));
                    }
                }
                else //如果文件不存在或waterMarkType不为1采用文字水印
                {
                    if (!string.IsNullOrEmpty(waterStr) && !string.IsNullOrEmpty(waterFont) && !string.IsNullOrEmpty(waterSize))
                    {
                        Brush brush = new SolidBrush(Color.FromArgb(144, 255, 255, 255));
                        int markHeight = 20;
                        int markWidth = 12 * waterStr.Length;
                        int drawHeight = height - markHeight - 10;
                        int drawWidth = width - markWidth - 10;

                        if (drawHeight > 0 && drawWidth > 0)
                        {
                            graphic.DrawString(waterStr, new Font(waterFont, Utils.StrToInt(waterSize, 12)), brush,
                                new PointF(drawWidth, drawHeight));
                        }
                    }
                }
            }

            try
            {
                bitmap.Save(thumNailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                graphic.Dispose();
            }

        }

        #endregion

        #endregion
    }

    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
    }




   

}
