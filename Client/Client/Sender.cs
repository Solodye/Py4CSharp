using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// 与发送，发送信息预处理有关的方法在Sender类里面
    /// </summary>
    static class Sender
    {
        
        /// <summary>
        /// 预处理信息的函数，发送，需要先将字符串信息处理成可以发送的byte[]数组型
        /// 注意！这种基于字符串的方法不能处理二进制文件
        /// </summary>
        /// <param name="SPLIT_STR">切分字符串</param>
        /// <param name="TRAILER">尾标</param>
        /// <param name="mclassname">算法类名称</param>
        /// <param name="parameters">存放参数的字符数组</param>
        /// <returns></returns>
        internal static byte[] PreTreatment(string SPLIT_STR, string TRAILER, string mclassname, string[] parameters)
        {
            //添加方法类名
            List<byte> msgsend = System.Text.Encoding.UTF8.GetBytes(mclassname).ToList<byte>();
            //循环处理要发送的信息
            List<byte> msgsplit = System.Text.Encoding.UTF8.GetBytes(SPLIT_STR).ToList<byte>();
            foreach (string p in parameters)
            {
                //添加分隔符
                msgsend.AddRange(msgsplit);
                //添加参数值
                List<byte> msgpara = System.Text.Encoding.UTF8.GetBytes(p).ToList<byte>();
                msgsend.AddRange(msgpara);
            }
            //测试长度
            Console.WriteLine(msgsend.ToArray().Length);
            //添加末尾标志
            List<byte> msgtail = System.Text.Encoding.UTF8.GetBytes(TRAILER).ToList<byte>();
            msgsend.AddRange(msgtail);
            return msgsend.ToArray();
        }

        /// <summary>
        /// 传递文件，字符串的二进制形式
        /// </summary>
        /// <param name="SPLIT_STR"></param>
        /// <param name="TRAILER"></param>
        /// <param name="mclassname"></param>
        /// <param name="byteArrList"></param>
        /// <returns></returns>
        internal static byte[] PreTreatment(string SPLIT_STR, string TRAILER, string mclassname, List<byte[]> byteArrList)
        {
            //添加方法类名
            List<byte> msgsend = System.Text.Encoding.UTF8.GetBytes(mclassname).ToList<byte>();
            //循环处理要发送的信息
            List<byte> msgsplit = System.Text.Encoding.UTF8.GetBytes(SPLIT_STR).ToList<byte>();
            foreach (byte[] b in byteArrList)
            {
                //添加分隔符
                msgsend.AddRange(msgsplit);
                //添加byte参数值
                msgsend.AddRange(b.ToArray<byte>());
            }
            //测试长度
            Console.WriteLine(msgsend.ToArray().Length);
            //添加末尾标志
            List<byte> msgtail = System.Text.Encoding.UTF8.GetBytes(TRAILER).ToList<byte>();
            msgsend.AddRange(msgtail);
            return msgsend.ToArray();
        }

    }
}
