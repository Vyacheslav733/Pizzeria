using Microsoft.Extensions.Logging;
using Microsoft.Reporting.WinForms;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
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
    public partial class FormReportGroupedOrders : Form
    {
        private readonly ReportViewer reportViewer;
        private readonly ILogger _logger;
        private readonly IReportLogic _logic;

        public FormReportGroupedOrders(ILogger<FormReportGroupedOrders> logger, IReportLogic logic)
        {
            InitializeComponent();
            _logger = logger;
            _logic = logic;
            reportViewer = new ReportViewer
            {
                Dock = DockStyle.Fill
            };
            reportViewer.LocalReport.LoadReportDefinition(new FileStream("ReportGroupedOrders.rdlc", FileMode.Open));
            Controls.Clear();
            Controls.Add(reportViewer);
            Controls.Add(panel);
        }

        private void ButtonMake_Click(object sender, EventArgs e)
        {
            try
            {
                var dataSource = _logic.GetGroupedOrders();
                var source = new ReportDataSource("DataSetGroupedOrders", dataSource);
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(source);

                reportViewer.RefreshReport();
                _logger.LogInformation("Загрузка списка группированных заказов");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка загрузки списка группированных заказов на период");
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonToPDF_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog { Filter = "pdf|*.pdf" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _logic.SaveGroupedOrdersToPdfFile(new ReportBindingModel
                    {
                        FileName = dialog.FileName,
                    });
                    _logger.LogInformation("Сохранение списка группированных заказов");
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка сохранения списка группированных заказов");
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
