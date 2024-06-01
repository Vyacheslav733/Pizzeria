using Microsoft.Extensions.Logging;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PizzeriaView
{
    public partial class FormCreateSupply : Form
    {
        private readonly ILogger _logger;
        private readonly IPizzaLogic _logicP;
        private readonly IShopLogic _logicS;
        private List<ShopViewModel> _shopList = new List<ShopViewModel>();
        private List<PizzaViewModel> _pizzaList = new List<PizzaViewModel>();

        public FormCreateSupply(ILogger<FormCreateSupply> logger, IPizzaLogic logicP, IShopLogic logicS)
        {
            InitializeComponent();
            _logger = logger;
            _logicP = logicP;
            _logicS = logicS;
        }

        private void FormCreateSupply_Load(object sender, EventArgs e)
        {
            _shopList = _logicS.ReadList(null);
            _pizzaList = _logicP.ReadList(null);
            if (_shopList != null)
            {
                comboBoxShop.DisplayMember = "ShopName";
                comboBoxShop.ValueMember = "Id";
                comboBoxShop.DataSource = _shopList;
                comboBoxShop.SelectedItem = null;
                _logger.LogInformation("Загрузка магазинов для поставок");
            }
            if (_pizzaList != null)
            {
                comboBoxPizza.DisplayMember = "PizzaName";
                comboBoxPizza.ValueMember = "Id";
                comboBoxPizza.DataSource = _pizzaList;
                comboBoxPizza.SelectedItem = null;
                _logger.LogInformation("Загрузка пиццы для поставок");
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxShop.SelectedValue == null)
            {
                MessageBox.Show("Выберите магазин", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxPizza.SelectedValue == null)
            {
                MessageBox.Show("Выберите пиццу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _logger.LogInformation("Создание поставки");
            try
            {
                var operationResult = _logicS.MakeSupply(new SupplyBindingModel
                {
                    ShopId = Convert.ToInt32(comboBoxShop.SelectedValue),
                    PizzaId = Convert.ToInt32(comboBoxPizza.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                if (!operationResult)
                {
                    throw new Exception("Ошибка при создании поставки. Дополнительная информация в логах.");
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания поставки");
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
