using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;

using System.Threading.Tasks;
using System.Timers;


namespace Common
{
    #region AIP声明 

    #endregion

    partial class  Program
    {

        // private static PerformanceCounter avgCounter64Sample;
        public static double UnixTimestamp(DateTime dt) => dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        public static bool HashCompare(string s1, string s2)
        {
            if (object.Equals(s1.TrimEnd(), s2.TrimEnd()))
                return true;
            else
                return false;
        }
        public static ConcurrentDictionary<string, string> m_table_cache = new ConcurrentDictionary<string, string>();


        //public XLua.luaFunc luaFunc= new XLua.luaFunc();

        static async Task DoSomethingAsync()
        {

            int val = 13;
            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

            val *= 2;
            Console.WriteLine(val);

            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

            Console.WriteLine(val);

                
        }
        public class  classT
        {

            int a;
            int b;
        }

   

        static void Main(string[] args)
        {

            //  //Hashtable
            //  string sr1 = valueInfoList.ReadFromCSV("D:\\work\\colorland_server_lastest\\Server\\ServerRelease\\otherdata\\buffer\\bufferMain.csv");

            //  string sr2 = valueInfoList.ReadFromCSV("D:\\work\\colorland_server_lastest\\Server\\ServerRelease\\otherdata\\buffer\\bufferMain1.csv");
            ////  using (HashAlgorithm hash = HashAlgorithm.Create())

            //      if (String.Equals(sr1, sr2))
            //        Console.WriteLine("Equal");
            //      else
            //          Console.WriteLine("Not Equal");

            //     bool b= HashCompare(sr1, sr2);


            //  string hash = GetHash("data.txt");

            //    Console.WriteLine("Hash: {0}", hash);


            //System.Threading.Timer aTimer = new System.Threading.Timer((new TimerCallback(test11)),null, 0,2000);

            string filename = "./a.lua";
            XLua.luaFunc luaFunc = new XLua.luaFunc();

            luaFunc.ExecFile(filename);

            Console.ReadLine();
            //aTimer.Dispose(); ;

        }

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            // t.DeleteMeInDB();
        }
        static public void test11(Object expireTime)
        {
            Console.WriteLine("11");

            
        }

        public void test(ref int expireTime)
        {
            Console.WriteLine("11");
            expireTime++;
        }


        public static void Fuc_test(Func<string> action)
        {
           action();
        }
        public void RegisterDynamicLoad<T>(string name, Func<string, T> factory) where T : class
        {


        }

    }
}

namespace buffer
{
    public class info
    {

        static public info m_instance = new info();
        public info()
        {
        }

        Dictionary<int,int> m_dir = new Dictionary<int, int>();
        public Dictionary<string, List<string>> m_applicant = new Dictionary<string, List<string>>();


        Dictionary<string, int> dic_XwId = new Dictionary<string, int>();
        
        List<int> XwId_cache = new List<int>();
        private volatile int m_iMutilId = 1;

        //get id
        public int GetXwID(string xw_name)
        {
            if (XwId_cache.Count > 0)
            {
                int iMutilId = XwId_cache[0];
                dic_XwId.TryAdd(xw_name, iMutilId);
                XwId_cache.RemoveAt(0);
                return iMutilId;
            }
            else
            {
                dic_XwId.TryAdd(xw_name, m_iMutilId);
                return m_iMutilId++;
            }
        }

        public void RecycleXwID(string xw_name)
        {
            if (dic_XwId.TryGetValue(xw_name, out int v))
            {
                XwId_cache.Add(v);
                dic_XwId.Remove(xw_name);
                XwId_cache.Sort();
            }

        }


        //add to capical  

        public void printinfo()
        {
            Console.WriteLine("nice to meet you");
            m_dir.Add(1, 2);
        }
        public void add1()
        {
            string leader_name = "a";
            List<string> applicant = new List<string>();
            applicant.Add(leader_name);
            m_applicant[leader_name] = applicant;

        }
        public void add2()
        {
            string leader_name = "a";
            if (m_applicant.TryGetValue(leader_name, out List<string> value))
            {
                value.Add("b");
            }

        }
        public void spilt()
        {
            string str_applist = "112;333;";
            string[] do_str = str_applist.Split(';');
            string s1 = do_str[0];
            string s2 = do_str[1];
            string s3 = do_str[2];
            int c = do_str.Length;
            foreach(string applicant in do_str)
            {
                string tt = applicant;
            }

        }
        public void ReadFromCvs(string path)
        {              
        }

        public  List<string>  GetTeamMember(int c)
        {
            List<string> temp = new List<string>();
            List<string> contex = new List<string>(new string[] { " 2", "7" });

            if (1==c)
            {
                temp = contex;
            }
            return temp;
        }
        public void func_test(string s)
        {
            Console.WriteLine(s);
        }


    }
}