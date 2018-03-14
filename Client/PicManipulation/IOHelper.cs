using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


namespace PicManipulation
{
    public static class IOHelper
    {
        //将二进制流转换为Image（BItmap）类型的方法，这个方法可能会经常使用
        public static Image ConvertImage(byte[] imgb)
        {
            Image img = null;
            if (imgb != null)
            {
                MemoryStream ms = new MemoryStream(imgb);
                img = Image.FromStream(ms);
            }
            return img;
        }

        //在本地直接读取二进制文件的方法
        public static byte[] ReadBinaryFile(string path)
        {
            byte[] buffer = null;
            using (FileStream fs = File.OpenRead(path))
            {
                int filelength = 0;
                filelength = (int)fs.Length; //获得文件长度 
                buffer = new Byte[filelength]; //建立一个字节数组 
                fs.Read(buffer, 0, filelength); //按字节流，一次性读取
            }
            return buffer;
        }

        //这是在本机读取图片的方法,是ReadBinaryFile方法的特例
        public static Image ReadPicture(string picPath)
        {
            byte[] imgb = ReadBinaryFile(picPath);
            return ConvertImage(imgb);
        }


        public static byte[] ConvertBinaryArray(Image _img)
        {
            MemoryStream ms = new MemoryStream();
            _img.Save(ms, ImageFormat.Png);
            byte[] b = ms.GetBuffer();
            return b;
        }
    }
}
