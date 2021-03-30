using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lb_dop
{
    class Clock
    {
        static bool IsBreak = false;
        static bool IsProcess = false;
        static int[] time;
        static Label lb;

        public async static void Start(Label mask)
        {
            if (IsProcess)
                return;

            lb = mask;

            time = new int[2] { 0, 0 };
            IsProcess = true;
            String IsZeroSec = "";
            String IsZeroMin = "";

            while (true)
            {
                if (IsBreak)
                {
                    IsProcess = false;
                    IsBreak = false;
                    break;
                }

                if (time[1] >= 60)
                {
                    time[1] = 0;
                    time[0]++;
                }

                if (time[0] < 10) IsZeroMin = "0";
                else IsZeroMin = String.Empty;
                if (time[1] < 10) IsZeroSec = "0";
                else IsZeroSec = String.Empty;

                String Out = $"{IsZeroMin}{time[0]} ։ {IsZeroSec}{time[1]}";
                mask.Text = Out;
                time[1]++;

                await Task.Delay(1000);
            }
        }

        public static void Stop()
        {
            if (IsProcess)
                IsBreak = true;
        }
    }
}
