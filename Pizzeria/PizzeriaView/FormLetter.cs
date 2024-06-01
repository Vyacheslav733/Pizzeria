using Microsoft.Extensions.Logging;
using Pizzeria;
using PizzeriaBusinessLogic.MailWorker;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.DI;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PizzeriaView
{
    public partial class FormLetter : Form
    {
        private readonly ILogger _logger;
        private readonly IMessageInfoLogic _logic;
        private readonly AbstractMailWorker _worker;

        public MessageInfoViewModel? model;
        public string? messageId;

        public FormLetter(ILogger<FormLetter> logger, IMessageInfoLogic logic, AbstractMailWorker worker)
        {
            InitializeComponent();
            _logger = logger;
            _logic = logic;
            _worker = worker;
        }

        private void FormLetter_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(messageId))
            {
                ReloadLetter();
                return;
            }
            else if (model != null)
            {
                ConfigurateToCreateAnsver();
                return;
            }
            _logger.LogError("Для формы не переданно сведений о письме, на которое отвечаем!");
            DialogResult = DialogResult.Abort;
            Close();
            return;
        }

        private void ReloadLetter()
        {
            _logger.LogInformation("Загрузка существующего письма с id:{}", messageId);
            model = _logic.ReadElement(new MessageInfoSearchModel
            {
                MessageId = messageId
            });
            if (model != null)
            {
                _logger.LogInformation("Письмо найдено");
                textBoxEmail.Text = model.SenderName;
                textBoxDate.Text = model.DateDelivery.ToString();
                textBoxSubject.Text = model.Subject;
                textBoxBody.Text = model.Body;


                if (model.IsReply)
                {
                    _logger.LogInformation("Письмо само и есть ответ");
                    buttonReply.Visible = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.ReplyMessageId))
                    {
                        _logger.LogInformation("У письма есть ответ.");
                        buttonReply.Text = "Прочитать ответ";
                    }
                }
                return;
            }
            _logger.LogWarning("Письмо с таким id не удалось найти");
            DialogResult = DialogResult.Abort;
            Close();
            return;
        }

        private void ConfigurateToCreateAnsver()
        {
            textBoxEmail.Text = model.SenderName;
            labelDate.Visible = false;
            textBoxDate.Visible = false;
            textBoxSubject.Text = $"re: {model.Subject}";
            textBoxBody.ReadOnly = false;
            buttonReply.Visible = false;
            buttonSend.Visible = true;
            _logger.LogInformation("Запущена форма создания нового письма - ответа");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonReply_Click(object sender, EventArgs e)
        {
            var service = DependencyManager.Instance.Resolve<FormLetter>();
            if (service is FormLetter form)
            {
                if (!string.IsNullOrEmpty(model.ReplyMessageId))
                {
                    form.messageId = model.ReplyMessageId;
                }
                else
                {
                    form.model = model;
                }

                if (form.ShowDialog() != DialogResult.Cancel)
                {
                    buttonReply.Visible = false;
                }
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (model == null)
            {
                return;
            }
            string subject = textBoxSubject.Text;
            string text = textBoxBody.Text;

            Task.Run(() => _worker.MailSendReplyAsync(new MailReplySendInfoBindingModel
            {
                MailAddress = model.SenderName,
                Subject = subject,
                Text = text,
                ParentMessageId = model.MessageId,
            }));
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
