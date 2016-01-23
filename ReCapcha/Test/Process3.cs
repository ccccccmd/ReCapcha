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
    public partial class Process3 : Form
    {
        private string Bg;
        private string Noise;
        public Process3()
        {
            InitializeComponent();
        }
        public Process3(string bg,string noise)
        {
            InitializeComponent();
            this.Bg = bg;
            this.Noise = noise;
            this.Text = "噪点处理前后对比";
        }

        private void Process3_Load(object sender, EventArgs e)
        {
            listBox_Bg.Items.AddRange(Bg.Split(new string[] { "\r\n" }, StringSplitOptions.None));
            listBox_Noised.Items.AddRange(Noise.Split(new string[] { "\r\n" }, StringSplitOptions.None));
        }
    }
}
