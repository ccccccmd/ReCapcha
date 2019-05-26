namespace RECAPTCHA
{
    partial class Process2
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
            this.listBox_Bg = new System.Windows.Forms.ListBox();
            this.listBox_Grey = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox_Bg
            // 
            this.listBox_Bg.FormattingEnabled = true;
            this.listBox_Bg.HorizontalScrollbar = true;
            this.listBox_Bg.ItemHeight = 12;
            this.listBox_Bg.Location = new System.Drawing.Point(630, 43);
            this.listBox_Bg.Name = "listBox_Bg";
            this.listBox_Bg.Size = new System.Drawing.Size(603, 292);
            this.listBox_Bg.TabIndex = 1;
            // 
            // listBox_Grey
            // 
            this.listBox_Grey.FormattingEnabled = true;
            this.listBox_Grey.HorizontalScrollbar = true;
            this.listBox_Grey.ItemHeight = 12;
            this.listBox_Grey.Location = new System.Drawing.Point(21, 43);
            this.listBox_Grey.Name = "listBox_Grey";
            this.listBox_Grey.Size = new System.Drawing.Size(603, 292);
            this.listBox_Grey.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(292, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "去背景前";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(894, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "去背景后";
            // 
            // Process2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 367);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_Bg);
            this.Controls.Add(this.listBox_Grey);
            this.Name = "Process2";
            this.Text = "Process2";
            this.Load += new System.EventHandler(this.Process2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Bg;
        private System.Windows.Forms.ListBox listBox_Grey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}