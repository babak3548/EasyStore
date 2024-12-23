using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Result<T>
    {
        public bool ActionExecuteSucsess { get; set; }
        public string Message { get; set; }
        public T ResultObject { get; set; }

        public static Result<T> Sucsess(T obj, string messge = "")
        {
            return new Result<T> { ActionExecuteSucsess = true, Message = messge, ResultObject = obj };
        }

        public static Result<T> Fail(T obj, string messge = "")
        {
            return new Result<T> { ActionExecuteSucsess = false, Message = messge, ResultObject = obj };
        }
    }
    }
    

