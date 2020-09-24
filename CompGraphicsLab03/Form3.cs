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

namespace CompGraphicsLab03
{
    public partial class Form3 : Form
    {
        private Form1 _form1;
        private Bitmap image = null;
        private LinkedList<Tuple<int, int>> points;
        private Point old;
        private Point newP;
        private bool drawing = false;
        private Color penColor = Color.Black;
        private Graphics g;

        public Form3(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = image;
            g = Graphics.FromImage(pictureBox1.Image);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            _form1.Visible = true;
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {/*
            OpenFileDialog ofd = new OpenFileDialog();
            // маска для типа файлов
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    image = new Bitmap(ofd.FileName);
                    pictureBox1.Image = image;
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }*/
            FindBorder(newP.X, newP.Y);
            DrawBorder(); 
        }

        // получить координаты следующего пикселя по направлению
        private Tuple<int, int> nextPixel(int x, int y, int direction)
        {
            switch (direction)
            {
                case 0:
                    x += 1;
                    break;
                case 1:
                    x += 1;
                    y -= 1;
                    break;
                case 2:
                    y -= 1;
                    break;
                case 3:
                    x -= 1;
                    y -= 1;
                    break;
                case 4:
                    x -= 1;
                    break;
                case 5:
                    x -= 1;
                    y += 1;
                    break;
                case 6:
                    y += 1;
                    break;
                case 7:
                    x += 1;
                    y += 1;
                    break;
            }
            return Tuple.Create(x, y);
        }

        // получить цвет следующего пикселя по данному направлению
        private Color color(int x, int y, int direction)
        {
            var point = nextPixel(x, y, direction);
            return image.GetPixel(point.Item1, point.Item2);
        }

        /* 3 2 1
         * 4 X 0
         * 5 6 7
         */
        private void FindBorder(int x, int y)
        {
            var areaColor = image.GetPixel(x, y); // цвет области, границу которой ищем
            points = new LinkedList<Tuple<int, int>>();
            var count = 0;
            var maxCount = image.Width * image.Height;

            points.AddLast(Tuple.Create(x,y));
            int currentDirection = 6;

            // стартовые значения x, y и направления 
            var x_start = x;
            var y_start = y;
            var startDirection = 6;

            if (color(x,y,currentDirection) == areaColor)
            {
                y += 1;
                currentDirection = 4;
                points.AddLast(Tuple.Create(x, y));
            }
            else
            {
                while (color(x, y, currentDirection) != areaColor)
                {
                    currentDirection = (currentDirection + 1) % 8;
                }
                var pair = nextPixel(x, y, currentDirection);
                x = pair.Item1;
                y = pair.Item2;
                currentDirection = (currentDirection + 8 - 2) % 8;
                points.AddLast(Tuple.Create(x, y));
            }
            // идем по часовой стрелке, при этом держим границу слева относительно текущего пикселя
            while ((x != x_start || y != y_start || currentDirection != startDirection)&&count!=maxCount)
            {
                count++;
                if (color(x,y,currentDirection)==areaColor)
                {
                    var pair = nextPixel(x, y, currentDirection);
                    x = pair.Item1;
                    y = pair.Item2;
                    currentDirection = (currentDirection + 8 - 2) % 8;
                    points.AddLast(Tuple.Create(x, y));
                }
                else
                {
                    while (color(x,y,currentDirection)!=areaColor)
                    {
                        currentDirection = (currentDirection + 1) % 8;
                    }
                    var pair = nextPixel(x, y, currentDirection);
                    x = pair.Item1;
                    y = pair.Item2;
                    currentDirection = (currentDirection + 8 - 2) % 8;
                    points.AddLast(Tuple.Create(x, y));
                }
               
            }
        }

        private void DrawBorder()
        {
            foreach (Tuple<int, int> tp in points)
            {
                int i = tp.Item1;
                int j = tp.Item2;
                image.SetPixel(i, j, Color.Red);
            }
            pictureBox1.Image = image;
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var x = e.Location.X;
            var y = e.Location.Y;
            /*
            FindBorder(771, 312);
            DrawBorder();*/

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

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                newP = e.Location;
                Pen pen = new Pen(penColor, 1);
                g.DrawLine(pen, old.X, old.Y, newP.X, newP.Y);
                old = e.Location;
                pictureBox1.Image = image;
            }
        }
    }
}

