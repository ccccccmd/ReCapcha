using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace RECAPTCHA
{
    public partial class Test4Captcha : Form
    {
        public Test4Captcha()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty( URL))
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                string code = ImageProcess.GetCAPTCHACode(pictureBox1.Image, Path.GetDirectoryName(URL)+"\\zimo.txt",Convert.ToInt32( textBox1.Text.Trim( )),20,30,2,checkBox1.Checked ,Convert.ToInt32(  textBox2.Text.Trim( )));

             
                sw.Stop();
                label1.Text = code; label3.Text = sw.ElapsedMilliseconds.ToString()+"ms";

            }
            else
            {
                MessageBox.Show("请选择网站");
            }
         
        }

        private string[] pics;
        private string URL;
        private void button2_Click(object sender, EventArgs e)
        {
            if (pics != null && pics.Length > 0)
            {
                Random r = new Random();
                string url = pics[r.Next(0, pics.Length)];
                Image img = Image.FromFile(url);
                URL = url;
                pictureBox1.Image = img;
            }
            else
            {
                MessageBox.Show("选择网站");
            }

        }

        private Dictionary<string, string> dic = new Dictionary<string, string>();
        
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb.SelectedItem!=null)
            {
          
                string filePath = dic[lb.SelectedItem.ToString()];
              string[] imgs=  Directory.GetFiles(filePath, "*.jpg");
                if (imgs!=null&&imgs.Length>0)
                {
                    pictureBox1.Image = Image.FromFile(imgs[0]);
                    URL = imgs[0];
                    pics = imgs;   
                }
                else
                {
                    MessageBox.Show("还没有图片");
                }
            }
            else
            {
                MessageBox.Show("请选择网站");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location)+"\\captcha";
            dic.Add("上海第二工业大学", path + "\\sspu");
            dic.Add("酷我音乐注册", path + "\\kuwo");
            dic.Add("电玩网注册", path + "\\game"); 
            dic.Add("上海理工大学", path + "\\shanghailigong");
            dic.Add("上海同济大学", path + "\\tongji");
            dic.Add("中青宝在线社区", path + "\\zhongqingbao");
            dic.Add("金山网络个人中心", path + "\\jinshannetwork");
            dic.Add("复旦大学", path + "\\fudan");
        }
    }
}
