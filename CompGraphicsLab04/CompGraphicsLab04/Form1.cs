using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        LinkedListNode<LinkedList<Point>> current; // текущий полигон
        private Pen pen = new Pen(Color.Black);
        private Bitmap bmp;
        int index_point;
        int index_line;
        int index_polygon;
        bool isLocked = false;
        bool nextClickSetsPointForAffine = false;
        Point PointForAffine;
        public Form1()
        {
            InitializeComponent();

            radioButton1.Checked = true;

            points = new LinkedList<Point>();
            lines = new LinkedList<Tuple<Point, Point>>();
            polygons = new LinkedList<LinkedList<Point>>();

            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;

            index_point = 0;
            index_line = 0;
            index_polygon = 0;

            XBox.Minimum = -pictureBox1.Width;
            XBox.Maximum = pictureBox1.Width;
            YBox.Minimum = -pictureBox1.Height;
            YBox.Maximum = pictureBox1.Height;
        }

        private bool first_point_line = true;
        private Point first;
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(nextClickSetsPointForAffine)
            {
                nextClickSetsPointForAffine = false;
                PointForAffine = e.Location;
                PointForAffineLabel.Text = $"X:{e.Location.X}; Y:{e.Location.Y}";
            } else
            // Если выбрано задание Принадлежит ли точка выпуклому многоугольнику
            if (checkBox1.Checked)
            {
                MessageBox.Show(IsPointInside(treeView1.SelectedNode.Tag as LinkedListNode<LinkedList<Point>>, e.Location).ToString());
            }
            else if (checkBox2.Checked) // Если выбрано задание Классифицировать положение точки относительно ребра
            {
                var edge = (treeView1.SelectedNode.Tag as LinkedListNode<Tuple<Point, Point>>).Value;
                Position p =
                    PointPosition(edge, e.Location);
                switch (p)
                {
                    case Position.Left:
                        MessageBox.Show("Точка находится слева от ребра");
                        break;
                    case Position.Right:
                        MessageBox.Show("Точка находится справа от ребра");
                        break;
                    case Position.Undefined:
                        MessageBox.Show("Точка находится хз где от ребра");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (radioButton1.Checked) // если выбрана точка
                {
                    TreeNode node = treeView1.Nodes.Add("point" + ++index_point);
                    node.Tag = points.AddLast(e.Location);
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
                        TreeNode node = treeView1.Nodes.Add("line" + ++index_line);
                        node.Tag = lines.AddLast(Tuple.Create(first, e.Location));
                    }
                }
                if (radioButton3.Checked)
                {
                    current.Value.AddLast(e.Location);
                }
                DrawPrimitives();
            }
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
                Pen pen = new Pen(Color.Black, 1);
                pen.EndCap = LineCap.ArrowAnchor;
                g.DrawLine(pen, point.Item1, point.Item2);
            }
            foreach (var polygon in polygons) // рисуем полигоны
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
            TreeNode node = treeView1.Nodes.Add("polygon" + ++index_polygon);
            node.Tag = polygons.AddFirst(new LinkedList<Point>()); // добавляем новый полигон
            current = polygons.First; // переходим к новому полигону для добавления точек
        }

        // Кнопка "Очистить"
        private void Clear_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            points.Clear();
            lines.Clear();
            polygons.Clear();
            treeView1.Nodes.Clear();
            radioButton1.Checked = true;
            index_point = 0;
            index_line = 0;
            index_polygon = 0;

            MoveBtn.Enabled = false;
        }

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

        enum Position { Left, Right, Undefined }
        /// <summary>
        /// Определяет положение точки относительно направленного ребра
        /// </summary>
        private Position PointPosition(Tuple<Point, Point> edge, Point b)
        {
            // edge — направленное ребро (Oa в лекции)
            Point O = edge.Item1;
            Point a = edge.Item2;
            int sign = (b.X - O.X) * (a.Y - O.Y) - (b.Y - O.Y) * (a.X - O.X);

            if (sign > 0)
                return Position.Left;
            if (sign < 0)
                return Position.Right;
            return Position.Undefined;
        }

        private bool IsPointInside(LinkedListNode<LinkedList<Point>> polygon, Point point)
        {
            Position lastPosition = Position.Undefined;
            bool isFirst = true;
            var list = polygon.Value;
            var cur = list.First;
            var first_val = cur.Value;
            Tuple<Point, Point> edge;
            Position pos;
            do  //while (cur != )
            {
                edge = Tuple.Create(cur.Value, cur.Next.Value);
                pos = PointPosition(edge, point);
                if (isFirst)
                {
                    lastPosition = pos;
                    isFirst = false;
                }
                else
                {
                    if (pos != lastPosition)
                        return false;
                }
                cur = cur.Next;
            } while (cur != list.Last);

            edge = Tuple.Create(cur.Value, first_val);
            pos = PointPosition(edge, point);
            if (pos != lastPosition)
                return false;
            return true;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MoveBtn.Enabled = true;
            if (checkBox1.Checked || checkBox2.Checked)
            {
                MessageBox.Show("Выберите точку мышкой");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //isLocked = true;
            /* radioButton1.Enabled = false;
             radioButton2.Enabled = false;
             radioButton3.Enabled = false;*/

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private Point ShiftPoint(Point point, Point d)
        {
            double[,] shift_matr = {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { d.X, d.Y, 1 }
            };
            double[,] point_matr = { { point.X, point.Y, 1 } };

            var m = multMatrix(point_matr, shift_matr);
            return new Point((int)m[0, 0], (int)m[0, 1]);
        }

        private Point RotatePoint(Point point, Point O, double phi)
        {
            double[,] rotate_matr = { 
                { Math.Cos(phi), Math.Sin(phi), 0 },
                { -Math.Sin(phi), Math.Cos(phi), 0},
                { -O.X *Math.Cos(phi)+ O.Y*Math.Sin(phi)+ O.X, -O.X * Math.Sin(phi)- O.Y *Math.Cos(phi) + O.Y, 1 } };

            double[,] point_matr = { { point.X, point.Y, 1 } };
            var m = multMatrix(point_matr, rotate_matr);
            return new Point((int)m[0, 0], (int)m[0, 1]);
        }

        private Point ScalePoint(Point point, double alpha, double beta, Point p)
        {
            double[,] Scale_matr = {
                { alpha, 0, 0 },
                { 0, beta, 0 },
                { (1-alpha) * p.X, (1-beta) *  p.Y, 1 } };

            double[,] point_matr = { { point.X, point.Y, 1 } };
            var c = multMatrix(point_matr, Scale_matr);
            return new Point((int)c[0, 0], (int)c[0, 1]);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            angle_label.Text = (sender as TrackBar).Value.ToString();
        }

        private void ScaleVal_ValueChanged(object sender, EventArgs e)
        {
            scaleLabel.Text = (sender as TrackBar).Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            nextClickSetsPointForAffine = true;
        }

        private void MoveBtn_Click(object sender, EventArgs e)
        {
            var type = treeView1.SelectedNode.Tag.GetType();
            //var type = string.Concat(treeView1.SelectedNode.Text.TakeWhile(ch => char.IsLetter(ch)));
            //var index = int.Parse(string.Concat(treeView1.SelectedNode.Text.SkipWhile(ch => char.IsLetter(ch)))) - 1;

            var d = new Point((int)XBox.Value, (int)YBox.Value);
            if (type == typeof(LinkedListNode<Point>))//point
            {
                var o = (treeView1.SelectedNode.Tag as LinkedListNode<Point>);
                o.Value = ShiftPoint(o.Value, d);
            }
            else if (type == typeof(LinkedListNode<Tuple<Point, Point>>))//line
            {
                var o = (treeView1.SelectedNode.Tag as LinkedListNode<Tuple<Point, Point>>);
                o.Value = new Tuple<Point, Point>(ShiftPoint(o.Value.Item1, d), ShiftPoint(o.Value.Item2, d));
            }
            else if (type == typeof(LinkedListNode<LinkedList<Point>>))//polygon
            {
                var o = (treeView1.SelectedNode.Tag as LinkedListNode<LinkedList<Point>>);
                o.Value = new LinkedList<Point>(o.Value.Select(p => ShiftPoint(p, d)));
            }
            else
                throw new Exception();
            DrawPrimitives();

        }

        private void RotateBtn_Click(object sender, EventArgs e)
        {
            var type = treeView1.SelectedNode.Tag.GetType();
            double angle = Math.PI * RotateAngle.Value / 180.0;
/*
            if (type == typeof(LinkedListNode<Point>))//point
            {
                var o = (treeView1.SelectedNode.Tag as LinkedListNode<Point>);
                o.Value = RotatePoint(o.Value, PointForAffine, angle);
            }
            else if (type == typeof(LinkedListNode<Tuple<Point, Point>>))//line
            {
                var o = (treeView1.SelectedNode.Tag as LinkedListNode<Tuple<Point, Point>>);
                o.Value = new Tuple<Point, Point>(ShiftPoint(o.Value.Item1, d), ShiftPoint(o.Value.Item2, d));
            }
            else if (type == typeof(LinkedListNode<LinkedList<Point>>))//polygon
            {
                var o = (treeView1.SelectedNode.Tag as LinkedListNode<LinkedList<Point>>);
                o.Value = new LinkedList<Point>(o.Value.Select(p => ShiftPoint(p, d)));
            }
            else
                throw new Exception();
            DrawPrimitives();*/
        }
    }
}
