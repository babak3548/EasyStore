using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.EF;
using DataLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;
using UILayer.Models;

namespace UILayer.Controllers
{
    public class ContentController : Base0Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly ILogger<HomeController> _logger;
        ContentService _contentService;
        ProductService _service;

        public ContentController(ILogger<HomeController> logger, OnlineShopping _onlineShopping) : base("", _onlineShopping)
        {
            _logger = logger;
            _contentService = new ContentService(_onlineShopping);
            _service = new ProductService(_onlineShopping);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ShowContent(int contentId, string title = "")
        {
            ViewData["contents"] = _contentService.GetAll().ToList();
            var content = _contentService.FirstOrDefault(c => c.Id == contentId);
            if (content.ContentType == ContentTypes.Other)
            {
                return View("Other", content);
            }
            else
            {
                return View(content);
            }
           
        }

        public IActionResult ListByContentType(ContentTypes contentType)
        {
            return View("ContentList", _contentService.GetAll().Where(c => c.ContentType == contentType).ToList());
        }
        public IActionResult AllContent()
        {
            return View("ContentList", _contentService.GetAll().Where(c => c.ContentType != ContentTypes.UnActive && c.ContentType != ContentTypes.Other).OrderByDescending(o=>o.Id).ToList());
        }

        public IActionResult SearchContent(string searchValue)
        {
            var result = searchValue.Split(" ");

            IQueryable<Content> iquery = _contentService.GetAll();

            if (result.Length == 1) iquery = _contentService.GetAll().Where(c => c.Abstract.Contains(result[0]));
            else if (result.Length == 2) iquery = _contentService.GetAll().Where(c => c.Abstract.Contains(result[0]) || c.Abstract.Contains(result[1]));
            else if (result.Length >= 3) iquery = _contentService.GetAll().Where(c => c.Abstract.Contains(result[0]) || c.Abstract.Contains(result[1]) || c.Abstract.Contains(result[2]));

            return View("ContentList", iquery.ToList());
        }
    }
}