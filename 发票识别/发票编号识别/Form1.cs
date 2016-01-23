using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;

namespace 发票编号识别
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.cameras = new FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
            int i = 1;
            int count = cameras.Count;
            foreach (FilterInfo camera in this.cameras)
            {
                if (!this.cameraDict.ContainsKey(camera.Name))
                    this.cameraDict.Add(camera.Name, camera.MonikerString);
                else
                {
                    this.cameraDict.Add(camera.Name + "-" + i.ToString(), camera.MonikerString);
                    i++;
                }
            }
            this.cb_Devices.DataSource = new List<string>(cameraDict.Keys); //Bind camera names to combobox

            if (this.cb_Devices.Items.Count == 0)
                btn_check.Enabled = false;
        }
        private const int CameraWidth = 640;  // constant Width
        private const int CameraHeight = 480; // constant Height

        private FilterInfoCollection cameras; //Collection of Cameras that connected to PC
        private VideoCaptureDevice device; //Current chosen device(camera) 
        private Dictionary<string, string> cameraDict = new Dictionary<string, string>();
        private Pen pen = new Pen(Brushes.Red, 4); //is used for drawing rectangle around card
        private Font font = new Font("Tahoma", 15, FontStyle.Bold); //is used for writing string on card
        private Process invoiceProcess = new Process();
        private InvoiceCollection invoices;

        private int frameCounter = 0;
        private void btn_check_Click(object sender, EventArgs e)
        {
            if (btn_check.Text == "检测")
            {
                btn_check.Text = "停止";
                this.device = new VideoCaptureDevice(cameraDict[cb_Devices.SelectedItem.ToString()]);

                this.device.NewFrame += new NewFrameEventHandler(device_NewFrame);
                //  this.device.DesiredFrameSize = new Size(CameraWidth, CameraHeight);
                device.VideoResolution = device.VideoCapabilities[0];//分辨率1280*1024
                device.Start();
            }
            else
            {
                StopCamera();
                btn_check.Text = "检测";
                pb_Cinema.Image = null;
            }
        }

        void device_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap shoot = eventArgs.Frame.Clone() as Bitmap;
            try
            {
                frameCounter++;
                if (frameCounter > 10)
                {
                    invoices = invoiceProcess.GetInvoiceCollection(shoot);
                    frameCounter = 0;
                }
                if (invoices!=null&&invoices.Count>0)
                {
                    using (Graphics graph = Graphics.FromImage(shoot))
                    {
                        foreach (Invoice invoice in invoices)
                        {
                            graph.DrawPolygon(pen, invoice.Corners); //Draw a polygon around card
                            PointF point = invoiceProcess.GetStringPoint(invoice.Corners); //Find Top left corner
                            point.Y += 10;
                            graph.DrawString(invoice.Code == null ? "" : invoice.Code, font, Brushes.Lime, point); //Write string on card
                        }
                    }
                }
                //Draw Rectangle around cards and write card strings on card
             
            }
            catch (Exception ex)
            {
                File.AppendAllText("e:\\error.txt",ex.Message);
                throw;

            }
            pb_Cinema.Image = ResizeShoot(shoot);
        }

        private void StopCamera()
        {
            if (device != null && device.IsRunning)
            {
                device.SignalToStop();
                device.WaitForStop();
                device = null;
            }
        }

        public Bitmap ResizeShoot(Bitmap shoot)
        {
            ResizeBilinear rb = new ResizeBilinear(pb_Cinema.Width, pb_Cinema.Height);
            return rb.Apply(shoot);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           StopCamera();
        }
   
    
    }
}
