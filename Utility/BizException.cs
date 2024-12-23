using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class BizException:Exception
    {
        public BizException(string message):base(message)
        {

        }
        public BizException(string message,Exception ex) : base(message, ex)
        {

        }
    }
}
