using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PicManipulation
{
    public static  class ImageHandler
    {
        public static Image ResizeImage(Image img, Size size) 
        {
            int sourceWidth = img.Width;
            int sourceHeight = img.Height;

           // float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (float)size.Width / (float)sourceWidth;
            nPercentH = (float)size.Height / (float)sourceHeight;

            //实现锁定比例缩放
            //if (nPercentH<nPercentW)
            //{
            //    nPercent=nPercentH;
            //}
            //else
            //{
            //    nPercent=nPercentW;
            //}

            int destWidth=(int)(sourceWidth*nPercentW);
            int destHeight=(int)(sourceHeight*nPercentH);

            Bitmap b = new Bitmap(destWidth,destHeight);
            Graphics g = Graphics.FromImage(b);
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(img,0,0,destWidth,destHeight);
            g.Dispose();
            return b;

        }
    }
}
