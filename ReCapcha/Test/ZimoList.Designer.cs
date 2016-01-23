namespace CaptchaRecogition
{
    partial class ZimoList
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
            this.txt_listedit = new System.Windows.Forms.TextBox();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.list_Zimo = new System.Windows.Forms.ListBox();
            this.list_edit = new System.Windows.Forms.Button();
            this.list_delete = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.groupBox_edit = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_edit.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_listedit
            // 
            this.txt_listedit.Location = new System.Drawing.Point(100, 23);
            this.txt_listedit.Name = "txt_listedit";
            this.txt_listedit.Size = new System.Drawing.Size(51, 21);
            this.txt_listedit.TabIndex = 40;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(85, 52);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(71, 23);
            this.btn_Cancel.TabIndex = 39;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click_1);
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(8, 52);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(71, 23);
            this.btn_OK.TabIndex = 38;
            this.btn_OK.Text = "确定修改";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // list_Zimo
            // 
            this.list_Zimo.FormattingEnabled = true;
            this.list_Zimo.HorizontalScrollbar = true;
            this.list_Zimo.ItemHeight = 12;
            this.list_Zimo.Location = new System.Drawing.Point(12, 12);
            this.list_Zimo.Name = "list_Zimo";
            this.list_Zimo.Size = new System.Drawing.Size(551, 292);
            this.list_Zimo.TabIndex = 37;
            // 
            // list_edit
            // 
            this.list_edit.Location = new System.Drawing.Point(236, 310);
            this.list_edit.Name = "list_edit";
            this.list_edit.Size = new System.Drawing.Size(71, 23);
            this.list_edit.TabIndex = 36;
            this.list_edit.Text = "编辑";
            this.list_edit.UseVisualStyleBackColor = true;
            this.list_edit.Click += new System.EventHandler(this.list_edit_Click_1);
            // 
            // list_delete
            // 
            this.list_delete.Location = new System.Drawing.Point(133, 310);
            this.list_delete.Name = "list_delete";
            this.list_delete.Size = new System.Drawing.Size(71, 23);
            this.list_delete.TabIndex = 35;
            this.list_delete.Text = "删除";
            this.list_delete.UseVisualStyleBackColor = true;
            this.list_delete.Click += new System.EventHandler(this.list_delete_Click_1);
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(340, 310);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 23);
            this.btn_Close.TabIndex = 41;
            this.btn_Close.Text = "关闭窗口";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // groupBox_edit
            // 
            this.groupBox_edit.BackColor = System.Drawing.Color.LightGray;
            this.groupBox_edit.Controls.Add(this.label1);
            this.groupBox_edit.Controls.Add(this.btn_OK);
            this.groupBox_edit.Controls.Add(this.btn_Cancel);
            this.groupBox_edit.Controls.Add(this.txt_listedit);
            this.groupBox_edit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_edit.ForeColor = System.Drawing.Color.Blue;
            this.groupBox_edit.Location = new System.Drawing.Point(193, 117);
            this.groupBox_edit.Name = "groupBox_edit";
            this.groupBox_edit.Size = new System.Drawing.Size(177, 81);
            this.groupBox_edit.TabIndex = 42;
            this.groupBox_edit.TabStop = false;
            this.groupBox_edit.Text = "修改框";
            this.groupBox_edit.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 41;
            this.label1.Text = "输入准确字符";
            // 
            // ZimoList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 351);
            this.Controls.Add(this.groupBox_edit);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.list_Zimo);
            this.Controls.Add(this.list_edit);
            this.Controls.Add(this.list_delete);
            this.Name = "ZimoList";
            this.Text = "ZimoList";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ZimoList_FormClosed);
            this.Load += new System.EventHandler(this.ZimoList_Load);
            this.groupBox_edit.ResumeLayout(false);
            this.groupBox_edit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_listedit;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.ListBox list_Zimo;
        private System.Windows.Forms.Button list_edit;
        private System.Windows.Forms.Button list_delete;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.GroupBox groupBox_edit;
        private System.Windows.Forms.Label label1;
    }
}