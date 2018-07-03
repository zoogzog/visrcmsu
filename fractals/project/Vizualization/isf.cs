using Tao.OpenGl;
using Tao.Platform.Windows;
using Tao.FreeGlut;
using System;
using System.Collections.Generic;

namespace ISFFractals
{
     
    public class SimpleTransformation
    {
        //Container for storing parameters of transformation

        private double a1;
        private double a2;
        private double a3;
        private double a4;
        private double a5;
        private double a6;

        private double p;

        public SimpleTransformation(double A1, double A2, double A3, double A4, double A5, double A6, double P)
        {
            a1 = A1;
            a2 = A2;
            a3 = A3;
            a4 = A4;
            a5 = A5;
            a6 = A6;

            p = P;
        }
        public double x_Transofrmation(double x, double y)
        {
            return (a1 * x + a2 * y + a5);
        }
        public double y_Transofrmation(double x, double y)
        {
            return (a3 * x + a4 * y + a6);
        }
        public double Probability ()
        {
            return p;
        }
        public void SetParameter(double A2, double A3)
        {
            a2 = A2;
            a3 = A3;
        }
    }

    public class DrawISF
    {

        protected void ISF_Draw(List <SimpleTransformation> List_of_Transformations, double X, double Y, double S, int itterations, double cor, double cog, double cob, int r_d)
        {
            double x1=0.0, x2=0.0;
            double y1=0.0, y2=0.0;
            double rand_d=0.0, p=0.0;
            Random randomizer;
            if (r_d != 1)
            {
                randomizer = new Random(DateTime.Now.Millisecond);
            }
            else
            {
                randomizer = new Random(DateTime.Now.Millisecond);
            }

            for (int i = 0; i <= itterations; i++)
            {
                rand_d = randomizer.NextDouble();
                p = List_of_Transformations[0].Probability();
                int j = 0;
                while (p < rand_d)
                {
                    j++;
                    p += List_of_Transformations[j].Probability();

                }
                x2 = List_of_Transformations[j].x_Transofrmation(x1, y1);
                y2 = List_of_Transformations[j].y_Transofrmation(x1, y1);

                x1 = x2;
                y1 = y2;


                double x = (S * x2+ X);
                double y = (S * y2+ Y);
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glColor3f((float)cor, (float)cog, (float)cob);
                Gl.glVertex2f((float)x, (float)(y-0.01));
                Gl.glEnd();
            }


        }
    }

    public class ISF_Fern:DrawISF
    {
        private double Red;
        private double Green;
        private double Blue;

        private List<SimpleTransformation> Current_List = new List<SimpleTransformation>();
        private SimpleTransformation Box_from_List;

        public ISF_Fern()
        {
            Box_from_List = new SimpleTransformation(0.0, 0.0, 0.0, 0.16, 0.0, 0.0, 0.01);
            Current_List.Add(Box_from_List);
            Box_from_List = new SimpleTransformation(0.85, 0.04, -0.04, 0.85, 0.0, 1.6, 0.85);
            Current_List.Add(Box_from_List);
            Box_from_List = new SimpleTransformation(0.2, -0.26, 0.23, 0.22, 0.0, 1.6, 0.07);
            Current_List.Add(Box_from_List);
            Box_from_List = new SimpleTransformation(-0.15, 0.28, 0.26, 0.24, 0.0, 0.44, 0.07);
            Current_List.Add(Box_from_List);
            Red = 1;
            Green = 1;
            Blue = 1;
        }
        public void SetColor(double R, double G, double B)
        {
            Red = R;
            Green = G;
            Blue = B;
        }
        public void Draw(double x0, double y0, double scale, int iterations)
        {
            this.ISF_Draw(Current_List, x0, y0, scale, iterations, Red, Green, Blue,2);
        }

    }

    public class ISF_Cobweb:DrawISF
    {
        private double Red;
        private double Green;
        private double Blue;

        List<SimpleTransformation> Current_List = new List<SimpleTransformation>();
        SimpleTransformation Box_from_List;

        public void SetColor(double R, double G, double B)
        {
            Red = R;
            Green = G;
            Blue = B;
        }
        public ISF_Cobweb()
        {
            Box_from_List = new SimpleTransformation(-0.81, -0.38, 0.38, -0.81, 0, 0, 0.83);
            Current_List.Add(Box_from_List);
            Box_from_List = new SimpleTransformation(-0.3, 0.25, -0.25, -0.3, 1.3, 0.25, 0.17);
            Current_List.Add(Box_from_List);
            Red = 1;
            Green = 1;
            Blue = 1;
        }
        public void Draw(double x0, double y0, double scale, int iterations)
        {
            this.ISF_Draw(Current_List, x0, y0, scale, iterations, Red, Green, Blue,2);
        }

    }

    public class ISF_Dragon : DrawISF
    {
        private double Red;
        private double Green;
        private double Blue;

        List<SimpleTransformation> Current_List = new List<SimpleTransformation>();
        SimpleTransformation Box_from_List;

