using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Common
{
    public class valueInfo
    {
        public Dictionary<string, string> m_infos = new Dictionary<string, string>();

        public void ClearAll()
        {
            m_infos.Clear();
        }


        public bool ContainKey(string key)
        {
            return m_infos.ContainsKey(key);
        }

        public void CopyFrom(valueInfo sourceInfo)
        {
            m_infos.Clear();
            if (sourceInfo != null)
            {
                foreach (string key in sourceInfo.m_infos.Keys)
                {
                    m_infos.Add(key, sourceInfo.m_infos[key]);
                }
            }
        }

        public string GetUpdateStr()
        {
            string reta = "";
            foreach (string key in m_infos.Keys)
            {
                string value = m_infos[key];
                if (reta != "")
                    reta += ",";
                reta += ("`" + key + "`=" + value);

            }
            return reta;
        }

        public string GetWhereStr()
        {
            string staticWhereInfo = null;
            if (m_infos.TryGetValue("where", out staticWhereInfo))
            {
                return staticWhereInfo;
            }
            else
            {
                string reta = " WHERE 1=1 ";
                foreach (string key in m_infos.Keys)
                {
                    string value = m_infos[key];
                    reta += " and `" + key + "`=" + value + " ";
                }
                return reta;
            }
        }

        public string GetInsertStr()
        {
            string reta = "";
            string firstStr = "";
            string secondStr = "";

            foreach (string key in m_infos.Keys)
            {
                string value = m_infos[key];
                if (firstStr == "")
                {
                    firstStr += "(";
                }
                else
                {
                    firstStr += ",";
                }
                firstStr = firstStr + "`" + key + "`";

                if (secondStr == "")
                {
                    secondStr += "VALUES(";
                }
                else
                {
                    secondStr += ",";
                }
                secondStr += value;

            }
            firstStr += ")";
            secondStr += ")";

            reta = firstStr + " " + secondStr;
            return reta;
        }

        public string GetDuplicateInsertStr()
        {
            string reta = "";
            string firstStr = "";
            string secondStr = "";
            string fourthStr = "";
            string thirdStr = "ON DUPLICATE KEY UPDATE";

            foreach (string key in m_infos.Keys)
            {
                string value = m_infos[key];
                if (firstStr == "")
                {
                    firstStr += "(";
                }
                else
                {
                    firstStr += ",";
                }
                firstStr = firstStr + "`" + key + "`";

                if (secondStr == "")
                {
                    secondStr += "VALUES(";
                }
                else
                {
                    secondStr += ",";
                }
                secondStr += value;

                fourthStr += key + "=" + value + ",";

            }
            firstStr += ")";
            secondStr += ")";

            fourthStr = fourthStr.Substring(0, fourthStr.LastIndexOf(","));
            reta = firstStr + " " + secondStr + " " + thirdStr + " " + fourthStr + ";";

            return reta;
        }

        public bool HaveValue(string key)
        {
            return m_infos.ContainsKey(key);
        }

        public string GetValue(string key)
        {
            string result;
            if (m_infos.TryGetValue(key, out result))
                return result;
            return "";
        }
        public int Count
        {
            get { return m_infos.Count; }
        }

        public Dictionary<string, string>.KeyCollection Keys
        {
            get { return m_infos.Keys; }
        }

        public void SetValueDbStr(string key, string value)
        {
            SetValue(key, "'" + value + "'");
        }

        public bool CheckLowestByInfo(valueInfo inputInfo)
        {
            foreach (string key in inputInfo.m_infos.Keys)
            {
                int value = inputInfo.GetValueInt(key);
                int myValue = GetValueInt(key);
                if (value > myValue)
                    return false;
            }
            return true;
        }

        public bool SetValue(string key, string value1, string value2)
        {
            string value = value1 + "_" + value2;
            string oldValue = GetValue(key);
            if (oldValue == value)
                return false;
            if (m_infos.ContainsKey(key))
                m_infos[key] = value;
            else
            {
                m_infos.Add(key, value);
            }
            return true;
        }
        public bool SetValue(string key, string value)
        {
            if (m_infos.ContainsKey(key))
            {
                string oldValue = GetValue(key);
                if (oldValue == value)
                    return false;
            }

            m_infos[key] = value;
            return true;
        }
        public bool SetValueLong(string key, long value)
        {
            long oldValue = GetValueLong(key);
            if (oldValue == value)
                return false;
            if (m_infos.ContainsKey(key))
                m_infos[key] = value.ToString();
            else
            {
                m_infos.Add(key, value.ToString());
            }
            return true;
        }
        public int GetValueIntDf(string key, int dfValue)
        {
            if (HaveValue(key) == false)
                return dfValue;
            return GetValueInt(key);
        }

        public float GetValueFloatDf(string key, float dfValue)
        {
            if (HaveValue(key) == false)
                return dfValue;
            return GetValueFloat(key);
        }

        public int GetValueInt(string key)
        {
            string value = GetValue(key);//.Split('.')[0];

            if (value != "")
            {
                //return GlobalDefine.IntParse(value);
            }
            return 0;
        }

        public long GetValueLong(string key)
        {
            string value = GetValue(key).Split('.')[0];

            if (value != "")
            {

                return long.Parse(value);
            }
            return 0;
        }

        public void AddValueInt(string key, int value)
        {
            int oldValue = GetValueInt(key);
            oldValue += value;
            SetValueInt(key, oldValue);
        }

        public float GetValueFloat(string key, float defaultValue = 0.0f)
        {
            float result = defaultValue;
            string value = GetValue(key);
            bool ok = !string.IsNullOrEmpty(value) && float.TryParse(value, out result);
            return result;
        }

        public Dictionary<string, string> GetValueDicSS(string key)
        {
            Dictionary<string, string> res_dic = new Dictionary<string, string>();
            string value = GetValue(key).Trim();
            if (value != "" && value != "NONE")
            {
                string[] temp_str = value.Split(':');
                for (int i = 0; i < temp_str.Length; ++i)
                {
                    string[] temp_sub_str = temp_str[i].Split('=');
                    if (temp_sub_str.Length > 1)
                    {
                        res_dic.Add(temp_sub_str[0], temp_sub_str[1]);
                    }
                }
            }

            return res_dic;
        }

        public Dictionary<string, int> GetValueDicSI(string key)
        {
            Dictionary<string, int> res_dic = new Dictionary<string, int>();
            string value = GetValue(key).Trim();
            if (value != "" && value != "NONE")
            {
                string[] temp_str = value.Split(';');
                for (int i = 0; i < temp_str.Length; ++i)
                {
                    string[] temp_sub_str = temp_str[i].Split('=');
                    if (temp_sub_str.Length > 1)
                    {
                        int temp;
                        if (int.TryParse(temp_sub_str[1], out temp))
                        {
                            res_dic.Add(temp_sub_str[0], temp);
                        }
                    }
                }
            }

            return res_dic;
        }

        public Dictionary<int, int> GetValueDicII(string key)
        {
            Dictionary<int, int> res_dic = new Dictionary<int, int>();
            string value = GetValue(key).Trim();
            if (value != "" && value != "NONE")
            {
                string[] temp_str = value.Split(':');
                for (int i = 0; i < temp_str.Length; ++i)
                {
                    string[] temp_sub_str = temp_str[i].Split('=');
                    if (temp_sub_str.Length > 1)
                    {
                        int temp1, temp2;
                        if (int.TryParse(temp_sub_str[0], out temp1) && int.TryParse(temp_sub_str[1], out temp2))
                        {
                            res_dic.Add(temp1, temp2);
                        }
                    }
                }
            }
            return res_dic;
        }

        public Dictionary<int, int> GetValueDicII2(string key)
        {
            Dictionary<int, int> res_dic = new Dictionary<int, int>();
            string value = GetValue(key).Trim();
            if (value != "" && value != "NONE")
            {
                string[] temp_str = value.Split(';');
                for (int i = 0; i < temp_str.Length; ++i)
                {
                    string[] temp_sub_str = temp_str[i].Split('=');
                    if (temp_sub_str.Length > 1)
                    {
                        int temp1, temp2;
                        if (int.TryParse(temp_sub_str[0], out temp1) && int.TryParse(temp_sub_str[1], out temp2))
                        {
                            res_dic.Add(temp1, temp2);
                        }
                    }
                }
            }
            return res_dic;
        }

        public List<int> GetValueListInt(string key)
        {
            List<int> res_list = new List<int>();
            string value = GetValue(key);
            if (value != "")
            {
                string[] temp_str = value.Split(':');
                for (int i = 0; i < temp_str.Length; ++i)
                {
                    int temp;
                    if (int.TryParse(temp_str[i].Trim(), out temp))
                    {
                        res_list.Add(temp);
                    }
                }
            }

            return res_list;
        }

        public List<int> GetValueListInt2(string key)
        {
            List<int> res_list = new List<int>();
            string value = GetValue(key);
            if (value != "" && value != "NONE")
            {
                string[] temp_str = value.Split(';');
                for (int i = 0; i < temp_str.Length; ++i)
                {
                    int temp;
                    if (int.TryParse(temp_str[i].Trim(), out temp))
                    {
                        res_list.Add(temp);
                    }
                }
            }

            return res_list;
        }

        public List<string> GetValueListStr(string key)
        {
            List<string> res_list = new List<string>();
            string value = GetValue(key);
            if (value != "")
            {
                string[] temp_str = value.Split(':');
                for (int i = 0; i < temp_str.Length; ++i)
                {
                    res_list.Add(temp_str[i]);
                }
            }

            return res_list;
        }


        public bool SetValueInt(string key, int value)
        {
            return SetValue(key, value.ToString());
        }

        public bool SetValueFloat(string key, float value)
        {
            return SetValue(key, value.ToString());
        }

        public void LoadFromStr(string mainStr)
        {
            m_infos.Clear();
            string[] vs = mainStr.Split(';');
            for (int i = 0; i < vs.Length; i++)
            {
                string[] subV = vs[i].Split('=');
                if (subV.Length == 2)
                {
                    SetValue(subV[0], subV[1]);
                }
            }
        }

        public void SaveToStr(ref string retaStr)
        {
            retaStr = "";
            foreach (KeyValuePair<string, string> valuePair in m_infos)
            {
                retaStr += valuePair.Key + "=" + valuePair.Value + ";";
            }
        }

        public string SaveToStr()
        {
            string retaStr = "";
            foreach (KeyValuePair<string, string> valuePair in m_infos)
            {
                retaStr += valuePair.Key + "=" + valuePair.Value + ";";
            }
            return retaStr;
        }

        public void WriteToCmd()    //写入
        {

        }

        public void ReadFromCmd()   //读出
        {

        }
    }



    public class valueInfoList
    {
        public Dictionary<string, valueInfo> m_infoList = new Dictionary<string, valueInfo>();

        public int Count
        {
            get { return m_infoList.Count; }
        }

        public  static string ReadFromCSV(string filename)
        {
            if (false == System.IO.File.Exists(filename))
            {
                return null;
            }
            Encoding encoding = Encoding.Default;
            FileStream fs = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, encoding);
            string content = sr.ReadToEnd();
      
            sr.Close();
            fs.Close();
            if (null == content)
                return null;
            string[] arrLine = content.Split('\n');
            if (null == arrLine)
                return null;

            valueInfoList reta = new valueInfoList();
            //第一行中文备注，第二行key
            string[] arrKey = arrLine[1].Remove(arrLine[1].Length - 1).Split(',');
            for (int i = 2; i < arrLine.Length - 1; i++)
            {
                string strLine = arrLine[i];
                //每行末尾有一个‘/r’
                strLine = strLine.Remove(arrLine[i].Length - 1);
                string[] arrWord = strLine.Split(',');

                valueInfo viNode = new valueInfo();
                for (int j = 0; j < arrKey.Length; j++)
                {
                    viNode.SetValue(arrKey[j], arrWord[j]);
                }
                reta.m_infoList.Add(i.ToString(), viNode);
            }
            return content;
        }

        static public valueInfoList ReadFromIni(string filename)
        {
            if (false == System.IO.File.Exists(filename))
            {
                return null;
            }
            Encoding encoding = Encoding.Default;
            FileStream fs = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, encoding);
            string content = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            if (null == content)
                return null;
            string[] arrLine = content.Split('\n');
            if (null == arrLine)
                return null;

            valueInfoList reta = new valueInfoList();
            string listKey = "";
            valueInfo viNode = new valueInfo();
            for (int i = 0; i < arrLine.Length - 1; i++)
            {
                string strLine = arrLine[i];
                //每行末尾有一个‘/r’
                strLine = strLine.Remove(arrLine[i].Length - 1);

                if (string.IsNullOrWhiteSpace(strLine))
                {
                    continue;
                }

                if (strLine.Trim()[0] == '[')
                {
                    if (listKey != "")
                    {
                        reta.m_infoList.Add(listKey, viNode);
                        listKey = "";
                        viNode = new valueInfo();
                    }
                    listKey = strLine.Substring(1, strLine.Length - 2);
                }
                else
                {
                    string[] arrWord = strLine.Split('=');
                    if (arrWord.Length > 1)
                    {
                        viNode.SetValue(arrWord[0], arrWord[1]);
                    }
                }
            }
            if (listKey != "")
            {
                reta.m_infoList.Add(listKey, viNode);
            }
            else if (viNode.Count > 0)
            {
                reta.m_infoList.Add("CONFIG", viNode);
            }

            return reta;
        }

        static public List<string> ReadFromTxt(string filename)
        {
            if (false == System.IO.File.Exists(filename))
            {
                return null;
            }
            Encoding encoding = Encoding.Default;
            FileStream fs = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, encoding);
            string content = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            if (null == content)
                return null;
            string[] arrLine = content.Split('\n');
            if (null == arrLine)
                return null;

            List<string> rate = new List<string>();

            for (int i = 0; i < arrLine.Length - 1; i++)
            {
                string strLine = arrLine[i];
                //每行末尾有一个‘/r’
                strLine = strLine.Remove(arrLine[i].Length - 1);

                if (string.IsNullOrWhiteSpace(strLine))
                {
                    continue;
                }
                rate.Add(strLine);
            }
            return rate;
        }
        public string GetInfoValue(string id, string key)
        {
            if (m_infoList.Count <= 0)
            {
                return "";
            }
            valueInfo vi = null;
            if (m_infoList.TryGetValue(id, out vi))
            {
                if (vi != null)
                {
                    string value = vi.GetValue(key);
                    return value;
                }
            }
            return "";
        }

        public int GetInfoValueInt(string id, string key)
        {
            if (m_infoList.Count <= 0)
            {
                return 0;
            }
            valueInfo vi = null;
            if (m_infoList.TryGetValue(id, out vi))
            {
                if (vi != null)
                {
                    int value = vi.GetValueInt(key);
                    return value;
                }
            }
            return 0;
        }

        public float GetInfoValueFloat(string id, string key)
        {
            if (m_infoList.Count <= 0)
            {
                return 0;
            }
            valueInfo vi = null;
            if (m_infoList.TryGetValue(id, out vi))
            {
                if (vi != null)
                {
                    float value = vi.GetValueFloat(key);
                    return value;
                }
            }
            return 0;
        }

        public bool SetInfoValue(string id, string key, string value)
        {
            string oldValue = GetInfoValue(id, key);
            if (oldValue == value)
                return false;
            if (m_infoList.ContainsKey(id))
            {
                valueInfo vi = null;
                if (m_infoList.TryGetValue(id, out vi))
                {
                    vi.SetValue(key, value);
                }
            }
            else
            {
                valueInfo vi = new valueInfo();
                vi.SetValue(key, value);
                m_infoList.Add(id, vi);
            }
            return true;
        }

        public bool SetInfoValueInt(string id, string key, int value)
        {
            int oldValue = GetInfoValueInt(id, key);
            if (oldValue == value)
                return false;
            if (m_infoList.ContainsKey(id))
            {
                valueInfo vi = null;
                if (m_infoList.TryGetValue(id, out vi))
                {
                    vi.SetValueInt(key, value);
                }
            }
            else
            {
                valueInfo vi = new valueInfo();
                vi.SetValueInt(key, value);
                m_infoList.Add(id, vi);
            }
            return true;
        }

        public bool SetInfoValueFloat(string id, string key, float value)
        {
            float oldValue = GetInfoValueInt(id, key);
            if (oldValue == value)
                return false;
            if (m_infoList.ContainsKey(id))
            {
                valueInfo vi = null;
                if (m_infoList.TryGetValue(id, out vi))
                {
                    vi.SetValueFloat(key, value);
                }
            }
            else
            {
                valueInfo vi = new valueInfo();
                vi.SetValueFloat(key, value);
                m_infoList.Add(id, vi);
            }
            return true;
        }
    }

}