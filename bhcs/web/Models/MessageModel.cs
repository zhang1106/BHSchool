using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using data; 


namespace web.Models
{
    public class MessageModel
    {
        public message Current { get; set; }

        public IList<message> Messages { get; set; }

        public static IList<message> Documents {
            get
            {
                var dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(@"~\Content\documents"));
                var files = dir.GetFiles();
                var docs = from f in files
                           select new message() { title = f.Name , body=$"/Content/documents/{f.Name}"};
                return docs.ToList();
            }
        }
    }
}