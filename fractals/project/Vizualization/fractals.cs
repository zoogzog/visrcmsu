using Tao.OpenGl;
using Tao.Platform.Windows;
using Tao.FreeGlut;
using System;

namespace Fractals
{
    public class Harter_Dragon
    {
        private int Red;
        private int Green;
        private int Blue;

        public Harter_Dragon()
        {
            Red = 255;
            Green = 255;
            Blue = 255;
        }
        private void PrimitiveDraw(double x1, double y1, double x2, double y2, int recursion_depth)
        {
            if (recursion_depth == 0)
            {
                Gl.glColor3d(Red, Green, Blue);
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2d(x1, 1-y1);
                Gl.glVertex2d(x2, 1-y2);
                Gl.glEnd();
            }
            else
            {
                Double cos = Math.Sqrt(2) / 2;
                Double sin = Math.Sqrt(2) / 2;
                Double x3 = x1 + ((x2 - x1) * cos + (y2 - y1) * sin) / Math.Sqrt(2);
                Double y3 = y1 + (-(x2 - x1) * sin + (y2 - y1) * cos) / Math.Sqrt(2);

                this.PrimitiveDraw(x1, y1, x3, y3, recursion_depth - 1);
                this.PrimitiveDraw(x2, y2, x3, y3, recursion_depth - 1);
            }
        }
        public void SetColor (int R, int G, int B)
        {
            Red = R;
            Green = G;
            Blue = B;
        }
        public void Draw(double x, double y, int recursion_depth)
        {
            this.PrimitiveDraw(x, 0.5, x + 0.3, 0.2, recursion_depth);
            this.PrimitiveDraw(x + 0.6, 0.5, x + 0.3, 0.2, recursion_depth);
        }    
    }

    public class Minkovsky_Square
    {
        private int Red;
        private int Green;
        private int Blue;

        public Minkovsky_Square()
        {
            Red = 255;
            Green = 255;
            Blue = 255;
        }
        private void PrimitiveDraw(double x1, double y1, double x2, double y2, int recursion_depth)
        {
            Console.WriteLine(x1 + " " + y1);

            if (recursion_depth == 0)
            {
                Gl.glColor3d(Red, Green, Blue);
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2d(x1, 1 - y1);
                Gl.glVertex2d(x2, 1 - y2);
                Gl.glEnd();
            }
            else
            {
                PrimitiveDraw(x1, y1, x2 / 4 + 3 * x1 / 4, y2 / 4 + 3 * y1 / 4, recursion_depth - 1);
                PrimitiveDraw(x1 / 4 + 3 * x2 / 4, y1 / 4 + 3 * y2 / 4, x2, y2, recursion_depth - 1);
                if (y1 == y2)
                {
                    PrimitiveDraw(x1 / 2 + x2 / 2, y1 + (x2 - x1) / 4, x1 / 2 + x2 / 2, y1, recursion_depth - 1);
                    PrimitiveDraw(x1 / 2 + x2 / 2, y1, x1 / 2 + x2 / 2, y1 - (x2 - x1) / 4, recursion_depth - 1);

                    PrimitiveDraw(3 * x1 / 4 + x2 / 4, y1, 3 * x1 / 4 + x2 / 4, y1 - (x2 - x1) / 4, recursion_depth - 1);
                    PrimitiveDraw(3 * x2 / 4 + x1 / 4, y1 + (x2 - x1) / 4, 3 * x2 / 4 + x1 / 4, y1, recursion_depth - 1);

                    PrimitiveDraw(3 * x1 / 4 + x2 / 4, y1 - (x2 - x1) / 4, x1 / 2 + x2 / 2, y1 - (x2 - x1) / 4, recursion_depth - 1);
                    PrimitiveDraw(x1 / 2 + x2 / 2, y1 + (x2 - x1) / 4, 3 * x2 / 4 + x1 / 4, y1 + (x2 - x1) / 4, recursion_depth - 1);
                }
                else
                {
                    PrimitiveDraw(x1 + (y2 - y1) / 4, y1 / 2 + y2 / 2, x1, y1 / 2 + y2 / 2, recursion_depth - 1);
                    PrimitiveDraw(x1, y1 / 2 + y2 / 2, x1 - (y2 - y1) / 4, y1 / 2 + y2 / 2, recursion_depth - 1);

                    PrimitiveDraw(x1, 3 * y1 / 4 + y2 / 4, x1 + (y2 - y1) / 4, 3 * y1 / 4 + y2 / 4, recursion_depth - 1);
                    PrimitiveDraw(x1 - (y2 - y1) / 4, 3 * y2 / 4 + y1 / 4, x1, 3 * y2 / 4 + y1 / 4, recursion_depth - 1);

                    PrimitiveDraw(x1 + (y2 - y1) / 4, 3 * y1 / 4 + y2 / 4, x1 + (y2 - y1) / 4, y1 / 2 + y2 / 2, recursion_depth - 1);
                    PrimitiveDraw(x1 - (y2 - y1) / 4, y1 / 2 + y2 / 2, x1 - (y2 - y1) / 4, 3 * y2 / 4 + y1 / 4, recursion_depth - 1);
                }
            }
        }
        public void SetColor(int R, int G, int B)
        {
            Red = R;
            Green = G;
            Blue = B;
        }
        public void Draw(double x, double y, int recursion_depth)
        {
            this.PrimitiveDraw(x, y, x, y+0.5, recursion_depth);
            this.PrimitiveDraw(x + 0.5, y, x, y, recursion_depth);
            this.PrimitiveDraw(x, y + 0.5, x + 0.5, y + 0.5, recursion_depth);
            this.PrimitiveDraw(x + 0.5, y + 0.5, x + 0.5, y, recursion_depth);
        }
    }

