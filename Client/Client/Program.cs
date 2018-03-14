using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //自配置文件初始化配置
            /*
            ClientConfigManager cfger = new ClientConfigManager();
            Console.WriteLine(cfger.PORT.ToString());
            Console.ReadKey();
             * */
            /*
            ClientConfigManager cfger = new ClientConfigManager();
            ClientCore cc = new ClientCore(ref cfger);
            Console.ReadLine();
             */
            //byte[] a = System.Text.Encoding.UTF8.GetBytes("23一4二2344三");
           // byte[] b = System.Text.Encoding.UTF8.GetBytes("二2344");
            Console.WriteLine("wiegw".LastIndexOf("g"));
            Console.WriteLine("wiegw".Substring(0,"wiegw".LastIndexOf("g")));
            Console.WriteLine("wfeqwfwffffffwf".Split("wf".ToCharArray()));
            string[] s = Regex.Split("wfeqwfwffffffwf","wf");
            for (int i = 0; i < s.Length; i++)
            {
                Console.WriteLine(s[i]+ "====");
            }
                Console.ReadKey();

            
        }
    }
}
