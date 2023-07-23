using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_Server.Controller
{
    class BaseController
    {
        protected RequestCode requestCode = RequestCode.RequestNone;
        public RequestCode GetRequestCode
        {
            get { return requestCode; }
        }
    }
}
