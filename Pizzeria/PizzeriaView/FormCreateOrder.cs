using Microsoft.Extensions.Logging;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
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

namespace PizzeriaView
{
    public partial class FormCreateOrder : Form
    {
        private readonly ILogger _logger;
        private readonly IPizzaLogic _logicP;
        private readonly IOrderLogic _logicO;
        private readonly IClientLogic _logicC;
        private List<PizzaViewModel>? _listPizzas;
        private List<ClientViewModel>? _listClients;

        public FormCreateOrder(ILogger<FormCreateOrder> logger, IPizzaLogic logicP, IOrderLogic logicO, IClientLogic logicC)
        {
            InitializeComponent();
            _logger = logger;
            _logicP = logicP;
            _logicO = logicO;
            _logicC = logicC;
        }

        private void FormCreateOrder_Load(object sender, EventArgs e)
        {
            _listPizzas = _logicP.ReadList(null);
            if (_listPizzas != null)
            {
                comboBoxPizza.DisplayMember = "PizzaName";
                comboBoxPizza.ValueMember = "Id";
                comboBoxPizza.DataSource = _listPizzas;
                comboBoxPizza.SelectedItem = null;
                _logger.LogInformation("Загрузка пиццы для заказа");
            }
            _listClients = _logicC.ReadList(null);
            if (_listClients != null)
            {
                comboBoxUser.DisplayMember = "ClientFIO";
                comboBoxUser.ValueMember = "Id";
                comboBoxUser.DataSource = _listClients;
                comboBoxUser.SelectedItem = null;
                _logger.LogInformation("Загрузка клиентов для заказа");
            }
        }

        private void CalcSum()
        {
            if (comboBoxPizza.SelectedValue != null && comboBoxUser.SelectedValue != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int idPizza = Convert.ToInt32(comboBoxPizza.SelectedValue);
                    var Pizza = _logicP.ReadElement(new PizzaSearchModel
                    {
                        Id = idPizza
                    });
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = Math.Round(count * (Pizza?.Price ?? 0), 2).ToString();
                    _logger.LogInformation("Расчет суммы заказа");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка расчета суммы заказа");
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TextBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void ComboBoxPizza_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxPizza.SelectedValue == null)
            {
                MessageBox.Show("Выберите пиццу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxUser.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _logger.LogInformation("Создание заказа");
            try
            {
                var operationResult = _logicO.CreateOrder(new OrderBindingModel
                {
                    ClientId = Convert.ToInt32(comboBoxUser.SelectedValue),
                    PizzaId = Convert.ToInt32(comboBoxPizza.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToDouble(textBoxSum.Text)
                });
                if (!operationResult)
                {
                    throw new Exception("Ошибка при создании заказа. Дополнительная информация в логах.");
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания заказа");
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
