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


        bool Equal(double x, double y, double eps = 0.001)
        => Math.Abs(x - y) < eps;

        //R, G, B —значения цвета в цветовой модели RGBв диапазоне [0; 1]
        //MAX—максимум из трёх значений (R, G, B)MIN—минимум из трёх значений (R, G, B)
        (double H, double S, double V) ConvertRGBtoHSV(double R, double G, double B)
        {
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

        (double R, double G, double B) ConvertHSVtoRGB(double H, double S, double V)
        {
            double Hi = Math.Floor(H / 60f) % 6;
            double f = H / 60f - Math.Floor(H / 60f);
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
        void test()
        {
            for (int i = 0; i < 100; ++i)
            {
                Random rnd = new Random();
                double R = rnd.NextDouble(), G = rnd.NextDouble(), B = rnd.NextDouble();

                // RGB -> HSV
                var res = ConvertRGBtoHSV(R, G, B);
                // HSV -> RGB
                var res1 = ConvertHSVtoRGB(res.H, res.S, res.V);
                // Проверяем, что в результате получили исходные значения
                Debug.Assert(Equal(res1.R, R));
                Debug.Assert(Equal(res1.G, G));
                Debug.Assert(Equal(res1.B, B));
                this.Text = ($"{R} {G} {B}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            test();
            // Исправить этот колхоз
            Bitmap myBitmap = new Bitmap(@"c:\Users\Dima\CompGraphicsLabs\CompGraphicsLab02\test.jpg");

            for (int i = 0; i < 10 /*myBitmap.Width*/; ++i)
                for (int j = 0; j < 10 /*myBitmap.Height*/; ++j)
                {
                    //  this.Text = $"{myBitmap.GetPixel(i, j).R}";
                }
        }
    }
}
