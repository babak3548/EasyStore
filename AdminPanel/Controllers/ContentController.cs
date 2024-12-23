using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.EF;
using DataLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using Utility;
using DataLayer.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminPanel.Controllers
{
    public class ContentController : BaseController
    {
        private ContentService _contentService;

        public ContentController(ContentService contentService)
        {
            _contentService = contentService;
        }
        public IActionResult Index(int? pageIndex)
        {
            var result = _contentService.GetAll(Pagination.Create(pageIndex ?? 1));
            
            return View(result);
        }
        public IActionResult Item(int? id)
        {
            ViewData["ContentType"] = new SelectList(EnumUtility.EnumToList<ContentTypes>(), "Id", "Name");
            if (id== null)
            {
                return View(new Content());
            }

            var content = _contentService.Find(a => a.Id == id.Value).FirstOrDefault();
        
            if (content == null)
                return NotFound();

            // ViewData["ContentType"] = EnumUtility.EnumToCurrentLanguageList<ContentTypes>(((int) content.ContentType).ToString());
          
           
            //
            return View(content);
        }
        [HttpPost]
        public IActionResult Item(Content content,string htmlShowValue)
        {
            if (ModelState.IsValid)
            {
                if (content.Id <= 0)
                {
                    content.RegisterDate = DateTime.Now;
                }
            
                 content.UpdateDate = DateTime.Now;
                if (htmlShowValue.Length > 5) content.ShowValue = htmlShowValue;

                _contentService.Update(content);
                _contentService.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(content);
        }
        public IActionResult Delete(int id)
        {

            var content = _contentService.Find(a => a.Id == id).FirstOrDefault();
            if(content == null)
            {
                return NotFound();
            }
            _contentService.Delete(content);
            _contentService.SaveChanges();
            return RedirectToAction("Index");
        }


        [AllowAnonymous]
        [HttpPost]
        [AcceptVerbs()]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, string inputFileName, int contentId)
        {
            if (files.Count == 0) return new RedirectResult(Url.Action("Item", new { id = contentId }) + "#imgArea");
            // return RedirectToAction("Item", new { id = productId  });
            string physicalPath = new AppSetting().ImagePathOtherFileServer;  // webRootPath.Replace("Upload\\UploadFileAdminSite", Paths.AdminSiteFilePath);

            var formFile = files.FirstOrDefault();

            var content = _contentService.FirstOrDefault(c=>c.Id== contentId);
            if (content == null)
            {
                return NotFound();
            }
            // foreach (var formFile in files) 
            // {
            var fileInfo = new System.IO.FileInfo(formFile.FileName);

            if ( formFile.Length > 30000000  )
            {
                throw new MyException((byte)ExceptionType.validation, "validation", "سایز فایل نباید بزرگتر از 30 مگا بایت باشد و  نباشد وفرمت عکس جیپق باید باشد");
            }

            var filename = Guid.NewGuid() + fileInfo.Extension;

            if (formFile.Length > 0)
            {
                var filePath = physicalPath + "\\" + filename;
                using (var stream = System.IO.File.Create(filePath))
                {
                    await formFile.CopyToAsync(stream);
                }
            }

            if (inputFileName == "BanerImageAdress")
            {
           //   (physicalPath +  content.BanerImageAdress) .RemoveFile(); // عکس های قبلی رو حذف می کنیم
                content.BanerImageAdress = filename;
            }
             if (inputFileName == "VideoAdress")
            {
              //  (physicalPath + content.VideoAdress).RemoveFile();
                 content.VideoAdress = filename;
            }

            _contentService.Update(content);
            _contentService.SaveChanges();

            return new RedirectResult(Url.Action("Item", new { id = contentId }) + "#imgArea");
            //RedirectToAction("Item" , new { id  = productId });
        }
    }
}