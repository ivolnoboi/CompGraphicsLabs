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
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked) // если выбрано "добавление точки"
            {
                points.Add(e.Location);
                DrawPoints();
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
    }
}
