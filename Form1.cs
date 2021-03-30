using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lb_dop
{
    public partial class Form1 : Form
    {
        Panel[] ActivityPanels;

        List<String> matches;
        List<Panel> HealthList;
        List<Panel> ListAllPanels;

        Dictionary<int, Anim.Speed> dict;

        Color StartColor = Color.FromArgb(60, 60, 60);
        Color FlickColor = Color.FromArgb(255, 214, 0);
        Color MoveColor = Color.FromArgb(90, 90, 90);

        int ClickCounter;
        int unsuccessfulClick;
        int AmountOfPoint;
        int HealthInterval = 150;

        int[] RandomSequence;
        int[] RandomColor;
        int[,] colors;

        bool IsStarted = false;
        bool IsAnimEnd = true;
        bool IsEndGame = false;
        bool IsColored;

        Anim.Speed HealthSpeed = Anim.Speed._4;

        Random r;

        public Form1()
        {
            InitializeComponent();
        }

        private void HideControls()
        {
            lbTEXT.Text = String.Empty;
            lbNEWGAME.Text = String.Empty;
            lbLVL.Text = String.Empty;
            lbCLOCK.Text = String.Empty;

            foreach (var Health in HealthList)
                Health.BackColor = Color.FromArgb(22, 22, 25);
            foreach (Panel p in ActivityPanels)
                p.BackColor = Color.FromArgb(22, 22, 25);

            pLine.BackColor = Color.FromArgb(22, 22, 25);
        }

        private Anim.Speed Complexity(int lvl)
        {
            if (lvl <= dict.Count) return dict[lvl];
            else return Anim.Speed._15;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            dict = new Dictionary<int, Anim.Speed>(){
                {1, Anim.Speed._5 },
                {2, Anim.Speed._8 },
                {3, Anim.Speed._10 },
                {4, Anim.Speed._15 }
            };

            HealthList = Dock.Controls.OfType<Panel>().Where<Panel>(a => a.Name.Contains("hp")).ToList();

            ResetData();
            HideControls();

            IsColored = false;
            IsAnimEnd = false;
            await Task.Delay(400);
            await Anim.ChangeTextAsync(lbTEXT, "NEW GAME", Anim.Speed._14);
            Anim.Transfusion(lbTEXT, FlickColor.R, FlickColor.G, FlickColor.B, Anim.Speed._12);
            IsAnimEnd = true;

            colors = new int[,] { { 87, 130, 207 },   //blue
                                  { 255, 128, 128 },  //red
                                  { 192, 255, 192 },  //green
                                  { 255, 214, 0  }    //yellow
            };

            ListAllPanels = Controls.OfType<Panel>().Where<Panel>(d => d.Name.Contains("panel")).ToList();
            foreach (Panel panel in ListAllPanels)
            {
                panel.MouseEnter += (s, a) =>
                {
                    Panel pnl = s as Panel;
                    if (pnl.BackColor == StartColor)
                        pnl.BackColor = MoveColor;
                };

                panel.MouseLeave += (s, a) =>
                {
                    Panel pnl = s as Panel;
                    if (pnl.BackColor == MoveColor)
                        pnl.BackColor = StartColor;
                };

                panel.MouseClick += async (s, a) =>
                { //CLICK PANEL
                    Panel pnl = s as Panel;
                    if (!IsEndGame && IsStarted)
                    {

                        if (!(pnl.BackColor == StartColor || pnl.BackColor == MoveColor))
                            return;

                        if (pnl.Name == matches[ClickCounter] && !IsEndGame)
                        {
                            ClickCounter++;
                            if (ClickCounter == AmountOfPoint)
                            {                                       //WIN
                                IsStarted = false;
                                IsAnimEnd = false;
                                IsEndGame = true;
                                IsBreak = true;
                                Clock.Stop();
                                Anim.ChangeColor(ActivityPanels, 192, 255, 192, Anim.Speed._12); //green

                                await Anim.ChangeTextAsync(lbTEXT, "CONTINUE", Anim.Speed._15);
                                Anim.StopTransfusion();
                                Anim.Transfusion(lbTEXT, FlickColor.R, FlickColor.G, FlickColor.B, Anim.Speed._12);

                                IsAnimEnd = true;
                                return;
                            }
                            pnl.BackColor = Color.FromArgb(192, 255, 192); //green
                        }
                        else if (!IsEndGame)
                        {
                            unsuccessfulClick++;

                            switch (unsuccessfulClick)
                            {
                                case 1:
                                    hp3.Visible = false;
                                    break;
                                case 2:
                                    hp2.Visible = false;
                                    break;
                                case 3:
                                    hp1.Visible = false;    //LOSE

                                    IsStarted = false;
                                    IsEndGame = true;
                                    IsAnimEnd = false;
                                    tsmLevel.Enabled = false;

                                    Clock.Stop();
                                    Anim.ChangeColor(ActivityPanels, 22, 22, 25, Anim.Speed._5);
                                    Anim.ChangeText(lbTEXT, "GAME OVER", Anim.Speed._14);
                                    Anim.ChangeColor(lbTEXT, 169, 0, 0, Anim.Speed._5);
                                    Anim.ChangeColor(pLine, 169, 0, 0, Anim.Speed._5);

                                    IsFirstHealth = false;
                                    IsAnimEnd = true;
                                    return;
                            }

                            if (pnl.BackColor == MoveColor || pnl.BackColor == StartColor) //ANIMATION RED PANEL
                                pnl.BackColor = Color.FromArgb(255, 128, 128); //red
                            if (pnl.BackColor == Color.FromArgb(255, 128, 128))
                            {
                                IsAnimEnd = false;
                                Anim.ChangeColor(pnl, StartColor.R, StartColor.G, StartColor.B, Anim.Speed._2);
                                IsAnimEnd = true;
                            }
                        }
                    }
                };
            }
        }

        private void ResetData()
        {
            ActivityPanels = Controls.OfType<Panel>().Where<Panel>(e => e.Name.Contains("panel") && e.BackColor != Color.FromArgb(22, 22, 25)).ToArray();
            ClickCounter = 0;
            unsuccessfulClick = 0;
            counterLVL = 1;
            AmountOfPoint = 4;

            RandomSequence = new int[ActivityPanels.Length];
            for (int i = 0; i < ActivityPanels.Length; i++)
                RandomSequence[i] = i;

            RandomColor = new int[] { 0, 1, 2, 3 };
        }

        private async void NewGame()
        {
            IsAnimEnd = false;
            AreaClear();
            IsBreak = true;
            IsFirstHealth = false;

            foreach (var Health in HealthList)
                Health.Visible = false;

            Anim.ChangeText(lbTEXT, String.Empty, Anim.Speed._15);
            Anim.ChangeColor(pLine, FlickColor.R, FlickColor.G, FlickColor.B, Anim.Speed._8);
            ResetData();
            lbLVL.Text = $"LVL {counterLVL}";

            await Task.Delay(1000);
            StartGame();
        }

        private void ResetSettingsToContinue()
        {
            ClickCounter = 0;
            IsFirstHealth = false;
            ActivityPanels = Controls.OfType<Panel>().Where<Panel>(e => e.Name.Contains("panel") && e.BackColor != Color.FromArgb(22, 22, 25)).ToArray();
        }

        private void AreaClear()
        {
            Anim.ChangeColor(ActivityPanels, StartColor.R, StartColor.G, StartColor.B, Anim.Speed._5);
        }

        private async void StartGame()
        {
            r = new Random();
            matches = new List<string>();
            RandomSequence.Mix();
            flag = true;
            IsEndGame = false;
            IsAnimEnd = false;
            RandomColor.Mix();
            Clock.Stop();
            lbCLOCK.Text = String.Empty;

            for (int i = 0; i < 4; i++)
            {
                matches.Add(ActivityPanels[RandomSequence[i]].Name);
                if (IsColored)
                    await Anim.ChangeColorAsync(ActivityPanels[RandomSequence[i]], colors[RandomColor[i], 0], colors[RandomColor[i], 1], colors[RandomColor[i], 2], Complexity(counterLVL));
                else
                    await Anim.ChangeColorAsync(ActivityPanels[RandomSequence[i]], FlickColor.R, FlickColor.G, FlickColor.B, Complexity(counterLVL));
                await Anim.ChangeColorAsync(ActivityPanels[RandomSequence[i]], StartColor.R, StartColor.G, StartColor.B, Complexity(counterLVL));
            }

            foreach (var Health in HealthList)
                Health.Visible = true;

            IsStarted = true;
            IsBreak = false;
            Clock.Start(lbCLOCK);

            Anim.ChangeColor(lbTEXT, 255, 128, 128, Anim.Speed._5);
            Anim.ChangeText(lbTEXT, "HEALTH", Anim.Speed._15);

            if (!IsFirstHealth)
            {
                HealthAnimation();
                IsFirstHealth = true;
            }
            IsAnimEnd = true;
        }

        bool IsFirstHealth = false;
        bool flag = false;
        bool IsBreak = false;
        private async void HealthAnimation()
        {
            Anim.Speed Speed = HealthSpeed;
            int H_Interval = HealthInterval;

        Here:
            if (flag)
                flag = false;

            await Task.Run(() =>
            {
                while (!_1H || !_2H || !_3H)
                {
                }
            });

            while (true)
            {
                int unsuccessful = HealthList.Count - unsuccessfulClick + 1;

                if (flag)
                    goto Here;

                if (unsuccessful > 1)
                {
                    Health1(Speed);
                    await Task.Delay(H_Interval);

                    if (flag)
                        goto Here;
                }

                if (unsuccessful > 2)
                {
                    Health2(Speed);
                    await Task.Delay(H_Interval);

                    if (flag)
                        goto Here;
                }

                if (unsuccessful > 3)
                {
                    Health3(Speed);
                    await Task.Delay(H_Interval);

                    if (flag)
                        goto Here;
                }

                if (unsuccessful < 2 || IsBreak)
                    break;

                Speed = HealthSpeed;
                H_Interval = HealthInterval;
            }
        }

        bool _1H = true;
        bool _2H = true;
        bool _3H = true;
        private async void Health1(Anim.Speed Speed)
        {
            if (hp1.BackColor == Color.FromArgb(22, 22, 25)) //black
                hp1.BackColor = Color.FromArgb(255, 128, 128); //red

            if (hp1.BackColor == Color.FromArgb(255, 128, 128))
            {
                _1H = false;
                await Anim.ChangeColorAsync(hp1, 22, 22, 25, Speed);
                _1H = true;
            }
        }

        private async void Health2(Anim.Speed Speed)
        {
            if (hp2.BackColor == Color.FromArgb(22, 22, 25)) //black
                hp2.BackColor = Color.FromArgb(255, 128, 128); //

            if (hp2.BackColor == Color.FromArgb(255, 128, 128))
            {
                _2H = false;
                await Anim.ChangeColorAsync(hp2, 22, 22, 25, Speed);
                _2H = true;
            }
        }

        private async void Health3(Anim.Speed Speed)
        {
            if (hp3.BackColor == Color.FromArgb(22, 22, 25)) //black
                hp3.BackColor = Color.FromArgb(255, 128, 128); //red

            if (hp3.BackColor == Color.FromArgb(255, 128, 128))
            {
                _3H = false;
                await Anim.ChangeColorAsync(hp3, 22, 22, 25, Speed);
                _3H = true;
            }
        }

        private void tsmRed_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsm = sender as ToolStripMenuItem;
            IsColored = (tsm.Text.Length > 1) ? true : false;
            FlickColor = tsm.BackColor;

            Anim.ChangeColor(pLine, FlickColor.R, FlickColor.G, FlickColor.B, Anim.Speed._4);
        }

        private async void уровеньToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (IsAnimEnd)
            {
                Clock.Stop();
                lbCLOCK.Text = String.Empty;

                int lvl = Convert.ToInt32((sender as ToolStripMenuItem).Text.Remove(1));
                await SetTileByLvl(lvl);

                NewGame();

                counterLVL = lvl;
                Anim.ChangeText(lbLVL, $"LVL {counterLVL}", Anim.Speed._15);
            }
        }

        private void оченьМедленноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch ((sender as ToolStripMenuItem).Text)
            {
                case "Очень медленно":
                    HealthSpeed = Anim.Speed._1;
                    break;
                case "Медленно":
                    HealthSpeed = Anim.Speed._2;
                    break;
                case "Средне":
                    HealthSpeed = Anim.Speed._3;
                    break;
                case "Быстро":
                    HealthSpeed = Anim.Speed._4;
                    break;
                case "Очень быстро":
                    HealthSpeed = Anim.Speed._5;
                    break;
            }
        }

        private void toolStripComboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                HealthInterval = Convert.ToInt32(toolStripComboBox1.Text);
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            HealthInterval = Convert.ToInt32(toolStripComboBox1.Text);
        }

        int counterLVL;
        private async void lbTEXT_Click(object sender, EventArgs e)
        {
            if (lbTEXT.Text == "CONTINUE")
            {
                AreaClear();
                await SetTileByLvl(counterLVL + 1);
                counterLVL++;

                lbLVL.Text = $"LVL {counterLVL}";
                Anim.StopTransfusion();
                Anim.ChangeText(lbTEXT, "", Anim.Speed._15);
                ResetSettingsToContinue();
                await Task.Delay(1000);
                StartGame();
            }

            if (lbTEXT.Text == "NEW GAME")
            {
                ChangeH(609);
                ChangeW(1139);
                await Task.Delay(500);
                this.MinimumSize = new Size(1139, 609);

                lbLVL.Visible = true;
                lbNEWGAME.Visible = true;
                tsmColorTools.Enabled = true;
                tsmLevel.Enabled = true;

                await SetTileByLvl(1);
                AreaClear();
                Anim.StopTransfusion();
                Anim.ChangeText(lbNEWGAME, "N", Anim.Speed._15);
                Anim.ChangeText(lbLVL, "LVL 1", Anim.Speed._15);
                Anim.ChangeText(lbTEXT, "", Anim.Speed._15);
                Anim.ChangeColor(pLine, FlickColor.R, FlickColor.G, FlickColor.B, Anim.Speed._8);
                ResetData();
                await Task.Delay(1000);
                StartGame();
            }
        }

        private async void ShiftAnimation(Panel pn, bool IsBack)
        {
            int x = 0;
            while (x != 12)
            {
                if (IsBack)
                    pn.Location = new Point(pn.Location.X - 4, pn.Location.Y);
                else
                    pn.Location = new Point(pn.Location.X + 4, pn.Location.Y);

                await Task.Delay(1);
                x++;
            }
        }

        private async Task SetTileByLvl(int lvl)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>()
            {
                { 1, 16},
                { 2, 20},
                { 3, 24},
                { 4, 32}
            };
            if (lvl > dic.Count)
                return;

            IsAnimEnd = false;
            int NumPanel = 0;
            int j = 0;
            if (lvl == 4 || counterLVL == 4)
                j = 1;

            if (lvl - counterLVL > 0)
                for (int i = 0; i < lvl - counterLVL + j; i++)
                {
                    foreach (Panel panel in ListAllPanels)
                    {
                        ShiftAnimation(panel, true);
                        await Task.Delay(1);
                    }
                    await Task.Delay(100);
                }
            else if (lvl - counterLVL != 0)
                for (int i = 0; i < counterLVL - lvl + j; i++)
                {
                    foreach (Panel panel in ListAllPanels)
                    {
                        ShiftAnimation(panel, false);
                        await Task.Delay(1);
                    }
                    await Task.Delay(100);
                }

            foreach (Panel panel in ListAllPanels)
            {
                NumPanel = Convert.ToInt32(panel.Name.Remove(0, 5));
                if (NumPanel >= 1 && NumPanel <= dic[lvl])
                    Anim.ChangeColor(panel, StartColor.R, StartColor.G, StartColor.B, Anim.Speed._3);
                else
                    Anim.ChangeColor(panel, 22, 22, 25, Anim.Speed._3);
            }
            await Task.Delay(1000);
            ActivityPanels = Controls.OfType<Panel>().Where<Panel>(e => e.Name.Contains("panel") && e.BackColor != Color.FromArgb(22, 22, 25)).ToArray();
            IsAnimEnd = true;
        }

        private async void ChangeW(int width)
        {
            while (true)
            {
                if (this.Width >= width)
                    break;
                this.Size = new Size(this.Size.Width + 60, this.Size.Height);

                await Task.Delay(1);
            }
        }

        private async void ChangeH(int height)
        {
            while (true)
            {
                if (this.Height >= height)
                    break;
                this.Size = new Size(this.Size.Width, this.Size.Height + 30);
                await Task.Delay(1);
            }
        }

        private async void label2_Click(object sender, EventArgs e)
        {
            if (IsAnimEnd)
            {
                Anim.StopTransfusion();
                Anim.ChangeText(lbNEWGAME, "N", Anim.Speed._15);

                if (counterLVL != 1)
                    foreach (Panel p in ActivityPanels)
                        Anim.ChangeColor(p, StartColor.R, StartColor.G, StartColor.B, Anim.Speed._10);
                await SetTileByLvl(1);
                tsmLevel.Enabled = true;

                Clock.Stop();
                lbCLOCK.Text = String.Empty;

                if (lbTEXT.Text != "")
                    NewGame();
            }
        }

        private async void lbNEWGAME_MouseEnter(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            Anim.ChangeColor(lb, FlickColor.R, FlickColor.G, FlickColor.B, Anim.Speed._15);
            if (IsAnimEnd && lb.Text == "N")
            {
                IsAnimEnd = false;
                await Anim.ChangeTextAsync(lbNEWGAME, "NEW GAME", Anim.Speed._15);
                IsAnimEnd = true;
            }
        }

        private async void lbNEWGAME_MouseLeave(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            Anim.ChangeColor(lb, MoveColor.R, MoveColor.G, MoveColor.B, Anim.Speed._15);
            if (IsAnimEnd && lb.Text == "NEW GAME")
            {
                IsAnimEnd = false;
                await Anim.ChangeTextAsync(lbNEWGAME, "N", Anim.Speed._15);
                IsAnimEnd = true;
            }
        }

        private async void lbTEXT_MouseEnter(object sender, EventArgs e)
        {
            if (IsAnimEnd)
            {
                if (lbTEXT.Text == "HEALTH")
                {
                    IsAnimEnd = false;
                    await Anim.ChangeTextAsync(lbTEXT, $"{3 - unsuccessfulClick}", Anim.Speed._15);
                    IsAnimEnd = true;
                }

                if (lbTEXT.Text == "NEW GAME" && !IsEndGame || lbTEXT.Text == "CONTINUE")
                    Anim.StopTransfusion();

                if (lbTEXT.Text == "GAME OVER" && IsEndGame)
                {
                    IsAnimEnd = false;
                    Anim.ChangeColor(lbTEXT, FlickColor.R, FlickColor.G, FlickColor.B, Anim.Speed._15);
                    await Anim.ChangeTextAsync(lbTEXT, "NEW GAME", Anim.Speed._15);
                    IsAnimEnd = true;
                }
            }
        }

        private async void lbTEXT_MouseLeave(object sender, EventArgs e)
        {
            if (IsAnimEnd)
            {
                if (lbTEXT.Text == "1" || lbTEXT.Text == "2" || lbTEXT.Text == "3")
                {
                    IsAnimEnd = false;
                    await Anim.ChangeTextAsync(lbTEXT, "HEALTH", Anim.Speed._15);
                    IsAnimEnd = true;
                }

                if (lbTEXT.Text == "NEW GAME" && !IsEndGame || lbTEXT.Text == "CONTINUE")
                {
                    Anim.StopTransfusion();
                    Anim.Transfusion(lbTEXT, FlickColor.R, FlickColor.G, FlickColor.B, Anim.Speed._12);
                }

                if (lbTEXT.Text == "NEW GAME" && IsEndGame)
                {
                    IsAnimEnd = false;
                    Anim.ChangeColor(lbTEXT, 169, 0, 0, Anim.Speed._15);
                    await Anim.ChangeTextAsync(lbTEXT, "GAME OVER", Anim.Speed._15);
                    IsAnimEnd = true;
                }
            }
        }

        private async void lbLVL_MouseEnter(object sender, EventArgs e)
        {
            if (lbLVL.Text.Contains("LVL") && IsAnimEnd)
            {
                IsAnimEnd = false;
                await Anim.ChangeTextAsync(lbLVL, $"LEVEL {counterLVL}", Anim.Speed._14);
                IsAnimEnd = true;
            }
        }

        private async void lbLVL_MouseLeave(object sender, EventArgs e)
        {
            if (IsAnimEnd)
                if (lbLVL.Text.Contains("LEVEL"))
                {
                    IsAnimEnd = false;
                    await Anim.ChangeTextAsync(lbLVL, $"LVL {counterLVL}", Anim.Speed._15);
                    IsAnimEnd = true;
                }
        }
    }
}
