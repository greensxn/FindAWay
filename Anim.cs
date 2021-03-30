using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lb_dop
{
    class Anim
    {
        //static bool IsBreak = false;

        public static async void ChangeColor(Panel panel, int R, int G, int B, Speed speed)
        {
            int cR = panel.BackColor.R;
            int cG = panel.BackColor.G;
            int cB = panel.BackColor.B;

            String sp = speed.ToString().Remove(0, 1);
            int Speed = Convert.ToInt32(sp);
            Char[] symb = new char[3];
            symb[0] = (R > cR) ? '+' : (R == cR) ? '=' : '-';
            symb[1] = (G > cG) ? '+' : (G == cG) ? '=' : '-';
            symb[2] = (B > cB) ? '+' : (B == cB) ? '=' : '-';

            int counter = 0;
            while (true) {

                if (symb[0] == '+' && cR != R) cR++;
                else if (symb[0] == '-' && cR != R) cR--;

                if (symb[1] == '+' && cG != G) cG++;
                else if (symb[1] == '-' && cG != G) cG--;

                if (symb[2] == '+' && cB != B) cB++;
                else if (symb[2] == '-' && cB != B) cB--;

                panel.BackColor = Color.FromArgb(cR, cG, cB);

                if (cR == R && cG == G && cB == B /*|| IsBreak*/) {
                    //IsBreak = false;
                    return;
                }

                counter++;
                if (counter < Speed)
                    continue;

                counter = 0;
                await Task.Delay(1);
            }
        }

        public static async Task ChangeColorAsync(Panel panel, int R, int G, int B, Speed speed)
        {
            int cR = panel.BackColor.R;
            int cG = panel.BackColor.G;
            int cB = panel.BackColor.B;

            String sp = speed.ToString().Remove(0, 1);
            int Speed = Convert.ToInt32(sp);
            Char[] symb = new char[3];
            symb[0] = (R > cR) ? '+' : (R == cR) ? '=' : '-';
            symb[1] = (G > cG) ? '+' : (G == cG) ? '=' : '-';
            symb[2] = (B > cB) ? '+' : (B == cB) ? '=' : '-';

            int counter = 0;
            while (true) {

                if (symb[0] == '+' && cR != R) cR++;
                else if (symb[0] == '-' && cR != R) cR--;

                if (symb[1] == '+' && cG != G) cG++;
                else if (symb[1] == '-' && cG != G) cG--;

                if (symb[2] == '+' && cB != B) cB++;
                else if (symb[2] == '-' && cB != B) cB--;

                panel.BackColor = Color.FromArgb(cR, cG, cB);

                if (cR == R && cG == G && cB == B) {
                    return;
                }

                counter++;
                if (counter < Speed)
                    continue;

                counter = 0;
                await Task.Delay(1);
            }
        }

        public static async void ChangeColor(Panel[] panels, int R, int G, int B, Speed speed)
        {
            String sp = speed.ToString().Remove(0, 1);
            int Speed = Convert.ToInt32(sp);

            int counter = 0;
            int counterCorrect = 0;
            //while (true) {
            while (true) {
                foreach (Panel panel in panels) {
                    counterCorrect = 0;

                    int cR = panel.BackColor.R;
                    int cG = panel.BackColor.G;
                    int cB = panel.BackColor.B;
                    Char[] symb = new char[3];
                    symb[0] = (R > cR) ? '+' : (R == cR) ? '=' : '-';
                    symb[1] = (G > cG) ? '+' : (G == cG) ? '=' : '-';
                    symb[2] = (B > cB) ? '+' : (B == cB) ? '=' : '-';


                    if (symb[0] == '+' && cR != R) cR++;
                    else if (symb[0] == '-' && cR != R) cR--;

                    if (symb[1] == '+' && cG != G) cG++;
                    else if (symb[1] == '-' && cG != G) cG--;

                    if (symb[2] == '+' && cB != B) cB++;
                    else if (symb[2] == '-' && cB != B) cB--;

                    panel.BackColor = Color.FromArgb(cR, cG, cB);

                    foreach (Panel pl in panels) {
                        if (pl.BackColor.R == R && pl.BackColor.G == G && pl.BackColor.B == B) {
                            counterCorrect++;
                        }
                        if (counterCorrect == panels.Length) {
                            return;
                        }
                    }
                }
                counter++;
                if (counter < Speed)
                    continue;

                counter = 0;
                await Task.Delay(1);
            }
        }

        static bool IsStop = false;
        static bool IsTransfusion = false;
        public async static void Transfusion(Label label, int R, int G, int B, Speed speed)
        {
            IsTransfusion = true;
            int sR = label.ForeColor.R;
            int sG = label.ForeColor.G;
            int sB = label.ForeColor.B;

            String sp = speed.ToString().Remove(0, 1);
            int Speed = Convert.ToInt32(sp);

            while (!IsStop) {

                int cR = label.ForeColor.R;
                int cG = label.ForeColor.G;
                int cB = label.ForeColor.B;

                Char[] symb = new char[3];
                symb[0] = (R > cR) ? '+' : (R == cR) ? '=' : '-';
                symb[1] = (G > cG) ? '+' : (G == cG) ? '=' : '-';
                symb[2] = (B > cB) ? '+' : (B == cB) ? '=' : '-';

                int counter = 0;
                while (true) {

                    if (symb[0] == '+' && cR != R) cR++;
                    else if (symb[0] == '-' && cR != R) cR--;

                    if (symb[1] == '+' && cG != G) cG++;
                    else if (symb[1] == '-' && cG != G) cG--;

                    if (symb[2] == '+' && cB != B) cB++;
                    else if (symb[2] == '-' && cB != B) cB--;

                    label.ForeColor = Color.FromArgb(cR, cG, cB);

                    if (cR == R && cG == G && cB == B) {
                        break;
                    }

                    counter++;
                    if (counter < Speed)
                        continue;

                    counter = 0;
                    await Task.Delay(1);
                }

                cR = label.ForeColor.R;
                cG = label.ForeColor.G;
                cB = label.ForeColor.B;

                symb = new char[3];
                symb[0] = (sR > cR) ? '+' : (sR == cR) ? '=' : '-';
                symb[1] = (sG > cG) ? '+' : (sG == cG) ? '=' : '-';
                symb[2] = (sB > cB) ? '+' : (sB == cB) ? '=' : '-';

                counter = 0;
                while (true) {

                    if (symb[0] == '+' && cR != sR) cR++;
                    else if (symb[0] == '-' && cR != sR) cR--;

                    if (symb[1] == '+' && cG != sG) cG++;
                    else if (symb[1] == '-' && cG != sG) cG--;

                    if (symb[2] == '+' && cB != sB) cB++;
                    else if (symb[2] == '-' && cB != sB) cB--;

                    label.ForeColor = Color.FromArgb(cR, cG, cB);

                    if (cR == sR && cG == sG && cB == sB) {
                        break;
                    }

                    counter++;
                    if (counter < Speed)
                        continue;

                    counter = 0;
                    await Task.Delay(1);
                }
            }
            IsStop = false;
        }

        public static void StopTransfusion()
        {
            if (IsTransfusion) {
                IsStop = true;
                IsTransfusion = false;
            }
        }

        public static async void ChangeColor(Label label, int R, int G, int B, Speed speed)
        {
            int cR = label.ForeColor.R;
            int cG = label.ForeColor.G;
            int cB = label.ForeColor.B;

            String sp = speed.ToString().Remove(0, 1);
            int Speed = Convert.ToInt32(sp);
            Char[] symb = new char[3];
            symb[0] = (R > cR) ? '+' : (R == cR) ? '=' : '-';
            symb[1] = (G > cG) ? '+' : (G == cG) ? '=' : '-';
            symb[2] = (B > cB) ? '+' : (B == cB) ? '=' : '-';

            int counter = 0;
            while (true) {

                if (symb[0] == '+' && cR != R) cR++;
                else if (symb[0] == '-' && cR != R) cR--;

                if (symb[1] == '+' && cG != G) cG++;
                else if (symb[1] == '-' && cG != G) cG--;

                if (symb[2] == '+' && cB != B) cB++;
                else if (symb[2] == '-' && cB != B) cB--;

                label.ForeColor = Color.FromArgb(cR, cG, cB);

                if (cR == R && cG == G && cB == B /*|| IsBreak*/) {
                    //IsBreak = false;
                    break;
                }

                counter++;
                if (counter < Speed)
                    continue;

                counter = 0;
                await Task.Delay(1);
            }
        }

        public async static void ChangeText(Label label, string text, Speed speed)
        {
            String sp = speed.ToString().Remove(0, 1);
            int Speed = 1000 - Convert.ToInt32(sp) * 66 - 9;

            StringBuilder myName = new StringBuilder(label.Text);
            if (label.Text.Length < text.Length) {
                for (int i = 0; i < label.Text.Length; i++) {
                    myName[i] = text[i];
                    label.Text = myName.ToString();
                    await Task.Delay(Speed);
                }
                for (int i = label.Text.Length; i < text.Length; i++) {
                    label.Text += text[i];
                    await Task.Delay(Speed);
                }
            }
            else {
                for (int i = 0; i < text.Length; i++) {
                    myName[i] = text[i];
                    label.Text = myName.ToString();
                    await Task.Delay(Speed);
                }
                //for (int i = text.Length; i < label.Text.Length+1; i++) {

                //}

                while (label.Text.Length != text.Length) {
                    label.Text = label.Text.Remove(label.Text.Length - 1);
                    await Task.Delay(Speed);
                }
            }


        }

        public async static Task ChangeTextAsync(Label label, string text, Speed speed)
        {
            String sp = speed.ToString().Remove(0, 1);
            int Speed = 1000 - Convert.ToInt32(sp) * 66 - 5;

            StringBuilder myName = new StringBuilder(label.Text);
            if (label.Text.Length < text.Length) {
                for (int i = 0; i < label.Text.Length; i++) {
                    myName[i] = text[i];
                    label.Text = myName.ToString();
                    await Task.Delay(Speed);
                }
                for (int i = label.Text.Length; i < text.Length; i++) {
                    label.Text += text[i];
                    await Task.Delay(Speed);
                }
            }
            else {
                for (int i = 0; i < text.Length; i++) {
                    myName[i] = text[i];
                    label.Text = myName.ToString();
                    await Task.Delay(Speed);
                }
                //for (int i = text.Length; i < label.Text.Length+1; i++) {

                //}

                while (label.Text.Length != text.Length) {
                    label.Text = label.Text.Remove(label.Text.Length - 1);
                    await Task.Delay(Speed);
                }
            }


        }

        public enum Speed
        {
            _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15
        }

    }
}