        public void SetColor(double R, double G, double B)
        {
            Red = R;
            Green = G;
            Blue = B;
        }
        public void Draw(double x0, double y0, double scale, int iterations)
        {
            this.ISF_Draw(Current_List, x0, y0, scale, iterations, Red, Green, Blue,2);
        }
        public ISF_Dragon()
        {
            Box_from_List = new SimpleTransformation(0.824074, 0.281482, -0.212346, 0.864198, -1.88229, -0.110607, 0.787473);
            Current_List.Add(Box_from_List);
            Box_from_List = new SimpleTransformation(0.088272, 0.520988, -0.463889, -0.377778, 0.78536, 8.095795, 0.212527);
            Current_List.Add(Box_from_List);

            Red = 1;
            Green = 1;
            Blue = 1;
        }

    }

    public class ISF_Star : DrawISF
    {
        private double Red;
        private double Green;
        private double Blue;

        private List<SimpleTransformation> Current_List = new List<SimpleTransformation>();
        private SimpleTransformation Box_from_List;

        public void SetColor(double R, double G, double B)
        {
            Red = R;
            Green = G;
            Blue = B;
        }
        public void Draw(double x0, double y0, double scale, int iterations)
        {
            this.ISF_Draw(Current_List, x0, y0, scale, iterations, Red, Green, Blue,2);
        }
        public ISF_Star()
        {
            Box_from_List = new SimpleTransformation(0.6,  0,     0,     0.21, 0,    0,    0.3);
            Current_List.Add(Box_from_List);
            Box_from_List = new SimpleTransformation(-0.55, 0.75, -0.75, -0.55, 1.55, 0.75, 0.7);
            Current_List.Add(Box_from_List);

            Red = 1;
            Green = 1;
            Blue = 1;
        }
    }

    public class ISF_Spiral : DrawISF
    {
        private double Red;
        private double Green;
        private double Blue;

        private double current;
        private long step;

        private List<SimpleTransformation> Current_List = new List<SimpleTransformation>();
        private SimpleTransformation Box_from_List;

        public void SetColor(double R, double G, double B)
        {
            Red = R;
            Green = G;
            Blue = B;
        }
        public void Draw(double x0, double y0, double scale, int iterations)
        {
            this.ISF_Draw(Current_List, x0, y0, scale, iterations, Red, Green, Blue,1);
        }
        public ISF_Spiral()
        {
            Box_from_List = new SimpleTransformation(-0.61, 0.7, -0.7, -0.61, 0.00, 0.0, 0.9);
            Current_List.Add(Box_from_List);
            Box_from_List = new SimpleTransformation( 0.21, 0.0,  0.0,  0.21, 0.79, 0.0, 0.1);
            Current_List.Add(Box_from_List);

            Red = 0.95;
            Green = 0.13;
            Blue = 0.15;
            step = 0;
        }
        public void Tick(int iterations)
        {

            step++;


            this.SetParameter(0.15 * Math.Cos(1.0 * step / 100));
            this.Draw(0.45, 0.5, 0.5, iterations);

        }
        private void SetParameter(double parameter)
        {
            Current_List[1].SetParameter(parameter, -parameter);
        }
    }

    public class ISF_WindFern:DrawISF
    {
        private double Red;
        private double Green;
        private double Blue;

        private double current;
        private long step;

        private List<SimpleTransformation> Current_List = new List<SimpleTransformation>();
        private SimpleTransformation Box_from_List;


     
        public ISF_WindFern()
        {
            Box_from_List = new SimpleTransformation(0.0, 0.0, 0.0, 0.16, 0.0, 0.0, 0.01);
            Current_List.Add(Box_from_List);
            Box_from_List = new SimpleTransformation(0.85, 0.00, 0.00, 0.85, 0.0, 1.6, 0.85);
            Current_List.Add(Box_from_List);
            Box_from_List = new SimpleTransformation(0.2, -0.26, 0.23, 0.22, 0.0, 1.6, 0.07);
            Current_List.Add(Box_from_List);
            Box_from_List = new SimpleTransformation(-0.15, 0.28, 0.26, 0.24, 0.0, 0.44, 0.07);
            Current_List.Add(Box_from_List);
            Red = 0.1;
            Green = 0.99;
            Blue = 0.15;
        }
        public void SetColor(double R, double G, double B)
        {
            Red = R;
            Green = G;
            Blue = B;
            current = 0.4;
            step = 0;
        }
        private void Draw(double x0, double y0, double scale, int iterations)
        {
            this.ISF_Draw(Current_List, x0, y0, scale, iterations, Red, Green, Blue,1);
        }
        private void SetParameter(double parameter)
        {
            Current_List[1].SetParameter(parameter, -parameter);
        }
        public void Tick(int iterations)
        {
            
            step++;
               

            this.SetParameter(0.08 * Math.Cos(1.0 * step / 100));
            this.Draw(0.4, 0, 0.07, iterations); 

        }
    }
}