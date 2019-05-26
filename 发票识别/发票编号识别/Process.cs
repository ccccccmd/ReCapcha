using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using Point = System.Drawing.Point;

namespace 发票编号识别
{
    public class Process
    {
        protected const int InvoiceWidth = 360;
        protected const int InvoiceHeight = 230;

        public InvoiceCollection GetInvoiceCollection(Bitmap source)
        {
            InvoiceCollection collection = new InvoiceCollection(); //Collection that will hold cards
            Bitmap temp = source.Clone() as Bitmap; //Clone image to keep original image

            FiltersSequence seq = new FiltersSequence();
            seq.Add(Grayscale.CommonAlgorithms.BT709); //First add  grayScaling filter
            seq.Add(new OtsuThreshold()); //Then add binarization(thresholding) filter
            temp = seq.Apply(source); // Apply filters on source image

            //Extract blobs from image whose size width and height larger than 150
            BlobCounter extractor = new BlobCounter();
            extractor.FilterBlobs = true;
            extractor.MinWidth = extractor.MinHeight = InvoiceHeight - 50;
            extractor.MaxWidth = extractor.MaxHeight = InvoiceWidth + 50;
            extractor.ProcessImage(temp); //去掉不符合宽高的色块

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
                    invoice.Code = ImageProcess.ImageProcess.GetCAPTCHACode(target, "zimo.txt", 4, 20, 30, 2, false, 4);
                    collection.Add(invoice); //Add card to collection
                }
                catch (Exception ex)
                {
                    File.AppendAllText("E:\\error2.txt", ex.Message + "**********" + ex.Source + "---" + DateTime.Now);
                    throw;
                }
            }

            return collection;
        }

        public Point GetStringPoint(Point[] points)
        {
            Point[] tempArr = new Point[points.Length];
            Array.Copy(points, tempArr, points.Length);
            Array.Sort(tempArr, new PointComparer());

            return tempArr[0].X < tempArr[1].X ? tempArr[0] : tempArr[1];
        }
    }

    /// <summary>
    ///     发票图片 集合
    /// </summary>
    public class InvoiceCollection : CollectionBase
    {

        public Invoice this[int index]
        {
            get => List[index] as Invoice;
            set => List[index] = value;
        }

        public void Add(Invoice invoice)
        {
            List.Add(invoice);
        }

        public List<Bitmap> ToImageList()
        {
            List<Bitmap> list = new List<Bitmap>();

            foreach (Invoice invoice in List)
                list.Add(invoice.Image);
            return list;
        }
    }

    /// <summary>
    ///     发票图片类
    /// </summary>
    public class Invoice
    {
        //375   330
        protected const int InvoiceWidth = 360;
        protected const int InvoiceHeight = 230;

        public Invoice(Bitmap bmp, IntPoint[] cornerIntPoints)
        {
            Image = bmp;

            //Convert AForge.IntPoint Array to System.Drawing.Point Array
            int total = cornerIntPoints.Length;
            Corners = new Point[total];

            for (int i = 0; i < total; i++)
            {
                Corners[i].X = cornerIntPoints[i].X;
                Corners[i].Y = cornerIntPoints[i].Y;
            }
        }

        public Point[] Corners { get; }

        public Bitmap Image { get; }

        public string Code { get; set; }

        /// <summary>
        ///     截取发票编号
        /// </summary>
        /// <returns></returns>
        public Bitmap GetTarget()
        {
            if (Image == null)
                return null;
            //  Crop crop = new Crop(new Rectangle(70,  157,130, 20));
            //Crop crop = new Crop(new Rectangle((int)(InvoiceWidth*0.31),(int)(InvoiceHeight*0.75),(int)(InvoiceWidth*0.60),(int)(InvoiceHeight*0.09)));
            Crop crop = new Crop(new Rectangle((int) (InvoiceWidth * 0.19), (int) (InvoiceHeight * 0.73),
                (int) (InvoiceWidth * 0.33), (int) (InvoiceHeight * 0.12)));
            return crop.Apply(Image);
        }
    }

    public class PointComparer : IComparer<Point>
    {
        public int Compare(Point p1, Point p2)
        {
            return p2.Y.CompareTo(p1.Y);
        }
    }
}