using System;
using System.Drawing;
using System.Windows.Forms;

namespace Fractal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int w = 800, h = 600, p, f_index;
        double cx, cy, wx, r;
        bool firstPaint = true;
        Bitmap bm = new Bitmap(800, 600);

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if(firstPaint)
            {
                this.comboBox1.SelectedIndex = 0;
                f_index = 0;
                cx = 0.0;
                cy = 0.0;
                wx = 8.0;
                r = wx / (double)w;
                Redraw();
                firstPaint = false;
            }
        }

        private void OnClick(object sender, MouseEventArgs e)
        {
            cx = cx + ((double)(e.X - w / 2) * r);
            cy = cy - ((double)(e.Y - h / 2) * r);
            Redraw();
        }

        private void OnZoomIn(object sender, EventArgs e)
        {
            wx = wx * 0.5;
            r = wx / (double)w;
            Redraw();

            this.button2.Enabled = true;
            if (wx <= 0.00000001)
            {
                this.button1.Enabled = false;
            }
        }

        private void OnZoomOut(object sender, EventArgs e)
        {
            wx = wx * 2.0;
            r = wx / (double)w;
            Redraw();

            this.button1.Enabled = true;
            if(wx >= 8.0)
            {
                this.button2.Enabled = false;
            }
        }

        private void OnFractalChanged(object sender, EventArgs e)
        {
            f_index = this.comboBox1.SelectedIndex;
            cx = 0.0;
            cy = 0.0;
            wx = 8.0;
            r = wx / (double)w;
            this.button1.Enabled = true;
            this.button2.Enabled = false;
            Redraw();
        }

        private void OnPrecisionChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        private void OnReset(object sender, EventArgs e)
        {
            cx = 0.0;
            cy = 0.0;
            wx = 8.0;
            r = wx / (double)w;
            this.button1.Enabled = true;
            this.button2.Enabled = false;
            Redraw();
        }

        private void Redraw()
        {
            int i, j;
            p = (int)this.numericUpDown1.Value;
            Graphics g = this.CreateGraphics();
            for (i = 0; i < w; i++)
            {
                for (j = 0; j < h; j++)
                {
                    if (IsInSet((double)(i - w / 2) * r + cx, cy - (double)(j - h / 2) * r) == p)
                    {
                        bm.SetPixel(i, j, Color.White);
                    }
                    else
                    {
                        bm.SetPixel(i, j, Color.Black);
                    }
                }
            }
            g.DrawImage(bm, 0, 0, w, h);
        }

        private int IsInSet(double c_re, double c_im)
        {
            int i;
            double z_re, z_im;
            z_re = 0;
            z_im = 0;
            i = 0;
            while(i < p)
            {
                if(f_index == 0)
                {
                    (z_re, z_im) = Mandelbrot(z_re, z_im, c_re, c_im);
                }
                else if(f_index == 1)
                {
                    (z_re, z_im) = Mandelbrot3(z_re, z_im, c_re, c_im);
                }
                else if (f_index == 2)
                {
                    (z_re, z_im) = Mandelbrot4(z_re, z_im, c_re, c_im);
                }
                
                if(z_re*z_re + z_im*z_im > 4)
                {
                    break;
                }
                else
                {
                    i++;
                }
            }
            return i;
        }

        private (double re, double im) Mandelbrot(double zn_re, double zn_im, double c_re, double c_im)
        {
            return (zn_re*zn_re - zn_im*zn_im + c_re, 2*zn_re*zn_im + c_im);
        }

        private (double re, double im) Mandelbrot3(double zn_re, double zn_im, double c_re, double c_im)
        {
            return (zn_re * zn_re * zn_re - 3 * zn_re * zn_im * zn_im + c_re, 3 * zn_re * zn_re * zn_im - zn_im * zn_im * zn_im + c_im);
        }

        private (double re, double im) Mandelbrot4(double zn_re, double zn_im, double c_re, double c_im)
        {
            return (zn_re * zn_re * zn_re * zn_re - 6 * zn_re * zn_re * zn_im * zn_im + zn_im * zn_im * zn_im * zn_im + c_re, 4 * zn_re * zn_re * zn_re * zn_im - 4 * zn_re * zn_im * zn_im * zn_im + c_im);
        }
    }
}
