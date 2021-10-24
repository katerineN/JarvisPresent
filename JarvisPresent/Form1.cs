using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JarvisPresent
{
    public partial class Form1 : Form
    {
        private Bitmap bitmap;
        private Graphics gr;
        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bitmap;
            gr = Graphics.FromImage(bitmap);
        }

        //Очистить
        private void button2_Click(object sender, EventArgs e)
        {
            gr.Clear(Color.White);
            pictureBox1.Invalidate();
        }
    }
}