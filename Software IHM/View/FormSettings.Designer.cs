
namespace IHM_ESP.View
{
    partial class FormSettings
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Ganho proporcional",
            "1"}, -1);
            this.listView_settings = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tb_value = new System.Windows.Forms.TextBox();
            this.btn_set_value = new System.Windows.Forms.Button();
            this.lb_min_value = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lb_max_value = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // listView_settings
            // 
            this.listView_settings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView_settings.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView_settings.HideSelection = false;
            this.listView_settings.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listView_settings.Location = new System.Drawing.Point(12, 12);
            this.listView_settings.Name = "listView_settings";
            this.listView_settings.Size = new System.Drawing.Size(405, 426);
            this.listView_settings.TabIndex = 0;
            this.listView_settings.UseCompatibleStateImageBehavior = false;
            this.listView_settings.View = System.Windows.Forms.View.Details;
            this.listView_settings.SelectedIndexChanged += new System.EventHandler(this.listView_settings_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Parâmetro";
            this.columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Valor";
            this.columnHeader2.Width = 100;
            // 
            // tb_value
            // 
            this.tb_value.Location = new System.Drawing.Point(422, 123);
            this.tb_value.Name = "tb_value";
            this.tb_value.Size = new System.Drawing.Size(100, 20);
            this.tb_value.TabIndex = 1;
            this.tb_value.Validating += new System.ComponentModel.CancelEventHandler(this.tb_value_Validating);
            // 
            // btn_set_value
            // 
            this.btn_set_value.Location = new System.Drawing.Point(422, 149);
            this.btn_set_value.Name = "btn_set_value";
            this.btn_set_value.Size = new System.Drawing.Size(100, 23);
            this.btn_set_value.TabIndex = 2;
            this.btn_set_value.Text = "Confirmar";
            this.btn_set_value.UseVisualStyleBackColor = true;
            this.btn_set_value.Click += new System.EventHandler(this.btn_set_value_Click);
            // 
            // lb_min_value
            // 
            this.lb_min_value.AutoSize = true;
            this.lb_min_value.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_min_value.Location = new System.Drawing.Point(423, 12);
            this.lb_min_value.Name = "lb_min_value";
            this.lb_min_value.Size = new System.Drawing.Size(86, 16);
            this.lb_min_value.TabIndex = 3;
            this.lb_min_value.Text = "Valor mínimo";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lb_max_value
            // 
            this.lb_max_value.AutoSize = true;
            this.lb_max_value.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_max_value.Location = new System.Drawing.Point(423, 56);
            this.lb_max_value.Name = "lb_max_value";
            this.lb_max_value.Size = new System.Drawing.Size(90, 16);
            this.lb_max_value.TabIndex = 4;
            this.lb_max_value.Text = "Valor máximo";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(423, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Ajustar valor";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lb_max_value);
            this.Controls.Add(this.lb_min_value);
            this.Controls.Add(this.btn_set_value);
            this.Controls.Add(this.tb_value);
            this.Controls.Add(this.listView_settings);
            this.Name = "FormSettings";
            this.Text = "FormSettings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_settings;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox tb_value;
        private System.Windows.Forms.Button btn_set_value;
        private System.Windows.Forms.Label lb_min_value;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_max_value;
    }
}