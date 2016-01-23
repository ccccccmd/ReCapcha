namespace 发票编号识别
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cb_Devices = new System.Windows.Forms.ComboBox();
            this.btn_check = new System.Windows.Forms.Button();
            this.pb_Cinema = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Cinema)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_Devices
            // 
            this.cb_Devices.FormattingEnabled = true;
            this.cb_Devices.Location = new System.Drawing.Point(116, 3);
            this.cb_Devices.Name = "cb_Devices";
            this.cb_Devices.Size = new System.Drawing.Size(121, 20);
            this.cb_Devices.TabIndex = 0;
            // 
            // btn_check
            // 
            this.btn_check.Location = new System.Drawing.Point(243, 3);
            this.btn_check.Name = "btn_check";
            this.btn_check.Size = new System.Drawing.Size(75, 23);
            this.btn_check.TabIndex = 1;
            this.btn_check.Text = "检测";
            this.btn_check.UseVisualStyleBackColor = true;
            this.btn_check.Click += new System.EventHandler(this.btn_check_Click);
            // 
            // pb_Cinema
            // 
            this.pb_Cinema.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_Cinema.Location = new System.Drawing.Point(1, 32);
            this.pb_Cinema.Name = "pb_Cinema";
            this.pb_Cinema.Size = new System.Drawing.Size(1247, 612);
            this.pb_Cinema.TabIndex = 2;
            this.pb_Cinema.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 648);
            this.Controls.Add(this.pb_Cinema);
            this.Controls.Add(this.btn_check);
            this.Controls.Add(this.cb_Devices);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Cinema)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_Devices;
        private System.Windows.Forms.Button btn_check;
        private System.Windows.Forms.PictureBox pb_Cinema;
    }
}

