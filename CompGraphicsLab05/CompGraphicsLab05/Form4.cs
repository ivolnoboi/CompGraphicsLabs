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

        private float[,] BezierMatrix = { { -1,  3, -3,  1 },  // Марица Безье
                                          {  3, -6,  3,  0 },
                                          { -3,  3,  0,  0 },
                                          {  1,  0,  0,  0 }};

        private PointF additionalPoint;
        private int index_of_moving_point; // индекс точки, которую будем передвигать

        public Form4(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();
            points = new List<PointF>();
            radioButton1.Checked = true;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            index_of_moving_point = -1;
            checkBox2.Checked = true;
            additionalPoint = new PointF();
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
            checkBox2.Checked = true;
            index_of_moving_point = -1;
            additionalPoint = new PointF();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked) // если выбрано "добавление точки"
            {
                points.Add(e.Location);
            }
            if (radioButton2.Checked) // если выбрано "удаление точки"
            {
                DeletePoint(e.Location.X, e.Location.Y);
            }
            DrawElements();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioButton3.Checked)
            {
                index_of_moving_point = points.FindIndex(el => (el.X > e.X - 3) && (el.X < e.X + 3) && (el.Y > e.Y - 3) && (el.Y < e.Y + 3));
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (radioButton3.Checked)
            {
                if (index_of_moving_point >= 0)
                {
                    points[index_of_moving_point] = new PointF(e.X, e.Y);
                    DrawElements();
                    index_of_moving_point = -1;
                }
            }
        }

        private void DrawElements()
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            if (checkBox2.Checked)
                DrawPoints();
            if (checkBox1.Checked)
                DrawAdditionalLinesBetweenPoints();
            DrawCurveBezie(points);
            pictureBox1.Image = bmp;
        }
        private void DrawPoints()
        {
            SolidBrush solidBrush = new SolidBrush(Color.Red);
            Graphics g = Graphics.FromImage(bmp);
            Pen p = new Pen(Color.Red);
            foreach (var point in points)
            {
                if (point != additionalPoint)
                {
                    g.DrawEllipse(p, point.X - 2, point.Y - 2, 4, 4);
                    g.FillEllipse(solidBrush, point.X - 2, point.Y - 2, 4, 4);
                }
            }
            pictureBox1.Image = bmp;
        }
        private void DrawCurveBezie(List<PointF> points_lst)
        {
            int points_size = points_lst.Count();
            if (points_size == 4)
            {
                DrawCurveFor4Points(points_lst[0], points_lst[1], points_lst[2], points_lst[3]);
            }
            else if (points_size > 4)
            {
                if (points_size % 2 == 0)
                {
                    DrawCurve(points_lst);
                }
                else
                {
                    AddAdditionalPoint(ref points_lst);
                    DrawCurve(points_lst);
                }
            }
        }

        private void DrawAdditionalLinesBetweenPoints()
        {
            Graphics g = Graphics.FromImage(bmp);
            if (points.Count != 0)
            {
                var first_point = points[0];
                for (int i = 1; i < points.Count; i++)
                {
                    g.DrawLine(new Pen(Color.Gray), first_point, points[i]);
                    first_point = points[i];
                }
            }
        }

        private void DeletePoint(float x, float y)
        {
            int index_for_delete = points.FindIndex(el => (el.X > x - 3) && (el.X < x + 3) && (el.Y > y - 3) && (el.Y < y + 3));
            if (index_for_delete >= 0)
            {
                points.RemoveAt(index_for_delete);
                bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                DrawElements();
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

        // Функция для отрисовки каждой точки кривой
        private PointF GetNextPointOfCurve(PointF p0, PointF p1, PointF p2, PointF p3, float t)
        {
            float[,] MatrPointsX = { { p0.X }, { p1.X }, { p2.X }, { p3.X } };
            float[,] MatrPointsY = { { p0.Y }, { p1.Y }, { p2.Y }, { p3.Y } };

            float[,] MatrParametrs = { { t * t * t, t * t, t, 1 } };

            float X = multMatrix(multMatrix(MatrParametrs, BezierMatrix), MatrPointsX)[0, 0];
            float Y = multMatrix(multMatrix(MatrParametrs, BezierMatrix), MatrPointsY)[0, 0];

            return new PointF(X, Y);
        }

        // Нарисовать кривую по 4 опорным точкам
        private void DrawCurveFor4Points(PointF p0, PointF p1, PointF p2, PointF p3)
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

        private PointF GetExtraPoint(PointF point1, PointF point2)
        {
            return new PointF((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
        }

        // Добавление дополнительной точки между двумя последними точками
        private void AddAdditionalPoint(ref List<PointF> lst)
        {
            if (additionalPoint.IsEmpty)
            {
                additionalPoint = GetExtraPoint(lst[lst.Count - 2], lst[lst.Count - 1]); // Вычисляем середину отрезка между двумя последними точками
                lst.Add(lst[lst.Count - 1]);
                lst[lst.Count - 2] = additionalPoint;
            }
            else
            {
                DeletePoint(additionalPoint.X, additionalPoint.Y);
                additionalPoint = new PointF();
            }
        }

        private void DrawCurve(List<PointF> p_lst)
        {
            int count = p_lst.Count();
            PointF point0 = p_lst[0];
            PointF point1 = p_lst[1];
            PointF point2 = p_lst[2];
            PointF point3 = GetExtraPoint(p_lst[2], p_lst[3]);
            DrawCurveFor4Points(point0, point1, point2, point3);

            var index = 3;
            while (index < count - 4)
            {
                point0 = point3;
                point1 = p_lst[index];
                point2 = p_lst[index + 1];
                point3 = GetExtraPoint(p_lst[index + 1], p_lst[index + 2]);
                DrawCurveFor4Points(point0, point1, point2, point3);
                index += 2;
            }

            point0 = point3;
            point1 = p_lst[count - 3];
            point2 = p_lst[count - 2];
            point3 = p_lst[count - 1];
            DrawCurveFor4Points(point0, point1, point2, point3);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DrawElements();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            DrawElements();
        }

    }
}
