using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReCapcha;

namespace CaptchaRecogition
{
    public partial class gp_down : Form
    {
        public gp_down()
        {
            InitializeComponent();
        }
//环境变量
        private static string imgurl;
        private string vcodeFile;
        private string imgOriginal;
        private string imgBged;
        private string imgNoised;
        private string imgBinaried;
        private string imgGreied;
        private int graytype=2;
        private Image nullImg = Image.FromFile("img\\null.jpg");
        private int noiseThreshold=1;
        private int minWidth=8;
        private bool isFourChars = false;
        /// <summary>
        /// 三种方式下灰度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_gray_Click(object sender, EventArgs e)
        {
            Image img = pborigin.Image;
            Image img_grey = (Image)img.Clone();
             imgOriginal = Image2Num((Bitmap )img_grey);
            if (!Directory.Exists("experiment\\"))
            {
                Directory.CreateDirectory("experiment\\");
            }
            //imgurl = pborigin.ImageLocation;
            WriteToFile(imgOriginal, "experiment\\" + Path.GetFileNameWithoutExtension(imgurl) + "_original.txt");
            //WriteToFile(imgGraying, "e:\\yanzhengma\\gray"+pborigin.ImageLocation);
            //灰度化处理       
            //img_gray = ccccccmd.ImageHelper.Gray(img_gray, ccccccmd.ImageHelper.AlgorithmsType.AverageValue);//平均
            //img_gray = ccccccmd.ImageHelper.Gray(img_gray, ccccccmd.ImageHelper.AlgorithmsType.MaxValue );//最大值
          // img_gray = ccccccmd.ImageHelper.Gray(img_gray, ccccccmd.ImageHelper.AlgorithmsType.WeightAverage);//加权
            graytype = 2;
            if (rb_max.Checked)
            {
                graytype = 1;
            }
            if (rb_quanzhong .Checked )
            {
                graytype = 3;
            }
            img_grey = ImageProcess.Gray((Bitmap)img_grey, graytype);
             imgGreied = Image2Num((Bitmap)img_grey);
             WriteToFile(imgGreied, "experiment\\" + Path.GetFileNameWithoutExtension(imgurl)+"_greied.txt");
            pb_grey.Image = img_grey;
        }
        /// <summary>
        /// 图片转换成像素数值
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        #region 图片转换成像素数值
        public string Image2Num(Bitmap img)
        {

            StringBuilder sb = new StringBuilder();
            string imgPixel = "";
            int height = img.Height;
            int width = img.Width;
            BitmapData bdata = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytelength = width * height * 4;
            byte[] bytes = new byte[bytelength];
            Marshal.Copy(bdata.Scan0, bytes, 0, bytelength);
            int row = 1;
            for (int i = 0; i < bytelength; i += 4)
            {
                imgPixel = bytes[i] + "," + bytes[i + 1] + "," + bytes[i + 2] ;
                if (imgPixel.Length<11)
                {
                    for (int j = 0; j < 11-imgPixel.Length ; j++)
                    {
                        imgPixel += " ";
                    }
                }
                imgPixel += "\t";
                sb.Append(imgPixel);
                if (row % width == 0)
                {
                    sb.Append("\r\n");
                }
                row++;
            }
            img.UnlockBits(bdata);
            return sb.ToString();
        } 
        #endregion
        /// <summary>
        /// 将数据写入文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public void WriteToFile(string data, string path)
        {
            File.WriteAllText(path,data);
        }

