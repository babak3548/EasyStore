//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using DataLayer.Enums;
//using UILayer.Miscellaneous;
//using Utility;
//using System.Drawing;
//using System.Xml.Serialization;
//using DataLayer;
//using ServiceLayer;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using DataLayer.EF;
//using System.Threading.Tasks;

//namespace UILayer.Controllers
//{
//    public class UploadController : Base0Controller
//    {
//        IHostingEnvironment _hostingEnvironment;
     

//        bool flagTrueExtention = false;
//        //
//        // GET: /Adminstration/Upload/
//        public UploadController(IHostingEnvironment hostingEnvironment, OnlineShopping _onlineShopping)
//            : base("Upload",  _onlineShopping)
//        {
//            _hostingEnvironment = hostingEnvironment;
//            //    string webRootPath = _hostingEnvironment.WebRootPath;
//            //    //string contentRootPath = _hostingEnvironment.ContentRootPath;
//        }

//        public ActionResult Index()
//        {

//            return View();
//        }

//        /// <summary>
//        /// این اکشن برای آپلود عکس توسط کابر پیاده سازی شده است
//        /// </summary>
//        /// <param name="InputName"></param>
//        /// <returns></returns>
//        [AllowAnonymous]
//        [HttpPost]
//      //  [AcceptVerbs()]
//        public async Task<string> UploadImageAsync(    string InputName)//List<IFormFile> files,  (FormCollection formCollection, IEnumerable<HttpPostedFileBase> file)
//        {
//             string webRootPath = _hostingEnvironment.ContentRootPath + "\\";
//            //   string contentRootPath = _hostingEnvironment.ContentRootPath;

//            var imageEditor =new  ImageEditor();
//            try
//            {
//                if (!ValidateAccessToActionBool(RolesSystem.UserValue)) { return ("لاگین نمایید" ); };
                
//                string physicalPath = webRootPath + Paths.AdminUploadPath;

//                var formFile = Request.Form.Files[0];
                
//                //var strigValue = Request.Form.Keys;

//                if (string.IsNullOrEmpty(formFile.FileName)) return ("لطفا از کلید بالا(بروز) فایل مورد نظر انتخاب نمایید");
//                var fileInfo = new System.IO.FileInfo(formFile.FileName);

//                foreach (var item in DataLayer.Enums.DefualtValue.ImgExtention)
//                {
//                    if (item.Value.ToLower() == fileInfo.Extension.ToLower()) flagTrueExtention = true;
//                }
//                if (!flagTrueExtention | formFile.Length > 1000000)
//                {
//                  return ("سایز فایل نباید بزرگتر از یک مگا بایت باشد وفرمت عکس یکی از فرمت های رایج عکس باید باشد");
//                }

//                var filename = Guid.NewGuid() ;

//                if (formFile.Length > 0)
//                {
//                    var filePath = physicalPath + filename + fileInfo.Extension;

//                    using (var stream = System.IO.File.Create(filePath))
//                    {
//                        await formFile.CopyToAsync(stream);
//                    }
//                }
//                imageEditor.saveImage(physicalPath + filename + fileInfo.Extension, physicalPath + filename + 1 + fileInfo.Extension, new Size(300, 300));
//                return  (string.Concat( Paths.AdminUploadUri ,filename , fileInfo.Extension));
//            }
//            catch (Exception ex)
//            {
//                return  ("بار گذاری فایل با مشکل مواجه شده است");
//                // throw ex;
//            }


//        }

//        [AllowAnonymous]
//        [HttpPost]
//        [AcceptVerbs()]
//        public async System.Threading.Tasks.Task<string> UploadFileAsync(List<IFormFile> files, string InputName)
//        {
//            try
//            {
//                if (!ValidateAccessToActionBool(RolesSystem.UserValue)) { return "لطفا لاگین نمایید"; };
//                string webRootPath = _hostingEnvironment.WebRootPath;
//                string physicalPath = webRootPath.Replace("Upload\\UploadFile", Paths.UserUploadFiles);

//                var formFile = files.FirstOrDefault();
//                var fileInfo = new System.IO.FileInfo(formFile.FileName);
    
//                foreach (var item in DataLayer.Enums.DefualtValue.TextExtention)
//                {
//                    if (item.Value.ToLower() == fileInfo.Extension.ToLower()) flagTrueExtention = true;
//                }
//                if (!flagTrueExtention) return UIUtility.ResourceManager.GetString("NoSupported_Format");

//                if (!flagTrueExtention | formFile.Length > 2000000)
//                {
//                    throw new MyException((byte)ExceptionType.validation, "validation", "سایز فایل نباید بزرگتر از دو مگا بایت باشد وفرمت زیپ (رر) یا  فایل متنی  باید باشد");
//                }

//                var filename = Guid.NewGuid() + fileInfo.Extension;


//                if (formFile.Length > 0)
//                {
//                    var filePath = physicalPath + filename + fileInfo.Extension;

//                    using (var stream = System.IO.File.Create(filePath))
//                    {
//                        await formFile.CopyToAsync(stream);
//                    }
//                }


//                return (Paths.AdminUploadUri + filename);
//            }
//            catch (Exception ex)
//            {
//                return "آپلود فایل نا موفق بود";

//                // throw;
//            }

//        }

