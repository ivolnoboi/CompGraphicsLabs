using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompGraphicsLab06
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private Pen pen;
        private Projection projection;
        private int NextClicksAreLine = 0;
        private (Point, Point) line;
        /// <summary>
        /// Текущий многогранник
        /// </summary>
        private Polyhedron curPolyhedron;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);
            pen = new Pen(Color.DarkRed, 2);
            projection = new Projection();
            radioButton1.Checked = true;
            projBox.SelectedIndex = 0;
        }
        private void Draw()
        {
            graphics.Clear(Color.White);
            // graphics.Clear(Color.White);
            Random r = new Random();
            pen = new Pen(Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)), 2);
            List<Edge> edges = projection.Project(curPolyhedron,projBox.SelectedIndex);

            //Смещение по центру pictureBox
            var centerX = pictureBox1.Width / 2;
            var centerY = pictureBox1.Height / 2;

            //Смещение по центру фигуры
            //Тоже, конечно, так себе решение, но лучше, чем было
            var figureLeftX = edges.Min(e => e.From.X < e.To.X ? e.From.X : e.To.X);
            var figureLeftY = edges.Min(e => e.From.Y < e.To.Y ? e.From.Y : e.To.Y);
            var figureRightX = edges.Max(e => e.From.X > e.To.X ? e.From.X : e.To.X);
            var figureRightY = edges.Max(e => e.From.Y > e.To.Y ? e.From.Y : e.To.Y);
            var figureCenterX = (figureRightX - figureLeftX) / 2;
            var figureCenterY = (figureRightY - figureLeftY) / 2;

            foreach (Edge line in edges)
            {
                var p1 = (line.From).ConvertToPoint();
                var p2 = (line.To).ConvertToPoint();
                graphics.DrawLine(pen,p1.X+centerX-figureCenterX,p1.Y+centerY-figureCenterY,p2.X+centerX-figureCenterX,p2.Y+centerY-figureCenterY);
            }
            pictureBox1.Invalidate();
        }

        /// <summary>
        /// Построение куба
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Point3D start = new Point3D(0, 0, 0); //new Point3D(250, 150, 200);
            float len = 150;

            List<Point3D> points = new List<Point3D>
            {
                start,
                start + new Point3D(len, 0, 0),
                start + new Point3D(len, 0, len),
                start + new Point3D(0, 0, len),

                start + new Point3D(0, len, 0),
                start + new Point3D(len, len, 0),
                start + new Point3D(len, len, len),
                start + new Point3D(0, len, len)
            };

            curPolyhedron = new Polyhedron(points);
            curPolyhedron.AddEdges(0, new List<int> { 1, 4 });
            curPolyhedron.AddEdges(1, new List<int> { 2, 5 });
            curPolyhedron.AddEdges(2, new List<int> { 6, 3 });
            curPolyhedron.AddEdges(3, new List<int> { 7, 0 });
            curPolyhedron.AddEdges(4, new List<int> { 5 });
            curPolyhedron.AddEdges(5, new List<int> { 6 });
            curPolyhedron.AddEdges(6, new List<int> { 7 });
            curPolyhedron.AddEdges(7, new List<int> { 4 });

            Draw();
        }

        /// <summary>
        /// Построение тетраэдра
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            Point3D start = new Point3D(0, 0, 0);  //= new Point3D(250, 150, 200);
            float len = 150;

            List<Point3D> points = new List<Point3D>
            {
                start,
                start + new Point3D(len, 0, len),
                start + new Point3D(len, len, 0),
                start + new Point3D(0, len, len),
            };

            curPolyhedron = new Polyhedron(points);
            curPolyhedron.AddEdges(0, new List<int> { 1, 3, 2 });
            curPolyhedron.AddEdges(1, new List<int> { 3 });
            curPolyhedron.AddEdges(2, new List<int> { 1, 3 });

            Draw();
        }

        /// <summary>
        /// Очистить экран
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            pictureBox1.Invalidate();
        }

        /// <summary>
        /// Построение и рисование октаэдра
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            Point3D start = new Point3D(0, 0, 0); //= new Point3D(250 + 75, 150, 200 + 75);
            float len = 150;

            List<Point3D> points = new List<Point3D>
            {
                start,
                start + new Point3D(len , len , 0),
                start + new Point3D(-len, len , 0),
                start + new Point3D(0, len , -len ),
                start + new Point3D(0, len , len ),
                start + new Point3D(0,  2 *len, 0),
            };

            curPolyhedron = new Polyhedron(points);
            curPolyhedron.AddEdges(0, new List<int> { 1, 3, 2, 4 });
            curPolyhedron.AddEdges(5, new List<int> { 1, 3, 2, 4 });
            curPolyhedron.AddEdges(1, new List<int> { 3 });
            curPolyhedron.AddEdges(3, new List<int> { 2 });
            curPolyhedron.AddEdges(2, new List<int> { 4 });
            curPolyhedron.AddEdges(4, new List<int> { 1 });
            Draw();
        }

        // Применить преобразования
        private void button5_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) // Смещение по оси
            {
                float x = float.Parse(textBox1.Text);
                float y = float.Parse(textBox2.Text);
                float z = float.Parse(textBox3.Text);

                Affine.translate(curPolyhedron, x, y, z);
                Draw();
            }
            if (radioButton2.Checked) // Масштаб
            {
                float x = float.Parse(textBox1.Text) / 100;
                float y = float.Parse(textBox2.Text) / 100;
                float z = float.Parse(textBox3.Text) / 100;
                if (x > 0 && y > 0 && z > 0)
                {
                    Affine.scale(curPolyhedron, x, y, z);
                    Draw();
                }
            }
            if (radioButton3.Checked) // Поворот
            {
                float x = float.Parse(textBox1.Text);
                float y = float.Parse(textBox2.Text);
                float z = float.Parse(textBox3.Text);
                Affine.rotation(curPolyhedron, x, y, z);
                Draw();
            }
            if (radioButton4.Checked) // Отражение
            {
                string plane = "";
                switch (comboBox1.Text)
                {
                    case "Плоскость Oxy":
                        plane = "xy";
                        break;
                    case "Плоскость Oxz":
                        plane = "xz";
                        break;
                    case "Плоскость Oyz":
                        plane = "yz";
                        break;
                    default:
                        break;
                }
                if (plane!="")
                {
                    Affine.reflection(curPolyhedron, plane);
                    Draw();
                }
            }
            if (radioButton5.Checked) // Масштабирование относительно центра
            {
                float a = float.Parse(textBox4.Text) / 100;
                Affine.scaleCenter(curPolyhedron, a);
                Draw();
            }
        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            comboBox1.Text = "";
            textBox4.Text = "100";
        }

        private void radioButton2_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "100";
            textBox2.Text = "100";
            textBox3.Text = "100";
            comboBox1.Text = "";
            textBox4.Text = "100";
        }

        private void radioButton3_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            comboBox1.Text = "";
            textBox4.Text = "100";
        }

        private void radioButton5_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "100";
            comboBox1.Text = "";
        }

        private void radioButton4_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "100";
        }

        private void projBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (curPolyhedron!=null)
                Draw();
        }

        private void rotateOX_Click(object sender, EventArgs e)
        {
            rotateOY.Checked = rotateOZ.Checked = rotateOwn.Checked = false;
        }

        private void rotateOY_Click(object sender, EventArgs e)
        {
            rotateOX.Checked = rotateOZ.Checked = rotateOwn.Checked = false;
        }

        private void rotateOZ_Click(object sender, EventArgs e)
        {
            rotateOY.Checked = rotateOX.Checked = rotateOwn.Checked = false;
        }

        private void rotateBtn_Click(object sender, EventArgs e)
        {
            if (rotateOX.Checked)
                Affine.rotateCenter(curPolyhedron, (float)rotateAngle.Value, 0, 0);
            else if (rotateOY.Checked)
                Affine.rotateCenter(curPolyhedron, 0, (float)rotateAngle.Value, 0);
            else if (rotateOZ.Checked)
                Affine.rotateCenter(curPolyhedron, 0, 0, (float)rotateAngle.Value);
            else if (rotateOwn.Checked)
                //"Время пострелять..."(с)
                Affine.rotateAboutLine(curPolyhedron, (float)rotateAngle.Value, new Edge(float.Parse(rX1.Text), float.Parse(rY1.Text), float.Parse(rZ1.Text),
                    float.Parse(rX2.Text), float.Parse(rY2.Text), float.Parse(rZ2.Text)));
            Draw();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (NextClicksAreLine-- == 2)
            {
                line.Item1 = e.Location;
                for (int i = e.Location.X - 1; i <= e.Location.X + 1; i++)
                    graphics.DrawLine(Pens.Red, i, e.Y - 1, i, e.Y + 1);
                pictureBox1.Invalidate();
            }
            else if (NextClicksAreLine-- == 1)
            {
                line.Item2 = e.Location;
                for (int i = e.Location.X - 1; i <= e.Location.X + 1; i++)
                    graphics.DrawLine(Pens.Red, i, e.Y - 1, i, e.Y + 1);
                graphics.DrawLine(Pens.Red, line.Item1, line.Item2);
                pictureBox1.Invalidate();
            }
        }

        private void rotateOwn_Click(object sender, EventArgs e)
        {
            rotateOY.Checked = rotateOZ.Checked = rotateOX.Checked = false;
        }
    }
}