        /// <summary>
        /// 从硬盘加载验证码
        /// </summary>
        public void GetVCodeFromFile()
        {
            Random r = new Random();
            if (string.IsNullOrEmpty(vcodeFile ))
            {
                vcodeFile = "captcha\\shanghailigong";
            }
        var pics=   Directory.GetFiles(vcodeFile,"*.*",SearchOption.TopDirectoryOnly).Where(f=>f.EndsWith(".jpg")||f.EndsWith (".png")).ToList ();
            string url = pics[r.Next (0,pics.Count )];
            imgurl = url;
            Image img = Image.FromFile(url);
            pictureBox1.Image = img;
            pborigin.Load(url);
        }
        /// <summary>
        /// 从网络获取验证码
        /// </summary>
        /// <param name="url"></param>
        public void GetVCodeFromUrl(string url)
        {
            url = url + new Random().Next(1,9999);
            WebRequest re = WebRequest.Create(url);
            WebResponse rp = re.GetResponse();
            Stream s = rp.GetResponseStream();
            Image img = Image.FromStream(s);
            s.Close();
            s.Dispose();
            pictureBox1.Image = img;
            pborigin.Image = img;
        }
        /// <summary>
        /// 下载保存验证码图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        public void SaveCodeImageFromUrl(string url, string path)
        {
         
            WebRequest re = WebRequest.Create(url);
            WebResponse rp = re.GetResponse();
            Stream s = rp.GetResponseStream();
            Image img = Image.FromStream(s);
            s.Close();
            s.Dispose();
            img.Save(path);
        }
        /// <summary>
        /// 去背景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_bg_Click(object sender, EventArgs e)
        {
            Image img_noise = (Image)pb_grey.Image.Clone();
            //得到灰度图像前景色临界值

            // img_noise = ccccccmd.ImageHelper.RemoveBlock(img_noise,2);//报错
      //int v = ImageProcess.GetDgGrayValue(img_noise);
            int v = ImageProcess.ComputeThresholdValue((Bitmap)img_noise);
            //Image test = ImageProcess.RemoveBg((Bitmap)img_noise ,v );
           // int v2 = Test.ImageProcess.GetDgGrayValue(img_noise);
              Image img_bg= ImageProcess.RemoveBg((Bitmap)img_noise ,v);//-----------去背景，内存法
             imgBged = Image2Num((Bitmap)img_bg);
              WriteToFile(imgBged, "experiment\\" + Path.GetFileNameWithoutExtension(imgurl) + "_bged.txt");
              pb_bg.Image = img_bg;         
        }
        /// <summary>
        /// 去噪点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_noise_Click(object sender, EventArgs e)
        {
         Image   img_noise = ImageProcess.RemoveNoise((Bitmap)pb_bg.Image.Clone(), noiseThreshold);
            //img_noise = ccccccmd.ImageHelper.ClearNoise(img_noise, v, 2);
         imgNoised = Image2Num((Bitmap)img_noise);
         WriteToFile(imgNoised, "experiment\\" + Path.GetFileNameWithoutExtension(imgurl) + "_noised.txt");
            pb_noise.Image = img_noise;
        }
        /// <summary>
        /// 二值化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_binary_Click(object sender, EventArgs e)
        {
            //去噪处理后的图片
            Image imgBinary = (Image)pb_noise.Image.Clone();
            //int v2 = ccccccmd.ImageHelper.ComputeThresholdValue((Bitmap)imgBinary);
           // int v2 = ImageProcess.ComputeThresholdValue((Bitmap)imgBinary);
           // imgBinary = ImageProcess.PBinary((Bitmap)imgBinary, v2);
            imgBinary = ImageProcess.Binary((Bitmap)imgBinary);
             imgBinaried = Image2Num((Bitmap)imgBinary);
            WriteToFile(imgBinaried, "experiment\\" + Path.GetFileNameWithoutExtension(imgurl) + "_binaried.txt");
            pb_noise_binary.Image = imgBinary;
        }
        /// <summary>
        /// 更换验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pborigin_Click(object sender, EventArgs e)
        {
            GetVCodeFromFile();
        }
        private List<Bitmap> cutResult = new List<Bitmap>(4);
        //字模存储路径
        private string zimoPath = "";
        //字模数据
        private string[] zimoData;
        private void btn_cutImage_Click(object sender, EventArgs e)
        {
            //  Image img_cut =(Image) pb_gray_binary.Image.Clone();
            Image img_cut = (Image)pb_noise_binary.Image.Clone();
            List<Bitmap> list = ImageProcess.CutImage(img_cut, 20, 30,isFourChars,minWidth );
            cutResult = list;
            //  ccccccmd.ImageHandler handler = new ccccccmd.ImageHandler();
            //   List<Bitmap> list=     handler.CutImage(img_cut,56);
            //Bitmap[] list = ccccccmd.ImageHelper.CutImage(4,1,(Bitmap )img_cut);
            pb1.Image = list[0];
            if (list.Count>=2)
            {
                pb2.Image = list[1];
            }
            else
            {
                pb2.Image = nullImg;
            }
            if (list.Count>=3)
            {
                pb3.Image = list[2];
            }
            else
            {
                pb3.Image = nullImg;
            }
            if (list.Count>=4)
            {
                pb4.Image = list[3];
            }
            else
            {
                pb4.Image = nullImg ;
            }
            if (list.Count >= 5)
            {
                pb5.Image = list[4];
            }
            else
            {
                pb5.Image = nullImg;
            }
            if (list.Count >= 6)
            {
                pb6.Image = list[5];
            }
            else
            {
                pb6.Image = nullImg;
            }

          
        }
        /// <summary>
        /// 字模学习入库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CodeStudy_Click(object sender, EventArgs e)
        {
            if (cutResult != null)
            {
                string[] zimos = File.ReadAllLines(zimoPath);
                string zimo = string.Empty;
                StringBuilder sb = new StringBuilder();
                string[] stuCode = new string[] { txt1.Text.Trim(), txt2.Text.Trim(), txt3.Text.Trim(), txt4.Text.Trim() ,txt5.Text.Trim( ),txt6.Text.Trim( ),txt7.Text.Trim(),txt8.Text.Trim()};
                for (int i = 0; i < cutResult.Count; i++)
                {

                    if (!string.IsNullOrEmpty(stuCode[i]))
                    {
                        ImageProcess.WriteZimo(cutResult[i], zimoPath, stuCode[i]);  
                    }   
                }
                //File.AppendAllText(zimoPath, sb.ToString());
                txt1.Text = "";
                txt2.Text = "";
                txt3.Text = "";
                txt4.Text = "";
                txt5.Text = "";
                txt6.Text = "";
                txt7.Text = "";
                txt8.Text = "";
            }
            else
            {
                MessageBox.Show("验证码分割过程错误！");
            }
            groupBox_Study.Enabled = false;
        }
        /// <summary>
        /// 识别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_recognize_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(zimoPath))
            {
                #region past
                //Image img = (Image)pborigin.Image.Clone();
                //img = ImageProcess.Gray((Bitmap)img);
                //int v = ImageProcess.GetDgGrayValue(img);
                //img = ImageProcess.RemoveBg((Bitmap)img, v);
                //img = ImageProcess.RemoveNoise((Bitmap)img, 1);
                //img = ImageProcess.Binary((Bitmap)img);
                //List<Bitmap> list = ImageProcess.CutImage(img, img.Width / 4, img.Height);
                //cutResult = list;
                //pictureBox2.Image = list[0];
                //pictureBox3.Image = list[1];
                //pictureBox4.Image = list[2];
                //pictureBox5.Image = list[3];
                //StringBuilder sb = new StringBuilder();
                ////读取字模
                //string[] zimo = File.ReadAllLines(zimoPath);
                //if (zimo.Length > 0)
                //{
                //    for (int j = 0; j < cutResult.Count; j++)
                //    {
                //        Dictionary<string, int> d = new Dictionary<string, int>();
                //        for (int i = 0; i < zimo.Length; i++)
                //        {
                //            string[] dic = zimo[i].Split(new string[] { "--" }, StringSplitOptions.None);
                //            //计算相似度
                //            int rate = ImageProcess.CalcRate(ImageProcess.GetBinaryCode(cutResult[j]), dic[1]);
                //            //把字符和其对应的相似度存入字典
                //            if (d.ContainsKey(dic[0]))
                //            {
                //                if (rate > d[dic[0]])
                //                {
                //                    d.Remove(dic[0]);
                //                    d.Add(dic[0], rate);
                //                }
                //            }
                //            else
                //            {
                //                d.Add(dic[0], rate);
                //            }
                //        }
                //        //找到相似度最高的
                //        sb.Append(Sort(d));
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("字模数据库为空!请直接学习添加！");
                //    groupBox_Study.Visible = true;
                //} 
                #endregion

                //List<Bitmap> list = ImageProcess.PreProcess(pborigin.Image, 1, pborigin.Image.Width/4,
                //    pborigin.Image.Height,graytype);
                Image img = (Image)pborigin.Image.Clone();
                List<Bitmap> list = ImageProcess.PreProcess(img, noiseThreshold, 20,30,graytype,isFourChars ,minWidth);
                cutResult = list;
                pictureBox2.Image = list[0];
                if (list.Count >= 2)
                {
                    pictureBox3.Image = list[1];
                }
                else
                {
                    pictureBox3.Image = nullImg;
                }
                if (list.Count>=3)
                {
                    pictureBox4.Image = list[2];
                }
                else
                {
                    pictureBox4.Image = nullImg;
                }
                if (list.Count >= 4)
                {
                    pictureBox5.Image = list[3];
                }
                else
                {
                    pictureBox5.Image = nullImg;
                }
                if (list.Count >=5)
                {
                    pictureBox6.Image = list[4];
                }
                else
                {
                    pictureBox6.Image = nullImg;
                }
                if (list.Count >= 6)
                {
                    pictureBox7.Image = list[5];
                }
                else
                {
                    pictureBox7.Image = nullImg;
                }

                if (list.Count >= 7)
                {
                    pictureBox10.Image = list[6];
                }
                else
                {
                    pictureBox10.Image = nullImg;
                }
                if (list.Count >= 8)
                {
                    pictureBox11.Image = list[7];
                }
                else
                {
                    pictureBox11.Image = nullImg;
                }
     
                //string sb = ImageProcess.GetYZMCode(pborigin.Image, zimoPath, 1, pborigin.Image.Width/4,
                //    pborigin.Image.Height,graytype);
                Image img2 = (Image)pborigin.Image.Clone();
                string sb = ImageProcess.GetYZMCode(img2, zimoPath, noiseThreshold , 20, 30,graytype,isFourChars ,minWidth );
                lbl_recoResult.Text = sb;
                btn_OK.Enabled = true;
                btn_NO.Enabled = true;
            }
            else
            {
                MessageBox.Show("没有加载字模位置！");
            }
          

        }
        /// <summary>
        /// 按相似度排序
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string Sort(Dictionary<string, int> d)
        {

            var l = d.OrderByDescending(k => k.Value).ToList();
            
            return l[0].Key;

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.ShowDialog();
            zimoPath = o.FileName;
            if (zimoPath!=null)
            {
                btn_showZimo.Enabled = true;
            }
          //LoadList();
  
        }


