using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Math;
using System.IO;
using Newtonsoft.Json;

namespace CompGraphicsLab06
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private Pen pen;
        private Projection projection;
        private List<Point3D> pointsRotate;
        private static List<Color> Colors;

        /// <summary>
        /// Текущий многогранник
        /// </summary>
        private Polyhedron curPolyhedron;
        private List<Polyhedron> scene = new List<Polyhedron>();
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
            pointsRotate = new List<Point3D>();

            Colors = new List<Color>();
            Random r = new Random(Environment.TickCount);
            for (int i = 0; i < 1000; ++i)
                Colors.Add(Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)));
        }

        private void ClearPictureBox()
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);
        }

        private void Draw()
        {

            if (checkBox1.Checked)
            {
                float x = float.Parse(textBox16.Text);
                float y = float.Parse(textBox15.Text);
                float z = float.Parse(textBox14.Text);
                DrawByFaces(DeleteNonFrontFaces.DeleteFaces(curPolyhedron, new Point3D(x * 100, y * 100, z * 100)));
            }
            else
            {
                if (checkBox2.Checked)
                {
                    DrawByEdges();
                    ZBufferOn(Colors);
                }
                else
                {
                    DrawByEdges();
                }
            }
        }
        private void DrawByEdges()
        {
            if (curPolyhedron.IsEmpty())
                return;
            Random r = new Random(Environment.TickCount);
            // graphics.Clear(Color.White);
            ClearPictureBox();
            foreach (var curPolyhedron in scene)
            {
                if (curPolyhedron.IsEmpty())
                    return;

                // graphics.Clear(Color.White);

                pen = new Pen(Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)), 2);
                List<Edge> edges = projection.Project(curPolyhedron, projBox.SelectedIndex);

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

                var fixX = centerX - figureCenterX + (figureLeftX < 0 ? Math.Abs(figureLeftX) : -Math.Abs(figureLeftX));
                var fixY = centerY - figureCenterY + (figureLeftY < 0 ? Math.Abs(figureLeftY) : -Math.Abs(figureLeftY));

                foreach (Edge line in edges)
                {
                    var p1 = (line.From).ConvertToPoint();
                    var p2 = (line.To).ConvertToPoint();
                    if (!NeedCentering.Checked)//Центрирование?
                        graphics.DrawLine(pen, p1.X + centerX - figureCenterX, p1.Y + centerY - figureCenterY, p2.X + centerX - figureCenterX, p2.Y + centerY - figureCenterY);
                    else
                        graphics.DrawLine(pen, p1.X + fixX, p1.Y + fixY, p2.X + fixX, p2.Y + fixY);

                    // graphics.DrawLine(pen,p1.X+ fixX, p1.Y+ fixY, p2.X+ fixX, p2.Y+ fixY);
                }
                /*
                //--------Рисование по граням (тест, что грани выделены правильно)---------
                List<Point3D> points = projection.Project2(curPolyhedron, projBox.SelectedIndex);

                foreach (List<int> face in curPolyhedron.Faces)
                {
                    pen = new Pen(Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)), 2);
                    foreach (var point1 in face)
                    {
                        foreach (var point2 in face)
                        {
                            var p1 = points[point1].ConvertToPoint();
                            var p2 = points[point2].ConvertToPoint();
                            graphics.DrawLine(pen, p1.X + centerX - figureCenterX, p1.Y + centerY - figureCenterY, p2.X + centerX - figureCenterX, p2.Y + centerY - figureCenterY);
                        }
                    }
                }*/
                //--------------------------------------------------------------------

                pictureBox1.Invalidate();
            }
        }

        private void DrawByFaces(List<List<int>> visibleFaces)
        {
            if (curPolyhedron.IsEmpty())
                return;
            ClearPictureBox();
            // graphics.Clear(Color.White);
            Random r = new Random();
            pen = new Pen(Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)), 2);
            List<Edge> edges = projection.Project(curPolyhedron, projBox.SelectedIndex);

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

            var fixX = centerX - figureCenterX + (figureLeftX < 0 ? Math.Abs(figureLeftX) : -Math.Abs(figureLeftX));
            var fixY = centerY - figureCenterY + (figureLeftY < 0 ? Math.Abs(figureLeftY) : -Math.Abs(figureLeftY));

            //--------Рисование по граням 
            List<Point3D> points = projection.Project2(curPolyhedron, projBox.SelectedIndex);
            pen = new Pen(Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)), 2);

            foreach (List<int> face in visibleFaces)
            {
                var p1 = points[face[0]].ConvertToPoint();
                var p2 = points[face[face.Count - 1]].ConvertToPoint();
                if (!NeedCentering.Checked)//Центрирование?
                    graphics.DrawLine(pen, p1.X + centerX - figureCenterX, p1.Y + centerY - figureCenterY, p2.X + centerX - figureCenterX, p2.Y + centerY - figureCenterY);
                else
                    graphics.DrawLine(pen, p1.X + fixX, p1.Y + fixY, p2.X + fixX, p2.Y + fixY);
                for (var i = 1; i < face.Count; i++)
                {
                    p1 = points[face[i - 1]].ConvertToPoint();
                    p2 = points[face[i]].ConvertToPoint();
                    if (!NeedCentering.Checked)//Центрирование?
                        graphics.DrawLine(pen, p1.X + centerX - figureCenterX, p1.Y + centerY - figureCenterY, p2.X + centerX - figureCenterX, p2.Y + centerY - figureCenterY);
                    else
                        graphics.DrawLine(pen, p1.X + fixX, p1.Y + fixY, p2.X + fixX, p2.Y + fixY);

                }
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

            curPolyhedron.AddFace(new List<int> { 3, 2, 1, 0 });
            curPolyhedron.AddFace(new List<int> { 1, 2, 6, 5 });
            curPolyhedron.AddFace(new List<int> { 0, 4, 7, 3 });
            curPolyhedron.AddFace(new List<int> { 7, 4, 5, 6 });
            curPolyhedron.AddFace(new List<int> { 2, 3, 7, 6 });
            curPolyhedron.AddFace(new List<int> { 0, 1, 5, 4 });

            scene.Add(curPolyhedron);
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

            curPolyhedron.AddFace(new List<int> { 0, 2, 1 });
            curPolyhedron.AddFace(new List<int> { 0, 1, 3 });
            curPolyhedron.AddFace(new List<int> { 0, 3, 2 });
            curPolyhedron.AddFace(new List<int> { 1, 2, 3 });

            scene.Add(curPolyhedron);
            Draw();
        }

        /// <summary>
        /// Очистить экран
        /// </summary>
        private void Clear_Click(object sender, EventArgs e)
        {
            curPolyhedron.Clear();
            scene.Clear();

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);

            pointsRotate.Clear();
            NeedCentering.Checked = false;
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

            curPolyhedron.AddFace(new List<int> { 0, 1, 3 });
            curPolyhedron.AddFace(new List<int> { 0, 4, 1 });
            curPolyhedron.AddFace(new List<int> { 0, 3, 2 });
            curPolyhedron.AddFace(new List<int> { 0, 2, 4 });
            curPolyhedron.AddFace(new List<int> { 1, 5, 3 });
            curPolyhedron.AddFace(new List<int> { 5, 1, 4 });
            curPolyhedron.AddFace(new List<int> { 3, 5, 2 });
            curPolyhedron.AddFace(new List<int> { 5, 4, 2 });

            scene.Add(curPolyhedron);
            Draw();
        }

        /// <summary>
        /// Создание икосаэдра
        /// </summary>
        private void createIcosahedron_Click(object sender, EventArgs e)
        {
            float r = 100 * (1 + (float)Math.Sqrt(5)) / 4; // радиус полувписанной окружности 

            List<Point3D> points = new List<Point3D>
            {
                new Point3D(0, -50, -r),
                new Point3D(0, 50, -r),
                new Point3D(50, r, 0),
                new Point3D(r, 0, -50),
                new Point3D(50, -r, 0),
                new Point3D(-50, -r, 0),
                new Point3D(-r, 0, -50),
                new Point3D(-50, r, 0),
                new Point3D(r, 0, 50),
                new Point3D(-r, 0, 50),
                new Point3D(0, -50, r),
                new Point3D(0, 50, r)
            };

            Polyhedron iko = new Polyhedron(points);

            iko.AddEdges(0, new List<int> { 1, 3, 4, 5, 6 });
            iko.AddEdges(1, new List<int> { 2, 3, 6, 7 });
            iko.AddEdges(2, new List<int> { 3, 7, 8, 11 });
            iko.AddEdges(3, new List<int> { 4, 8 });
            iko.AddEdges(4, new List<int> { 5, 8, 10 });
            iko.AddEdges(5, new List<int> { 6, 9, 10 });
            iko.AddEdges(6, new List<int> { 7, 9 });
            iko.AddEdges(7, new List<int> { 9, 11 });
            iko.AddEdges(8, new List<int> { 10, 11 });
            iko.AddEdges(9, new List<int> { 10, 11 });
            iko.AddEdges(10, new List<int> { 11 });

            curPolyhedron = iko;
            Draw();
        }

        /// <summary>
        /// Создание додекаэдра
        /// </summary>
        private void createDodecahedron_Click(object sender, EventArgs e)
        {
            float r = 100 * (3 + (float)Math.Sqrt(5)) / 4; // радиус полувписанной окружности 
            float x = 100 * (1 + (float)Math.Sqrt(5)) / 4; // половина стороны пятиугольника в сечении 

            List<Point3D> points = new List<Point3D>
            {
                new Point3D(0, -50, -r),
                new Point3D(0, 50, -r),
                new Point3D(x, x, -x),
                new Point3D(r, 0, -50),
                new Point3D(x, -x, -x),
                new Point3D(50, -r, 0),
                new Point3D(-50, -r, 0),
                new Point3D(-x, -x, -x),
                new Point3D(-r, 0, -50),
                new Point3D(-x, x, -x),
                new Point3D(-50, r, 0),
                new Point3D(50, r, 0),
                new Point3D(-x, -x, x),
                new Point3D(0, -50, r),
                new Point3D(x, -x, x),
                new Point3D(0, 50, r),
                new Point3D(-x, x, x),
                new Point3D(x, x, x),
                new Point3D(-r, 0, 50),
                new Point3D(r, 0, 50)
            };

            Polyhedron dode = new Polyhedron(points);

            dode.AddEdges(0, new List<int> { 1, 4, 7 });
            dode.AddEdges(1, new List<int> { 2, 9 });
            dode.AddEdges(2, new List<int> { 3, 11 });
            dode.AddEdges(3, new List<int> { 4, 19 });
            dode.AddEdges(4, new List<int> { 5 });
            dode.AddEdges(5, new List<int> { 6, 14 });
            dode.AddEdges(6, new List<int> { 7, 12 });
            dode.AddEdges(7, new List<int> { 8 });
            dode.AddEdges(8, new List<int> { 9, 18 });
            dode.AddEdges(9, new List<int> { 10 });
            dode.AddEdges(10, new List<int> { 11, 16 });
            dode.AddEdges(11, new List<int> { 17 });
            dode.AddEdges(12, new List<int> { 13, 18 });
            dode.AddEdges(13, new List<int> { 14, 15 });
            dode.AddEdges(14, new List<int> { 19 });
            dode.AddEdges(15, new List<int> { 16, 17 });
            dode.AddEdges(16, new List<int> { 18 });
            dode.AddEdges(17, new List<int> { 19 });

            //Affine.translate(dode, 200, 200, 0);
            curPolyhedron = dode;

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
                if (plane != "")
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
            if (curPolyhedron != null)
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

        }

        private void rotateOwn_Click(object sender, EventArgs e)
        {
            rotateOY.Checked = rotateOZ.Checked = rotateOX.Checked = false;
        }

        // добавить точку для фигуры вращения
        private void addPointButton_Click(object sender, EventArgs e)
        {
            float x = float.Parse(textBox6.Text);
            float y = float.Parse(textBox7.Text);
            float z = float.Parse(textBox8.Text);

            pointsRotate.Add(new Point3D(x, y, z));
            DrawCurve();
        }

        // рисует кривую по точкам для фигуры вращения
        private void DrawCurve()
        {
            graphics.Clear(Color.White);
            int startX = pictureBox1.Width / 2;
            int startY = pictureBox1.Height / 2;
            if (pointsRotate.Count > 1)
            {
                for (int i = 1; i < pointsRotate.Count; i++)
                {

                    graphics.DrawLine(new Pen(Color.Black), startX + pointsRotate[i - 1].ConvertToPoint().X,
                                                            startY + pointsRotate[i - 1].ConvertToPoint().Y,
                                                            startX + pointsRotate[i].ConvertToPoint().X,
                                                            startY + pointsRotate[i].ConvertToPoint().Y);
                }
            }
            pictureBox1.Invalidate();
        }

        // Нарисовать фигуру вращения
        private void drawFigureRotationButton_Click(object sender, EventArgs e)
        {
            int count = int.Parse(textBox5.Text); // количество разбиений
            string axis = comboBox2.Text;
            char axisF;
            if (axis == "Ось Oz")
                axisF = 'z';
            else if (axis == "Ось Oy")
                axisF = 'y';
            else axisF = 'x';

            curPolyhedron = RotateFigure.createPolyhedronForRotateFigure(pointsRotate, count, axisF);
            Draw();
        }

        delegate float func(float x, float y);
        /// <summary>
        /// Построить график
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Graphic(float X0, float X1, float Y0, float Y1, int countSplit, func f)
        {
            float dx = (X1 - X0) / countSplit;
            float dy = (Y1 - Y0) / countSplit;
            float currentX, currentY = Y0;

            List<Point3D> points = new List<Point3D>();

            // Добавляем точки
            for (int i = 0; i <= countSplit; ++i)
            {
                currentX = X0;
                for (int j = 0; j <= countSplit; ++j)
                {
                    points.Add(new Point3D(currentX, currentY, f(currentX, currentY)));
                    currentX += dx;
                }
                currentY += dy;
            }
            Polyhedron polyhedron = new Polyhedron(points);


            int N = countSplit + 1;

            // Добавляем ребра. Из каждой точки идет ребро вправо и вниз (таким образом делается сетка)
            for (int i = 0; i < N; ++i)
                for (int j = 0; j < N; ++j)
                {
                    if (j != N - 1)
                        polyhedron.AddEdge(i * N + j, i * N + j + 1); // вправо
                    if (i != N - 1)
                        polyhedron.AddEdge(i * N + j, (i + 1) * N + j); // вниз
                    if (j != N - 1 && i != N - 1)
                    {
                        // текущая точка, вправо от тек., вниз от тек., вправо и вниз от тек. образуют грань 
                        polyhedron.AddFace(new List<int> { i * N + j, i * N + j + 1, (i + 1) * N + (j + 1), (i + 1) * N + j});
                    }
                }
            Affine.scaleCenter(polyhedron, 40);
            Affine.rotateCenter(polyhedron, 60, 0, 0);

            curPolyhedron = polyhedron;
            scene.Add(curPolyhedron);
            pen.Width = 1;
            Draw();
        }
        private void DrawGraphic_Click(object sender, EventArgs e)
        {
            if (!float.TryParse(textBox12.Text, out float X0))
                X0 = -5;
            if (!float.TryParse(textBox11.Text, out float X1))
                X1 = 5;
            if (!float.TryParse(textBox10.Text, out float Y0))
                Y0 = -5;
            if (!float.TryParse(textBox9.Text, out float Y1))
                Y0 = 5;
            if (!int.TryParse(textBox13.Text, out int cnt))
                cnt = 10;

            func f;
            switch (graphicsList.SelectedIndex)
            {
                case 0:
                    f = (x, y) => (float)(Cos(x * x + y * y) / (x * x + y * y + 1));
                    break;
                case 1:
                    f = (x, y) => (float)(Sin(x + y));
                    break;
                case 2:
                    f = (x, y) => (float)(1 / (1 + x * x) + 1 / (1 + y * y));
                    break;
                case 3:
                    f = (x, y) => (float)(Sin(x * x + y * y));
                    break;
                case 4:
                    f = (x, y) => (float)(Sqrt(50 - x * x - y * y));
                    break;
                default:
                    f = (x, y) => 0;
                    break;
            }

            Graphic(X0, X1, Y0, Y1, cnt, f);
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog1.FileName;
                if (File.Exists(fName))
                {
                    curPolyhedron = JsonConvert.DeserializeObject<Polyhedron>(File.ReadAllText(fName, Encoding.UTF8));
                    scene.Add(curPolyhedron);
                    Draw();
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fName = saveFileDialog1.FileName;
                File.WriteAllText(fName, JsonConvert.SerializeObject(curPolyhedron, Formatting.Indented), Encoding.UTF8);
            }
        }

        private void NeedCentering_CheckedChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Draw();
        }


        private void ZBufferOn(List<Color> colors)
        {
            Bitmap bmp = ZBuffer.Z_buffer(pictureBox1.Width, pictureBox1.Height, scene, colors);
            pictureBox1.Image = bmp;
            pictureBox1.Invalidate();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
