using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
   public  class MyException :Exception
    {
       public string Title { get; set; }
       public byte ExceptionType { get; set; }
       public Exception BaseException { get; set; }
       public MyException(byte exceptionType, string title, string message, Exception baseException=null)
           : base(message)
       {
           Title = title;
           ExceptionType = exceptionType;
           BaseException=baseException;
           
       }
    }
}
