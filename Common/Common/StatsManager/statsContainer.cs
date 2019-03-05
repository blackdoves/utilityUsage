using System;
using System.Collections.Generic;
using System.Text;

namespace Common.StatsManager
{
    public class statsContainer
    {
        #region variable

        public DateTime beginTime;
        public DateTime endTime;

        public bool bTrigger = false;
        public string sFuncName = "";
        #endregion

        public statsContainer(string func_name)
        {
            endTime = beginTime = DateTime.Now;
            sFuncName = func_name;
        }
        public double getResult()
        {
            double result = 0;
            string duration = ((endTime - beginTime).TotalMilliseconds / 1000.0).ToString("n5");
            Double.TryParse(duration, out result);
            return result;

        }
    }
   
    //时间间隔数据
    public class DurationInfo
    {
        public double iAverTime;
        public double iMaxTime;
        public double iMinTime;
        public string sFuncName = "";
        public int triggleCount;

        public DurationInfo()
        {
            iAverTime = 0;
            iMaxTime = 0;
            iMinTime = 0;
            triggleCount = 0;
        }
    }
}
