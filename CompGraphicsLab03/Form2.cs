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
    public partial class Form2 : Form
    {
        private Form1 _form1;
        private Graphics g;
        public Form2(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            _form1.Visible = true;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                penFill.Color = colorDialog1.Color;
                button2.BackColor= colorDialog1.Color;

            }
            /*OpenFileDialog ofd = new OpenFileDialog
            {
                // Маска для файлов
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bmp = new Bitmap(ofd.FileName);
                    pictureBox1.Image = bmp;
                    button2.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }*/
        }
        private Point old;
        private bool drawing = false;

        private Color areaColor;
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            areaColor = ((Bitmap)pictureBox1.Image).GetPixel(e.X, e.Y);
            fillArea(e.X, e.Y);
        }
        void fillArea(int x, int y)
        {
            Bitmap bitmap = pictureBox1.Image as Bitmap;

            if (bitmap.GetPixel(x, y) != areaColor || bitmap.GetPixel(x, y) == penFill.Color)
                return;

            int x_left = x;
            while (x_left > 0 && bitmap.GetPixel(x_left, y) == areaColor)
            {
                x_left--;
            }

            int x_right = x;
            while (x_right < bitmap.Size.Width && bitmap.GetPixel(x_right, y) == areaColor)
            {
                x_right++;
            }
            g.DrawLine(penFill, x_left, y, x_right, y);

            if (y + 1 < bitmap.Height)
                for (int i = x_left + 1; i < x_right; ++i)
                    fillArea(i, y + 1);

            if (y - 1 > 0)
                for (int i = x_left + 1; i < x_right; ++i)
                    fillArea(i, y - 1);

            pictureBox1.Invalidate();
        }

        Pen pen = new Pen(Color.Black, 1);
        Pen penFill = new Pen(Color.Red, 1);
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                g.DrawLine(pen, old.X, old.Y, e.X, e.Y);
                old = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            old = e.Location;
            drawing = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pictureBox1.Invalidate();
        }
    }
}
