using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer1
{
    public static class Util
    {
        //===========================================================
        //  시간측정
        //===========================================================     
        public static double TimeInSeconds(DateTime stime)
        {
            TimeSpan dtime = DateTime.Now - stime;
            double dsec = (double)(dtime.Ticks / 10000000.0);
            return dsec;
        }

        //===========================================================
        //  16진수로
        //===========================================================
        public static string Hex(uint ival)
        {
            return String.Format("{0:X}", ival);
        }
        public static string Hex(int ival)
        {
            return String.Format("{0:X}", ival);
        }
    }
}
