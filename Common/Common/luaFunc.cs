using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using XLua;

namespace Common.XLua
{
    class luaFunc
    {
        LuaEnv m_luaenv = null;
        public luaFunc()
        {
            m_luaenv = new LuaEnv();
            m_luaenv.Global.Set("luaFunc", this);
            m_ScriptBuffer = new Dictionary<string, string>();
            m_RWLock4SB = new ReaderWriterLock();

        }
        ~luaFunc()
        {
            m_luaenv.Dispose();
        }

        private ReaderWriterLock m_RWLock4SB;
        private Dictionary<string, string> m_ScriptBuffer;

        public void ExecFile(string filename)
        {
            string str = File.ReadAllText(filename);
            if (str.Length == 0)
                return;
            try
            {

                m_luaenv.DoString(str, filename);
            }
            catch (Exception E)
            {
                Console.WriteLine("eee");

            }

        }
        public int macro_GetId(int id)
        {
            id = id + 1;
            return id;
        }

    }
}
