using Tao.OpenGl;
using Tao.Platform.Windows;
using Tao.FreeGlut;
using System;
using System.Collections.Generic;

namespace JuMaSet
{
    public struct Complex
    {
        public double re;
        public double im;
    }

    public class Functions
    {
        public Complex ListFunctions(Complex z, Complex c, int type)
        {
            Complex TempReturn;
            TempReturn.re = 0;
            TempReturn.im = 0;

            switch (type)
            {
                case 1:
                    {
                        //z^2+C
                        TempReturn.re = z.re * z.re - z.im * z.im + c.re;
                        TempReturn.im = 2 * z.re * z.im + c.im;
                        break;
                    }
                case 2:
                    {
                        //z^3+C
                        TempReturn.re = z.re * z.re * z.re - 3 * z.re * z.im * z.im + c.re;
                        TempReturn.im = 3 * z.re * z.re * z.im - z.im * z.im * z.im + c.im;
                        break;
                    }
                case 3:
                    {
                        //z^4+C
                        TempReturn.re = z.re * z.re * z.re * z.re - 6 * z.re * z.re * z.im * z.im + z.im * z.im * z.im * z.im + c.re;
                        TempReturn.im = 4 * z.re * z.re * z.re * z.im - 4 * z.re * z.im * z.im * z.im + c.im;
                        break;
                    }
                case 4:
                    {
                        //z^3+Cz
                        TempReturn.re = z.re * z.re * z.re - 3 * z.re * z.im * z.im + c.re*z.re - c.im*z.im;
                        TempReturn.im = 3 * z.re * z.re * z.im - z.im * z.im * z.im + c.im * z.re + c.re * z.im;
                        break;
                    }
                case 5:
                    {
                        //sin(z)+C
                        TempReturn.re = z.re - z.re * z.re * z.re / 6 + z.re * z.im * z.im / 2 + (z.re * z.re * z.re * z.re * z.re - 10 * z.re * z.re * z.re * z.im * z.im + 5 * z.re * z.im * z.im * z.im * z.im) / 120 + c.re;
                        TempReturn.im = z.im + z.im * z.im * z.im / 6 - z.re * z.re * z.im / 2 + (z.im * z.im * z.im * z.im * z.im - 10 * z.re * z.re * z.im * z.im * z.im + 5 * z.re * z.re * z.re * z.re * z.im) / 120 + c.im;
                        break;
                    }
                case 6:
                    {
                        //e^z+C
                        TempReturn.re = 1 + z.re + (z.re * z.re - z.im * z.im) / 2 + (z.re * z.re * z.re - 3 * z.re * z.im * z.im) / 6 +(z.re * z.re * z.re * z.re - 6 * z.re * z.re * z.im * z.im + z.im * z.im * z.im * z.im) / 24 + c.re;
                        TempReturn.im = z.im + z.re * z.im + (3 * z.re * z.re * z.im - z.im * z.im * z.im) / 6 +(z.re * z.re * z.re * z.im - z.re * z.im * z.im * z.im) / 6 + c.im;
                        break;
                    }
            }

            return TempReturn;
        }

    }

    public class Julia:Functions
    {
        //Julia Set Function=z*z+C

        private Complex C;

        private int iterations;

        private double MinX, MaxX;
        private double MinY, MaxY;

        private double Red;
        private double Green;
        private double Blue;

        public Julia()
        {

            Red = 25 / 255;
            Green = 5 / 255;
            Blue = 1 / 255;


            MinX = -1.5;
            MaxX = 1.5;
            MinY = -1.5;
            MaxY = 1.5;
            C.re= 0.11;
            C.im = -0.66;
            iterations = 100;

        }
        public void SetConstant (double rezc, double imc)
        {
            C.re = rezc;
            C.im= imc;
        }
        public void SetBorders(double minx, double maxx, double miny, double maxy, int iter)
        {
            MaxX = maxx;
            MaxY = maxy;
            MinX = minx;
            MinY = miny;
            iterations = iter;
        }
        public void Draw(int pixelx, int pixely, int type, int scheme)
        {
            Complex Temp1, Temp2;
            Complex Z;
            double r=0, g=0, b=0;
            double rad;

          

            for (int i = 0; i < pixelx; i++)
            {
                Z.re = MinX + i * (MaxX - MinX) / pixelx;
                for (int j = 0; j < pixely; j++)
                {
                    Z.im = MinY + j * (MaxY - MinY) / pixely;

                    Temp1 = Z;
                    r = 0; g = 0; b = 0;
                    for (int c = 1; c <= iterations; c++)
                    {
                        Temp2 = ListFunctions(Temp1, C, type);
                        Temp1 = Temp2;
                        rad = (Temp1.re - Z.re) * (Temp1.re - Z.re) + (Temp1.im - Z.im) * (Temp1.im - Z.im);
                        if (rad > 16)
                        {

                            switch (scheme)
                            {
                                case 1:
                                    {
                                        r = 0; 
                                        g = (double)(c - 1)*2 / iterations;
                                        b = 0;
                                        break;
                                    }
                                case 2:
                                    {
                                        r = (double)Math.Cos((c - 1)); 
                                        g = (double)Math.Cos((c - 1));
                                        b = 0;
                                        break;
                                    }
                                case 3:
                                    {
                                        if (c >= 1 && c < (int)iterations *1.3/ 4)
                                        {
                                            r = (double)(c - 1)*3 / iterations;
                                            g = 0;
                                            b = 0;
                                        }
                                        if (c > (int)iterations * 1.3 / 4 && c < (int)iterations * 2 / 4)
                                        {
                                            r = 0.3+(double)(c - 1)*2 / iterations;
                                            g = 0.25+(double)(c - 1)*1.5 / iterations;
                                            b = 0;
                                        }
                                        if (c >= (int)iterations * 2 / 4 && c <= (int)iterations * 3 / 4)
                                        {
                                            r =(double)(c - 1) * 2 / iterations;
                                            g=(double)(c - 1) * 2 / iterations;
                                            b = 0;
                                        }
                                        if (c > (int)iterations * 3 / 4 && c < iterations)
                                        {
                                            b = (double)(c - 1) / iterations;
                                            g = (double)(c - 1) / iterations;
                                            r = 0;
                                        }
                                        if (c == iterations)
                                        {
                                            r = 0;
                                            b = 0.8;
                                            g = 0.7;
                                        }
                                        break;
                                    }
                            }

                        
                            c = iterations;
                        }
                        else
                        {
                            if (c == iterations)
                            {
                                switch (scheme)
                                {
                                    case 1:
                                        {
                                            r = 0.1; g = 0.6; b = 0.7;
                                            break;
                                        }
                                    case 2:
                                        {
                                            r = 1; g = 0; b = 0;
                                            break;
                                        }
                                    case 3:
                                        {
                                            r = 1; g = 0; b = 0;
                                            break;
                                        }
                                }


                            }
                        }
                    }

                    Gl.glBegin(Gl.GL_POINTS);
                    Gl.glColor3d(r, g, b);
                    Gl.glVertex2d((Z.re - MinX) / (MaxX - MinX), (Z.im - MinY) / (MaxY - MinY));
                    Gl.glEnd();
                    
                }
            
            }


        }
       
    }

