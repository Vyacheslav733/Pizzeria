namespace PizzeriaView
{
    partial class FormMail
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.numericUpDownPage = new System.Windows.Forms.NumericUpDown();
            this.buttonPreveous = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPage)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView);
            this.panel1.Location = new System.Drawing.Point(3, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(795, 431);
            this.panel1.TabIndex = 0;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersWidth = 51;
            this.dataGridView.RowTemplate.Height = 29;
            this.dataGridView.Size = new System.Drawing.Size(795, 431);
            this.dataGridView.TabIndex = 2;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(806, 80);
            this.buttonOpen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(107, 31);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "Прочитать";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // numericUpDownPage
            // 
            this.numericUpDownPage.Location = new System.Drawing.Point(825, 287);
            this.numericUpDownPage.Name = "numericUpDownPage";
            this.numericUpDownPage.Size = new System.Drawing.Size(85, 27);
            this.numericUpDownPage.TabIndex = 4;
            this.numericUpDownPage.ValueChanged += new System.EventHandler(this.numericUpDownPage_ValueChanged);
            // 
            // buttonPreveous
            // 
            this.buttonPreveous.Location = new System.Drawing.Point(825, 323);
            this.buttonPreveous.Name = "buttonPreveous";
            this.buttonPreveous.Size = new System.Drawing.Size(39, 29);
            this.buttonPreveous.TabIndex = 5;
            this.buttonPreveous.Text = "<-";
            this.buttonPreveous.UseVisualStyleBackColor = true;
            this.buttonPreveous.Click += new System.EventHandler(this.buttonPreveous_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(872, 323);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(39, 29);
            this.buttonNext.TabIndex = 6;
            this.buttonNext.Text = "->";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // FormMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 428);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPreveous);
            this.Controls.Add(this.numericUpDownPage);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.panel1);
            this.Name = "FormMail";
            this.Text = "Письма";
            this.Load += new System.EventHandler(this.FormMail_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private DataGridView dataGridView;
        private Button buttonOpen;
        private NumericUpDown numericUpDownPage;
        private Button buttonPreveous;
        private Button buttonNext;
    }
}