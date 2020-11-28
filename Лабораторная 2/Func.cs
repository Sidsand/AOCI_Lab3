using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;

namespace Лабораторная_2
{
    class Func
    {
        public Image<Bgr, byte> sourceImage; //глобальная переменная

        PointF[] point = new PointF[4];
        int c = 0;

        public void Source(string fileName)
        {
            sourceImage = new Image<Bgr, byte>(fileName).Resize(640, 480, Inter.Linear);

        }

        public Image<Bgr, byte> Scale(double sX, double sY)
        {
            var resultImage = new Image<Bgr, byte>((int)(sourceImage.Width * sX), (int)(sourceImage.Height * sY));
            for (int x = 0; x < resultImage.Width - 1; x++)
            {
                for (int y = 0; y < resultImage.Height - 1; y++)
                {
                    double X = x / sX;
                    double Y = y / sY;
                    double baseX = Math.Floor(X);
                    double baseY = Math.Floor(Y);

                    if (baseX >= sourceImage.Width - 1 || baseY >= sourceImage.Height - 1) continue;

                    double rX = X - baseX;
                    double rY = Y - baseY;
                    double irX = 1 - rX;
                    double irY = 1 - rY;

                    Bgr c = new Bgr();
                    Bgr c1 = new Bgr();
                    Bgr c2 = new Bgr();

                    c1.Blue = sourceImage.Data[(int)baseY, (int)baseX, 0] * irX + sourceImage.Data[(int)baseY, (int)baseX + 1, 0] * rX;
                    c1.Green = sourceImage.Data[(int)baseY, (int)baseX, 1] * irX + sourceImage.Data[(int)baseY, (int)baseX + 1, 1] * rX;
                    c1.Red = sourceImage.Data[(int)baseY, (int)baseX, 2] * irX + sourceImage.Data[(int)baseY, (int)baseX + 1, 2] * rX;

                    c2.Blue = sourceImage.Data[(int)baseY + 1, (int)baseX, 0] * irX + sourceImage.Data[(int)baseY + 1, (int)baseX + 1, 0] * rX;
                    c2.Green = sourceImage.Data[(int)baseY + 1, (int)baseX, 0] * irX + sourceImage.Data[(int)baseY + 1, (int)baseX + 1, 0] * rX;
                    c2.Red = sourceImage.Data[(int)baseY + 1, (int)baseX, 0] * irX + sourceImage.Data[(int)baseY + 1, (int)baseX + 1, 0] * rX;

                    c.Blue = c1.Blue * irY + c2.Blue * rY;
                    c.Green = c1.Green * irY + c2.Green * rY;
                    c.Red = c1.Red * irY + c2.Red * rY;

                    resultImage[y, x] = c;

                }
            }

            return resultImage;
        }

        public Image<Bgr, byte> Shearing(double sX, double sY)
        {
            var resultImage = sourceImage.CopyBlank();

            for (int x = 0; x < resultImage.Width - 1; x++)
            {
                for (int y = 0; y < resultImage.Height - 1; y++)
                {

                    int newX = x + Convert.ToInt32(sX * (sourceImage.Height - y));
                    int newY = y + Convert.ToInt32(sY * x);

                    if (newX < resultImage.Width && newY < resultImage.Height && newX >= 0 && newY >= 0)
                    {
                        resultImage[newY, newX] = sourceImage[y, x];
                    }
                }
            }
            return resultImage;
        }

