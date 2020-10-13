using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


// Left 0, Up 90, Right 180
enum Direction { Left, Up, Rigth };
namespace CompGraphicsLab05
{

    using Point_t = Tuple<PointF, float>;
    using Section = Tuple<PointF, PointF, float, Color>;
    public partial class Form2 : Form
    {
        private Form1 _form1;
        private Dictionary<char, string> rules;
        private Graphics g;
        private string fName;
        public Form2(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();
            rules = new Dictionary<char, string>();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            fName = "";
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

        private Direction ConvertToDirection(string str)
        {
            if (str.ToLower() == "up")
                return Direction.Up;
            if (str.ToLower() == "left")
                return Direction.Left;
            return Direction.Rigth;
        }
        private float ConvertToRadians(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return (float)Math.PI;
                case Direction.Up:
                    return (float)(3 * Math.PI / 2);
                case Direction.Rigth:
                    return 0;
            }
            return 0;
        }
        private void LoadAndPrintFile(string fName)
        {
            rules.Clear();

            System.IO.StreamReader sReader = new System.IO.StreamReader(fName);

            string[] first_line = sReader.ReadLine().Split(' ');

            string current_state = first_line[0];
            float angle = (float)(float.Parse(first_line[1]) * Math.PI / 180.0f);
            Direction dir = ConvertToDirection(first_line[2]);

            while (!sReader.EndOfStream)
            {
                string str = sReader.ReadLine();
                rules.Add(str[0], str.Substring(2));
            }

            sReader.Close();

            for (int i = 0; i < numericUpDown1.Value; ++i)
            {
                StringBuilder new_state = new StringBuilder("");
                foreach (char symbol in current_state)
                {
                    // Если в наборе правил есть такой символ, то заменяем на его значение
                    if (rules.ContainsKey(symbol))
                        new_state.Append(rules[symbol]);
                    // Иначе просто добавляем символ
                    else
                        new_state.Append(symbol);
                }
                current_state = new_state.ToString();
            }
            print(current_state, angle, dir);
        }

        private void print(string state, float angle, Direction direction)
        {
            g.Clear(Color.White);
            List<Section> points = new List<Section>();

            float length = 20;
            float current_angle = ConvertToRadians(direction);

            PointF current_point = new PointF(0, 0);
            PointF next_point = new PointF(0, 0);

            Color color = Color.FromArgb(64, 0, 0);
            float width = fName.Contains("tree") ? 10 : 2;


            points.Add(Tuple.Create(current_point, next_point, width, color));

            Stack<Point_t> br_stack = new Stack<Point_t>();

            Random r = new Random(DateTime.Now.Millisecond);
            float d_angle = (angle * 0.5f);
            float rnd = (float)(checkBox1.Checked ? r.NextDouble() : 0.5);

            foreach (char symbol in state)
            {

                // Если встретили открывающую скобку, то запоминаем координаты точки и направление
                if (symbol == '[')
                {
                    br_stack.Push(Tuple.Create(current_point, current_angle));
                }
                // Если встретили закрывающую скобку, то восстанавливаем значения
                else if (symbol == ']')
                {
                    Point_t tuple = br_stack.Pop();
                    current_angle = tuple.Item2;
                    current_point = tuple.Item1;
                }
                else if (symbol == 'F')
                {
                    float x_new = (float)(current_point.X + length * Math.Cos(current_angle));
                    float y_new = (float)(current_point.Y + length * Math.Sin(current_angle));
                    next_point = new PointF(x_new, y_new);
                    points.Add(Tuple.Create(current_point, next_point, width, color));
                    current_point = next_point;
                }

                else if (symbol == '-')
                {
                    rnd = (float)(checkBox1.Checked ? r.NextDouble() : 0.5);
                    current_angle -= (angle - d_angle) + 2 * d_angle * rnd;
                }
                else if (symbol == '+')
                {
                    rnd = (float)(checkBox1.Checked ? r.NextDouble() : 0.5);
                    current_angle += (angle - d_angle) + 2 * d_angle * rnd;
                }
                else if (symbol == '{')
                {
                    width--;
                    length--;
                    color = Color.FromArgb(color.R - 3, color.G + 17, color.B);
                }
                else if (symbol == '}')
                {
                    width++;
                    length++;
                    color = Color.FromArgb(color.R + 3, color.G - 17, color.B);
                }
            }

            float x_min = points.Min(point => Math.Min(point.Item1.X, point.Item2.X));
            float x_max = points.Max(point => Math.Max(point.Item1.X, point.Item2.X));
            float y_min = points.Min(point => Math.Min(point.Item1.Y, point.Item2.Y));
            float y_max = points.Max(point => Math.Max(point.Item1.Y, point.Item2.Y));

            PointF centerFractal = new PointF(x_min + (x_max - x_min) / 2, y_min + (y_max - y_min) / 2);
            PointF centerPictureBox = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            float d = Math.Min(pictureBox1.Width / (x_max - x_min), pictureBox1.Height / (y_max - y_min));

            points = points.Select(point => scale(point, d, centerFractal, centerPictureBox)).ToList();


            Pen pen = new Pen(Color.DarkRed, 2);
            for (int i = 0; i < points.Count(); ++i)
            {
                g.DrawLine(new Pen(points[i].Item4, points[i].Item3), points[i].Item1, points[i].Item2);
                pictureBox1.Invalidate();
            }

        }
        private Section scale(Section points, float scale_factor, PointF centerFractal, PointF centerPictureBox)
        {
            return Tuple.Create
                (
                new PointF(
                centerPictureBox.X + (points.Item1.X - centerFractal.X) * scale_factor,
                centerPictureBox.Y + (points.Item1.Y - centerFractal.Y) * scale_factor),

                new PointF(
                centerPictureBox.X + (points.Item2.X - centerFractal.X) * scale_factor,
                centerPictureBox.Y + (points.Item2.Y - centerFractal.Y) * scale_factor),

                points.Item3,
                points.Item4
                );
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                fName = openFileDialog1.FileName;
            checkBox1.Enabled = fName.Contains("tree") || fName.Contains("bush");
            if (!checkBox1.Enabled)
                checkBox1.Checked = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (fName != "")
            {
                
                LoadAndPrintFile(fName);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
