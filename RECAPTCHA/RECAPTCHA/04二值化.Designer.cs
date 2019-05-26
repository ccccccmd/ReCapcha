namespace RECAPTCHA
{
    partial class Process4
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
            this.listBox_Binaried = new System.Windows.Forms.ListBox();
            this.listBox_Noised = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox_Binaried
            // 
            this.listBox_Binaried.FormattingEnabled = true;
            this.listBox_Binaried.HorizontalScrollbar = true;
            this.listBox_Binaried.ItemHeight = 12;
            this.listBox_Binaried.Location = new System.Drawing.Point(635, 49);
            this.listBox_Binaried.Name = "listBox_Binaried";
            this.listBox_Binaried.Size = new System.Drawing.Size(603, 292);
            this.listBox_Binaried.TabIndex = 1;
            // 
            // listBox_Noised
            // 
            this.listBox_Noised.FormattingEnabled = true;
            this.listBox_Noised.HorizontalScrollbar = true;
            this.listBox_Noised.ItemHeight = 12;
            this.listBox_Noised.Location = new System.Drawing.Point(12, 49);
            this.listBox_Noised.Name = "listBox_Noised";
            this.listBox_Noised.Size = new System.Drawing.Size(603, 292);
            this.listBox_Noised.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Location = new System.Drawing.Point(293, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "二值化前";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(896, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "二值化后";
            // 
            // Process4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 367);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_Binaried);
            this.Controls.Add(this.listBox_Noised);
            this.Name = "Process4";
            this.Text = "Process4";
            this.Load += new System.EventHandler(this.Process4_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Binaried;
        private System.Windows.Forms.ListBox listBox_Noised;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}