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
    public partial class Form3 : Form
    {
        private class Edge
        {
            public PointF left;
            public PointF right;

            public Edge(PointF _left, PointF _right)
            {
                left = _left;
                right = _right;
            }
        }

        private Form1 _form1;
        private Bitmap bmp;
        Graphics g;
        List<Edge> edges = new List<Edge>();
        Random rnd = new Random();
        double R;

        public Form3(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(bmp);
            initLLength.Maximum = pictureBox1.Height;
            initRLength.Maximum = pictureBox1.Height;
            initLLength.Value = pictureBox1.Height/2;
            initRLength.Value = pictureBox1.Height/2;

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

        private void NextStep_Click(object sender, EventArgs e)
        {
            if (edges.Count == 0)
            {
                double lLength;
                if (!Double.TryParse(initLLength.Text, out lLength))
                {
                    lLength = 100;
                };
                double rLength;
                if (!Double.TryParse(initRLength.Text, out rLength))
                {
                    rLength = 300;
                };
                if (!Double.TryParse(initRoughness.Text, out R))
                {
                    R = 0.4;
                }
                initLLength.Enabled = false;
                initRLength.Enabled = false;
                initRoughness.Enabled = false;
                Edge first = new Edge(new PointF(0, (float)(bmp.Height - lLength)), new PointF((float)(bmp.Width), (float)(bmp.Height - rLength)));
                edges.Add(first);
                DrawEdges();
            }
            else
            {
                List<Edge> scattered = new List<Edge>();
                foreach (Edge edge in edges)
                {
                    double length = Math.Sqrt((edge.right.X - edge.left.X) * (edge.right.X - edge.left.X) +
                        (edge.right.Y - edge.left.Y) * (edge.right.Y - edge.left.Y));
                    double newHeight = (edge.left.Y + edge.right.Y) / 2 + (rnd.NextDouble() - 0.5) * R * length;
                    PointF middle = new PointF((edge.left.X + edge.right.X) / 2, (int)newHeight);
                    scattered.Add(new Edge(edge.left, middle));
                    scattered.Add(new Edge(middle, edge.right));
                }
                edges = scattered;
                DrawEdges();

            }
        }

        private void DrawEdges()
        {
            pictureBox1.Image = bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            foreach (Edge edge1 in edges)
            {
                drawEdge(edge1);
            }
            pictureBox1.Invalidate();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            initLLength.Enabled = true;
            initRLength.Enabled = true;
            initRoughness.Enabled = true;
            edges = new List<Edge>();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            R = 0;
        }
        private void drawEdge(Edge edge) => g.DrawLine(Pens.Black, edge.left, edge.right);

        private void PlusBtn_Click(object sender, EventArgs e)
        {
            double R_tmp;
            if (Double.TryParse(initRoughness.Text, out R_tmp))
            {
                R_tmp += 0.1;
                initRoughness.Text = R_tmp.ToString();
            }
        }

        private void minusBtn_Click(object sender, EventArgs e)
        {
            double R_tmp;
            if (Double.TryParse(initRoughness.Text, out R_tmp))
            {
                R_tmp -= 0.1;
                initRoughness.Text = R_tmp.ToString();
            }
        }
    }
}
