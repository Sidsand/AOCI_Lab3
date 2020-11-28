using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Лабораторная_2
{
    public partial class Form1 : Form
    {

        private Func func = new Func();

        public Form1()
        {
            InitializeComponent();
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                func.Source(fileName);

                imageBox1.Image = func.sourceImage;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double sX = double.Parse(textBox1.Text);
            double sY = double.Parse(textBox2.Text);

            imageBox2.Image = func.Scale(sX, sY);
        }

        private void imageBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = (int)(e.Location.X / imageBox1.ZoomScale);
            int y = (int)(e.Location.Y / imageBox1.ZoomScale);
            
            imageBox1.Image = func.Bill(x,y);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double sX = double.Parse(textBox4.Text);
            double sY = double.Parse(textBox3.Text);

            imageBox2.Image = func.Shearing(sX, sY);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double r = double.Parse(textBox5.Text);
            double pX = double.Parse(textBox6.Text);
            double pY = double.Parse(textBox7.Text);
            imageBox2.Image = func.Rotation(r, pX, pY);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int qX = int.Parse(textBox8.Text);
            int qY = int.Parse(textBox9.Text);
            imageBox2.Image = func.Reflection(qX, qY);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Homography();
        }
    }
}
