using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Drawing2D;

namespace Light_Dots
{
    public partial class Form1 : Form
    {
        Thread myThread;
        int PosX = Cursor.Position.X;
        int PosY = Cursor.Position.Y;
        double d = 0;
        int StarsNum = 4500;
        int radius;
        int radiusK = 4;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            for(int i = 0; i < StarsNum; i++)
                dots.Add(new LDot(rnd.Next(0, picture.Width), rnd.Next(0, picture.Height)));
            for (int i = 0; i < StarsNum / 100; i++)
                Bigdots.Add(new LDot(rnd.Next(0, picture.Width), rnd.Next(0, picture.Height)));
            radius = picture.Height / radiusK;

            myThread = new Thread(Run);
            myThread.Start();
        }


        List<LDot> dots = new List<LDot>();
        List<LDot> Bigdots = new List<LDot>();

        public void Run()
        {
            reDrow();

            while (true)
            {
                textBox1.Invoke((MethodInvoker)(() => textBox1.Text = PointToClient(Cursor.Position).X.ToString()));
                textBox2.Invoke((MethodInvoker)(() => textBox2.Text = PointToClient(Cursor.Position).Y.ToString()));
                PosX = Convert.ToInt32(textBox1.Text);
                PosY = Convert.ToInt32(textBox2.Text);

                Random rnd = new Random();
                int R = 0;
                int G = 0;
                double k = 0;
                double kk = 0;

                foreach (LDot s in dots)
                {
                    d = Math.Pow(PosX - s.X, 2) + Math.Pow(PosY - s.Y, 2);
                    d = Math.Sqrt(d);

                    R = rnd.Next(0, 255);
                    G = rnd.Next(0, 255);
                    Color с = Color.FromArgb(255, R, G, 255);
                    kk = 1 - Math.Sin(d / radius);

                    if (d < radius)
                        s.ChengColor(Color.FromArgb(с.A, (int)(с.R * kk), (int)(с.G * kk), (int)(с.B * kk)));
                    else
                        s.ChengColor(picture.BackColor);
                }

                foreach (LDot s in Bigdots)
                {
                    d = Math.Pow(PosX - s.X, 2) + Math.Pow(PosY - s.Y, 2);
                    d = Math.Sqrt(d);
                    kk = 1 - Math.Sin(d / radius);
                    k = Convert.ToDouble(rnd.Next(70, 100)) / 100;

                    if (d < radius)
                        s.ChengColor(Color.FromArgb((int)(s.C.A * k), (int)(s.C.R * k * kk), (int)(s.C.G * k * kk), (int)(s.C.B * k * kk)));
                    else
                        s.ChengColor(picture.BackColor);
                }

                reDrow();
            }
        }


        public void reDrow()
        {
            Bitmap bmp = new Bitmap(picture.Width, picture.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(picture.BackColor);
            try
            {
                foreach (LDot s in dots)
                {
                    g.FillRectangle(s.Brush, s.X, s.Y, 1, 1);
                }
                foreach (LDot s in Bigdots)
                {
                    g.FillEllipse(s.Brush, s.X, s.Y, 3, 3);
                }
            }
            catch { }
            picture.Invoke((MethodInvoker)(() => picture.Image = bmp));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myThread.Abort();
        }

        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            myThread.Abort();
            radius = picture.Height / radiusK;

            Random rnd = new Random();
            dots.Clear();
            Bigdots.Clear();
            for (int i = 0; i < StarsNum; i++)
                dots.Add(new LDot(rnd.Next(0, picture.Width), rnd.Next(0, picture.Height)));
            for (int i = 0; i < StarsNum / 100; i++)
                Bigdots.Add(new LDot(rnd.Next(0, picture.Width), rnd.Next(0, picture.Height)));
            
            myThread = new Thread(Run);
            myThread.Start();
        }
    }
}

public class LDot
{
    public int X, Y;
    public Color C = Color.White;
    public SolidBrush Brush = new SolidBrush(Color.Blue);

    public LDot(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void ChengColor(Color c)
    {
        Brush = new SolidBrush(c);
    }
}