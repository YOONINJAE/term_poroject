using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer2
{
    class PrimeCSharp
    {
        private static bool isPrimeNumber(int num)
        {
            bool isprime = true;
            for (int i = 2; i < (int)Math.Sqrt(num) + 1; i++)
            {
                if (num % i == 0)
                {
                    isprime = false;
                    break;
                }
            }
            return isprime;
        }
        public static void FindNumberOfPrimeNumber(int NumStart, int nMax, out int nprime)
        {
            nprime = 0;
            for (int i = NumStart; i <= nMax; i++)
            {
                bool isprime = isPrimeNumber(i);
                if (isprime) nprime++;
            }
        }
    }
}
