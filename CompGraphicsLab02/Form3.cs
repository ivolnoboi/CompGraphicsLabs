using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                pictureBox2.Image = GetRgbChannels(input)[0];
            }
        }

        static Bitmap[] GetRgbChannels(Bitmap source)
        {
            Bitmap[] result = new Bitmap[3] { new Bitmap(source.Width, source.Height), new Bitmap(source.Width, source.Height), new Bitmap(source.Width, source.Height) };
            for (int i = 0; i < source.Width; i++)
            {
                for (int j = 0; j < source.Height; j++)
                {
                    Color color = source.GetPixel(i, j);
                    result[0].SetPixel(i, j, Color.FromArgb(color.A, color.R, 0, 0));
                    result[1].SetPixel(i, j, Color.FromArgb(color.A, 0, color.G, 0));
                    result[2].SetPixel(i, j, Color.FromArgb(color.A, 0, 0, color.B));
                }
            }
            return result;
        }
        // Получение канала G
        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap input = new Bitmap(pictureBox1.Image);
                pictureBox2.Image = GetRgbChannels(input)[1];
            }
        }
        //Получение канала B
        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap input = new Bitmap(pictureBox1.Image);
                pictureBox2.Image = GetRgbChannels(input)[2];
            }
        }
    }
}
