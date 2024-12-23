using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HttpResponse = Microsoft.AspNetCore.Http.HttpResponse;

namespace UILayer.BankGetWays.Meli
{
    public class Meli
    {
       public const string _MerchantKey = "";
        public const string _MerchantId = "";
       public const string _TerminalId = "";
       public const string _PurchasePage = "";
        public Task< PayResultData> GetToken(string paymentId, long amount , HttpResponse response)
        {
            PaymentRequest request = new PaymentRequest();

            request.MerchantKey = _MerchantKey;
            request.MerchantId = _MerchantId;
            request.TerminalId = _TerminalId;

            request.PurchasePage = "";

            request.OrderId = paymentId;
            request.Amount = amount;

            try
            {
               // request.OrderId = new Random().Next(1000, int.MaxValue).ToString();
                var dataBytes = Encoding.UTF8.GetBytes(string.Format("{0};{1};{2}", request.TerminalId, request.OrderId, request.Amount));

                var symmetric = SymmetricAlgorithm.Create("TripleDes");
                symmetric.Mode = CipherMode.ECB;
                symmetric.Padding = PaddingMode.PKCS7;

                var encryptor = symmetric.CreateEncryptor(Convert.FromBase64String(request.MerchantKey), new byte[8]);

                request.SignData = Convert.ToBase64String(encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length));

               // if (HttpContext.Request.Url != null)
                    request.ReturnUrl = string.Format("{0}/invoice/RedirectMeli", AppSetting.DomainName);

                var ipgUri = string.Format("{0}/api/v0/Request/PaymentRequest", request.PurchasePage);
                request.LocalDateTime = DateTime.Now;

                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(5)
                };
                response.Cookies.Append("Data", JsonConvert.SerializeObject(request), cookieOptions);


                var data = new
                {
                    request.TerminalId,
                    request.MerchantId,
                    request.Amount,
                    request.SignData,
                    request.ReturnUrl,
                    LocalDateTime = DateTime.Now,
                    request.OrderId,
                    //MultiplexingData = request.MultiplexingData
                };

                var res = CallApi<PayResultData>(ipgUri, data);
                res.Wait();

                return  res;
                //if (res != null && res.Result != null)
                //{
                //    if (res.Result.ResCode == "0")
                //    {
                //        Response.Redirect(string.Format("{0}/Purchase/Index?token={1}", request.PurchasePage, res.Result.Token));
                //    }
                //    ViewBag.Message = res.Result.Description;
                //    return View(); ;
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

   

      
        public Result<PurchaseResult>   Verify(PurchaseResult result)
        {
            try
            {
               // var cookie = Request.Cookies["Data"].Value;
              //  var model = JsonConvert.DeserializeObject<PaymentRequest>(cookie);

                var dataBytes = Encoding.UTF8.GetBytes(result.Token);

                var symmetric = SymmetricAlgorithm.Create("TripleDes");
                symmetric.Mode = CipherMode.ECB;
                symmetric.Padding = PaddingMode.PKCS7;

                var encryptor = symmetric.CreateEncryptor(Convert.FromBase64String(_MerchantKey), new byte[8]);

                var signedData = Convert.ToBase64String(encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length));

                var data = new
                {
                    token = result.Token,
                    SignData = signedData
                };

                var ipgUri = string.Format("{0}/api/v0/Advice/Verify", _PurchasePage);

                var res = CallApi<VerifyResultData>(ipgUri, data);
                if (res != null && res.Result != null)
                {
                    if (res.Result.ResCode == "0")
                    {
                        result.VerifyResultData = res.Result;
                        res.Result.Succeed = true;
                        return Result<PurchaseResult>.Sucsess(result,res.Result.Description 
                            + " RetrivalRefNo:" + res.Result.RetrivalRefNo + " SystemTraceNo:" + res.Result.SystemTraceNo); 
                       // ViewBag.Success = res.Result.Description;
                       // return View("Verify", result);
                    }
                   // ViewBag.Message = res.Result.Description;
                 //   return View("Verify");
                 return  Result<PurchaseResult>.Fail(result , res.Result.Description + "ResCode:" + res.Result.ResCode);
                }
            }
            catch (Exception ex)
            {
                // ViewBag.Message = ex.ToString();
                return Result<PurchaseResult>.Fail(result, "پرداخت ناموفق بود اگر مبلغی از حساب شما کم شده باشد به زودی به حساب شما بر می گردد"); 
            }

            return   Result<PurchaseResult>.Fail(result,"پرداخت ناموفق بود اگر مبلغی از حساب شما کم شده باشد به زودی به حساب شما بر می گردد"); 
        }


        public static async Task<T> CallApi<T>(string apiUrl, object value)
        {
          
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls; // | SecurityProtocolType.Ssl3;
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    var w = client.PostAsJsonAsync(apiUrl, value);
                    w.Wait();
                    HttpResponseMessage response = w.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadFromJsonAsync<T>();
                        result.Wait();
                        return result.Result;
                    }
                    return default(T);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
    }
}