    public class Square_Hole
    {
        private int Red;
        private int Green;
        private int Blue;

        public Square_Hole()
        {
            Red = 255;
            Green = 255;
            Blue = 255;
        }
        private void PrimitiveDraw(double x1, double y1, double x2, double y2, int recursion_depth)
        {
            if (recursion_depth == 0)
            {
                Gl.glColor3d(Red, Green, Blue);
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2d(x1, 1 - y1);
                Gl.glVertex2d(x2, 1 - y2);
                Gl.glEnd();
            }
            else
            {
                Double cos = 0.5 / Math.Sqrt(4.25);
                Double sin = 2.0 / Math.Sqrt(4.25);
                Double x3 = 2 * x2 / 5 + 3 * x1 / 5 + ((x2 - x1) * cos + (y2 - y1) * sin) * Math.Sqrt(4.25) / 5;
                Double y3 = 2 * y2 / 5 + 3 * y1 / 5 + (-(x2 - x1) * sin + (y2 - y1) * cos) * Math.Sqrt(4.25) / 5;

                this.PrimitiveDraw(x1, y1, 3 * x1 / 5 + 2 * x2 / 5, 3 * y1 / 5 + 2 * y2 / 5, recursion_depth - 1);
                this.PrimitiveDraw(2 * x1 / 5 + 3 * x2 / 5, 2 * y1 / 5 + 3 * y2 / 5, x2, y2, recursion_depth - 1);
                this.PrimitiveDraw(2 * x2 / 5 + 3 * x1 / 5, 2 * y2 / 5 + 3 * y1 / 5, x3, y3, recursion_depth - 1);
                this.PrimitiveDraw(x3, y3, 2 * x1 / 5 + 3 * x2 / 5, 2 * y1 / 5 + 3 * y2 / 5, recursion_depth - 1);
            }
        }
        public void SetColor(int R, int G, int B)
        {
            Red = R;
            Green = G;
            Blue = B;
        }
        public void Draw(double x, double y, int recursion_depth)
        {
            this.PrimitiveDraw(x, y, x, y + 0.90, recursion_depth);
            this.PrimitiveDraw(x + 0.90, y, x, y, recursion_depth);
            this.PrimitiveDraw(x , y + 0.90, x + 0.9, y + 0.90, recursion_depth);
            this.PrimitiveDraw(x + 0.90, y + 0.90, x + 0.9, y, recursion_depth);
        }
    }

    public class Serpinsky_Carpet
    {
        private int Red;
        private int Green;
        private int Blue;

