using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using System.Threading;

using System.Linq;

namespace Common
{

    class Performance
    {
        private DateTime BeginTime;
        private DateTime EndTime;
        //private ParamsModel Params;


        System.TimeSpan prevCpuTime = TimeSpan.Zero;

        static public Performance m_ins = new Performance();
        public Performance()
        {
        }


        public string GetUniqueID()
        {
            // return $"{TestMethod.TestClass.TestCollection.TestAssembly.Assembly.Name}{TestMethod.TestClass.Class.Name}{TestMethod.Method.Name}{Variation}";
            return $"";

        }

        public int Execute(string arguments, string workingDirectory)
        {
            return 0;
        }
        
       public void getTotalMemery()
        {
            BeginTime = DateTime.Now;
            GC.Collect();
            Console.WriteLine(string.Format("Before we start, total mem alloc'd {0} byte", GC.GetTotalMemory(false)));

        }

        public void getCpuInfo()
        {
            //     Stopwatch watch = Stopwatch.StartNew();
           // var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        }

        public void Execute(Action<int> action, Action<string> rollBack)
        {
            //List<Thread> arr = new List<Thread>();
            //BeginTime = DateTime.Now;
            //for (int i = 0; i < Params.RunCount; i++)
            //{
            //    if (Params.IsMultithread)
            //    {
            //        var thread = new Thread(new System.Threading.ThreadStart(() =>
            //        {
            //            action(i);
            //        }));
            //        thread.Start();
            //        arr.Add(thread);
            //    }
            //    else
            //    {
                   // action(i);
               // }
            //}
            //if (Params.IsMultithread)
            //{
            //    foreach (Thread t in arr)
            //    {
            //        while (t.IsAlive)
            //        {
            //            Thread.Sleep(10);
            //        }
            //    }

            //}
            rollBack(getResult());
        }















    //    Performance.Dog dog = new Performance.Dog();
    //    Performance.Host host = new Performance.Host(dog);

    //    //当前时间，从2008年12月31日23:59:50开始计时
    //    DateTime now = new DateTime(2015, 12, 31, 23, 59, 50);
    //    DateTime midnight = new DateTime(2016, 1, 1, 0, 0, 0);

    //    //等待午夜的到来
    //    Console.WriteLine("时间一秒一秒地流逝... ");
    //        while (now<midnight)
    //        {
    //            Console.WriteLine("当前时间: " + now);
    //            System.Threading.Thread.Sleep(1000);    //程序暂停一秒
    //            now = now.AddSeconds(1);                //时间增加一秒
    //        }

    ////午夜零点小偷到达,看门狗引发Alarm事件
    //Console.WriteLine("\n月黑风高的午夜: " + now);
    //        Console.WriteLine("小偷悄悄地摸进了主人的屋内... ");
    //        dog.OnAlarm();
    //        Console.ReadLine();

        public string getResult()
        {
            EndTime = DateTime.Now;
            string totalTime = ((EndTime - BeginTime).TotalMilliseconds / 1000.0).ToString("n4");
            string reval = string.Format("总共执行时间：{0}秒", totalTime);
            Console.Write(reval);
            return reval;
        }

        public double getCpu(int interval)
        {

            //当前时间
            var curTime = Process.GetCurrentProcess().TotalProcessorTime;
            //间隔时间内的CPU运行时间除以逻辑CPU数量
            var value = (curTime - prevCpuTime).TotalMilliseconds / interval / Environment.ProcessorCount;
            prevCpuTime = curTime;
            return value;
        }



        //事件发送者
        public class Dog
        {
            //1.声明关于事件的委托；
            public delegate void AlarmEventHandler(object sender, EventArgs e);

            //2.声明事件；   
            public event AlarmEventHandler Alarm;

            //3.编写引发事件的函数；
            public void OnAlarm()
            {
                if (this.Alarm != null)
                {
                    Console.WriteLine("\n狗报警: 有小偷进来了,汪汪~~~~~~~");
                    this.Alarm(this, new EventArgs());   //发出警报
                }
            }
        }

        //事件接收者
        public class Host
        {
            //４.编写事件处理程序
            void HostHandleAlarm(object sender, EventArgs e)
            {
                Console.WriteLine("主人: 抓住了小偷！");
            }

            //５.注册事件处理程序
            public Host(Dog dog)
            {
                dog.Alarm += new Dog.AlarmEventHandler(HostHandleAlarm);
            }
        }

    }
}