    public class Mandelbrot : Functions
    {
              //Julia Set Function=z*z+C

        private Complex C;

        private int iterations;

        private double MinX, MaxX;
        private double MinY, MaxY;

        private double Red;
        private double Green;
        private double Blue;

        public Mandelbrot()
        {

            Red = 25 / 255;
            Green = 5 / 255;
            Blue = 1 / 255;


            MinX = -3;
            MaxX = 1;
            MinY = -2;
            MaxY = 2;
            C.re= 0;
            C.im = 0;
            iterations = 100;

        }
        public void SetConstant (double rezc, double imc)
        {
            C.re = rezc;
            C.im= imc;
        }
        public void SetBorders(double minx, double maxx, double miny, double maxy, int iter)
        {
            MaxX = maxx;
            MaxY = maxy;
            MinX = minx;
            MinY = miny;
            iterations = iter;
        }
        public void Draw(int pixelx, int pixely, int type, int scheme)
        {
            Complex Temp1, Temp2;
            Complex Z;
            double r=0, g=0, b=0;
            double rad;

            for (int i = 0; i < pixelx; i++)
            {
                Z.re = MinX + i * (MaxX - MinX) / pixelx;
                for (int j = 0; j < pixely; j++)
                {
                    Z.im = MinY + j * (MaxY - MinY) / pixely;

                    Temp1 = C;
                    r = 0; g = 0; b = 0;
                    for (int c = 1; c <= iterations; c++)
                    {
                        Temp2 = ListFunctions(Temp1, Z, type);
                        Temp1 = Temp2;
                        rad = (Temp1.re - C.re) * (Temp1.re - C.re) + (Temp1.im - C.im) * (Temp1.im - C.im);

                        if (rad > 16)
                        {
                            switch (scheme)
                            {
                                case 1:
                                    {
                                        r = 0; 
                                        g = (double)(c - 1)*2 / iterations;
                                        b = 0;
                                        break;
                                    }
                                case 2:
                                    {
                                        r = (double)Math.Cos((c - 1)); 
                                        g = (double)Math.Cos((c - 1));
                                        b = 0;
                                        break;
                                    }
                                case 3:
                                    {
                                        if (c >= 1 && c < (int)iterations *1.3/ 4)
                                        {
                                            r = ((double)(c - 1))*3 / iterations;
                                            g = 0;
                                            b = 0;
                                        }
                                        if (c > (int)iterations * 1.3 / 4 && c < (int)iterations * 2 / 4)
                                        {
                                            r = 0.3+(double)(c - 1)*2 / iterations;
                                            g = 0.25+(double)(c - 1)*1.5 / iterations;
                                            b = 0;
                                        }
                                        if (c >= (int)iterations * 2 / 4 && c <= (int)iterations * 3 / 4)
                                        {
                                            r =(double)(c - 1) * 2 / iterations;
                                            g=(double)(c - 1) * 2 / iterations;
                                            b = 0;
                                        }
                                        if (c > (int)iterations * 3 / 4 && c < iterations)
                                        {
                                            b = (double)(c - 1) / iterations;
                                            g = (double)(c - 1) / iterations;
                                            r = 0;
                                        }
                                        if (c == iterations)
                                        {
                                            r = 0;
                                            b = 0.8;
                                            g = 0.7;
                                        }
                                        break;
                                    }
                            }

                        
                            c = iterations;
                        }
                        else
                        {
                            if (c == iterations) 
                            { 
                                switch(scheme)
                                {
                                    case 1:
                                        {
                                            r = 0.1; g = 0.6; b = 0.7;
                                            break;
                                        }
                                    case 2:
                                        {
                                            r = 1; g = 0; b = 0;
                                            break;
                                        }
                                    case 3:
                                        {
                                            r = 1; g = 0; b = 0;
                                            break;
                                        }
                                }
                            
                        
                            }

                        }
                    }


                    double x = (Z.re - MinX) / (MaxX - MinX);
                    double y = (Z.im - MinY) / (MaxY - MinY);

                    Gl.glBegin(Gl.GL_POINTS);
                    Gl.glColor3d(r, g, b);
                    Gl.glVertex2d(x, y);
                    Gl.glEnd();

                }
            
            }




        }
    }

}