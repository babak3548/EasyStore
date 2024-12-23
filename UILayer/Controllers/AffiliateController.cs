using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.EF;
using DataLayer.EFLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;

namespace UILayer.Controllers
{
    [Route("api/affiliate")]
    [ApiController]
    public class AffiliateController : ControllerBase
    {
        ProductService _service;
        LogService _LogService;
        public AffiliateController(OnlineShopping onlineShopping, EasyStoreLog _EasyStoreLog)
        {
            _service = new ProductService(onlineShopping);
            _LogService = new LogService(_EasyStoreLog);
        }


        // GET: api/Affiliate/5
        [HttpGet( Name = "Get")]
        [Route("{pagenum}")]
        public List<ProductTorob> Get(int pagenum)
        {
         var res=   _service.GetAll().Where(p=>p.Active== true ).Skip((pagenum - 1) * 200)
                .Take(200).Select(p=> new ProductTorob {availability = p.Available > 0 ? "instock" : "NoAvailable" ,
                old_price = (int)p.BeforDiscountPrice, page_url = AppSetting.DomainName + "/product/"+ p.NameForUrll
                , price= (int)p.Price}).ToList();
           
            return res;
        }

        //// POST: api/Affiliate
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Affiliate/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }


    public  class ProductTorob
    {
        public int product_id { get; set; }
        public string page_url { get; set; }
        public int price { get; set; }
        public string availability { get; set; } // 'instock' 
        public int old_price { get; set; }

}
}
