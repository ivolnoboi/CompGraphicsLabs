using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompGraphicsLab03
{
    public partial class Form4 : Form
    {
        private Form1 _form1;
        bool lineStrted = false;
        Point point;
        public Form4(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            _form1.Visible = true;
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void DrawLineB(System.Drawing.Pen pen, int x1, int y1, int x2, int y2)
        {
            var img = (pictureBox1.Image as Bitmap);
            var dy = Math.Abs(y2 - y1);
            var dx = Math.Abs(x2 - x1);
            int signX = x1 < x2 ? 1 : -1;
            int signY = y1 < y2 ? 1 : -1;
            
            int error = dx - dy;
            
            img.SetPixel(x2, y2, pen.Color);
            pictureBox1.Invalidate();
            while (x1 != x2 || y1 != y2)
            {
                img.SetPixel(x1, y1, pen.Color);
                pictureBox1.Invalidate();
                int error2 = error * 2;
                
                if (error2 > -dy)
                {
                    error -= dy;
                    x1 += signX;
                }
                if (error2 < dx)
                {
                    error += dx;
                    y1 += signY;
                }
            }  
        }

        private void DrawLineWu(System.Drawing.Pen pen, int x1, int y1, int x2, int y2)
        {
            float dx = x2 - x1; float dy = y2 - y1;
            var img = (pictureBox1.Image as Bitmap);
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (x1 > x2)
                {
                    (x1, x2) = (x2, x1);
                    (y1, y2) = (y2, y1);
                }
                img.SetPixel(x2, y2, pen.Color);
                pictureBox1.Invalidate();

                float gradient = dy / dx;
                float y = y1 + gradient;
                for (var x = x1 + 1; x <= x2 - 1; x++)
                {
                    img.SetPixel(x, (int)y, Color.FromArgb((int)((1 - (y - (int)y)) * 255), pen.Color.R, pen.Color.G, pen.Color.B));
                    img.SetPixel(x, (int)y + 1, Color.FromArgb((int)((y - (int)y) * 255), pen.Color.R, pen.Color.G, pen.Color.B));
                    pictureBox1.Invalidate();
                    y += gradient;
                }
            }
            else
            {
                if (y1 > y2)
                {
                    (x1, x2) = (x2, x1);
                    (y1, y2) = (y2, y1);
                }
                img.SetPixel(x2, y2, pen.Color);
                pictureBox1.Invalidate();

                float gradient = dx / dy;
                float x = x1 + gradient;
                for (var y = y1 + 1; y <= y2 - 1; y++)
                {
                    img.SetPixel((int)x, y, Color.FromArgb((int)((1 - (x - (int)x)) * 255), pen.Color.R, pen.Color.G, pen.Color.B));
                    img.SetPixel((int)x + 1, y, Color.FromArgb((int)((x - (int)x) * 255), pen.Color.R, pen.Color.G, pen.Color.B));
                    pictureBox1.Invalidate();
                    x += gradient;
                }
            }
        }


        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (lineStrted)
            {
                if (checkBox1.Checked)
                    DrawLineWu(Pens.Black, point.X, point.Y, e.X, e.Y);
                else
                    DrawLineB(Pens.Black, point.X, point.Y, e.X, e.Y);
            }
            else
                point = e.Location;
            lineStrted = !lineStrted;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox2.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }
    }
}
