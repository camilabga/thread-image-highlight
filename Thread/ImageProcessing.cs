using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

namespace ImageProcessing
{
    public class ImageProcessing
    {
        public Bitmap AlargamentoContrasteImage;
        public Bitmap EqualizacaoHistogramaImage;
        private readonly int ImageWidth;
        private readonly int ImageHeight;
        public ImageProcessing(string filepath)
        {
            try
            {
                EqualizacaoHistogramaImage = new Bitmap(filepath);
                AlargamentoContrasteImage = new Bitmap(filepath);
                ImageWidth = AlargamentoContrasteImage.Width;
                ImageHeight = AlargamentoContrasteImage.Height;
            }
            catch (Exception exception)
            {
                Console.WriteLine("File not found");
                throw exception;
            }
        }
        public void ColorChange_gray()
        {
            int x, y;

            for (x = 0; x < ImageWidth; x++)
            {
                for (y = 0; y < ImageHeight; y++)
                {
                    Color pixelColor = AlargamentoContrasteImage.GetPixel(x, y);
                    int avg = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    Color grayColor = Color.FromArgb(avg, avg, avg);
                    AlargamentoContrasteImage.SetPixel(x, y, grayColor);
                }
            }
            AlargamentoContrasteImage.Save(@"C:\Users\Petch\source\repos\Thread\Image\grayImage.jpg");
        }
        public List<int> Max_min()
        {
            int[] values = { -1, 256 };
            List<int> max_min = new List<int>(values);
         
            for(int i = 0; i < ImageWidth; i++)
            {
                for(int j=0; j < ImageHeight; j++)
                {
                    Color pixel = AlargamentoContrasteImage.GetPixel(i, j);
                    if ((int)pixel.R > max_min[0]) max_min[0] = (int)pixel.R;
                    else if ((int)pixel.R < max_min[1]) max_min[1] = (int)pixel.R;
                }
            }
            return max_min;

        }

        public void AlargamentoContraste()
        {
            var max_min = Max_min();

            for (int i = 0; i < ImageWidth; i++)
            {
                for (int j = 0; j < ImageHeight; j++)
                {
                    Color Pixel = AlargamentoContrasteImage.GetPixel(i, j);
                    int Func_G = (255/ (max_min[0] - max_min[1]) )*( (int)Pixel.R - max_min[1]);
                    Color ContrasteColor = Color.FromArgb(Func_G, Func_G, Func_G);
                    AlargamentoContrasteImage.SetPixel(i, j, ContrasteColor);
                }

            }
            AlargamentoContrasteImage.Save(@"C:\Users\Petch\source\repos\Thread\Image\AlargamentoContraste.png");

        }
        public int[] Histograma()
        {
            int[] Hist = new int[256];
            for (int i = 0; i < ImageWidth; i++)
            {
                for (int j = 0; j < ImageHeight; j++)
                {
                    Color Pixel = EqualizacaoHistogramaImage.GetPixel(i, j);
                    Hist[(int)Pixel.R] += 1;
                }
            }
            return Hist;
        }
        public double[] ProbabilidadeOcorrencia()
        {
            double[] Prob = new double[256];
            int i = 0;
            var Hist = Histograma();
            foreach (int value in Hist)
            {
                Prob[i] = (double)value / (EqualizacaoHistogramaImage.Width * EqualizacaoHistogramaImage.Height);
                i++;
            }
            return Prob;
        }
        public double[] ProbabilidadeAcumulada()
        {
            double[] Prob = ProbabilidadeOcorrencia();
            double[] ProbAcumulado = new double[256];
            for (int i =0; i < 256; i++)
            {
                double cont = 0;
                for(int j= 0; j < i; j++)
                {
                    cont += Prob[j];
                }
                ProbAcumulado[i] = cont;
            }
            return ProbAcumulado;
        }

        public void EqualizacaoHistograma()
        {
            var ProbAcumulado = ProbabilidadeAcumulada();
            for (int i = 0; i < ImageWidth; i++)
            {
                for (int j = 0; j < ImageHeight; j++)
                {
                    Color Pixelget = EqualizacaoHistogramaImage.GetPixel(i, j);
                    int pixel = (int)Math.Round(ProbAcumulado[Pixelget.R] * 255);
                    Color Pixelset = Color.FromArgb(pixel, pixel, pixel);
                    EqualizacaoHistogramaImage.SetPixel(i, j, Pixelset);
                }
            }
            EqualizacaoHistogramaImage.Save(@"C:\Users\Petch\source\repos\Thread\Image\EqualizacaoHistograma.png");
        }
    }
}
