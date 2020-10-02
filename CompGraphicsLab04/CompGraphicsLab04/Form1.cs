using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompGraphicsLab04
{
    public partial class Form1 : Form
    {
        private LinkedList<Point> points; // список всех точек на pictureBox
        private LinkedList<Tuple<Point, Point>> lines; // список всех отрезков на pictureBox
        private LinkedList<LinkedList<Point>> polygons; // список всех полигонов на pictureBox
        LinkedList<Point> current; // текущий полигон
        private Pen pen = new Pen(Color.Black);
        private Bitmap bmp;
        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            points = new LinkedList<Point>();
            lines = new LinkedList<Tuple<Point, Point>>();
            current = new LinkedList<Point>();
            polygons = new LinkedList<LinkedList<Point>>();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
        }

        private bool first_point_line = true;
        private Point first;
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked) // если выбрана точка
            {
                points.AddLast(e.Location);
                checkedListBox1.Items.Add("point");
            }
            if (radioButton2.Checked) // если выбран отрезок
            {
                if (first_point_line)
                {
                    first_point_line = false; // говорим, что первая точка уже есть
                    first = e.Location;
                }
                else
                {
                    first_point_line = true; // добавляем вторую точку и говорим, что отрезок завершён
                    lines.AddLast(Tuple.Create(first, e.Location));
                    checkedListBox1.Items.Add("line");
                }
            }
            if (radioButton3.Checked)
            {
                current.AddLast(e.Location);
            }
            DrawPrimitives();
        }

        private void DrawPrimitives()
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            foreach (var point in points) // рисуем точки
            {
                bmp.SetPixel(point.X, point.Y, Color.Black);
            }
            foreach (var point in lines) // рисуем отрезки
            {
                g.DrawLine(pen, point.Item1, point.Item2);
            }
            foreach (var polygon in polygons)
            {
                // рисуем полигон
                var cur = polygon.First;
                if (cur != polygon.Last)
                    g.DrawLine(pen, polygon.First.Value, polygon.Last.Value);
                while (cur != polygon.Last)
                {
                    g.DrawLine(pen, cur.Value, cur.Next.Value);
                    cur = cur.Next;
                }
            }
            pictureBox1.Image = bmp;
        }

        private void radioButton3_MouseClick(object sender, MouseEventArgs e)
        {
            // HELP HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL 
            // Не знаю, что с этим делать. Добавляет список точек в список полигонов, а потом удаляет его. Я так понимаю, копию он не создаёт.
            // А нужно, чтоб он добавлял копию, а потом обнулял текущий, чтоб можно было рисовать несколько полигонов.
            polygons.AddLast(current);
            checkedListBox1.Items.Add("polygon"); // А ещё хз, куда вот это вставлять. По идее оно должно добавляться, когда закончили рисовать полигон
            current.Clear(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // HELP HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL HEPL 
            // А ещё после отчистки полигоны не рисуются. Нужно на radioButton полигон нажать, чтоб хотя бы один нарисовался
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            /* points = new LinkedList<Point>();
             lines = new LinkedList<Tuple<Point, Point>>();
             current = new LinkedList<Point>();
             polygons = new LinkedList<LinkedList<Point>>();*/
            points.Clear();
            lines.Clear();
            current.Clear();
            polygons.Clear();
        }
        // С List тоже не получилось сделать. Также не добавляются полигоны.

        //перемножение матриц
        private double[,] multMatrix(double[,] m1, double[,] m2)
        {
            double[,] res = new double[m1.GetLength(0), m2.GetLength(1)];

            for (int i = 0; i < m1.GetLength(0); ++i)
                for (int j = 0; j < m2.GetLength(1); ++j)
                    for (int k = 0; k < m2.GetLength(0); k++)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }

            return res;
        }
    }
}
