using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Common.StatsManager
{
    class performance
    {
        private int m_iStateHandle = 0;
        public bool b_isOpen = false;  //开关
        public static readonly int cd = 10000; //ms

        //cpu 计算参数
        private long pervTickCount = Environment.TickCount;
        System.TimeSpan prevCpuTime = TimeSpan.Zero;
        private int cpuInteval = 1000;
        private double cpuUsage = 0.0f;


        public static readonly string path = Path.Combine(Environment.CurrentDirectory, "performance.txt");    //path => Path.Combine(.. + DateTime.Now.ToString("yyyyddMM_HHmm")); -- per min  


        public ConcurrentDictionary<string, statsContainer> dic_section = new ConcurrentDictionary<string, statsContainer>(); //<唯一ID，代码段状态>
        public ConcurrentDictionary<string, List<double>> dic_func = new ConcurrentDictionary<string, List<double>>();   //<代码段, 多个时间列表>
        public ConcurrentDictionary<string, DurationInfo> dic_dur = new ConcurrentDictionary<string, DurationInfo>();  //代码段, 统计后的信息

        static public performance m_instance = new performance();
        public performance()
        {
        }
        public void init(bool open = false)
        {
            if (open)
            {
                PrintLog();
            }
            b_isOpen = open;
        }

        /// </summary>
        /// 由需要测试代码段开始  调用加入事件 返回一个唯一id
        /// 并在代码段结束时 调用TriggerEvent(唯一id)   
        /// e.g  string id = AddEvent("Test");  ...{do something}.... TriggerEvent(id);
        /// </summary>

        #region 公共方法
        //add  code section 
        public string AddEvent(string func_name)
        {
            if (!b_isOpen)
            {
                return "";
            }
            string unique_id = GetUniqueID(func_name);
            statsContainer contain = new statsContainer(func_name);
            if (!dic_section.TryGetValue(unique_id, out statsContainer v))
            {
                dic_section.TryAdd(unique_id, contain);
            }
            return unique_id;
        }

        //trigger code section
        public void TriggerEvent(string unique_id)
        {
            if (!b_isOpen)
            {
                return;
            }
            if (dic_section.TryGetValue(unique_id, out statsContainer v))
            {
                if (!v.bTrigger)
                {
                    v.endTime = DateTime.Now;
                    double duration = v.getResult();
                    AddDurationToFunc(v.sFuncName, duration);
                    v.bTrigger = true;
                }
            }
        }
        //remove 
        public void RemoveAllEvent()
        {
            dic_section.Clear();
        }

        /// </summary>
        ///  提供另一种测试方法 
        ///  需要测试的类或方法 按以下方式调用 返回经历时间
        ///  e.g TimeSpan time = Execute(() =>{ do something }, "func_test");
        ///  
        /// </summary>
        //count by  m_ins. Execute  

        public void Execute(Action action, string code_name)
        {
            if (!b_isOpen)
            {
                return;
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            double result = 0;
            string duration = ((stopwatch.Elapsed).TotalMilliseconds / 1000.0).ToString("n5");
            Double.TryParse(duration, out result);
            AddDurationToFunc(code_name, result);
        }
        #endregion


        private void CountDuration()
        {
            foreach (var var in dic_func)
            {
                string fucn_name = var.Key.ToString();
                List<double> temp = var.Value;
                if (temp.Count > 0)
                {
                    if (dic_dur.ContainsKey(fucn_name))
                    {
                        dic_dur.TryRemove(fucn_name, out DurationInfo v);
                    }
                    DurationInfo d = new DurationInfo();
                    d.iMaxTime = temp.Max();
                    d.iMinTime = temp.Min();
                    d.iAverTime = temp.Average();
                    d.sFuncName = fucn_name;
                    d.triggleCount = temp.Count();
                    dic_dur.TryAdd(fucn_name, d);
                }
            }
        }
        //print info
        private void PrintLog()
        {
            Thread printThread = new Thread(() =>
            {
                while (true)
                {
                    lock (this)
                    {
                        CountDuration();
                        List<string> list_reval = new List<string>();
                        foreach (var v in dic_dur)
                        {
                            string reval = string.Format("{0}代码段 ：最大执行时间{1}s， 最小执行时间{2}，平均执行时间{3}, 执行次数{4}\n",
                             v.Key.ToString(),
                             v.Value.iMaxTime,
                             v.Value.iMinTime,
                             v.Value.iAverTime,
                             v.Value.triggleCount
                             );
                            list_reval.Add(reval);
                        }
                        if (list_reval.Count > 0)
                        {
                            list_reval.Add(formatSysInfo());
                            SaveToDoc(list_reval);
                        }
                        list_reval.Clear();
                        Thread.Sleep(cd);
                    }
                }
            });
            printThread.Start();
        }

        public static int DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt32((dateTime - start).TotalSeconds);
        }
        private string GetUniqueID(string func_name)
        {
            string reta = DateTimeToUnixTimestamp(DateTime.UtcNow).ToString() + "_" + func_name + "_" + m_iStateHandle.ToString();
            m_iStateHandle++;
            return reta;
        }

        //add to dic
        private void AddDurationToFunc(string func_name, double duration)
        {
            if (func_name == "" || duration <= 0)
            {
                return;
            }
            if (dic_func.TryGetValue(func_name, out List<double> v))
            {
                v.Add(duration);
            }
            else
            {
                List<double> s = new List<double>();
                s.Add(duration);
                dic_func.TryAdd(func_name, s);
            }

        }


        #region  Utils
        private long getGCTotalMem()
        {
            GC.Collect();
            long gc_mem = GC.GetTotalMemory(false);
            Console.WriteLine(string.Format("Now, total mem alloc'd {0} byte", gc_mem));
            return gc_mem;
        }

        public double getCpu()
        {
            var curTime = Process.GetCurrentProcess().TotalProcessorTime;
            long now = Environment.TickCount;
            long interval = now - pervTickCount;
            if (interval < cpuInteval)
                return cpuUsage;
            // CPU使用比例 =  间隔时间内的CPU运行时间除以逻辑CPU数量
            cpuUsage = (curTime - prevCpuTime).TotalMilliseconds / cpuInteval / Environment.ProcessorCount;
            prevCpuTime = curTime;
            pervTickCount = now;
            return cpuUsage;
        }
        public long getMem()
        {
            Process currentProcess = Process.GetCurrentProcess();
            long mem = (int)currentProcess.WorkingSet64 / 1024 / 1024;
            return mem;
        }
        private string formatSysInfo()
        {
            return string.Format("当前cpu:{0:0.00},当前mem内存{1}，记录时间{2}", getCpu(), getMem(), DateTime.Now.ToString("yyyyddMM_HH:mm:ss"));
        }

        public static void SaveToDoc(List<string> ls)
        {
            if (File.Exists(path))
            {
                using (FileStream fs = File.OpenWrite(path))         // or File.Delete(path);
                {
                    StreamWriter sw = new StreamWriter(fs);
                    foreach (string s in ls)
                    {
                        sw.WriteLine(s);
                    }
                    sw.Close();
                }
            }
            else
            {
                using (FileStream fs = File.Create(path))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    foreach (string s in ls)
                    {
                        sw.WriteLine(s);
                    }
                    sw.Close();
                }
            }
        }
        #endregion
      
    }
}