//        [AllowAnonymous]
//        [HttpPost]
//        [AcceptVerbs()]
//        public async System.Threading.Tasks.Task<string> UploadFileAdminSiteAsync(List<IFormFile> files, string InputName)
//        {
//            if (!ValidateAccessToActionBool(RolesSystem.AdminValue)) { return "لاگین نمایید"; };
//            try
//            {
//                string webRootPath = _hostingEnvironment.WebRootPath;
//                string physicalPath = webRootPath.Replace("Upload\\UploadFileAdminSite", Paths.AdminSiteFilePath);

//               var formFile = files.FirstOrDefault();
//                var fileInfo = new System.IO.FileInfo(formFile.FileName);

//                if (formFile.Length > 1000000)
//                {
//                    throw new MyException((byte)ExceptionType.validation, "validation", "سایز فایل نباید بزرگتر از یک مگا بایت باشد وفرمت عکس یکی از فرمت های رایج عکس باید باشد");
//                }

//                var filename = Guid.NewGuid() + fileInfo.Extension;

//               // Request.Files[InputName].SaveAs(physicalPath + filename);

//                if (formFile.Length > 0)
//                {
//                    var filePath = physicalPath + filename + fileInfo.Extension;

//                    using (var stream = System.IO.File.Create(filePath))
//                    {
//                        await formFile.CopyToAsync(stream);
//                    }
//                }

//                return (Paths.AdminSiteFileUri + filename);
//            }
//            catch (Exception ex)
//            {
//                return UIUtility.ResourceManager.GetString("No_succece_Upload_file");

//                // throw;
//            }

//        }


//        // [XmlRoot("urlset")]
//        // [DataContract(Name = "urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
//        //     [XmlArrayItem("urlset")]
//        //[XmlArray("BarList"), XmlArrayItem(typeof(Bar), ElementName = "Bar")]
//        //  [XmlArray(ElementName = "urlset")]
//        //  [XmlType("link")]
//        //        [XmlArray(ElementName = "MyStrings",
//        //Namespace = "DataLayer", IsNullable = true)]
//        //        [XmlAttribute("urlset")]

            
//        public string sitemapAddress { get { return ConfigurationExtensions.GetConnectionString(AppSetting.Configuration, "sitemapAddress"); } }


//        //[XmlArray("urlsetw")]
//        //[XmlElement("url")]
//        public ActionResult UpdateSiteMap(string lastChangedate = "1392/08/08")
//        {
//             if (!ValidateAccessToActionBool(RolesSystem.UserValue)) return RedirectToAction("LoginView", "User"); 

//            try
//            {
//                ProductService _Productservice = new ProductService(objectContext);
//                CategoryService _categoryService = new CategoryService(objectContext);
//                BusinessOwnerService _businessOwnerService = new BusinessOwnerService(objectContext);

//                string contentRootPath = _hostingEnvironment.ContentRootPath;
//                string FirstSitemapFileList = contentRootPath + "FirstSitemapFileList.xml";
//                string LastSitemapFileList = contentRootPath + "LastSitemapFileList.xml";
//                string FinalFile = contentRootPath + sitemapAddress;

//                string trurNumspace = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><urlset   xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"   xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"   xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9   http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\">";
//                string warnigNamespace = "<urlset xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/DataLayer\">";

               
//                string firestSiteMapStr = System.IO.File.ReadAllText(FirstSitemapFileList);
//                string lastSiteMapStr = System.IO.File.ReadAllText(LastSitemapFileList);

              
//                List<url> firestSiteMapList = XmlSerializerUtility.DeSerialize<List<url>>(firestSiteMapStr);
               
//                List<url> ProductSiteMapList = _Productservice.CreateSiteMapList(lastChangedate).ToList();
//                List<url> CategorySiteMapList = _categoryService.CreateSiteMapList(lastChangedate).ToList();
//                List<url> businessOwnerSiteMapList =_businessOwnerService.CreateSiteMapList(lastChangedate).ToList();
               
//                List<url> lastSiteMapList = XmlSerializerUtility.DeSerialize<List<url>>(lastSiteMapStr);
//                List<url> FinalSiteMapList = new List<url>();

//                FinalSiteMapList.AddRange(firestSiteMapList);
//                FinalSiteMapList.AddRange(ProductSiteMapList);
//                FinalSiteMapList.AddRange(CategorySiteMapList);
//                FinalSiteMapList.AddRange(businessOwnerSiteMapList);
//                FinalSiteMapList.AddRange(lastSiteMapList);

//                string FinalSiteMapstr = XmlSerializerUtility.Serialize(FinalSiteMapList);
//                FinalSiteMapstr = FinalSiteMapstr.Replace("ArrayOfurl", "urlset");
//                FinalSiteMapstr = FinalSiteMapstr.Replace("<lastmod />", "");
//                FinalSiteMapstr = FinalSiteMapstr.Replace(warnigNamespace, trurNumspace);
                
//                System.IO.File.WriteAllText(FinalFile, FinalSiteMapstr);
//                return RedirectToAction("ShowMessage", "ShowContent", new { message = "آپدیت فایل سایت مپ انجام شد." });
//            }
//            catch (Exception ex )
//            {
//                string mes = ex.Message!=null? ex.Message:"" + "**********" + ex.InnerException.Message!=null ?ex.InnerException.Message:"";
//                return RedirectToAction("ShowMessage", "ShowContent", new { message = "هشدار فایل سایت مپ ممکن است ازبین رفته باشد بک آپ آن را برگردانید"  +mes});
//            }
 

            
//        }
//    }
//}
