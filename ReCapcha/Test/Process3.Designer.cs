namespace CaptchaRecogition
{
    partial class Process3
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
            this.listBox_Noised = new System.Windows.Forms.ListBox();
            this.listBox_Bg = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox_Noised
            // 
            this.listBox_Noised.FormattingEnabled = true;
            this.listBox_Noised.HorizontalScrollbar = true;
            this.listBox_Noised.ItemHeight = 12;
            this.listBox_Noised.Location = new System.Drawing.Point(639, 29);
            this.listBox_Noised.Name = "listBox_Noised";
            this.listBox_Noised.Size = new System.Drawing.Size(603, 292);
            this.listBox_Noised.TabIndex = 1;
            // 
            // listBox_Bg
            // 
            this.listBox_Bg.FormattingEnabled = true;
            this.listBox_Bg.HorizontalScrollbar = true;
            this.listBox_Bg.ItemHeight = 12;
            this.listBox_Bg.Location = new System.Drawing.Point(12, 29);
            this.listBox_Bg.Name = "listBox_Bg";
            this.listBox_Bg.Size = new System.Drawing.Size(603, 292);
            this.listBox_Bg.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(280, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "去噪点前";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(874, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "去噪点后";
            // 
            // Process3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1251, 337);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_Noised);
            this.Controls.Add(this.listBox_Bg);
            this.Name = "Process3";
            this.Text = "Process3";
            this.Load += new System.EventHandler(this.Process3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Noised;
        private System.Windows.Forms.ListBox listBox_Bg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}