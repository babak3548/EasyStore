using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnarSoft.UILayer.Hubs
{
    public class SeetionChat
    {
        string Title;//مثلا در مورد کالای 1
        string TypeSubject;
        List<string> ListMessage;
        Person personStarter;
        Person personPartner;
        public SeetionChat()
        {
            ListMessage = new List<string>();
        }

    }
}