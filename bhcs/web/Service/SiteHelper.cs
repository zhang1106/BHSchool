using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.AspNet.Identity;

namespace web.Service
{
    public class SiteHelper
    {
        public static string UserName
        {
            get
            {
                return HttpContext.Current.User.Identity.GetUserName();
            }
        }

        public static bool RegsitrationClosed
        {
            get
            {
                var closed = ConfigurationManager.AppSettings["RegistrationClosed"];
                return closed == null || Boolean.Parse(closed);
            }
        }
           
    }
}