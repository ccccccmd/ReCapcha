using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace CaptchaRecogition
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageProcess
    {
        /// <summary>
        /// 图片灰度化处理指针法
        /// </summary>
        /// <param name="img">待处理图片</param>
        /// <param name="type">1：最大值；2：平均值；3：加权平均；默认平均值</param>
        /// <returns>灰度处理后的图片</returns>
        public static Image Gray(Bitmap img, int type = 2)
        {
            Func<int, int, int, int> getGrayValue;
            switch (type)
            {
                case 1:
                    getGrayValue = GetGrayValueByMax;
                    break;
                case 2:
                    getGrayValue = GetGrayValueByPingjunzhi;
                    break;
                default:
                    getGrayValue = GetGrayValueByQuanzhong;
                    break;
            }
            int height = img.Height;
            int width = img.Width;
            BitmapData bdata = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite,
                PixelFormat.Format32bppRgb);
            unsafe
            {
                byte* ptr = (byte*)bdata.Scan0.ToPointer();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        int v = getGrayValue(ptr[0], ptr[1], ptr[2]);
                        //   int v = (ptr[0] + ptr[1] + ptr[2]) / 3;
                        ptr[0] = ptr[1] = ptr[2] = (byte)v;
                        ptr += 4;
                    }
                    ptr += bdata.Stride - width * 4;
                }
            }
            img.UnlockBits(bdata);
            return img;
        }
        private static int GetGrayValueByMax(int r, int g, int b)
        {
            int max = r;
            max = max > g ? max : g;
            max = max > b ? max : b;
            return max;
        }

        private static int GetGrayValueByPingjunzhi(int r, int g, int b)
        {
            return (r + g + b) / 3;
        }
        private static int GetGrayValueByQuanzhong(int b, int g, int r)
        {
            return (int)(r * 0.3 + g * 0.59 + b * 0.11);
        }
        /// <summary>
        /// 去除背景
        /// </summary>
        /// <param name="img">原图片</param>
        /// <param name="dgGrayValue">前景背景分界灰度值</param>
        /// <returns></returns>
        public static Image RemoveBg(Bitmap img, int dgGrayValue)
        {

            int width = img.Width;
            int height = img.Height;
            BitmapData bdata = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite,
                PixelFormat.Format32bppRgb); //红绿蓝个八位，其余8位没使用
            unsafe
            {
                byte* ptr = (byte*)bdata.Scan0.ToPointer();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (ptr[1] > dgGrayValue)//背景点
                        {
                            ptr[0] = ptr[1] = ptr[2] = 255;
                        }
                        ptr += 4;
                    }
                    ptr += bdata.Stride - width * 4;
                }
            }
            #region 内存法
            //获取位图中第一个像素数据的地址

            //int byteNum = width * height * 4;
            ////byte[] four = new byte[width *height ];
            //byte[] rgbValue = new byte[byteNum];
            ////把内存中的图像copy到数组
            //Marshal.Copy(ptr, rgbValue, 0, byteNum);
            //for (int i = 0; i < rgbValue.Length; i += 4)
            //{
            //    if (rgbValue[i] >= dgGrayValue) //是背景点
            //    {
            //        rgbValue[i] = rgbValue[i + 1] = rgbValue[i + 2] = 255;
            //        //  four[i/4] = rgbValue[4];
            //    }
            //    else
            //    {
            //        //不是背景点的做标记，下一阶段处理噪点用**第四个字节默认值都是255**我们标记为 111
            //        rgbValue[i + 3] = 111;
            //    }
            //}
            ////将修改好的数据复制到内存
            //Marshal.Copy(rgbValue, 0, ptr, byteNum); 
            #endregion
            //从内存中解锁
            img.UnlockBits(bdata);
            return img;
        }

        /// <summary>
        /// 得到灰度图像前景背景的临界值 最大类间方差法
        /// </summary>
        /// <param name="img">灰度图像</param>
        /// <returns>灰度图像前景背景的临界值</returns>
        public static int GetDgGrayValue(Image img)
        {
            Bitmap bmp = new Bitmap(img);
            int[] pixelNum = new int[256];         
            int n, n1, n2;
            int total;                              //total为总和，累计值
            double m1, m2, sum, csum, fmax, sb;     //sb为类间方差，fmax存储最大方差值
            int k, t, q;
            int threshValue = 1;                      // 阈值
        
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //返回各个点的颜色，以RGB表示
                    pixelNum[bmp.GetPixel(i, j).R]++;            //相应的直方图加1
                }
            }
            //直方图平滑化
            for (k = 0; k <= 255; k++)
            {
                total = 0;
                for (t = -2; t <= 2; t++) //与附近2个灰度做平滑化
                {
                    q = k + t;
                    if (q < 0)                   
                        q = 0;
                    if (q > 255)
                        q = 255;
                    total = total + pixelNum[q]; 
                }
                pixelNum[k] = (int)((float)total / 5.0 + 0.5);    // pixelNum[k] 的灰度值是前后5个点的平均值
            }
            //求阈值
            sum = csum = 0.0;
            n = 0;
            //计算总的图象的点数和质量矩，为后面的计算做准备
            for (k = 0; k <= 255; k++)
            {
                sum += (double)k * (double)pixelNum[k];     //x*f(x)质量矩，也就是每个灰度的值乘以其点数（归一化后为概率），sum为其总和
                n += pixelNum[k];                       //n为图象总像素点数，归一化后就是累积概率
            }

            fmax = -1.0;                          //类间方差sb不可能为负，所以fmax初始值为-1不影响计算的进行
            n1 = 0;
            for (k = 0; k < 256; k++)                  //对每个灰度（从0到255）计算一次分割后的类间方差sb
            {
                n1 += pixelNum[k];                //n1为在当前阈值遍前景图象的点数
                if (n1 == 0) { continue; }            //没有分出前景后景
                n2 = n - n1;                        //n2为背景图象的点数
                if (n2 == 0) { break; }               //n2为0表示全部都是后景图象，与n1=0情况类似，之后的遍历不可能使前景点数增加，所以此时可以退出循环
                csum += (double)k * pixelNum[k];    //前景的“灰度的值*其像素点数”的总和
                m1 = csum / n1;                     //m1为前景的平均灰度
                m2 = (sum - csum) / n2;               //m2为背景的平均灰度
                sb = (double)n1 * (double)n2 * (m1 - m2) * (m1 - m2);   //sb为类间方差
                if (sb > fmax)                  //如果算出的类间方差大于前一次算出的类间方差
                {
                    fmax = sb;                    
                    threshValue = k;              //k就是最佳阈值
                }
            }
            return threshValue;
        }

        /// <summary>
        /// 去除噪点
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="maxAroundPoints">噪点的最大粘连数</param>
        /// <returns></returns>
        public static Image RemoveNoise(Bitmap img, int maxAroundPoints=1)
        {
            int width = img.Width;
            int height = img.Height;
            BitmapData bdata = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite,
                PixelFormat.Format32bppRgb);

            #region 指针法

            unsafe
            {
                byte* ptr = (byte*)bdata.Scan0.ToPointer();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (i == 0 || i == height - 1 || j == 0 || j == width - 1) //边界点，直接当作噪点去除掉
                        {
                            ptr[0] = ptr[1] = ptr[2] = 255;
                        }
                        else
                        {
                            int aroundPoint = 0;
                            if (ptr[0] != 255) //看标记，不是背景点
                            {
                                //判断其周围8个方向与自己相连接的有几个点
                                if ((ptr - 4)[0] != 255) aroundPoint++; //左边
                                if ((ptr + 4)[0] != 255) aroundPoint++; //右边
                                if ((ptr - width * 4)[0] != 255) aroundPoint++; //正上方
                                if ((ptr - width * 4 + 4)[0] != 255) aroundPoint++; //右上角
                                if ((ptr - width * 4 - 4)[0] != 255) aroundPoint++; //左上角
                                if ((ptr + width * 4)[0] != 255) aroundPoint++; //正下方
                                if ((ptr + width * 4 + 4)[0] != 255) aroundPoint++; //右下方
                                if ((ptr + width * 4 - 4)[0] != 255) aroundPoint++; //左下方
                            }
                            if (aroundPoint < maxAroundPoints)//目标点是噪点
                            {
                                ptr[0] = ptr[1] = ptr[2] = 255; //去噪点
                            }
                        }
                        ptr += 4;
                    }
                    ptr += bdata.Stride - width * 4;
                }
            }
            img.UnlockBits(bdata);

            #endregion

            return img;
        }

        /// <summary>
        /// 二值化处理
        /// </summary>
        /// <param name="src"></param>
        /// <param name="v">二值化阈值</param>
        /// <returns></returns>
        public static Bitmap PBinary(Bitmap src, int v)
        {
            int w = src.Width;
            int h = src.Height;
            Bitmap dstBitmap = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
            BitmapData srcData = src.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            BitmapData dstData = dstBitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb);
            unsafe
            {
                byte* pIn = (byte*)srcData.Scan0.ToPointer();
                byte* pOut = (byte*)dstData.Scan0.ToPointer();
                byte* p;
                int r, g, b;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        p = pIn;
                        r = p[2];
                        g = p[1];
                        b = p[0];
                        pOut[0] = pOut[1] = pOut[2] = (byte)(((byte)(0.2125 * r + 0.7154 * g + 0.0721 * b) >= v) ? 255 : 0);
                        //pOut[0] = pOut[1] = pOut[2] = (byte)(r >= v? 255 : 0);
                        pIn += 4;
                        pOut += 4;
                    }
                    pIn += srcData.Stride - w * 4;
                    pOut += srcData.Stride - w * 4;
                }
                src.UnlockBits(srcData);
                dstBitmap.UnlockBits(dstData);
                return dstBitmap;
            }
        }

        /// <summary>
        /// 二值化处理
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Bitmap Binary(Bitmap img)
        {
            int width = img.Width;
            int height = img.Height;
            BitmapData bdata = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite,
                PixelFormat.Format32bppRgb);
            unsafe
            {
                byte* start = (byte*)bdata.Scan0.ToPointer();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (start[0] != 255)
                        {
                            start[0] = start[1] = start[2] = 0;
                        }
                        start += 4;
                    }
                    start += bdata.Stride - width * 4;
                }
            }
            img.UnlockBits(bdata);
            return img;
        }

        public static int ComputeThresholdValue(Bitmap img)
        {
            int i;
            int k;
            double csum;
            int thresholdValue = 1;
            int[] ihist = new int[0x100];
            for (i = 0; i < 0x100; i++)
            {
                ihist[i] = 0;
            }
            int gmin = 0xff;
            int gmax = 0;
            for (i = 1; i < (img.Width - 1); i++)
            {
                for (int j = 1; j < (img.Height - 1); j++)
                {
                    int cn = img.GetPixel(i, j).R; //生成直方图
                    ihist[cn]++;
                    if (cn > gmax)
                    {
                        gmax = cn; //找到最大像素点R
                    }
                    if (cn < gmin)
                    {
                        gmin = cn; //找到最小像素点R

                    }
                }
            }
            double sum = csum = 0.0;
            int n = 0;
            for (k = 0; k <= 0xff; k++)
            {
                sum += k * ihist[k];
                n += ihist[k];
            }
            if (n == 0)
            {
                return 60;
            }
            double fmax = -1.0;
            int n1 = 0;
            for (k = 0; k < 0xff; k++)
            {
                n1 += ihist[k];
                if (n1 != 0)
                {
                    int n2 = n - n1;
                    if (n2 == 0)
                    {
                        return thresholdValue;
                    }
                    csum += k * ihist[k];
                    double m1 = csum / ((double)n1);
                    double m2 = (sum - csum) / ((double)n2);
                    double sb = ((n1 * n2) * (m1 - m2)) * (m1 - m2);
                    if (sb > fmax)
                    {
                        fmax = sb;
                        thresholdValue = k;
                    }
                }
            }
            return thresholdValue;
        }

        #region 图片切割

        /// <summary>
        /// 坐标点
        /// </summary>
        public class Boundary
        {
            public int StartY { get; set; }
            public int EndY { get; set; }
            public int StartX { get; set; }
            public int EndX { get; set; }
        }


        /// <summary>
        /// 字符数目是四个，有切割后不是4个的话会做进一步处理
        /// </summary>
        /// <param name="img"></param>
        /// <param name="ww"></param>
        /// <param name="hh"></param>
        /// <param name="is4Chars">是否是4个字</param>
        /// <returns></returns>
        public static List<Bitmap> CutImage(Image img, int ww, int hh, bool is4Chars = true,int charWidth=10)
        {
            Bitmap bmp = null;
            List<Boundary> list = GetImgBoundaryList(img, ref bmp, is4Chars,charWidth);
            List<Bitmap> imgList = new List<Bitmap>();

            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Boundary bd = list[i];
                    int _startY = bd.StartY;
                    int _endY = bd.EndY;
                    int _startX = bd.StartX;
                    int _endX = bd.EndX;
                    Bitmap bmp1 = new Bitmap(_endY - _startY, _endX - _startX);
                    //bmp1 = new ImageHandler().TrimBmp(bmp1);
                    for (int j = _startX; j < _endX; j++)
                    {
                        for (int k = _startY; k < _endY; k++)
                        {
                            Color pixelColor = bmp.GetPixel(k, j);
                            bmp1.SetPixel(k - _startY, j - _startX, pixelColor);
                        }
                    }
                    //归一化
                    Bitmap _bmp1 = Normalized(bmp1, ww, hh);
                    for (int j = 0; j < _bmp1.Height; j++)
                    {
                        for (int k = 0; k < _bmp1.Width; k++)
                        {
                            Color pixelColor = _bmp1.GetPixel(k, j);
                            if (pixelColor.R != 255)
                            {
                                _bmp1.SetPixel(k, j, Color.Black);
                            }
                        }
                    }
                    imgList.Add(_bmp1);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return imgList;

        }

        /// <summary>
        /// 确定是四字符会有精细处理
        /// </summary>
        /// <param name="img"></param>
        /// <param name="_bmp"></param>
        /// <param name="is4Chars"></param>
        /// <returns></returns>
        public static List<Boundary> GetImgBoundaryList(Image img, ref Bitmap _bmp, bool is4Chars,int charWidth=10)
        {
            List<Boundary> list = new List<Boundary>();
            Bitmap bmp = new Bitmap(img);
            int startY = 0;
            int endY = 0;
            int startX = 0;
            int endX = 0;
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    startY = GetStartBoundaryY(img, startY);
                    if (startY >= 0)
                    {
                        endY = GetEndBoundaryY(img, startY + 1);
                        //标记
                        startX = GetStartBoundaryX(img, startY, endY);
                        int _startX = startX;
                        bool flag = false;
                        while (!flag && _startX < img.Height)
                        {
                            endX = GetEndBoundaryX(img, _startX, startY, endY);
                            flag = endX - _startX >=5;//最小高度
                            _startX = _startX + 1;
                        }
                        if (endX > 0 && endX - startX >= 5)//最小高度
                        {
                            list.Add(new Boundary { EndY = endY, EndX = endX, StartY = startY, StartX = startX });
                        }
                        startY = endY;
                    }
                    else
                    {
                        break;
                    }
                }
                if (is4Chars&&list.Count!=4)
                {
                    list = NotFourChars(img, out _bmp, list,charWidth);
                }
                else
                {
                    _bmp = bmp;
                }
                List<Boundary> tempBdList = list;
                for (int i = 0; i < tempBdList.Count; i++)
                {
                    if (
                        !((tempBdList[i].EndY - tempBdList[i].StartY >= charWidth) &&//最小宽度比较
                          (tempBdList[i].EndX - tempBdList[i].StartX >= 5)))//最小高度
                    {
                        list.Remove(tempBdList[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return list;
        }
        /// <summary>
        /// 对第一次切割后不是4张小图的做处理
        /// </summary>
        /// <param name="img"></param>
        /// <param name="_bmp"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<Boundary> NotFourChars(Image img, out Bitmap _bmp, List<Boundary> list,int charWidth)
        {
            if (list.Count == 0)
            {
                _bmp = TrimBmp(img);
                list.Add(new Boundary() { StartX = 0, EndX = _bmp.Height - 1, EndY = _bmp.Width - 1, StartY = 0 });
            }
            else
            {
                _bmp = new Bitmap(img);
            }
            if (list.Count == 1)
            {
                //平均切割
                Boundary bd = list[0];
                int _startY = bd.StartY;
                int _endY = _startY;
                int _startX = bd.StartX;
                int _endX = bd.EndX;
                int avr = (list[0].EndY - list[0].StartY) / 4;
                list.Clear();
                for (int i = 0; i < 4; i++)
                {
                    _endY = _startY + avr;
                    list.Add(new Boundary { EndY = _endY, EndX = _endX, StartY = _startY, StartX = _startX });
                    _startY = _endY+1;
                }
            }
            if (list.Count == 3)
            {
                Boundary max = list[0];
                int index = 0;
                for (int i = 1; i < list.Count; i++)
                {
                    Boundary _max = list[i];
                    if (max.EndY - max.StartY < _max.EndY - _max.StartY)
                    {
                        index = i;
                        max = _max;
                    }
                }
                //平分
                int _startY = max.StartY;
                int _endY = _startY;
                int _startX = max.StartX;
                int _endX = max.EndX;
                int avr = (max.EndY - max.StartY) / 2;
                list.Remove(max);
                for (int i = 0; i < 2; i++)
                {
                    _endY = _startY + avr;
                    list.Insert(index, new Boundary { EndY = _endY, EndX = _endX, StartY = _startY, StartX = _startX });
                    index = index + 1;
                    _startY = _endY + 1;
                }
            }
            if (list.Count == 2)
            {
                if (Math.Abs((list[1].EndY - list[1].StartY) - (list[0].EndY - list[0].StartY)) > 1.5*charWidth)
                {
                    Boundary max = (list[1].EndY - list[1].StartY) > (list[0].EndY - list[0].StartY)
                        ? list[1]
                        : list[0];
                    //平分
                    int index = (list[1].EndY - list[1].StartY) > (list[0].EndY - list[0].StartY) ? 1 : 0;
                    int _startY = max.StartY;
                    int _endY = _startY;
                    int _startX = max.StartX;
                    int _endX = max.EndX;
                    int avr = (max.EndY - max.StartY) / 3;
                    list.Remove(max);
                    for (int i = 0; i < 3; i++)
                    {
                        _endY = _startY + avr;
                        list.Insert(index,
                            new Boundary { EndY = _endY, EndX = _endX, StartY = _startY, StartX = _startX });
                        index = index + 1;
                        _startY = _endY + 1;
                    }
                }
                else
                {
                    List<Boundary> _temp = new List<Boundary>();
                    for (int i = 0; i < 2; i++)
                    {
                        Boundary max = list[i];
                        //平分
                        int _startY = max.StartY;
                        int _endY = _startY;
                        int _startX = max.StartX;
                        int _endX = max.EndX;
                        int avr = (max.EndY - max.StartY) / 2;
                        for (int j = 0; j < 2; j++)
                        {
                            _endY = _startY + avr;
                            _temp.Add(new Boundary { EndY = _endY, EndX = _endX, StartY = _startY, StartX = _startX });
                            _startY = _endY + 1;
                        }
                    }
                    list = _temp;
                }
            }
            return list;
        }

        public static Bitmap TrimBmp(Image img)
        {
            int[,] inputX = ConvertImgToArrayY(img);
            int[,] inputY = ConvertImgToArrayX(img);
            Point beyondX = GetXBeyond(inputX);
            Point beyondY = GetYBeyond(inputY);
            int w = beyondY.Y - beyondY.X + 1;
            int h = beyondX.Y - beyondX.X + 1;
            Bitmap bmp = new Bitmap(w, h);
            for (int i = 0; i < h; i++)
            {
                int _y = i + beyondX.X;
                for (int j = 0; j < w; j++)
                {
                    int _x = j + beyondY.X;
                    int v = inputX[_y, _x];
                    if (v == 1)
                    {
                        bmp.SetPixel(j, i, Color.Black);
                    }
                    else
                        bmp.SetPixel(j, i, Color.White);
                }
            }
            return bmp;
        }

        public static int[,] ConvertImgToArrayY(Image img)
        {
            Bitmap bmp = new Bitmap(img);
            int[,] input = new int[bmp.Height, bmp.Width];
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color color = bmp.GetPixel(j, i);
                    int v = color.R == 255 ? 0 : 1;
                    input[i, j] = v;
                }
            }
            return input;
        }

        public static int[,] ConvertImgToArrayX(Image img)
        {
            Bitmap bmp = new Bitmap(img);
            int[,] input = new int[bmp.Width, bmp.Height];
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color color = bmp.GetPixel(i, j);
                    int v = color.R == 255 ? 0 : 1;
                    input[i, j] = v;
                }
            }
            return input;
        }

        public static int[] GetArrayX(int[,] input, int y)
        {
            int w = input.GetLength(1);
            int[] xArray = new int[w];
            for (int j = 0; j < w; j++)
            {
                xArray[j] = input[y, j];
            }
            return xArray;
        }

        public static int[] GetArrayY(int[,] input, int x)
        {
            int h = input.GetLength(1);
            int[] yArray = new int[h];
            for (int j = 0; j < h; j++)
            {
                yArray[j] = input[x, j];
            }
            return yArray;
        }

        public static string Array2String(int[] array, bool flag)
        {
            string str = "";
            foreach (int a in array)
            {
                str = str + a;
            }
            if (flag)
            {
                while (str.StartsWith("0"))
                {
                    str = str.Remove(0, 1);
                }
                while (str.EndsWith("0"))
                {
                    int eIndex = str.LastIndexOf("0");
                    str = str.Remove(eIndex);
                }
            }
            return str;
        }

        public static Point GetYBeyond(int[,] input)
        {
            int h = 0;
            h = input.GetLength(0);
            int y1 = 0;
            for (int i = 0; i < h; i++)
            {
                int[] xArray = GetArrayX(input, i);
                string str = Array2String(xArray, false);
                if (str.IndexOf("1") > -1)
                {
                    y1 = i;
                    break;
                }
            }
            int y2 = 0;
            for (int i = h - 1; i > 0; i--)
            {
                int[] xArray = GetArrayX(input, i);
                string str = Array2String(xArray, false);
                if (str.IndexOf("1") > -1)
                {
                    y2 = i;
                    break;
                }
            }
            return new Point(y1, y2);
        }

        public static Point GetXBeyond(int[,] input)
        {
            int w = 0;
            w = input.GetLength(0);
            int x1 = 0;
            for (int i = 0; i < w; i++)
            {
                int[] yArray = GetArrayY(input, i);
                string str = Array2String(yArray, false);
                if (str.IndexOf("1") > -1)
                {
                    x1 = i;
                    break;
                }
            }
            int x2 = 0;
            for (int i = w - 1; i > 0; i--)
            {
                int[] yArray = GetArrayY(input, i);
                string str = Array2String(yArray, false);
                if (str.IndexOf("1") > -1)
                {
                    x2 = i;
                    break;
                }
            }
            return new Point(x1, x2);
        }

        /// <summary>
        /// Y轴开始的边界
        /// </summary>
        /// <param name="img"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int GetStartBoundaryY(Image img, int start)
        {
            Bitmap bmp = new Bitmap(img);
            int startB = 0;
            for (int i = start; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //遍历各个像素，获得bmp位图每个像素的RGB对象
                    Color pixelColor = bmp.GetPixel(i, j);
                    if (pixelColor.Name != "ffffffff")
                    {
                        startB = i;
                        break;
                    }
                }
                if (startB != 0)
                {
                    break;
                }
            }
            if (startB == start)
            {
                return startB + 2;
            }
            else
            {
                return startB - 1;
            }
        }

        /// <summary>
        /// Y轴结束的边界
        /// </summary>
        /// <param name="img"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        private static int GetEndBoundaryY(Image img, int start)
        {
            Bitmap bmp = new Bitmap(img);
            int endB = 0;
            for (int i = start; i < bmp.Width; i++)
            {
                int cnt = 0;
                for (int j = 0; j < bmp.Height; j++)
                {
                    //遍历各个像素，获得bmp位图每个像素的RGB对象
                    Color pixelColor = bmp.GetPixel(i, j);
                    //TODO：全白
                    if (pixelColor.Name == "ffffffff")
                    {
                        cnt++;
                        continue;
                    }
                    else
                        break;
                }
                if (bmp.Height -2<=cnt&&cnt <= bmp.Height)
                {
                    endB = i;
                    break;
                }
            }
            return endB;
        }

        /// <summary>
        /// X轴开始的边界
        /// </summary>
        /// <param name="img"></param>
        /// <param name="startY"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        private static int GetStartBoundaryX(Image img, int startY, int endY)
        {
            Bitmap bmp = new Bitmap(img);
            int startB = 0;
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = startY; j < endY; j++)
                {
                    //遍历各个像素，获得bmp位图每个像素的RGB对象
                    Color pixelColor = bmp.GetPixel(j, i);
                    if (pixelColor.Name != "ffffffff")
                    {
                        startB = i;
                        break;
                    }
                    else
                        continue;
                }
                if (startB != 0)
                {
                    break;
                }
                else
                    continue;
            }
            return startB - 1;
        }

        /// <summary>
        /// 获得X轴结束边界
        /// </summary>
        /// <param name="img"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        private static int GetEndBoundaryX(Image img, int startX, int startY, int endY)
        {
            Bitmap bmp = new Bitmap(img);
            int endB = 0;
            for (int i = startX + 1; i < bmp.Height; i++)
            {
                int cnt = 0;
                for (int j = startY; j < endY; j++)
                {
                    //遍历各个像素，获得bmp位图每个像素的RGB对象
                    Color pixelColor = bmp.GetPixel(j, i);
                    if (pixelColor.Name == "ffffffff")
                    {
                        cnt++;
                        continue;
                    }
                    else
                        break;
                }
                if (cnt == endY - startY&&i-startX>4)//防止把i的点也算进去
                {
                    endB = i;
                    break;
                }
            }
            return endB;
        }

        //--------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取有效区域
        /// </summary>
        /// <param name="dgGrayValue">灰度值</param>
        /// <param name="bm">图片对象</param>
        /// <returns></returns>
        public static Bitmap GetPicValidByValue(int dgGrayValue, Bitmap bm)
        {
            int posx1 = bm.Width;
            int posy1 = bm.Height;
            int posx2 = 0;
            int posy2 = 0;
            for (int i = 0; i < bm.Height; i++) //找有效区
            {
                for (int j = 0; j < bm.Width; j++)
                {
                    int pixelValue = bm.GetPixel(j, i).R;
                    if (pixelValue < dgGrayValue) //根据灰度值
                    {
                        if (posx1 > j) posx1 = j;
                        if (posy1 > i) posy1 = i;

                        if (posx2 < j) posx2 = j;
                        if (posy2 < i) posy2 = i;
                    }

                }

            }

            //复制新图
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            bm = bm.Clone(cloneRect, bm.PixelFormat);
            return bm;
        }

        /// <summary>
        /// 图片切割
        /// </summary>
        /// <param name="RowNum"></param>
        /// <param name="ColNum"></param>
        /// <param name="bm"></param>
        /// <returns></returns>
        public static Bitmap[] CutImage(int RowNum, int ColNum, Bitmap bm)
        {
            if (RowNum == 0 || ColNum == 0)
                return null;
            int singW = bm.Width / RowNum;
            int singH = bm.Height / ColNum;
            Bitmap[] PicArray = new Bitmap[RowNum * ColNum];

            Rectangle cloneRect;
            for (int i = 0; i < ColNum; i++) //找有效区
            {
                for (int j = 0; j < RowNum; j++)
                {
                    cloneRect = new Rectangle(j * singW, i * singH, singW, singH);
                    PicArray[i * RowNum + j] = GetPicValidByValue(bm.Clone(cloneRect, bm.PixelFormat), 128); //复制小块图
                }
            }
            return PicArray;

        }

        /// <summary>
        /// 对切割后小图操作
        /// </summary>
        /// <param name="singlepic"></param>
        /// <param name="dgGrayValue"></param>
        /// <returns></returns>
        public static Bitmap GetPicValidByValue(Bitmap singlepic, int dgGrayValue)
        {
            int posx1 = singlepic.Width;
            int posy1 = singlepic.Height;
            int posx2 = 0;
            int posy2 = 0;
            for (int i = 0; i < singlepic.Height; i++) //找有效区
            {
                for (int j = 0; j < singlepic.Width; j++)
                {
                    int pixelValue = singlepic.GetPixel(j, i).R;
                    if (pixelValue < dgGrayValue) //根据灰度值
                    {
                        if (posx1 > j) posx1 = j;
                        if (posy1 > i) posy1 = i;

                        if (posx2 < j) posx2 = j;
                        if (posy2 < i) posy2 = i;
                    }

                }

            }

            //复制新图
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            return singlepic.Clone(cloneRect, singlepic.PixelFormat);
        }

        #endregion

        /// <summary>
        /// 把图片的宽高统一，归一
        /// </summary>
        /// <param name="bitmap">需要处理的图片</param>
        public static Bitmap Normalized(Bitmap bitmap, int ww, int hh)
        {
            Bitmap temp = new Bitmap(ww, hh);
            Graphics myGraphics = Graphics.FromImage(temp);
            //源图像中要裁切的区域
            Rectangle sourceRectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            ////缩小后要绘制的区域
            Rectangle destRectangle = new Rectangle(0, 0, ww, hh);
            myGraphics.Clear(Color.White);
            ////绘制缩小的图像
            myGraphics.DrawImage(bitmap, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
            myGraphics.Dispose();
            return temp;
        }
        /// <summary>
        /// 获取图片特征码
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string GetBinaryCode(Bitmap img)
        {
            StringBuilder sb = new StringBuilder();
            int width = img.Width;
            int height = img.Height;
            BitmapData bdata = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppRgb);
            unsafe
            {
                byte* start = (byte*)bdata.Scan0.ToPointer();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (start[0] == 255)
                        {
                            sb.Append("0");

                        }
                        else
                        {
                            sb.Append("1");
                        }
                        start += 4;
                    }
                    start += bdata.Stride - width * 4;
                }
            }
            img.UnlockBits(bdata);
            return sb.ToString();

        }
        /// <summary>
        /// 计算相似度
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static int CalcRate(string t1, string t2)
        {
            if (t1.Length > 0 && t2.Length > 0)
            {
                char[] b1 = t1.ToCharArray();
                char[] b2 = t2.ToCharArray();
                var result = b1.Zip(b2, (b11, b22) => b11 ^ b22).ToArray();
                int cnt = 0;
                for (int i = 0; i < result.Length; i++)
                {
                    int str = result[i];
                    if (str == 0)
                    {
                        cnt++;
                    }
                }
                return cnt * 100 / result.Length;
            }
            else
                return 0;
        }
        /// <summary>
        /// 验证码的前期处理
        /// </summary>
        /// <param name="img">验证码图片</param>
        /// <param name="type">灰度处理方式1：最大值2：平均值3：加权平均</param>
        /// <param name="maxNearPoints">噪点最大粘连数目</param>
        /// <param name="smallPicWidth">切分小图宽度</param>
        /// <param name="smallPicHeight">切分小图高度</param>
        /// <returns></returns>
        public static List<Bitmap> PreProcess(Image img, int maxNearPoints, int smallPicWidth, int smallPicHeight,int type=2,bool is4Chars=true,int charWidth=10)
        {
            img = Gray((Bitmap)img,type);
            int v = ComputeThresholdValue((Bitmap)img);
            img = RemoveBg((Bitmap)img, v);
            img = RemoveNoise((Bitmap)img, maxNearPoints);
            img = Binary((Bitmap)img);
            List<Bitmap> list = CutImage(img, smallPicWidth, smallPicHeight,is4Chars,charWidth);
            return list;
        }
        /// <summary>
        /// 识别验证码(图片-->字符)
        /// </summary>
        /// <param name="img">验证码图片</param>
        /// <param name="zimoPath">字模数据库</param>
        /// <param name="maxNearPoints">噪点最大粘连数目</param>
        /// <param name="smallPicWidth">切分小图宽度</param>
        /// <param name="smallPicHeight">切分小图高度</param>
        /// <paramname="charWidht">验证码字符宽度，细节处理时用到</param>
        /// <returns>识别后的验证码字符</returns>
        public static string GetYZMCode(Image img, string zimoPath, int maxNearPoints, int smallPicWidth, int smallPicHeight,int type=2,bool is4Chars=true,int charWidth=10)
        {
            //1.0验证码前期处理
            List<Bitmap> list = PreProcess(img, maxNearPoints, smallPicWidth, smallPicHeight,type,is4Chars,charWidth);
            string yanzhengma = "";
            //2.0识别
            if (list.Count > 0)
            {
                //2.1读取字模
                string[] zimo = File.ReadAllLines(zimoPath);

                for (int i = 0; i < list.Count; i++)
                {
                    //2.2图片--->特征码
                    string code = GetBinaryCode(list[i]);
                    int rate = 0;
                    string subCode = "";
                    for (int j = 0; j < zimo.Length; j++)
                    {
                        string[] subZimo = zimo[j].Split(new string[] { "--" }, StringSplitOptions.None);
                        //2.3计算相似度
                        int temp = CalcRate(code, subZimo[1]);
                        if (temp > rate)
                        {
                            rate = temp;
                            subCode = subZimo[0];
                        }
                    }
                    yanzhengma += subCode;

                }
            }
            return yanzhengma;
        }
        /// <summary>
        /// 验证码识别
        /// </summary>
        /// <param name="img">验证码</param>
        /// <param name="zimoPath">字模位置</param>
        /// <param name="charNum">验证码字符个数</param>
        /// <returns>识别后的验证码字符</returns>
        public static string GetYZMCode(Image img, string zimoPath, int charNum)
        {
            return GetYZMCode(img, zimoPath, 1, img.Width / charNum, img.Height);
        }
        /// <summary>
        /// 字模库维护
        /// </summary>
        /// <param name="smallPic">小图片</param>
        /// <param name="zimoPath">字模路径</param>
        /// <param name="YZMCode">验证码字符</param>
        public static void WriteZimo(Bitmap smallPic, string zimoPath, string YZMCode)
        {
            string code = GetBinaryCode(smallPic);
            string zimo = YZMCode + "--" + code + "\r\n";
            string[] zimos = File.ReadAllLines(zimoPath);
            if (!zimos.Contains(zimo))
            {
                File.AppendAllText(zimoPath, zimo);
            }
        }
    }
}