        private void button4_Click(object sender, EventArgs e)
        {
            ZimoList form = new ZimoList(zimoPath);
            form.Show();
        }
        /// <summary>
        /// 显示个步骤处理结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Result1_Click(object sender, EventArgs e)
        {
            Process1 form = new Process1(imgOriginal ,imgGreied );
            form.Text = "灰度处理前后对比";
            form.Show();
        }

        private void btn_Result2_Click(object sender, EventArgs e)
        {
            Process2 form = new Process2(imgBged,imgGreied);
            form.Text = "背景处理前后对比";
            form.Show();
        }

        private void btn_Result3_Click(object sender, EventArgs e)
        {
            Process3 form = new Process3(imgBged ,imgNoised );
            form.Text = "噪点处理前后对比";
            form.Show();
        }

        private void btn_Result4_Click(object sender, EventArgs e)
        {
            Process4 form = new Process4(imgNoised,imgBinaried);
            form.Text = "二值化处理前后对比";
            form.Show();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            groupBox_Study.Enabled = true;
            btn_OK.Enabled = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            GetVCodeFromFile();
            
        }

        private void btn_NO_Click(object sender, EventArgs e)
        {
            GetVCodeFromFile();
            btn_NO.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetVCodeFromFile();
        }

        private void btn_ChooseVCodeUrl_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.Description = "--------------------选择验证码图片路径------------------";
            string path = Assembly.GetExecutingAssembly().Location;
            fd.SelectedPath = Path.GetDirectoryName(path)+"\\captcha";

