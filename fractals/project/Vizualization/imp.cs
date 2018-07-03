using System;
using System.Data;
using System.Drawing;

namespace ImageProcessing
{
    public class Grayscale
    {
        public Bitmap ImageGrayscale(Bitmap Picture)
        {
            Bitmap result = new Bitmap(Picture.Width, Picture.Height);
            Color PixelColor;
            int GrayValue;
            for (int i = 0; i < Picture.Width; i++)
            {
                for (int j = 0; j < Picture.Height; j++)
                {
                    PixelColor = Picture.GetPixel(i, j);
                    GrayValue = Convert.ToInt32(0.3f * PixelColor.R + 0.6f * PixelColor.G + 0.1f * PixelColor.B);
                    result.SetPixel(i, j, Color.FromArgb(GrayValue, GrayValue, GrayValue));
                }    
            }    
            return result;
        }
    }

    public class Blur
    {
        public Bitmap BlureMore (Bitmap Picture)
        {

            Bitmap ResultImage = new Bitmap(Picture, Picture.Size.Width, Picture.Size.Height);
            int temporary;

            int[,] TransformationArray = new int [Picture.Size.Width, Picture.Size.Height];

            for (int i = 0; i < Picture.Size.Width; i++)
            {
                for (int j = 0; j < Picture.Size.Height; j++)
                {
                    TransformationArray[i, j] = Picture.GetPixel(i, j).R;
                }    
            }

            for (int i = 1; i < Picture.Size.Width - 1; i++)
            {
                for (int j = 1; j < Picture.Size.Height - 1; j++)
                {
                    temporary = TransformationArray[i - 1, j - 1]+TransformationArray[i - 1, j] +TransformationArray[i - 1, j + 1] +
                                TransformationArray[i, j - 1] +2 * TransformationArray[i, j] +TransformationArray[i, j + 1] +
                                TransformationArray[i + 1, j - 1] +TransformationArray[i + 1, j] +TransformationArray[i + 1, j + 1];
                    temporary /= 10;

                    ResultImage.SetPixel(i, j, Color.FromArgb(temporary, temporary, temporary));
                }
               
            }
            return ResultImage;
           
        }
    }

    public class Borders
    {

        public Bitmap GetBorders(Bitmap Picture)
        {

            Bitmap ResultImage = new Bitmap(Picture.Width, Picture.Height);
            int[,] Temporary = new int[Picture.Width, Picture.Height];
            Color result_color;
            byte pixel_taken;
            //int result_temp;
            int temp1, temp2;
            long sum=0;
            

            for (int i = 0; i < Picture.Width; i++)
            {
                for (int j = 0; j < Picture.Height; j++)
                {
                    Temporary[i, j] = Picture.GetPixel(i, j).R;
                }
            }

            for (int i = 1; i < Picture.Width - 1; i++)
            {
                for (int j = 1; j < Picture.Height - 1; j++)
                {

                    temp1 = (int)((-Temporary[i - 1, j - 1] - 2 * Temporary[i, j - 1] - Temporary[i + 1, j - 1] + Temporary[i - 1, j + 1] + 2 * Temporary[i, j + 1] + Temporary[i + 1, j + 1]) / 6);//h1
                    temp2 = (int)((-Temporary[i - 1, j - 1] - 2 * Temporary[i - 1, j] - Temporary[i - 1, j + 1]+ Temporary[i + 1, j - 1] + 2 * Temporary[i + 1, j] + Temporary[i + 1, j + 1]) / 6);//h2
                    result_color = Color.FromArgb((int)Math.Sqrt((temp1 * temp1 + temp2 * temp2) / 2), (int)Math.Sqrt((temp1 * temp1 + temp2 * temp2) / 2), (int)Math.Sqrt((temp1 * temp1 + temp2 * temp2) / 2));
                    ResultImage.SetPixel(i, j, result_color);
                    sum += (int)Math.Sqrt((temp1 * temp1 + temp2 * temp2) / 2);
                }

            }

            
            for (int i = 1; i < Picture.Width - 1; i++)
            {
                for (int j = 1; j < Picture.Height - 1; j++)
                {
                    pixel_taken = ResultImage.GetPixel(i, j).R;
                    if (pixel_taken > sum / (Picture.Width * Picture.Height))
                        ResultImage.SetPixel(i, j, Color.FromArgb(pixel_taken, pixel_taken, pixel_taken));
                    else
                        ResultImage.SetPixel(i, j, Color.Black);
                }
                
            }
            return ResultImage;
        }
    }
}
