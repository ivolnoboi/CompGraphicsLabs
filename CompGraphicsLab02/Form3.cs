using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompGraphicsLab02
{
    public partial class Form3 : Form
    {
        private Form1 _form1;

        public Form3(Form1 form1)
        {
            _form1 = form1;
            InitializeComponent();
        }

        // Вернуться в меню (к выбору задания)
        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            _form1.Visible = true;
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Открыть изображение
        private void button2_Click(object sender, EventArgs e)
        {
            label4.Text = "";
            OpenFileDialog ofd = new OpenFileDialog();
            // маска для типа файлов
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(ofd.FileName);
                    pictureBox2.Image = null;
                    pictureBox3.Image = null;
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Получение канала R
        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap input = new Bitmap(pictureBox1.Image);
                var img = GetRgbChannels(input, "R");
                pictureBox2.Image = img;
                string maxS;
                pictureBox3.Image = DrawHistogram(img, "R", pictureBox3.Width, pictureBox3.Height, out maxS);
                label4.Text = maxS;
            }
        }

        // Получение канала G
        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap input = new Bitmap(pictureBox1.Image);
                var img = GetRgbChannels(input, "G");
                pictureBox2.Image = img;
                string maxS;
                pictureBox3.Image = DrawHistogram(img, "G", pictureBox3.Width, pictureBox3.Height, out maxS);
                label4.Text = maxS;
            }
        }
        //Получение канала B
        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap input = new Bitmap(pictureBox1.Image);
                var img = GetRgbChannels(input, "B");
                pictureBox2.Image = img;
                string maxS;
                pictureBox3.Image = DrawHistogram(img, "B", pictureBox3.Width, pictureBox3.Height, out maxS);
                label4.Text = maxS;
            }
        }

        static private Bitmap GetRgbChannels(Bitmap source, string channel)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);
            switch (channel)
            {
                case "R":
                    for (int i = 0; i < source.Width; i++)
                        for (int j = 0; j < source.Height; j++)
                        {
                            Color color = source.GetPixel(i, j);
                            result.SetPixel(i, j, Color.FromArgb(color.A, color.R, 0, 0));
                        }
                    break;
                case "G":
                    for (int i = 0; i < source.Width; i++)
                        for (int j = 0; j < source.Height; j++)
                        {
                            Color color = source.GetPixel(i, j);
                            result.SetPixel(i, j, Color.FromArgb(color.A, 0, color.G, 0));
                        }
                    break;
                case "B":
                    for (int i = 0; i < source.Width; i++)
                        for (int j = 0; j < source.Height; j++)
                        {
                            Color color = source.GetPixel(i, j);
                            result.SetPixel(i, j, Color.FromArgb(color.A, 0, 0, color.B));
                        }
                    break;
                default:
                    break;
            }
            return result;
        }

        public static Bitmap DrawHistogram(Bitmap image, string channel, int boxWidth, int boxHeight, out string maxS)
        {
            // ширина и высота входного изображения
            int width = image.Width, height = image.Height;
            Bitmap hist = new Bitmap(boxWidth, boxHeight);

            //массив, для хранения количества повторений каждого из значений каналов
            int[] arr = new int[256];

            Color color;
            // получаем количество повторений каждого из значений канала
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                {
                    color = image.GetPixel(i, j);
                    if (channel == "R")
                    {
                        arr[color.R]++;
                    }
                    else if (channel == "G")
                    {
                        arr[color.G]++;
                    }
                    else
                    {
                        arr[color.B]++;
                    }
                }

            // определяем коэффициент масштабирования по высоте
            int max = 0;
            for (int i = 0; i < 256; ++i)
            {
                if (arr[i] > max)
                    max = arr[i];
            }
            // выводим максимальное значение на график
            maxS = max.ToString();
            // коэффициент масштабирования
            double point = (double)max / boxHeight;

            // рисуем гистограмму
            Color histColor;
            if (channel == "R")
            {
                histColor = Color.Red;
            }
            else if (channel == "G")
            {
                histColor = Color.Green;
            }
            else if (channel == "B")
            {
                histColor = Color.Blue;
            }
            else
            {
                histColor = Color.Black;
            }
            for (int i = 0; i < 256; ++i)
            {
                for (var j = boxHeight - 1; j > boxHeight - arr[i] / point; --j)
                {
                    hist.SetPixel(i, j, histColor);
                }
            }
            return hist;
        }

        /*
        static Bitmap[] GetRgbChannels(Bitmap source)
        {
            Bitmap[] result = new Bitmap[3];
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMatrix[] matrices = new ColorMatrix[3];
            for (int i = 0; i < matrices.Length; i++)
            {
                float[][] elements ={
                new float[]{i == 0 ? 1 : 0, 0, 0, 0, 0},
                new float[]{0, i == 1 ? 1 : 0, 0, 0, 0},
                new float[]{0, 0, i == 2 ? 1 : 0, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 0}
                };
                matrices[i] = new ColorMatrix(elements);
            }
            int w = source.Width, h = source.Height;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Bitmap(source);
                imageAttributes.ClearColorMatrix();
                imageAttributes.SetColorMatrix(matrices[i], ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                using (Graphics g = Graphics.FromImage(result[i]))
                {
                    g.DrawImage(result[i], new Rectangle(0, 0, w, h), 0, 0, w, h, GraphicsUnit.Pixel, imageAttributes);
                }
            }
            return result;
        }*/
    }
}
