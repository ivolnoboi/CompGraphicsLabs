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
        }
        private void Draw()
        {
            // graphics.Clear(Color.White);
            Random r = new Random();
            pen = new Pen(Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)), 2);
            List<Edge> edges = projection.Project(curPolyhedron);
            foreach (Edge line in edges)
                graphics.DrawLine(pen, line.From.ConvertToPoint(), line.To.ConvertToPoint());
            pictureBox1.Invalidate();
        }

        /// <summary>
        /// Построение куба
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Point3D start = new Point3D(250, 150, 200);
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
            curPolyhedron.AddEdges(points[0], new List<Point3D> { points[1], points[4] });
            curPolyhedron.AddEdges(points[1], new List<Point3D> { points[2], points[5] });
            curPolyhedron.AddEdges(points[2], new List<Point3D> { points[6], points[3] });
            curPolyhedron.AddEdges(points[3], new List<Point3D> { points[7], points[0] });
            curPolyhedron.AddEdges(points[4], new List<Point3D> { points[5] });
            curPolyhedron.AddEdges(points[5], new List<Point3D> { points[6] });
            curPolyhedron.AddEdges(points[6], new List<Point3D> { points[7] });
            curPolyhedron.AddEdges(points[7], new List<Point3D> { points[4] });

            Draw();
        }

        /// <summary>
        /// Построение тетраэдра
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            Point3D start = new Point3D(250, 150, 200);
            float len = 150;

            List<Point3D> points = new List<Point3D>
            {
                start,
                start + new Point3D(len, 0, len),
                start + new Point3D(len, len, 0),
                start + new Point3D(0, len, len),
            };

            curPolyhedron = new Polyhedron(points);
            curPolyhedron.AddEdges(points[0], new List<Point3D> { points[1], points[3], points[2] });
            curPolyhedron.AddEdges(points[1], new List<Point3D> { points[3] });
            curPolyhedron.AddEdges(points[2], new List<Point3D> { points[1], points[3] });

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
            Point3D start = new Point3D(250 + 75, 150, 200 + 75);
            float len = 150;

            List<Point3D> points = new List<Point3D>
            {
                start,
                start + new Point3D(len / 2, len / 2, 0),
                start + new Point3D(-len / 2, len / 2, 0),
                start + new Point3D(0, len / 2, -len / 2),
                start + new Point3D(0, len / 2, len / 2),
                start + new Point3D(0, len, 0),
            };

            curPolyhedron = new Polyhedron(points);
            curPolyhedron.AddEdges(points[0], new List<Point3D> { points[1], points[3], points[2], points[4] });
            curPolyhedron.AddEdges(points[5], new List<Point3D> { points[1], points[3], points[2], points[4] });
            curPolyhedron.AddEdges(points[1], new List<Point3D> { points[3] });
            curPolyhedron.AddEdges(points[3], new List<Point3D> { points[2] });
            curPolyhedron.AddEdges(points[2], new List<Point3D> { points[4] });
            curPolyhedron.AddEdges(points[4], new List<Point3D> { points[1] });
            Draw();
        }

        // Применить преобразования
        private void button5_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked) // смещение по оси
            {
                float x = float.Parse(textBox1.Text);
                float y = float.Parse(textBox2.Text);
                float z = float.Parse(textBox3.Text);

                curPolyhedron = Affine.translate(curPolyhedron, x, y, z);
                Draw();
            }
            if(radioButton2.Checked)
            {
                float x = float.Parse(textBox1.Text)/100;
                float y = float.Parse(textBox2.Text)/100;
                float z = float.Parse(textBox3.Text)/100;
                if (x > 0 && y > 0 && z > 0)
                {
                    curPolyhedron = Affine.scale(curPolyhedron, x, y, z);
                    Draw();
                }
            }
            if (radioButton3.Checked)
            {
                float x = float.Parse(textBox1.Text);
                float y = float.Parse(textBox2.Text);
                float z = float.Parse(textBox3.Text);
                curPolyhedron = Affine.rotation(curPolyhedron, x, y, z);
            }
        }
    }
}
