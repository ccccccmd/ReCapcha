namespace RECAPTCHA
{
    partial class Process1
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
            this.listBox_Original = new System.Windows.Forms.ListBox();
            this.listBox_Grey = new System.Windows.Forms.ListBox();
            this.原始图片 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox_Original
            // 
            this.listBox_Original.FormattingEnabled = true;
            this.listBox_Original.HorizontalScrollbar = true;
            this.listBox_Original.ImeMode = System.Windows.Forms.ImeMode.On;
            this.listBox_Original.ItemHeight = 12;
            this.listBox_Original.Location = new System.Drawing.Point(2, 36);
            this.listBox_Original.Name = "listBox_Original";
            this.listBox_Original.Size = new System.Drawing.Size(604, 304);
            this.listBox_Original.TabIndex = 0;
            // 
            // listBox_Grey
            // 
            this.listBox_Grey.FormattingEnabled = true;
            this.listBox_Grey.HorizontalScrollbar = true;
            this.listBox_Grey.ItemHeight = 12;
            this.listBox_Grey.Location = new System.Drawing.Point(631, 36);
            this.listBox_Grey.Name = "listBox_Grey";
            this.listBox_Grey.Size = new System.Drawing.Size(603, 304);
            this.listBox_Grey.TabIndex = 0;
            // 
            // 原始图片
            // 
            this.原始图片.AutoSize = true;
            this.原始图片.Location = new System.Drawing.Point(271, 13);
            this.原始图片.Name = "原始图片";
            this.原始图片.Size = new System.Drawing.Size(53, 12);
            this.原始图片.TabIndex = 1;
            this.原始图片.Text = "原始图片";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(890, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "灰度化后";
            // 
            // Process1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 358);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.原始图片);
            this.Controls.Add(this.listBox_Grey);
            this.Controls.Add(this.listBox_Original);
            this.Name = "Process1";
            this.Text = "ProcessResults";
            this.Load += new System.EventHandler(this.ProcessResults_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Original;
        private System.Windows.Forms.ListBox listBox_Grey;
        private System.Windows.Forms.Label 原始图片;
        private System.Windows.Forms.Label label2;
    }
}