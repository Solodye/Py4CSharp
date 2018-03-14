using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Client
{
    public class Receiver
    {

        internal static List<string> Receive(Socket socket, string SPLIT_STR, string TRAILER, int BUFSIZE, int MAXMAX_BLOCK_TIME)
        {
            Console.WriteLine("开始接收数据");

            //尾标法开始接收数据
            //这里字节数组不能灵活运用，接收完毕后使用字符串匹配函数
            string recvStr = "";
            //接收成功的标志
            bool success = true;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while(true)
            {
                byte[] buffer = new byte[BUFSIZE];
                try
                {
                    socket.Receive(buffer);
                }
                catch (Exception e)
                {
                    Console.WriteLine("接收信息错误：" + e.Message);
                }
                //如果一直为空或者一直接收\0，即服务端结束了，超过规定时间 则接收失败，中断接收
                //这里用正则表达匹配开头就是\0的情况
                if (buffer.Length > 0 && 
                    !Regex.IsMatch(System.Text.Encoding.UTF8.GetString(buffer),"^\0")) watch.Restart();
                else
                {
                    if(watch.Elapsed.TotalSeconds > double.Parse(MAXMAX_BLOCK_TIME.ToString()))
                    success = false;
                    break;
                }
                //这里字节数组不能灵活运用，考虑使用字符串匹配函数
                string bufStr = System.Text.Encoding.UTF8.GetString(buffer);
                recvStr = string.Concat(recvStr, bufStr);
                //自尾部寻找，如果有尾标，那说明接收成功，将尾标剔除，break出去
                int trailerPos = recvStr.LastIndexOf(TRAILER);
                if (trailerPos > 0)
                {
                    recvStr = recvStr.Substring(0, trailerPos);
                    break;
                }
            }


            //处理接收好的数据
            if (!success)
            {
                return null;
            }
            //string[] solveArr = recvStr.Split(SPLIT_STR.ToCharArray());
            //正则化会切分出很多个字符
            string[] solveArr = Regex.Split(recvStr, SPLIT_STR);
            List<string> solve = new List<string>();
            foreach (string str in solveArr)
            {
                if (!string.IsNullOrWhiteSpace(str))
                    solve.Add(str);
            }
            return solve;
        }
    }
}
