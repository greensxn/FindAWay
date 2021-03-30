using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lb_dop
{
    public static class Ext
    {
        static Random r;

        public static void Mix(this int[] data)
        {
            r = new Random();

            for (int i = data.Length - 1; i >= 1; i--) {
                int j = r.Next(i + 1);
                var temp = data[j];
                data[j] = data[i];
                data[i] = temp;
            }
        }
    }
}
