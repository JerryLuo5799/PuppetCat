using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PuppetCat.AspNetCore.Core
{
    public class MailUtils
    {
        /// <summary>
        /// send email
        /// </summary>
        /// <param name="smtpserver">SMTP</param>
        /// <param name="enableSsl">is enable SSL</param>
        /// <param name="userName">account</param>
        /// <param name="pwd">password</param>
        /// <param name="nickName">from nickname</param>
        /// <param name="fromEmail">from</param>
        /// <param name="toEmail">to</param>
        /// <param name="subj">subject</param>
        /// <param name="bodys">content</param>
        public static async Task SendMailAsync(string smtpserver, bool enableSsl, string userName, string pwd, string nickName, string fromMail, string toMail, string subj, string bodys)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Host = smtpserver;
            smtpClient.Credentials = new NetworkCredential(userName, pwd);
            smtpClient.EnableSsl = enableSsl;
            MailMessage mailMessage = new MailMessage();
            MailAddress fromAddress = new MailAddress(fromMail, nickName);
            mailMessage.From = fromAddress;
            string[] arrToMail = toMail.Split(';');
            foreach(string to in arrToMail)
            {
                if(!string.IsNullOrEmpty(to))
                {
                    mailMessage.To.Add(to);
                }
            }
            //MailAddress toAddress = new MailAddress(toMail);
            //MailMessage mailMessage = new MailMessage(fromAddress, toAddress);
            mailMessage.Subject = subj;
            mailMessage.Body = bodys;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.Normal;
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