        public Serpinsky_Carpet()
        {
            Red = 255;
            Green = 255;
            Blue = 255;
        }
        public void SetColor(int R, int G, int B)
        {
            Red = R;
            Green = G;
            Blue = B;
        }
        public void Draw(double x, double y, double delta, int recursion_depth, int what)
        {
            if (recursion_depth == 0)
            {
                if (what == 1)
                {
                    Gl.glColor3d(Red, Green, Blue);
                    Gl.glBegin(Gl.GL_POLYGON);
                    Gl.glVertex2d(x, 1 - y);
                    Gl.glVertex2d(x, 1 - (y + delta));
                    Gl.glVertex2d(x + delta, 1 - (y + delta));
                    Gl.glVertex2d(x + delta, 1 - y);
                    Gl.glEnd();
                }
            }
            else
            {

                Gl.glColor3d(Red, Green, Blue);
                Gl.glBegin(Gl.GL_POLYGON);
                Gl.glVertex2d(x, 1 - y);
                Gl.glVertex2d(x, 1 - (y+delta));
                Gl.glVertex2d(x+delta, 1 - (y+delta));
                Gl.glVertex2d(x+delta, 1 - y);
                Gl.glEnd();

                Gl.glColor3d(0, 0, 0);
                Gl.glBegin(Gl.GL_POLYGON);
                Gl.glVertex2d(x + delta / 3, 1 - (y+delta/3));
                Gl.glVertex2d(x + delta / 3, 1 - (y + delta / 3 + delta / 3));
                Gl.glVertex2d(x + delta / 3 + delta / 3, 1 - (y + delta / 3 + delta / 3));
                Gl.glVertex2d(x + delta / 3 + delta / 3, 1 - (y + delta / 3));
                Gl.glEnd();

                this.Draw(x, y, delta/3, recursion_depth-1, 0);
                this.Draw(x+delta/3, y, delta/3, recursion_depth-1, 0);
                this.Draw(x+2*delta/3, y, delta/3, recursion_depth-1, 0);

                this.Draw(x, y+delta/3, delta/3, recursion_depth-1, 0);

                this.Draw(x, y+delta/3*2, delta/3, recursion_depth-1, 0);
                this.Draw(x + delta / 3, y + delta / 3 * 2, delta / 3, recursion_depth - 1, 0);
                this.Draw(x+delta/3*2, y+delta/3*2, delta/3, recursion_depth-1, 0);

                this.Draw(x+delta/3*2, y+delta/3, delta/3, recursion_depth-1, 0);
            }
        }
    }

    public class Koch_Curve
    {
        private int Red;
        private int Green;
        private int Blue;

        public Koch_Curve()
        {
            Red = 255;
            Green = 255;
            Blue = 255;
        }
        private void PrimitiveDraw(double x1, double y1, double x2, double y2, int recursion_depth)
        {
            if (recursion_depth == 0)
            {
                Gl.glColor3d(Red, Green, Blue);
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2d(x1, 1 - y1);
                Gl.glVertex2d(x2, 1 - y2);
                Gl.glEnd();
            }
            else
            {
                Double cos = 1.0 / 2;
                Double sin = Math.Sqrt(3) / 2;
                Double x3 = x2 / 3 + 2 * x1 / 3 + ((x2 - x1) * cos + (y2 - y1) * sin) / 3;
                Double y3 = y2 / 3 + 2 * y1 / 3 + (-(x2 - x1) * sin + (y2 - y1) * cos) / 3;

                this.PrimitiveDraw(x1, y1, x2 / 3 + 2 * x1 / 3, y2 / 3 + 2 * y1 / 3, recursion_depth - 1);
                this.PrimitiveDraw(x1 / 3 + 2 * x2 / 3, y1 / 3 + 2 * y2 / 3, x2, y2, recursion_depth - 1);
                this.PrimitiveDraw(x2 / 3 + 2 * x1 / 3, y2 / 3 + 2 * y1 / 3, x3, y3, recursion_depth - 1);
                this.PrimitiveDraw(x3, y3, x1 / 3 + 2 * x2 / 3, y1 / 3 + 2 * y2 / 3, recursion_depth - 1);
            }
        }
        public void SetColor(int R, int G, int B)
        {
            Red = R;
            Green = G;
            Blue = B;
        }
        public void Draw(double x, double y, int recursion_depth)
        {
            this.PrimitiveDraw(x, y, x+0.9, y, recursion_depth);
        }
    }
}