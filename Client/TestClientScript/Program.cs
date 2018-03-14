using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Client;
using System.Diagnostics;
using PicManipulation;


namespace TestClientScript
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientCore client = new ClientCore();
            //这样不传参，尝试调用空参就会出错
            //还有个bug就是服务端下线时接收到的东西全是\0\0\0跳不出while循环
            //List<string> solve = client.RequestAlgorithm("GetLongStr");
            //尝试一下传输图片 打成二进制
            
            //List<string> solve = client.RequestAlgorithm("PlusAndMinor", "6", "8");
            
            //问题：用utf8编码和解码过的byte数组具有不同的长度
            byte[] imageb = IOHelper.ReadBinaryFile(@"D:\数据\图片_照片\IMG_20180302_200425.jpg");
            byte[] image2b = IOHelper.ReadBinaryFile(@"C:\Users\Xiblade\Desktop\test.jpg");
            Console.WriteLine(imageb.Length);
            /*
            string s = System.Text.Encoding.UTF8.GetString(imageb);
            imageb = System.Text.Encoding.UTF8.GetBytes(s);
            Console.WriteLine(imageb.Length);
            */

            //客户端已经完成了发送二进制文件的部分
            byte[] filename_b = System.Text.Encoding.UTF8.GetBytes("20173154");
            List<string> solve = client.RequestAlgorithm("HandlePicture",filename_b.ToList<byte>() ,imageb.ToList<byte>(),image2b.ToList<byte>());
            Console.ReadKey();
        }
    }
}
