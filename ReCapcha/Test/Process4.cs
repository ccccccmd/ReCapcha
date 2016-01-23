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
    public partial class Process4 : Form
    {
        public Process4()
        {
            InitializeComponent();
        }

        private string Noise;
        private string Binary;

        public Process4(string noise, string binary)
        {
            InitializeComponent();
            this.Noise = noise;
            this.Binary = binary;
            this.Text = "二值化处理前后对比";
        }

        private void Process4_Load(object sender, EventArgs e)
        {
            listBox_Noised.Items.AddRange(Noise.Split(new string[] { "\r\n" }, StringSplitOptions.None));

            listBox_Binaried.Items.AddRange(Binary.Split(new string[] { "\r\n" }, StringSplitOptions.None));
        }
    }
}
