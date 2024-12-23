using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class ExceptionForDisplay : Exception
    {
        public ExceptionForDisplay(string message) : base(message)
        {
        }
    }
}
