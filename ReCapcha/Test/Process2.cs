using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CaptchaRecogition
{
    public partial class Process2 : Form
    {
        private string Bg;
        private string Grey;
        public Process2()
        {
            InitializeComponent();
        }
        public Process2(string bg, string grey)
        {
            InitializeComponent();
            this.Bg = bg;
            this.Grey = grey;
            this.Text = "背景处理前后对比";
        }

        private void Process2_Load(object sender, EventArgs e)
        {
            listBox_Grey.Items.AddRange(Grey.Split(new string[] { "\r\n" }, StringSplitOptions.None));
            listBox_Bg.Items.AddRange(Bg.Split(new string[] { "\r\n" }, StringSplitOptions.None));
        }
    }
}
