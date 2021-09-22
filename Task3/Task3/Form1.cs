using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task3
{
    public partial class Form1 : Form
    {
        Bitmap picture;
        Bitmap changedPicture;
        double currH = 0;
        double currS = 0;
        double currV = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                picture = new Bitmap(Image.FromFile(ofd.FileName), newSize: pictureBox1.Size);
                pictureBox1.Image = picture;
                changedPicture = (Bitmap)picture.Clone();
                pictureBox2.Image = changedPicture;
                trackH.Enabled = true;
                trackS.Enabled = true;
                trackV.Enabled = true;
                button2.Enabled = true;
            
                trackH.Value = 0;
                trackS.Value = 0;
                trackV.Value = 0;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currH = trackH.Value * 1.8;
            Convert(currH, currS, currV);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            currS = trackS.Value / 100.0;
            Convert(currH, currS, currV);
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            currV = trackV.Value / 100.0;
            Convert(currH, currS, currV);
        }
        bool Equal(double x, double y)
        { 
           return Math.Abs(x - y) < 0.001;
        }
        
        (double H, double S, double V) RGBtoHSV(double R, double G, double B)
        {
            double max = Math.Max(R, Math.Max(G, B)) * 1.0;
            double min = Math.Min(R, Math.Min(G, B)) * 1.0;
            double H = 0.0, S = 0.0, V = 0.0;
            if (Equal(max, min))
            {
                H = 0.0;
            }
            else
            {
                if (Equal(max, R) && G >= B)
                {
                    H = 60.0 * ((G - B) / (max - min)) + 0.0;
                }
                else
                if (Equal(max, R) && G < B)
                {
                    H = 60.0 * ((G - B) / (max - min)) + 360.0;
                }
                else
                if (Equal(max, G))
                {
                    H = 60.0 * ((B - R) / (max - min)) + 120.0;
                }
                else
                if (Equal(max, B))
                {
                    H = 60.0 * ((R - G) / (max - min)) + 240.0;
                }
            }
            if (!Equal(max, 0.0))
            {
                S = 1.0 - (min / max);
            }
            V = max;
            return (H, S, V);
        }

        (double R, double G, double B) HSVtoRGB(double H, double S, double V)
        {
            double Hi = Math.Floor(H / 60.0) % 6;
            double f = H / 60.0 - Math.Floor(H / 60.0);
            double p = V * (1 - S) * 1.0;
            double q = V * (1 - f * S) * 1.0;
            double t = V * (1 - (1 - f) * S) * 1.0;

            if (Equal(Hi,0))
            {
                return (V, t, p);
            }
            if (Equal(Hi, 1))
            {
                return (q, V, p);
            }
            if (Equal(Hi, 2))
            {
                return (p, V, t);
            }
            if (Equal(Hi, 3))
            {
                return (p, q, V);
            }
            if (Equal(Hi, 4))
            {
                return (t, p, V);
            }
            if (Equal(Hi, 5))
            {
                return (V, p, q);
            }
            return (0, 0, 0);
        }

        void Convert(double H, double S, double V)
        {
            changedPicture = (Bitmap)picture.Clone();
            for (int i = 0; i < changedPicture.Width; ++i)
            {
                for (int j = 0; j < changedPicture.Height; ++j)
                {
                    var pixel = changedPicture.GetPixel(i, j);
                    var hsl = RGBtoHSV(pixel.R / 255.0, pixel.G / 255.0, pixel.B / 255.0);
                    var newH = hsl.H;
                    var newS = hsl.S;
                    var newV = hsl.V;
                    newH += H * 1.0;
                    newS += S * 1.0;
                    newV += V * 1.0;
                    if (newH > 360)
                        newH -= 360;
                    if (newH < 0)
                        newH += 360;
                    newS = Math.Min(1, newS);
                    newS = Math.Max(0, newS);
                    newV = Math.Min(1, newV);
                    newV = Math.Max(0, newV);
                    var newRGB = HSVtoRGB(newH, newS, newV);
                    changedPicture.SetPixel(i, j, Color.FromArgb(pixel.A, (int)(newRGB.R * 255.0), (int)(newRGB.G * 255.0), (int)(newRGB.B * 255.0)));
                }
            }
            pictureBox2.Image = changedPicture;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            var path = "";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                path = fbd.SelectedPath;
            }
            changedPicture.Save(path + "\\result.png");
        }
    }
}
