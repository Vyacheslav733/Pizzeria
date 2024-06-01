namespace PizzeriaView
{
    partial class FormImplementer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxFIO = new System.Windows.Forms.TextBox();
            this.labelFIO = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelWorkExperience = new System.Windows.Forms.Label();
            this.numericUpDownWorkExperience = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownQualification = new System.Windows.Forms.NumericUpDown();
            this.labelQualification = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWorkExperience)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQualification)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxFIO
            // 
            this.textBoxFIO.Location = new System.Drawing.Point(157, 12);
            this.textBoxFIO.Name = "textBoxFIO";
            this.textBoxFIO.Size = new System.Drawing.Size(382, 27);
            this.textBoxFIO.TabIndex = 3;
            // 
            // labelFIO
            // 
            this.labelFIO.AutoSize = true;
            this.labelFIO.Location = new System.Drawing.Point(12, 15);
            this.labelFIO.Name = "labelFIO";
            this.labelFIO.Size = new System.Drawing.Size(139, 20);
            this.labelFIO.TabIndex = 2;
            this.labelFIO.Text = "ФИО исполнителя:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(157, 50);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(205, 27);
            this.textBoxPassword.TabIndex = 5;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(12, 53);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(65, 20);
            this.labelPassword.TabIndex = 4;
            this.labelPassword.Text = "Пароль:";
            // 
            // labelWorkExperience
            // 
            this.labelWorkExperience.AutoSize = true;
            this.labelWorkExperience.Location = new System.Drawing.Point(12, 99);
            this.labelWorkExperience.Name = "labelWorkExperience";
            this.labelWorkExperience.Size = new System.Drawing.Size(105, 20);
            this.labelWorkExperience.TabIndex = 6;
            this.labelWorkExperience.Text = "Опыт работы:";
            // 
            // numericUpDownWorkExperience
            // 
            this.numericUpDownWorkExperience.Location = new System.Drawing.Point(157, 97);
            this.numericUpDownWorkExperience.Name = "numericUpDownWorkExperience";
            this.numericUpDownWorkExperience.Size = new System.Drawing.Size(124, 27);
            this.numericUpDownWorkExperience.TabIndex = 8;
            // 
            // numericUpDownQualification
            // 
            this.numericUpDownQualification.Location = new System.Drawing.Point(157, 142);
            this.numericUpDownQualification.Name = "numericUpDownQualification";
            this.numericUpDownQualification.Size = new System.Drawing.Size(124, 27);
            this.numericUpDownQualification.TabIndex = 10;
            // 
            // labelQualification
            // 
            this.labelQualification.AutoSize = true;
            this.labelQualification.Location = new System.Drawing.Point(12, 144);
            this.labelQualification.Name = "labelQualification";
            this.labelQualification.Size = new System.Drawing.Size(114, 20);
            this.labelQualification.TabIndex = 9;
            this.labelQualification.Text = "Квалификация:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(422, 203);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(136, 40);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(265, 203);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(130, 40);
            this.buttonSave.TabIndex = 11;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // FormImplementer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 255);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.numericUpDownQualification);
            this.Controls.Add(this.labelQualification);
            this.Controls.Add(this.numericUpDownWorkExperience);
            this.Controls.Add(this.labelWorkExperience);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textBoxFIO);
            this.Controls.Add(this.labelFIO);
            this.Name = "FormImplementer";
            this.Text = "Исполнитель";
            this.Load += new System.EventHandler(this.FormImplementer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWorkExperience)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQualification)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBoxFIO;
        private Label labelFIO;
        private TextBox textBoxPassword;
        private Label labelPassword;
        private Label labelWorkExperience;
        private NumericUpDown numericUpDownWorkExperience;
        private NumericUpDown numericUpDownQualification;
        private Label labelQualification;
        private Button buttonCancel;
        private Button buttonSave;
    }
}