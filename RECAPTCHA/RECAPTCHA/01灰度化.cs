using System;
using System.Windows.Forms;

namespace RECAPTCHA
{
    public partial class Process1 : Form
    {
        private readonly string Grey;

        private readonly string Original;

        public Process1()
        {
            InitializeComponent();
        }

        public Process1(string original, string grey)
        {
            InitializeComponent();
            Original = original;
            Grey = grey;
            Text = "灰度处理前后对比";
        }

        private void ProcessResults_Load(object sender, EventArgs e)
        {
            listBox_Original.Items.AddRange(Original.Split(new[] {"\r\n"}, StringSplitOptions.None));
            listBox_Grey.Items.AddRange(Grey.Split(new[] {"\r\n"}, StringSplitOptions.None));
        }
    }
}