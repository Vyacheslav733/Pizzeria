namespace PizzeriaView
{
    partial class FormReportGroupedOrders
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
            this.panel = new System.Windows.Forms.Panel();
            this.buttonToPDF = new System.Windows.Forms.Button();
            this.buttonMake = new System.Windows.Forms.Button();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.buttonToPDF);
            this.panel.Controls.Add(this.buttonMake);
            this.panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(970, 52);
            this.panel.TabIndex = 1;
            // 
            // buttonToPDF
            // 
            this.buttonToPDF.Location = new System.Drawing.Point(486, 12);
            this.buttonToPDF.Name = "buttonToPDF";
            this.buttonToPDF.Size = new System.Drawing.Size(411, 29);
            this.buttonToPDF.TabIndex = 5;
            this.buttonToPDF.Text = "В PDF";
            this.buttonToPDF.UseVisualStyleBackColor = true;
            this.buttonToPDF.Click += new System.EventHandler(this.buttonToPDF_Click);
            // 
            // buttonMake
            // 
            this.buttonMake.Location = new System.Drawing.Point(49, 12);
            this.buttonMake.Name = "buttonMake";
            this.buttonMake.Size = new System.Drawing.Size(377, 29);
            this.buttonMake.TabIndex = 4;
            this.buttonMake.Text = "Сформировать";
            this.buttonMake.UseVisualStyleBackColor = true;
            this.buttonMake.Click += new System.EventHandler(this.ButtonMake_Click);
            // 
            // FormReportGroupedOrders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 450);
            this.Controls.Add(this.panel);
            this.Name = "FormReportGroupedOrders";
            this.Text = "Отчёт по группированным заказам ";
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel;
        private Button buttonToPDF;
        private Button buttonMake;
    }
}