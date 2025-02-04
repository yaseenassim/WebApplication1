using Newtonsoft.Json.Linq;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using System.Diagnostics;

namespace WebApplication1.Services
{
    public class EmailSender
    {
        public static void SendEmail(string senderEmail, string senderName, string reveiverEmail,
            string receiverName, string subject, string message)
        {
            var apiInstance = new TransactionalEmailsApi();
            SendSmtpEmailSender sender = new SendSmtpEmailSender(senderName, senderEmail);

            SendSmtpEmailTo receiver1 = new SendSmtpEmailTo(reveiverEmail, receiverName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(receiver1);

            string HtmlContent = null;
            string TextContent = message;
           
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(sender, To, null, null, HtmlContent, TextContent, subject);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                Console.WriteLine("Brevo Response: " + result.ToJson());
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an exception: " + e.Message);
            }
        }
    }
}
