using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using CaptchaRecogition;

namespace 发票编号识别
{
    public class Process
    {
        protected const int InvoiceWidth = 360;
        protected const int InvoiceHeight = 230;
        public InvoiceCollection GetInvoiceCollection(Bitmap source)
        {
            InvoiceCollection collection = new InvoiceCollection();  //Collection that will hold cards
            Bitmap temp = source.Clone() as Bitmap; //Clone image to keep original image

            FiltersSequence seq = new FiltersSequence();
            seq.Add(Grayscale.CommonAlgorithms.BT709);  //First add  grayScaling filter
            seq.Add(new OtsuThreshold()); //Then add binarization(thresholding) filter
            temp = seq.Apply(source); // Apply filters on source image

            //Extract blobs from image whose size width and height larger than 150
            BlobCounter extractor = new BlobCounter();
            extractor.FilterBlobs = true;
            extractor.MinWidth = extractor.MinHeight = InvoiceHeight - 50;
            extractor.MaxWidth = extractor.MaxHeight = InvoiceWidth + 50;
            extractor.ProcessImage(temp);//去掉不符合宽高的色块

            //Will be used transform(extract) cards on source image 
            QuadrilateralTransformation quadTransformer = new QuadrilateralTransformation();

            //Will be used resize(scaling) cards 
            ResizeBilinear resizer = new ResizeBilinear(InvoiceWidth, InvoiceHeight);

            foreach (Blob blob in extractor.GetObjectsInformation())
            {
                //Get Edge points of card
                List<IntPoint> edgePoints = extractor.GetBlobsEdgePoints(blob);
                //Calculate/Find corners of card on source image from edge points
                List<IntPoint> corners = PointsCloud.FindQuadrilateralCorners(edgePoints);

                quadTransformer.SourceQuadrilateral = corners; //Set corners for transforming card 
                quadTransformer.AutomaticSizeCalculaton = true;
                //提取图像
                Bitmap invocieImg = quadTransformer.Apply(source); //Extract(transform) card image

                if (invocieImg.Width < invocieImg.Height) //If card is positioned horizontally
                {
                    invocieImg.RotateFlip(RotateFlipType.Rotate90FlipNone); //Rotate
                }
                invocieImg = resizer.Apply(invocieImg); //归一化图像
                //       File.AppendAllText("e:\\info.txt", "aaaaaaaaaaaaaaa");
                //    File.AppendAllText("e:\\info.txt",invocieImg.FrameDimensionsList.ToString());
               //invocieImg.Save("e:\\" + Guid.NewGuid() + ".jpg");
                 //   Thread.Sleep(1000);
                Invoice invoice = new Invoice(invocieImg, corners.ToArray()); //Create invoice Object
                Bitmap target = invoice.GetTarget();
                //try
                //{
                // //   target.Save("e:\\imgt\\" + Guid.NewGuid() + "taar.jpg");
                //}
                //catch (Exception ex)
                //{
                //    File.AppendAllText("E:\\error2.txt", ex.Message + "**********"+ex.Source);
                //    throw;
                //}

                try
                {
                    invoice.Code = ImageProcess.GetYZMCode(target, "zimo.txt", 4, 20, 30, 2, false, 4);
                    collection.Add(invoice); //Add card to collection
                }
                catch (Exception ex)
                {
                    File.AppendAllText("E:\\error2.txt", ex.Message + "**********" + ex.Source+"---"+DateTime.Now);
                    throw;
                }
         
            }
            return collection;
        }
        public System.Drawing.Point GetStringPoint(System.Drawing.Point[] points)
        {
            System.Drawing.Point[] tempArr = new System.Drawing.Point[points.Length];
            Array.Copy(points, tempArr, points.Length);
            Array.Sort(tempArr, new PointComparer());

            return tempArr[0].X < tempArr[1].X ? tempArr[0] : tempArr[1];
        }
    }
    /// <summary>
    /// 发票图片 集合
    /// </summary>
    public class InvoiceCollection : CollectionBase
    {
        public void Add(Invoice invoice)
        {
            List.Add(invoice);
        }

        public Invoice this[int index]
        {
            get { return List[index] as Invoice; }
            set { List[index] = value; }
        }
        public List<Bitmap> ToImageList()
        {
            List<Bitmap> list = new List<Bitmap>();

            foreach (Invoice invoice in this.List)
                list.Add(invoice.Image);
            return list;
        }
    }
    /// <summary>
    /// 发票图片类
    /// </summary>
    public class Invoice
    {
        //375   330
        protected const int InvoiceWidth = 360;
        protected const int InvoiceHeight = 230;
        private Bitmap image;
        private System.Drawing.Point[] corners;
        public System.Drawing.Point[] Corners
        {
            get { return this.corners; }
        }
        public Bitmap Image
        {
            get { return this.image; }
        }
        public Invoice(Bitmap bmp, IntPoint[] cornerIntPoints)
        {
            this.image = bmp;

            //Convert AForge.IntPoint Array to System.Drawing.Point Array
            int total = cornerIntPoints.Length;
            corners = new System.Drawing.Point[total];

            for (int i = 0; i < total; i++)
            {
                this.corners[i].X = cornerIntPoints[i].X;
                this.corners[i].Y = cornerIntPoints[i].Y;
            }
        }
        /// <summary>
        /// 截取发票编号
        /// </summary>
        /// <returns></returns>
        public Bitmap GetTarget()
        {
            if (image == null)
                return null;
          //  Crop crop = new Crop(new Rectangle(70,  157,130, 20));
            //Crop crop = new Crop(new Rectangle((int)(InvoiceWidth*0.31),(int)(InvoiceHeight*0.75),(int)(InvoiceWidth*0.60),(int)(InvoiceHeight*0.09)));
            Crop crop = new Crop(new Rectangle((int)(InvoiceWidth * 0.19), (int)(InvoiceHeight * 0.73), (int)(InvoiceWidth * 0.33), (int)(InvoiceHeight * 0.12)));
            return crop.Apply(image);
        }

        public string Code { get; set; }
    }

    public class PointComparer : IComparer<System.Drawing.Point>
    {
        public int Compare(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            return p2.Y.CompareTo(p1.Y);
        }
    }
}
