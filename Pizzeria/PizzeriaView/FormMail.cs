using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using Pizzeria;
using PizzeriaBusinessLogic.BusinessLogics;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.DI;
using PizzeriaContracts.SearchModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PizzeriaView
{
    public partial class FormMail : Form
    {
        private readonly ILogger _logger;
        private readonly IMessageInfoLogic _logic;
        private int currentPage = 1;
        private int pageLength = 2;

        public FormMail(ILogger<FormMail> logger, IMessageInfoLogic logic)
        {
            InitializeComponent();
            _logger = logger;
            _logic = logic;
        }

        private void LoadData()
        {
            try
            {
                dataGridView.FillAndConfigGrid(_logic.ReadList(null));
                _logger.LogInformation("Загрузка почтовых собщений");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка загрузки почтовых сообщений");
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormMail_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count <= 0)
                return;

            var service = DependencyManager.Instance.Resolve<FormLetter>();
            if (service is FormLetter form)
            {
                string? messageId = dataGridView.SelectedRows[0].Cells["MessageId"].Value.ToString();
                if (messageId == null) return;
                form.messageId = messageId;

                if (!Convert.ToBoolean(dataGridView.SelectedRows[0].Cells["IsReaded"].Value))
                {
                    _logic.Update(new MessageInfoBindingModel
                    {
                        MessageId = messageId,
                        IsReaded = true,
                        ReplyMessageId = dataGridView.SelectedRows[0].Cells["ReplyMessageId"].Value?.ToString()
                    });
                }

                form.ShowDialog();
                LoadData();
            }
        }

        private void buttonPreveous_Click(object sender, EventArgs e)
        {
            currentPage = Math.Max(1, currentPage - 1);
            LoadData();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadData();
        }

        private void numericUpDownPage_ValueChanged(object sender, EventArgs e)
        {
            pageLength = Math.Max(1, (int)numericUpDownPage.Value);
            LoadData();
        }
    }
}
