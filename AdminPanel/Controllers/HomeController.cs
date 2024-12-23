using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AdminPanel.Models;
using AdminPanel.Common;
using Utility;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Net;
using System.IO;

namespace AdminPanel.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        //public IEnumerable<GitHubBranch> Branches { get; private set; }
        public bool GetBranchesError { get; private set; }
        public HomeController(ILogger<HomeController> logger , IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }
       
        public IActionResult Index()
        {
           // ImageProcessing  xx= new ImageProcessing();
          //  xx.VaryQualityLevel();
          //  if (CurrentUserContract == null) return RedirectToAction("Index", "Users");
            return View();
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

        public  IActionResult getDigi()
        {
            //var request = new HttpRequestMessage(HttpMethod.Get,
            //    "https://seller.digikala.com/api/v1/orders/?page=1&size=50&order=asc&sort=order_item_id");
            //request.Headers.Add("Accept", "application/vnd.github.v3+json");

            //var client = _clientFactory.CreateClient();
            //client.DefaultRequestHeaders.Add("Authorization", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzM4NCJ9.eyJ0b2tlbl9pZCI6NzI5LCJwYXlsb2FkIjpudWxsfQ._77FDSwfctW9yt_tWOv9TDED2OeAXzzHdtVCtdZN26hMRF24nuI6WWYBIJ_4BvXx");
            //var response =  client.SendAsync(request).Result;

            //if (response.IsSuccessStatusCode)
            //{
            //    using var responseStream =  response.Content.ReadAsStreamAsync().Result;

            //    //Branches = await JsonSerializer.DeserializeAsync
            //    //    <IEnumerable<GitHubBranch>>(responseStream);
            //}
            //else
            //{
            //    GetBranchesError = true;
            //  //  Branches = Array.Empty<GitHubBranch>();
            //}
            //return View("");

            var url = "https://seller.digikala.com/api/v1/orders/?page=1&size=50&order=asc&sort=order_item_id";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.ContentType = "application/json, application/json;charset=UTF-8";
            httpRequest.Headers["Authorization"] = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzM4NCJ9.eyJ0b2tlbl9pZCI6NzI5LCJwYXlsb2FkIjpudWxsfQ._77FDSwfctW9yt_tWOv9TDED2OeAXzzHdtVCtdZN26hMRF24nuI6WWYBIJ_4BvXx";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
            return View();
        }


    }
}