        public Image<Bgr, byte> Rotation(double angle, double centerX, double centerY)
        {
            angle = (Math.PI / 180) * angle;
            var resultImage = sourceImage.CopyBlank();

            for (int x = 0; x < resultImage.Width - 1; x++)
            {
                for (int y = 0; y < resultImage.Height - 1; y++)
                {
                    double newX = Convert.ToInt32(Math.Cos(angle) * (x - centerX) - Math.Sin(angle) * (y - centerY)) + centerX;
                    double newY = Convert.ToInt32(Math.Sin(angle) * (x - centerX) + Math.Cos(angle) * (y - centerY)) + centerY;

                    if (newX < resultImage.Width && newY < resultImage.Height && newX >= 0 && newY >= 0)
                    {
                        double X = newX;
                        double Y = newY;
                        double baseX = Math.Floor(X);
                        double baseY = Math.Floor(Y);

                        if (baseX >= sourceImage.Width - 1 || baseY >= sourceImage.Height - 1) continue;

                        double rX = X - baseX;
                        double rY = Y - baseY;
                        double irX = 1 - rX;
                        double irY = 1 - rY;


                        Bgr c1 = new Bgr
                        {
                            Blue = sourceImage.Data[(int)baseY, (int)baseX, 0] * irX + sourceImage.Data[(int)baseY, (int)baseX + 1, 0] * rX,
                            Green = sourceImage.Data[(int)baseY, (int)baseX, 1] * irX + sourceImage.Data[(int)baseY, (int)baseX + 1, 1] * rX,
                            Red = sourceImage.Data[(int)baseY, (int)baseX, 2] * irX + sourceImage.Data[(int)baseY, (int)baseX + 1, 2] * rX
                        };

                        Bgr c2 = new Bgr
                        {
                            Blue = sourceImage.Data[(int)baseY + 1, (int)baseX, 0] * irX + sourceImage.Data[(int)baseY + 1, (int)baseX + 1, 0] * rX,
                            Green = sourceImage.Data[(int)baseY + 1, (int)baseX, 0] * irX + sourceImage.Data[(int)baseY + 1, (int)baseX + 1, 0] * rX,
                            Red = sourceImage.Data[(int)baseY + 1, (int)baseX, 0] * irX + sourceImage.Data[(int)baseY + 1, (int)baseX + 1, 0] * rX
                        };

                        Bgr c = new Bgr
                        {
                            Blue = c1.Blue * irY + c2.Blue * rY,
                            Green = c1.Green * irY + c2.Green * rY,
                            Red = c1.Red * irY + c2.Red * rY
                        };

                        resultImage[y, x] = c;
                    }
                }
            }

            return resultImage;
        }

        public Image<Bgr, byte> Reflection(int qX, int qY)
        {
            var resultImage = sourceImage.CopyBlank();
            int newX, newY;

            for (int x = 0; x < resultImage.Width - 1; x++)
            {
                for (int y = 0; y < resultImage.Height - 1; y++)
                {

                    if (qX == -1)
                    {
                        newX = x * qX + sourceImage.Width - 1;
                    }
                        else
                        {
                            newX = x;
                        }

                    if (qY == -1)
                    {
                        newY = y * qY + sourceImage.Height - 1;
                    }
                        else
                        {
                            newY = y;
                        }

                    resultImage[newY, newX] = sourceImage[y, x];
                }
            }

            return resultImage;
        }

        public Image<Bgr, byte> Bill(int x, int y)
        {
            var imgCopy = sourceImage.Copy();

            point[c] = new Point(x, y);
            c++;
            if (c >= 4) c = 0;

            int radius = 2;
            int thickness = 2;
            var color = new Bgr(Color.Red).MCvScalar;

            for (int i = 0; i < 4; i++)
                // функция, рисующая на изображении круг с заданными параметрами
                CvInvoke.Circle(imgCopy, new Point((int)point[i].X, (int)point[i].Y), radius, color, thickness);

            return imgCopy;
        }

        public Image<Bgr, byte> Homography()
        {
            var resultImage = sourceImage.CopyBlank();

            var destPoints = new PointF[]
            {
                 new PointF(sourceImage.Width - 1, 0),
                 new PointF(0, 0),
                 new PointF(0, sourceImage.Height - 1),
                 new PointF(sourceImage.Width - 1, sourceImage.Height - 1)
            };
            var homographyMatrix = CvInvoke.GetPerspectiveTransform(point, destPoints);

            CvInvoke.WarpPerspective(sourceImage, resultImage, homographyMatrix, resultImage.Size);

            return resultImage;
        }
    }
}
