using Microsoft.Extensions.Logging;
using Pizzeria;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.DI;
using PizzeriaContracts.SearchModels;
using PizzeriaDataModels.Models;

namespace PizzeriaView
{
    public partial class FormPizza : Form
    {
        private readonly ILogger _logger;
        private readonly IPizzaLogic _logic;
        private int? _id;
        private Dictionary<int, (IComponentModel, int)> _PizzaComponents;
        public int Id { set { _id = value; } }

        public FormPizza(ILogger<FormPizza> logger, IPizzaLogic logic)
        {
            InitializeComponent();
            _logger = logger;
            _logic = logic;
            _PizzaComponents = new Dictionary<int, (IComponentModel, int)>();
        }

        private void FormPizza_Load(object sender, EventArgs e)
        {
            if (_id.HasValue)
            {
                _logger.LogInformation("Загрузка пиццы");
                try
                {
                    var view = _logic.ReadElement(new PizzaSearchModel
                    {
                        Id = _id.Value
                    });
                    if (view != null)
                    {
                        textBoxName.Text = view.PizzaName;
                        textBoxPrice.Text = view.Price.ToString();
                        _PizzaComponents = view.PizzaComponents ?? new
                        Dictionary<int, (IComponentModel, int)>();
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка загрузки пиццы");
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
        }

        private void LoadData()
        {
            _logger.LogInformation("Загрузка ингредиент пиццы");
            try
            {
                if (_PizzaComponents != null)
                {
                    dataGridView.Rows.Clear();
                    foreach (var pc in _PizzaComponents)
                    {
                        dataGridView.Rows.Add(new object[] { pc.Key, pc.Value.Item1.ComponentName, pc.Value.Item2 });
                    }
                    textBoxPrice.Text = CalcPrice().ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка загрузки ингредиента пиццы");
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            var form = DependencyManager.Instance.Resolve<FormPizzaComponent>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                return;
            }
            _logger.LogInformation("Добавление нового ингридиента:{ ComponentName}-{ Count}", form.ComponentModel.ComponentName, form.Count);
            if (_PizzaComponents.ContainsKey(form.Id))
            {
                _PizzaComponents[form.Id] = (form.ComponentModel,
                form.Count);
            }
            else
            {
                _PizzaComponents.Add(form.Id, (form.ComponentModel,
                form.Count));
            }
            LoadData();
        }

        private void ButtonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = DependencyManager.Instance.Resolve<FormPizzaComponent>();
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                form.Id = id;
                form.Count = _PizzaComponents[id].Item2;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.ComponentModel == null)
                    {
                        return;
                    }
                    _logger.LogInformation("Изменение ингридиента:{ ComponentName}-{ Count}", form.ComponentModel.ComponentName, form.Count);
                    _PizzaComponents[form.Id] = (form.ComponentModel, form.Count);
                    LoadData();
                }
            }
        }

        private void ButtonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        _logger.LogInformation("Удаление ингредиента:{ ComponentName}-{ Count}", dataGridView.SelectedRows[0].Cells[1].Value);
                        _PizzaComponents?.Remove(Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void ButtonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_PizzaComponents == null || _PizzaComponents.Count == 0)
            {
                MessageBox.Show("Заполните ингредиенты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _logger.LogInformation("Сохранение пиццы");
            try
            {
                var model = new PizzaBindingModel
                {
                    Id = _id ?? 0,
                    PizzaName = textBoxName.Text,
                    Price = Convert.ToDouble(textBoxPrice.Text),
                    PizzaComponents = _PizzaComponents
                };
                var operationResult = _id.HasValue ? _logic.Update(model) : _logic.Create(model);
                if (!operationResult)
                {
                    throw new Exception("Ошибка при сохранении. Дополнительная информация в логах.");
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка сохранения пиццы");
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private double CalcPrice()
        {
            double price = 0;
            foreach (var elem in _PizzaComponents)
            {
                price += ((elem.Value.Item1?.Cost ?? 0) * elem.Value.Item2);
            }
            return Math.Round(price * 1.1, 2);
        }
    }
}
