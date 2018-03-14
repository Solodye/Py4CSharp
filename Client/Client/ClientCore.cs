using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Client;

namespace Client
{
    public class ClientCore
    {
        ClientConfigManager cfger = null;
        /// <summary>
        /// 传入一个只读的配置信息，启动循环的接收和发送程序
        /// 主要的循环都写在这里
        /// </summary>
        /// <param name="?"></param>
        public ClientCore(ref ClientConfigManager cfger)
        {
            this.cfger = cfger;
            JudgeConfigSuccess();
            
        }

        public ClientCore()
        {
            this.cfger = new ClientConfigManager();
        }

        private void JudgeConfigSuccess()
        {
            if(!this.cfger.loadDefaultSettingSuccess)
            {
                throw new Exception("配置文件配置失败");
                //这里想到了析构函数来实现一个类似“自杀”的行为，以后再实现
                
            }
        }

        /// <summary>
        /// 外界可以用这个函数来想方法服务器发送请求，返回字符串作为结果
        /// </summary>
        /// <param name="AlgorithmClassName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<string> RequestAlgorithm(string AlgorithmClassName, params string[] parameters)
        {
            //发起连接
            Socket socket = Connecter.Connect(this.cfger.PORT);
            if (socket == null) return null;

            // 发送调用函数信息
            socket.Send(Sender.PreTreatment(cfger.SPLIT_STR, cfger.TRAILER, AlgorithmClassName, parameters));

            //循环，接收返回结果
            List<string> solve = Receiver.Receive(socket, cfger.SPLIT_STR, cfger.TRAILER, cfger.BUFSIZE, cfger.MAX_BLOCK_SIZE);
            
            //关闭连接
            socket.Close();

            return solve;
        }

        /// <summary>
        /// 这个是向服务器发送单个二进制文件的函数
        /// </summary>
        /// <param name="AlgorithmClassName"></param>
        /// <param name="byteFile"></param>
        /// <returns></returns>
        public List<string> RequestAlgorithm(string AlgorithmClassName, byte[] byteFile)
        {
            //发起连接
            Socket socket = Connecter.Connect(this.cfger.PORT);
            if (socket == null) return null;

            //发送调用函数信息
            List<byte[]> byteArrList = new List<byte[]>();
            byteArrList.Add(byteFile);
            socket.Send(Sender.PreTreatment(cfger.SPLIT_STR, cfger.TRAILER, AlgorithmClassName, byteArrList));

            //循环，接收返回结果
            List<string> solve = Receiver.Receive(socket, cfger.SPLIT_STR, cfger.TRAILER, cfger.BUFSIZE, cfger.MAX_BLOCK_SIZE);

            //关闭连接
            socket.Close();

            return solve;
        }

        /// <summary>
        /// 向服务端发送多个二进制信息
        /// </summary>
        /// <param name="AlgorithmClassName"></param>
        /// <param name="byteFile"></param>
        /// <returns></returns>
        public List<string> RequestAlgorithm(string AlgorithmClassName, params List<byte>[] byteMsgs)
        {
            //发起连接
            Socket socket = Connecter.Connect(this.cfger.PORT);
            if (socket == null) return null;

            //发送调用函数信息
            List<byte[]> byteArrList = new List<byte[]>();
            foreach(List<byte>byteList in byteMsgs)
            {
                byteArrList.Add(byteList.ToArray());
            }
            socket.Send(Sender.PreTreatment(cfger.SPLIT_STR, cfger.TRAILER, AlgorithmClassName, byteArrList));

            //循环，接收返回结果
            List<string> solve = Receiver.Receive(socket, cfger.SPLIT_STR, cfger.TRAILER, cfger.BUFSIZE, cfger.MAX_BLOCK_SIZE);

            //关闭连接
            socket.Close();

            return solve;
        }
       

    }
}
