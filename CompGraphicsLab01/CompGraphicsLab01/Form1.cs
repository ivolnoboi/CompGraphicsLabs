using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CompGraphicsLab01
{
    public partial class Form1 : Form
    {
        private Graphics g;

        public Form1()
        {
            InitializeComponent();
            g = this.CreateGraphics();
        }

        public void DrawGraphic(Func<double,double> func, int lowBound, int upBound)
        {
            g.Clear(this.BackColor);
            g.Dispose();
            g = this.CreateGraphics();
            
            var OY = (int)((-lowBound / (double)(upBound - lowBound)) * g.VisibleClipBounds.Width);
            g.DrawLine(Pens.Red, OY, 0, OY, g.VisibleClipBounds.Height);
            var step = g.VisibleClipBounds.Width/(upBound-lowBound);
            var points = new Tuple<int, double>[(int)g.VisibleClipBounds.Width];
            for (var i = 0; i< g.VisibleClipBounds.Width; i++)
            {
                points[i] = new Tuple<int, double>(i, - func(lowBound + ((double)i / step)));
            }
            var min = points.Min(p => p.Item2);
            var max = points.Max(p => p.Item2);
            var stepV = g.VisibleClipBounds.Height / (max - min);

            var OX = (int)((-min / (double)(max - min)) * g.VisibleClipBounds.Height);
            g.DrawLine(Pens.Red, 0, OX, g.VisibleClipBounds.Width, OX);

            for (int i1 = 1; i1 < points.Length; i1++)
            {
                Tuple<int, double> p = points[i1];
                g.DrawLine(Pens.Black, points[i1-1].Item1, (int)(OX + points[i1-1].Item2 * stepV), points[i1].Item1, (int)(OX + points[i1].Item2*stepV));
            }
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            DrawGraphic(Math.Sin, -5, 5);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(new string[] { "Sin", "x^2", "x^3"});
            comboBox1.SelectedIndex = 0;
            DrawGraphic(Math.Sin, -5, 5);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Func<double, double> sel;
            switch (comboBox1.Text)
            {
                case "Sin": sel = Math.Sin;
                    break;
                case "x^2": sel = (x) => (Math.Pow(x,2));
                    break;
                case "x^3": sel = (x) => (Math.Pow(x, 3));
                    break;
                default:
                    sel = x => x;
                    break;
            }
            var down = int.Parse(textBox1.Text);
            var up = int.Parse(textBox2.Text);
            DrawGraphic(sel, down, up);
        }
    }
}
