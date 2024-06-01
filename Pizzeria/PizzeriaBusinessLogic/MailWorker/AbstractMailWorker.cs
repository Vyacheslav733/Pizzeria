using Microsoft.Extensions.Logging;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaBusinessLogic.MailWorker
{
    public abstract class AbstractMailWorker
    {
        protected string _mailLogin = string.Empty;
        protected string _mailPassword = string.Empty;
        protected string _smtpClientHost = string.Empty;
        protected int _smtpClientPort;
        protected string _popHost = string.Empty;
        protected int _popPort;
        private readonly IMessageInfoLogic _messageInfoLogic;
        private readonly ILogger _logger;

        public AbstractMailWorker(ILogger<AbstractMailWorker> logger, IMessageInfoLogic messageInfoLogic)
        {
            _logger = logger;
            _messageInfoLogic = messageInfoLogic;
        }
        public void MailConfig(MailConfigBindingModel config)
        {
            _mailLogin = config.MailLogin;
            _mailPassword = config.MailPassword;
            _smtpClientHost = config.SmtpClientHost;
            _smtpClientPort = config.SmtpClientPort;
            _popHost = config.PopHost;
            _popPort = config.PopPort;
            _logger.LogDebug("Config: {login}, {password}, {clientHost}, {clientPOrt}, {popHost}, {popPort}", _mailLogin, _mailPassword.Length, _smtpClientHost, _smtpClientPort, _popHost, _popPort);
        }
        public async void MailSendAsync(MailSendInfoBindingModel info)
        {
            if (string.IsNullOrEmpty(_mailLogin) || string.IsNullOrEmpty(_mailPassword))
            {
                return;
            }

            if (string.IsNullOrEmpty(_smtpClientHost) || _smtpClientPort == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(info.MailAddress) || string.IsNullOrEmpty(info.Subject) || string.IsNullOrEmpty(info.Text))
            {
                return;
            }

            _logger.LogDebug("Send Mail: {To}, {Subject}", info.MailAddress, info.Subject);
            await SendMailAsync(info);
        }
        public async void MailCheck()
        {
            if (string.IsNullOrEmpty(_mailLogin) || string.IsNullOrEmpty(_mailPassword))
            {
                return;
            }

            if (string.IsNullOrEmpty(_popHost) || _popPort == 0)
            {
                return;
            }

            if (_messageInfoLogic == null)
            {
                return;
            }

            var list = await ReceiveMailAsync();
            _logger.LogDebug("Check Mail: {Count} new mails", list.Count);
            foreach (var mail in list)
            {
                _messageInfoLogic.Create(mail);
            }
        }

        public async void MailSendReplyAsync(MailReplySendInfoBindingModel info)
        {
            if (string.IsNullOrEmpty(_mailLogin) || string.IsNullOrEmpty(_mailPassword))
            {
                return;
            }

            if (string.IsNullOrEmpty(_smtpClientHost) || _smtpClientPort == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(info.MailAddress) || string.IsNullOrEmpty(info.Subject) || string.IsNullOrEmpty(info.Text) || string.IsNullOrEmpty(info.ParentMessageId))
            {
                return;
            }

            _logger.LogDebug("Send Mail as reply: {To}, {Subject}, {parentId}", info.MailAddress, info.Subject, info.ParentMessageId);

            string? messageId = await SendMailAsync(info);
            if (string.IsNullOrEmpty(messageId))
            {
                throw new InvalidOperationException("Непредвиденная ошибка при отправке сообщения в ответ");
            }
            if (_messageInfoLogic.Create(new MessageInfoBindingModel
            {
                MessageId = messageId,
                DateDelivery = DateTime.Now,
                SenderName = _mailLogin,
                IsReply = true,
                Subject = info.Subject,
                Body = info.Text,
            }))
            {
                _messageInfoLogic.Update(new MessageInfoBindingModel()
                {
                    MessageId = info.ParentMessageId,
                    ReplyMessageId = messageId,
                    IsReaded = true
                });
            }
        }

        protected abstract Task<string?> SendMailAsync(MailSendInfoBindingModel info);
        protected abstract Task<List<MessageInfoBindingModel>> ReceiveMailAsync();
    }
}
