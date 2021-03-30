using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lb_dop
{
    class Level
    {


        public static void Lvl(int lvl, Control.ControlCollection controls)
        {
            Button a = new Button();
            a.BackColor = Color.Gainsboro;
            a.ForeColor = Color.Chocolate;
            a.Text = "123";
            a.Location = new System.Drawing.Point(117, 42);

            controls.Add(a);
        }


    }
}
