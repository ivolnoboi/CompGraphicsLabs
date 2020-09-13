using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompGraphicsLab02
{
    public partial class Form4 : Form
    {
        private Form1 _form1;
        public Form4(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();
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

        /// <summary>
        /// Сравнение на равенство двух вещественных чисел
        /// </summary>
        bool Equal(double x, double y, double eps = 0.001)
        => Math.Abs(x - y) < eps;


        /// <summary>
        /// Преобразование из RGB в HSV
        /// </summary>
        /// <param name="R">Красный (от 0 до 1)</param>
        /// <param name="G">Зеленый (от 0 до 1)</param>
        /// <param name="B">Голубой (от 0 до 1)</param>
        /// <returns>(Тон от 0 до 360, Насыщенность от 0 до 1, Яркость от 0 до 1)</returns>
        (double H, double S, double V) ConvertRGBtoHSV(double R, double G, double B)
        {
            //R, G, B — значения цвета в цветовой модели RGB в диапазоне [0; 1]
            // MAX — максимум из трёх значений (R, G, B)
            // MIN — минимум из трёх значений (R, G, B)
            double Max = Math.Max(Math.Max(R, G), B);
            double Min = Math.Min(Math.Min(R, G), B);
            double diff = Max - Min;

            // Hue - оттенок
            double H;
            if (Equal(Max, Min))
                H = 0;
            else
            if (Equal(Max, R) && G >= B)
                H = 60 * (G - B) / diff;
            else
            if (Equal(Max, R) && G < B)
                H = 60 * (G - B) / diff + 360;
            else
            if (Equal(Max, G))
                H = 60 * (B - R) / diff + 120;
            else
                //if (Equal(Max, B))
                H = 60 * (R - G) / diff + 240;

            double S = Equal(Max, 0) ? 0 : (1 - Min / Max);

            double V = Max;

            return (H, S, V);
        }

        /// <summary>
        /// Преобразование из HSV в RGB
        /// </summary>
        /// <param name="H">Тон (от 0 до 360)</param>
        /// <param name="S">Насыщенность (от 0 до 1)</param>
        /// <param name="V">Яркость (от 0 до 1)</param>
        /// <returns>(Red, Green, Blue) от 0 до 1</returns>
        (double R, double G, double B) ConvertHSVtoRGB(double H, double S, double V)
        {
            double Hi = Math.Floor(H / 60.0) % 6;
            double f = H / 60.0 - Math.Floor(H / 60.0);
            double p = V * (1 - S);
            double q = V * (1 - f * S);
            double t = V * (1 - (1 - f) * S);

            switch (Hi)
            {
                case 0:
                    return (V, t, p);
                case 1:
                    return (q, V, p);
                case 2:
                    return (p, V, t);
                case 3:
                    return (p, q, V);
                case 4:
                    return (t, p, V);
                case 5:
                    return (V, p, q);
                default:
                    return (0, 0, 0);
            }
        }
        /// <summary>
        /// Тестирование правильность перевода RGB -> HSV
        /// </summary>
        void Test()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 10000; ++i)
            {
                double R = rnd.NextDouble(), G = rnd.NextDouble(), B = rnd.NextDouble();

                // RGB -> HSV
                var res = ConvertRGBtoHSV(R, G, B);
                // HSV -> RGB
                var res1 = ConvertHSVtoRGB(res.H, res.S, res.V);

                // Проверяем, что в результате получили исходные значения
                Debug.Assert(Equal(res1.R, R));
                Debug.Assert(Equal(res1.G, G));
                Debug.Assert(Equal(res1.B, B));
            }
            MessageBox.Show("Тесты пройдены");
        }

        void Draw()
        {
            // Изменение значение H, S, V
            double dH = trackBarH.Value;
            double dS = trackBarS.Value / 100.0;
            double dV = trackBarV.Value / 100.0;

            Bitmap myBitmap = (Bitmap)picture.Clone();

            for (int i = 0; i < myBitmap.Width; ++i)
                for (int j = 0; j < myBitmap.Height; ++j)
                {
                    // Чтение пикселя
                    var px = myBitmap.GetPixel(i, j);
                    // Преобразование в HSV
                    var res = ConvertRGBtoHSV(px.R / 255.0, px.G / 255.0, px.B / 255.0);

                    // Изменение тона (при превышении 360 зацикливаем)
                    res.H += dH;
                    res.H = Math.Abs(res.H % 360);

                    // Изменение насыщенность (обрезаем при выходе за [0; 1])
                    res.S += dS;
                    res.S = Math.Min(1, res.S);
                    res.S = Math.Max(0, res.S);

                    // Изменение яркости (обрезаем при выходе за [0; 1])
                    res.V += dV;
                    res.V = Math.Min(1, res.V);
                    res.V = Math.Max(0, res.V);

                    // Для вывода на экран и сохранения в файл преобразуем обратно в RGB
                    var outres = ConvertHSVtoRGB(res.H, res.S, res.V);

                    myBitmap.SetPixel(i, j,
                        Color.FromArgb(px.A, (int)(outres.R * 255), (int)(outres.G * 255), (int)(outres.B * 255)));
                }
            // Сохраняем в файл и показываем результат
            pictureBox1.Image = myBitmap;
            myBitmap.Save("result.jpg");
        }
        private void button2_Click(object sender, EventArgs e)
        {
           // Test();
            Draw();
        }
        Bitmap picture;
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                // Маска для файлов
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    picture = new Bitmap(ofd.FileName);
                    pictureBox1.Image = picture;
                    button2.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

    }
}
