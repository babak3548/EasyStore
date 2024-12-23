using DataLayer.EFLog;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class LogService : BaseServiceLog<Log>
    {


        public LogService(EasyStoreLog EasyStoreLog)
            : base(EasyStoreLog)
        {
        }

        private string jsonSerializer(object obj)
        {

            try
            {
                if (obj == null || obj == "")
                {
                    return "";
                }
                var str = JsonSerializer.Serialize(obj);
                return str;
            }
            catch (Exception)
            {

                return "";
            }
        }
        public Log SaveInfo(string message, string traceIdentifier, object obj)
        {
            Log _Log = new Log();
            _Log.LogType = 2; //1 = error
            _Log.Message = message;
            _Log.StackTrace = "";
            _Log.InnerMessage = jsonSerializer(obj);
            _Log.TraceIdentifier = traceIdentifier.Length > 50 ?  traceIdentifier.Substring(0,48) : traceIdentifier;
            _Log.RegisterDate = DateTime.Now;

            _EasyStoreLog.Add(_Log);

            _EasyStoreLog.SaveChanges();
            return _Log;
        }
        public async Task<Log> SaveInfoAsync(string message, string traceIdentifier, object obj)
        {
            Log _Log = new Log();
            _Log.LogType = 2; //1 = error
            _Log.Message = message;
            _Log.StackTrace = "";
            _Log.InnerMessage = jsonSerializer(obj);
            _Log.TraceIdentifier = traceIdentifier;
            _Log.RegisterDate = DateTime.Now;

            _EasyStoreLog.Add(_Log);

            await _EasyStoreLog.SaveChangesAsync();
            return _Log;
        }
        public async Task<Log> SaveException(HttpContext httpContext, Exception exception, string traceIdentifier)
        {
          //  string query = "";
           // string Form = "";
            string QueryString = "";
            string Path = "";
            string hostValue = "";
            try
            {
           // query = httpContext.Request.Query != null ? jsonSerializer(httpContext.Request.Query) : "";
           // Form = httpContext.Request.Body != null ? jsonSerializer(httpContext.Request.Form) : "";
            QueryString = httpContext.Request.QueryString != null ? httpContext.Request.QueryString.Value : "";
            Path = httpContext.Request.Path != null ? httpContext.Request.Path.Value: "";
            hostValue = httpContext.Request.Host != null ? httpContext.Request.Host.Value : "";
            }
            catch (Exception e )
            {
            
            }


            Log _Log = new Log();
            _Log.LogType = 1; //1 = error
            _Log.Message = exception.Message;
            _Log.StackTrace = exception.StackTrace;
            _Log.InnerMessage =string.Concat( exception.InnerException?.Message , Environment.NewLine, " HttpContext Info : ", Environment.NewLine, hostValue, Environment.NewLine, Path, Environment.NewLine,  QueryString, Environment.NewLine) ;
            _Log.TraceIdentifier = traceIdentifier;
            _Log.RegisterDate = DateTime.Now;


            _EasyStoreLog.Add(_Log);
            try
            {
                await _EasyStoreLog.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var str = JsonSerializer.Serialize(_Log);
                string path = AppSetting.BaseServerPath + "\\logs\\";

                bool exists = System.IO.Directory.Exists(path);

                if (!exists)
                    System.IO.Directory.CreateDirectory(path);

                System.IO.File.AppendAllText(path + DateTime.Now.ToString("dd-MM-yy") + ".txt", str);
                throw;
            }

            return _Log;
        }

        public async Task<Log> SaveException(Exception exception, string traceIdentifier)
        {
            Log _Log = new Log();
            _Log.LogType = 1; //1 = error
            _Log.Message = exception.Message;
            _Log.StackTrace = exception.StackTrace;
            _Log.InnerMessage = exception.InnerException?.Message;
            _Log.TraceIdentifier = traceIdentifier;
            _Log.RegisterDate = DateTime.Now;


            _EasyStoreLog.Add(_Log);
            try
            {
                await _EasyStoreLog.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var str = JsonSerializer.Serialize(_Log);
                string path = AppSetting.BaseServerPath + "\\logs\\";

                bool exists = System.IO.Directory.Exists(path);

                if (!exists)
                    System.IO.Directory.CreateDirectory(path);

                System.IO.File.AppendAllText(path + DateTime.Now.ToString("dd-MM-yy") + ".txt", str);
                throw;
            }

            return _Log;
        }
    }
}
