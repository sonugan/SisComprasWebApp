using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EnvioEmail
{
    public class EmailSender
    {
        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="subject">Subject of the email to send</param>
        /// <param name="text">Body of the email to send</param>
        /// <param name="to">Email recipient(s)</param>
        /// <param name="from">Sender address</param>
        /// <param name="cc"></param>
        public static void SendMail(string smtp, string puerto, Email email)
        {
            if (string.IsNullOrEmpty(email.Desde)) return;//Cambios Mail
            
            SmtpClient mySmtpClient = new SmtpClient(smtp);
            mySmtpClient.Port = Convert.ToInt32(puerto);

            //SmtpClient mySmtpClient = new SmtpClient(ConfigurationManager.AppSettings.Get("SMTPServer"));

            //mySmtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("SMTPPort"));
            mySmtpClient.EnableSsl = false;
            mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mySmtpClient.UseDefaultCredentials = false;

            mySmtpClient.Credentials = new System.Net.NetworkCredential();

            // add from,to mailaddresses
            MailAddress fromAddress = new MailAddress(email.Desde);

            MailMessage myMail = new System.Net.Mail.MailMessage();

            myMail.From = fromAddress;

            foreach (string rec in email.Para.Split(','))
            {
                if (!string.IsNullOrEmpty(rec))//Cambios Mail
                    myMail.To.Add(new MailAddress(rec));
            }

            if (myMail.To.Count == 0) return; //Cambios Mail

            //Usuarios en copia
            if (!string.IsNullOrEmpty(email.Cc))
            {
                foreach (string rec in email.Cc.Split(','))
                {
                    if (!string.IsNullOrEmpty(rec))
                        myMail.CC.Add(new MailAddress(rec));
                }
            }

            // set subject and encoding
            myMail.Subject = email.Asunto;
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            // set body-message and encoding
            myMail.Body = email.Mensaje;
            myMail.BodyEncoding = System.Text.Encoding.UTF8;
            // text or html
            myMail.IsBodyHtml = true;
            
            mySmtpClient.Send(myMail);
        }
    }
}
