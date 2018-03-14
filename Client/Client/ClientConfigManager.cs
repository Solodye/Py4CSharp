using System;
using System.Net;
using System.Configuration;


namespace Client
{
    public class ClientConfigManager
    {
        // 由配置文件导出的属性
        public IPAddress HOST { get; set; }
        public IPEndPoint PORT { get; set; }
        public int BUFSIZE { get; set; }
        //public int MAXLISTEN { get; set; }
        public string SPLIT_STR { get; set; }
        public string TRAILER { get; set; }
        public int MAX_BLOCK_SIZE { get; set; }

        public bool loadDefaultSettingSuccess = true;
        public bool LoadDefaultSettingSuccess
        {
            get { return this.loadDefaultSettingSuccess; }
        }

        /// <summary>
        /// 构造器保证在初始化时，自配置文件载入相应的变量
        /// </summary>
        public ClientConfigManager()
        {
            this.loadDefaultSettingSuccess = true;
            try
            {
                //调用配置文件中的内容
                this.HOST = IPAddress.Parse(ConfigurationManager.AppSettings["HOST"].ToString());
                this.PORT = new IPEndPoint(this.HOST, int.Parse(ConfigurationManager.AppSettings["PORT"].ToString()));
                this.BUFSIZE = int.Parse(ConfigurationManager.AppSettings["BUFSIZE"].ToString());
                //this.MAXLISTEN = int.Parse(ConfigurationManager.AppSettings["MAXLISTEN"].ToString());
                this.SPLIT_STR = ConfigurationManager.AppSettings["SPLIT_STR"].ToString();
                this.TRAILER = ConfigurationManager.AppSettings["TRAILER"].ToString();
                this.MAX_BLOCK_SIZE = int.Parse(ConfigurationManager.AppSettings["MAX_BLOCK_TIME"].ToString());
            }
            catch 
            {
                Console.WriteLine("加载配置文件错误");
                loadDefaultSettingSuccess = false;
            }
          
            
        }
    }
}
