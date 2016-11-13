using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace web.Service
{
    public class BHEmail
    {
        public string Subject { get; set; }
        public string To {
            get {
                var debugEmail = System.Configuration.ConfigurationManager.AppSettings["debugEmail"];
                if(string.IsNullOrEmpty(debugEmail))
                {
                    return _To;
                }
                return debugEmail;
            }
            set { _To = value; }
        } string _To;
        public string From { get; set; }
        public string Body { get; set; }
        public string RelayServer { get { return "relay-hosting.secureserver.net";} }
        public void Send()
        {
            MailMessage oMail = new MailMessage();
            oMail.From = new MailAddress(From);
            oMail.To.Add(To);
            oMail.Subject = Subject;
            oMail.Body = Body;
            oMail.IsBodyHtml = true;
            oMail.Bcc.Add("info@huaxiabh.org");

            using (SmtpClient client = new SmtpClient())
            {
                client.Port = 25;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("hong.jack.zhang@gmail.com", "Staticbird6");
                //client.Host = RelayServer;
                client.Send(oMail);
            }
        }
        
        public static void Send(string subject, string body, string to)
        {
            var email = new BHEmail()
            {
                Subject = subject,
                Body = body,
                To = to,
                From= "info@huaxiabh.org"
            };
            email.Send();
        }
    }
}