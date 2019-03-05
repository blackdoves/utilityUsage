using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Common
{
    partial class Program
    {
        //static void Main(string[] args)
        //{
        //    string hash = GetHash("lockTest.pdb");

        //    Console.WriteLine("Hash: {0}", hash);

        //    Console.ReadKey();
        //}

        public static string GetHash(string pathSrc)
        {
            string filename = Path.GetFileName(pathSrc);

            string pathname = "copy_" + filename;

            string pathDest = Path.GetDirectoryName(pathSrc);
            string pathDest2 = Path.GetFullPath(pathSrc);

            File.Copy(pathSrc, pathDest, true);

            String md5Result;
            StringBuilder sb = new StringBuilder();
            MD5 md5Hasher = MD5.Create();

            using (FileStream fs = File.OpenRead(pathSrc))
            {
                foreach (Byte b in md5Hasher.ComputeHash(fs))
                    sb.Append(b.ToString("x2").ToLower());
            }

            md5Result = sb.ToString();

            // File.Delete(pathDest);

            return md5Result;
        }

       static public void task_reward(int active_reward)
        {
            int idx = 1;
            while (active_reward > 0)
            {
                if (active_reward % 10 > 0)
                {
                    Console.WriteLine("heihei..{0}", idx);
                }
                idx++;
                active_reward /= 10;
            }
        }

        static public bool HasTake(int num,int index)
        {
            int compare = 1 << (index-1);
            return (num & compare) == 1;
        }

        static public void updateStatus(ref int num, int index)
        {
            num = num | (1 << (index-1));
        }
    }
}