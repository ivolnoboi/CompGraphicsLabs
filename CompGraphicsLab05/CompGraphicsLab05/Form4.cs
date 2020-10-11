using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompGraphicsLab05
{
    public partial class Form4 : Form
    {
        private Form1 _form1;
        private List<PointF> points;
        private Bitmap bmp;
        // Марица Безье
        private float[,] BezierMatrix = { { -1,  3, -3,  1 },
                                          {  3, -6,  3,  0 },
                                          { -3,  3,  0,  0 },
                                          {  1,  0,  0,  0 }};
        public Form4(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();
            points = new List<PointF>();
            radioButton1.Checked = true;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
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

        // Очистка 
        private void button2_Click(object sender, EventArgs e)
        {
            points.Clear();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            radioButton1.Checked = true;
            label2.Text = "";
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked) // если выбрано "добавление точки"
            {
                points.Add(e.Location);
                DrawPoints();
                if (points.Count >= 4)
                    DrawCurve(points[0], points[1], points[2], points[3]);
            }
            if (radioButton2.Checked)
            {
                DeletePoint(e.Location.X, e.Location.Y);
            }
        }

        private void DrawPoints()
        {
            SolidBrush solidBrush = new SolidBrush(Color.Red);
            Graphics g = Graphics.FromImage(bmp);
            Pen p = new Pen(Color.Red);
            foreach (var point in points)
            {
                g.DrawEllipse(p, point.X - 2, point.Y - 2, 4, 4);
                g.FillEllipse(solidBrush, point.X - 2, point.Y - 2, 4, 4);
            }
            pictureBox1.Image = bmp;
        }

        private void DeletePoint(float x, float y)
        {
            int index_for_delete = 0;
            if (points.Exists(el => (el.X > x - 3) && (el.X < x + 3) && (el.Y > y - 3) && (el.Y < y + 3))) // если есть точка в области таких координат
            {
                index_for_delete = points.FindIndex(el => (el.X > x - 3) && (el.X < x + 3) && (el.Y > y - 3) && (el.Y < y + 3));
                points.RemoveAt(index_for_delete);
                bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                DrawPoints();
                label2.Text = "Точка удалена";
            }
            else
            {
                label2.Text = "Точка с данными координатами не найдена";
            }
        }

        //перемножение матриц
        private float[,] multMatrix(float[,] m1, float[,] m2)
        {
            float[,] res = new float[m1.GetLength(0), m2.GetLength(1)];

            for (int i = 0; i < m1.GetLength(0); ++i)
                for (int j = 0; j < m2.GetLength(1); ++j)
                    for (int k = 0; k < m2.GetLength(0); k++)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }

            return res;
        }

        private PointF GetNextPointOfCurve(PointF p0, PointF p1, PointF p2, PointF p3, float t)
        {
            float[,] MatrPointsX = { { p0.X }, { p1.X }, { p2.X }, { p3.X } };
            float[,] MatrPointsY = { { p0.Y }, { p1.Y }, { p2.Y }, { p3.Y } };

            float[,] MatrParametrs = { { t * t * t, t * t, t, 1 } };

            float X = multMatrix(multMatrix(MatrParametrs, BezierMatrix), MatrPointsX)[0, 0];
            float Y = multMatrix(multMatrix(MatrParametrs, BezierMatrix), MatrPointsY)[0, 0];

            return new PointF(X, Y);
        }

        private void DrawCurve(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            float t = 0.0F;
            while (t <= 1.0)
            {
                var pixel = GetNextPointOfCurve(p0, p1, p2, p3, t);
                bmp.SetPixel((int)pixel.X, (int)pixel.Y, Color.Black);
                t += 0.0001F;
            }
            pictureBox1.Image = bmp;
        }

    }
}
