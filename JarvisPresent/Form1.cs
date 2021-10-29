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
        private Pen myPen;
        private Pen pointPen;
        private List<Point> simple; // вершины примитива
        private List<Point> convexHull;
        private int drawing = 1; //пустой рисунок
        private Point start; //стартовая точка
        public Form1()
        {
            InitializeComponent();
            simple = new List<Point>();
            convexHull = new List<Point>();
            myPen = new Pen(Color.Black);
            myPen.Width = 1.5f;
            pointPen = new Pen(Color.Red);
            pointPen.Width = 1.5f;
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bitmap;
            gr = Graphics.FromImage(bitmap);
        }

        //Очистить
        private void button2_Click(object sender, EventArgs e)
        {
            gr.Clear(Color.White);
            drawing = 1;
            start = new Point();
            simple.Clear();
            convexHull.Clear();
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawing == 0) //нарисован примитив
            {
                MessageBox.Show("Для начала очистите нарисованное", "Ошибка");
            }

            if (drawing == 1) //рисунок пустой
            {
                simple.Add(new Point(e.X, e.Y));
                gr.DrawRectangle(myPen, e.X, e.Y, 1, 1);
                pictureBox1.Image = bitmap;
            }
            pictureBox1.Invalidate();
        }

        //поиск стартовой точки
        void FindStart()
        {
            int minX = Int32.MaxValue;
            foreach (Point p in simple)
            {
                if (p.X < minX)
                {
                    minX = p.X;
                    start = p;
                }
            }
        }
        
        //косинус
        double Scalar(Point p1, Point p2, Point p)
        {
            //gr.DrawLine(myPen, p1,p2);
            //gr.DrawLine(myPen, p2,p);
            pictureBox1.Image = bitmap;
            pictureBox1.Invalidate();
            var dx1 = p1.X - p2.X;
            var dy1 = p1.Y - p2.Y;
            var dx2 = p.X - p2.X;
            var dy2 = p.Y - p2.Y;
            double scal = dx1 * dx2 + dy1 * dy2;
            var len1 = Math.Sqrt(dx1 * dx1 + dy1 * dy1);
            var len2 = Math.Sqrt(dx2 * dx2 + dy2 * dy2);
            return scal / (len1 * len2);
        }

        //соединяем выпуклую оболочку
        void PrintConvexHull(List<Point> t)
        {
            for (int i = 0; i < t.Count - 1; i++)
            {
                gr.DrawLine(myPen, t[i], t[i+1]);
            }
            gr.DrawLine(myPen, t[1], t[t.Count - 1]);
            pictureBox1.Image = bitmap;
            pictureBox1.Invalidate();
        }

        //поиск точек выпуклоц оболочки
        void FindConvexHull()
        {
            List<Point> temp = simple;
            gr.DrawRectangle(pointPen, start.X, start.Y, 1, 1);

            Point current = start;
            Point end = start;
            int minY = pictureBox1.Height, minX = pictureBox1.Width;
            for (int i = 0; i < temp.Count; i++)
            {
                var t = temp[i];
                if (t.X >= start.X && t.Y >= start.Y &&
                    Math.Abs(t.X - start.X) < minX && Math.Abs(t.Y - start.Y) < minY && t != start)
                {
                    end = t;
                    minX = Math.Abs(t.X - start.X);
                    minY = Math.Abs(t.Y - start.Y);
                }
            }
            gr.DrawRectangle(pointPen, current.X, current.Y, 3, 3);
            gr.DrawRectangle(pointPen, end.X, end.Y, 3, 3);
            pictureBox1.Image = bitmap;
            pictureBox1.Invalidate();
            temp.Remove(end);
            temp.Remove(start);
            temp.Add(start);
            convexHull.Add(end);
            do
            {
                double minCos = Double.MaxValue;
                Point min = temp[0];
                convexHull.Add(current);
                gr.DrawRectangle(pointPen, current.X, current.Y, 3, 3);

                for (int i = 0; i < temp.Count; i++)
                {
                    double cos = Scalar(current, end, temp[i]);
                    //richTextBox1.Text += cos.ToString() + "\n";
                    /*if ((cos + 1) <= 0.1 || Double.IsNaN(cos))
                    {
                        continue;
                    }*/

                    if (cos <= minCos)
                    {
                        minCos = cos;
                        min = temp[i];
                    }
                }
                current = end;
                end = min;
                temp.Remove(end);
                pictureBox1.Image = bitmap;
                pictureBox1.Invalidate();
            } while (end != start);
            convexHull.Add(current);
            gr.DrawRectangle(pointPen, current.X, current.Y, 3, 3);
            pictureBox1.Image = bitmap;
            pictureBox1.Invalidate();
            PrintConvexHull(convexHull);
        }
        
        //построить выпуклую оболочку
        private void button1_Click(object sender, EventArgs e)
        {
            if (simple.Count == 0) 
            {
                MessageBox.Show("Для начала нарисуйте точки", "Ошибка");
            }
            if (drawing == 0) //нарисован примитив
            {
                MessageBox.Show("Для начала очистите нарисованное", "Ошибка");
            }
            drawing = 0; //примитив нарисован
            FindStart();
            FindConvexHull();
        }
    }
}