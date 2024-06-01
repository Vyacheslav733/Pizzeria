using Microsoft.Extensions.Logging;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
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
    public partial class FormSellPizza : Form
    {
        private readonly ILogger _logger;
        private readonly IPizzaLogic _logicP;
        private readonly IShopLogic _logicS;
        private List<PizzaViewModel> _pizzaList = new List<PizzaViewModel>();

        public FormSellPizza(ILogger<FormSellPizza> logger, IPizzaLogic logicP, IShopLogic logicS)
        {
            InitializeComponent();
            _logger = logger;
            _logicP = logicP;
            _logicS = logicS;
        }

        private void FormSellingPizza_Load(object sender, EventArgs e)
        {
            _pizzaList = _logicP.ReadList(null);
            if (_pizzaList != null)
            {
                comboBoxPizza.DisplayMember = "PizzaName";
                comboBoxPizza.ValueMember = "Id";
                comboBoxPizza.DataSource = _pizzaList;
                comboBoxPizza.SelectedItem = null;
                _logger.LogInformation("Загрузка пиццы для продажи");
            }
        }

        private void ButtonSell_Click(object sender, EventArgs e)
        {
            if (comboBoxPizza.SelectedValue == null)
            {
                MessageBox.Show("Выберите пиццу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _logger.LogInformation("Создание покупки");
            try
            {
                bool resout = _logicS.Sale(new SupplySearchModel
                {
                    PizzaId = Convert.ToInt32(comboBoxPizza.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                if (resout)
                {
                    _logger.LogInformation("Проверка пройдена, продажа  проведена");
                    MessageBox.Show("Продажа проведена", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    _logger.LogInformation("Проверка не пройдена");
                    MessageBox.Show("Продажа не может быть создана.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания покупки");
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