            fd.ShowDialog();
            vcodeFile = fd.SelectedPath;
         
            
        }

        private void rb_Local_MouseClick(object sender, MouseEventArgs e)
        {
            gp1.Enabled = true;
            gp2.Enabled = false;
            
        }

        private void rb_Net_MouseClick(object sender, MouseEventArgs e)
        {
            gp2.Enabled = true;
            txt_Url.Text = "http://kzone.kuwo.cn/mlog/st/VerifyCode?d=";
            gp1.Enabled = false;
            lb_Location.Text = "";
            gp_download.Enabled = false;
        }

        private void btn_Get_Click(object sender, EventArgs e)
        {
            if (rb_Local.Checked)
            {
                GetVCodeFromFile();
            }
            else
            {
                if (!string.IsNullOrEmpty (txt_Url.Text .Trim ()))
                {
                    GetVCodeFromUrl(txt_Url.Text .Trim ());
                }
                else
                {
                    MessageBox.Show("网络地址不能空！！");
                }
             
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            gp_download.Enabled = true;
        }

        private string downloadPath;
        private void button3_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.Description = "===========选择验证码存储路径============";
            string path = Assembly.GetExecutingAssembly().Location;
            fd.SelectedPath = Path.GetDirectoryName(path) + "\\captcha";
            fd.ShowDialog();
            downloadPath = fd.SelectedPath;
            lb_Location.Text  = downloadPath;
        }

        private void btn_down_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty (textBox1.Text .Trim ()))
            {
                int num = int.Parse(textBox1.Text.Trim());
                if (!string .IsNullOrEmpty (downloadPath ))
                {
                    Random r = new Random();
                    string url;
                    for (int i = 0; i <num ; i++)
                    {
                     url = txt_Url.Text + r.Next(1, 9999);
                        SaveCodeImageFromUrl(url, downloadPath+"\\"+i+".jpg");
                    }
                    MessageBox.Show("下载完毕！");

                }
                else
                {
                    MessageBox.Show("没有选择保存路径");
                }
            }
            else
            {
                MessageBox.Show("数量没有填！");
            }
        }

        private void txtNoise_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNoise.Text.Trim( )))
            {
                int.TryParse(txtNoise.Text.Trim(), out noiseThreshold);
            }
        }

        private void txtMinWidth_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty( txtMinWidth.Text.Trim( )))
            {
                int.TryParse(txtMinWidth.Text.Trim(), out minWidth);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isFourChars = checkBox1.Checked;
        }

        private void gp_down_Load(object sender, EventArgs e)
        {
            this.Text = "验证码自动识别";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Test4Captcha t4 = new Test4Captcha();
            t4.Text = "识别测试";
            t4.Show();
        }

    

    

    }
}
