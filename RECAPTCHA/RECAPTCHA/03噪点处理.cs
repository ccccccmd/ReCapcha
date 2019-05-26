using System;
using System.Windows.Forms;

namespace RECAPTCHA
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
