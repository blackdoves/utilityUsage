using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Common
{
    class special_program
    {
        static public special_program m_ins_speci = new special_program();
        public special_program()
        {
        }
        public void CreatThraed()
        {
            Thread loop = new Thread(() => print());
            loop.Start();
        }
        void print()
        {
            Console.WriteLine("111");
        }

    }

    class game_send
    {
        public void run()
        {
            while(true)
            {
               //account.me  loop send 

            }
        }
    }

    class mainThread
    {
        static public mainThread m_me = new mainThread();

        public mainThread()
        {
        }

        private bool BeginStart()
        {

            //init GameSocket


            //加载配置文件到内存

            //

            //

            return true;
        }

    }

}
