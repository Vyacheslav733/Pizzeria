using MailKit.Net.Pop3;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaBusinessLogic.MailWorker
{
    public class MailKitWorker : AbstractMailWorker
    {
        public MailKitWorker(ILogger<MailKitWorker> logger, IMessageInfoLogic messageInfoLogic) : base(logger, messageInfoLogic) { }

        protected override async Task<string?> SendMailAsync(MailSendInfoBindingModel info)
        {
            string? resount = null;
            using var objMailMessage = new MailMessage();
            using var objSmtpClient = new SmtpClient(_smtpClientHost, _smtpClientPort);
            try
            {
                ConfigurateSmtpClient(objSmtpClient);
                CreateMessage(objMailMessage, info);

                if (info is MailReplySendInfoBindingModel replyInfo)
                {
                    objMailMessage.Headers.Add("In-Reply-To", replyInfo.ParentMessageId);
                    objMailMessage.Headers.Add("References", replyInfo.ParentMessageId);

                    string messageGuid = Guid.NewGuid().ToString();
                    objMailMessage.Headers.Add("Message-Id", messageGuid);
                    resount = messageGuid;
                }

                await Task.Run(() => objSmtpClient.Send(objMailMessage));
            }
            catch (Exception)
            {
                throw;
            }
            return resount;
        }

        protected override async Task<List<MessageInfoBindingModel>> ReceiveMailAsync()
        {
            var list = new List<MessageInfoBindingModel>();
            using var client = new Pop3Client();
            await Task.Run(() =>
            {
                try
                {
                    client.Connect(_popHost, _popPort, SecureSocketOptions.SslOnConnect);
                    client.Authenticate(_mailLogin, _mailPassword);
                    for (int i = 0; i < client.Count; i++)
                    {
                        var message = client.GetMessage(i);
                        foreach (var mail in message.From.Mailboxes)
                        {
                            list.Add(new MessageInfoBindingModel
                            {
                                DateDelivery = message.Date.DateTime,
                                MessageId = message.MessageId,
                                SenderName = mail.Address,
                                Subject = message.Subject,
                                Body = message.TextBody
                            });
                        }
                    }
                }
                catch (MailKit.Security.AuthenticationException)
                { }
                finally
                {
                    client.Disconnect(true);
                }
            });
            return list;
        }

        private void CreateMessage(MailMessage objMailMessage, MailSendInfoBindingModel info)
        {
            objMailMessage.From = new MailAddress(_mailLogin);
            objMailMessage.To.Add(new MailAddress(info.MailAddress));
            objMailMessage.Subject = info.Subject;
            objMailMessage.Body = info.Text;
            objMailMessage.SubjectEncoding = Encoding.UTF8;
            objMailMessage.BodyEncoding = Encoding.UTF8;
        }

        private void ConfigurateSmtpClient(SmtpClient objSmtpClient)
        {

            objSmtpClient.UseDefaultCredentials = false;
            objSmtpClient.EnableSsl = true;
            objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            objSmtpClient.Credentials = new NetworkCredential(_mailLogin, _mailPassword);
        }
    }
}
