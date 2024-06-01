using Microsoft.Extensions.Logging;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.SearchModels;

namespace PizzeriaView
{
    public partial class FormImplementer : Form
    {
        private readonly ILogger _logger;
        private readonly IImplementerLogic _logic;
        private int? _id;
        public int Id { set { _id = value; } }

        public FormImplementer(ILogger<FormImplementer> logger, IImplementerLogic logic)
        {
            InitializeComponent();
            _logger = logger;
            _logic = logic;
        }

        private void FormImplementer_Load(object sender, EventArgs e)
        {
            if (_id.HasValue)
            {
                try
                {
                    _logger.LogInformation("Получение исполнителя");
                    var view = _logic.ReadElement(new ImplementerSearchModel
                    {
                        Id = _id.Value
                    });
                    if (view != null)
                    {
                        textBoxFIO.Text = view.ImplementerFIO;
                        textBoxPassword.Text = view.Password;
                        numericUpDownWorkExperience.Value = view.WorkExperience;
                        numericUpDownQualification.Value = view.Qualification;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка получения исполнителя");
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPassword.Text))
            {
                MessageBox.Show("Заполните пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _logger.LogInformation("Сохранение исполнителя");
            try
            {
                var model = new ImplementerBindingModel
                {
                    Id = _id ?? 0,
                    ImplementerFIO = textBoxFIO.Text,
                    Password = textBoxPassword.Text,
                    WorkExperience = (int)numericUpDownWorkExperience.Value,
                    Qualification = (int)numericUpDownQualification.Value
                };
                var operationResult = _id.HasValue ? _logic.Update(model) : _logic.Create(model);
                if (!operationResult)
                {
                    throw new Exception("Ошибка при создании или обновлении. Дополнительная информация в логах.");
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка сохранения исполнителя");
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
