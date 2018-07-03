using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Tao.OpenGl;
using Tao.FreeGlut;
using Tao.Platform.Windows;

//Task 1 namespaces
using Fractals;                     //fractals.cs
using ISFFractals;                  //isf.cs
using JuMaSet;                      //julia.cs


//Task 2 namespaces
using ImageProcessing;              //imp.cs


//Task 3 namespaces
using MCSurface;


namespace Vizualization
{
    public partial class Form1 : Form
    {
        #region Fractal classes
        private Harter_Dragon HarterDragon;
        private Minkovsky_Square MinkovskySquare;
        private Square_Hole SquareCutted;
        private Koch_Curve KochCurve;
        private Serpinsky_Carpet SerpinskyCarpet;

        private ISF_Fern Fern;
        private ISF_Cobweb Cobweb;
        private ISF_Dragon Dragon;
        private ISF_Star Star;
        private ISF_Spiral Spiral;
        private ISF_WindFern WindFern;

        private Julia JuliaSet;
        private Mandelbrot ManSet;
        #endregion

        #region Task1 additional variables
        private int depth_counter;
        private int iterations;

        private bool s_fractal;
        private bool i_fractal;
        private bool j_fractal;
        private bool m_fractal;
        private bool w_fractal;
        private bool w2_fractal;

        private int jmiterations;
        private double rec;
        private double imc;


        private double minx, maxx;
        private double miny, maxy;

        private int details;
        private double scale;
        //Colour Schemes
        private int scheme;
        #endregion

        #region Image Processing
        private Grayscale Gray;
        private Blur BlurPicture;
        private Borders Border;
        private Bitmap InputImage;

        private bool loadf;
        
        #endregion

        #region Vector Fields
        private Grid Surface;

        private bool drawv;
        private string fiload;
        private bool loaded;

        private double y_offset;
        private double z_offset;

        private double x_rotated;
        private double y_rotated;

        private double constant;

        private int iX, iY, iZ;
        private int timevalue;

        private int mousex, mousey;

        private bool LightONF;
        private bool InvNorm;
        #endregion

        public Form1()
        {
            InitializeComponent();

            #region Fractal classes constructors
            HarterDragon = new Harter_Dragon();
            MinkovskySquare = new Minkovsky_Square();
            SquareCutted = new Square_Hole();
            KochCurve = new Koch_Curve();
            SerpinskyCarpet = new Serpinsky_Carpet();
            Fern = new ISF_Fern();
            Cobweb = new ISF_Cobweb();
            Dragon = new ISF_Dragon();
            Star = new ISF_Star();
            Spiral = new ISF_Spiral();
            WindFern = new ISF_WindFern();

            JuliaSet = new Julia();
            ManSet = new Mandelbrot();
            #endregion


            #region Image Processing init
            Gray = new Grayscale();
            BlurPicture = new Blur();
            Border = new Borders();
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Images (*.bmp;*.jpg;*.gif)|*.bmp;*.jpg;*.gif|All files(*.*)|*.*";
            loadf = false;
            #endregion


            #region Fractal variables init
            loaded = false;
            depth_counter = 0;
            iterations = 1000;

            jmiterations = 100;
            rec = 0.11;
            imc = -0.66;

            minx = -1.5; maxx = 1.5;
            miny = -1.5; maxy = 1.5;

            s_fractal = false;
            i_fractal = false;
            j_fractal = false;
            m_fractal = false;
            w_fractal = false;
            w2_fractal = false;

            details = 800;
            scale = 1;
            #endregion


            #region Vector Fields init
            Surface = new Grid();

            openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter="(*.txt; *.res)|*.txt;*.res|All files(*.*)|*.*";
            drawv = false;
            

            y_offset = 0;
            z_offset = 0;

            x_rotated = 0;
            y_rotated = 0;

            constant = 1;
            timevalue = 0;
            LightONF = true;
            InvNorm = false;
            #endregion

            
            Fractal_GL_window.InitializeContexts();
            VectorFields.InitializeContexts();   
 
         
        }
        private void Form1_Load(object sender, EventArgs e) { }
        private void tabPage1_Click(object sender, EventArgs e) { }

        //------------------ Task1 --------------------

        private void InitGL()
        {
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glOrtho(0.0, 1.0, 0.0, 1.0, -1.0, 1.0);           
        }

        private void simpleOpenGlControl1_Load(object sender, EventArgs e) { InitGL(); }
   
        private void SFractal_Render_button_Click(object sender, EventArgs e)
        {
            w_fractal = false;
            w2_fractal = false;
            i_fractal = false;
            j_fractal = false;
            m_fractal = false;
            s_fractal = true;
            Fractal_GL_window.Refresh();       
        }

        private void IFSFractal_render_button_Click(object sender, EventArgs e)
        {
            s_fractal = false;
            m_fractal = false;
            j_fractal = false;
            try
            {

                iterations = int.Parse(textBox1.Text);
            }
            catch (Exception ex)
            {
            }
            if (!radioButton11.Checked) { w2_fractal = false; }
            if (!radioButton7.Checked) { w_fractal = false; }
            i_fractal = true;

            Fractal_GL_window.Refresh();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
                depth_counter =(int) numericUpDown1.Value;
        }

        private void Fractal_GL_window_Paint(object sender, PaintEventArgs e)
        {
     
            //Fractal_GL_window.SwapBuffers(); //Check this!!
            RenderScene();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
           //RenderScene();
        }

        private void RenderScene()
        {
            if (!w_fractal && !w2_fractal)
            {
               InitGL();
            }
            
            #region Constructed Fractals
            if (radioButton3.Checked && s_fractal)
            {
                //Draw Harter Dragon

                HarterDragon.SetColor(0, 255, 0);
                HarterDragon.Draw(0.2, 0.5, depth_counter);
                label3.Text = "2";
                //s_fractal = false;
            }
            if (radioButton2.Checked && s_fractal)
            {
                //Draw Minkovsky square
                int current_depth_counter = depth_counter;
                if (depth_counter >= 7)
                {
                    current_depth_counter = 6;
                }
                MinkovskySquare.SetColor(255, 0, 0);
                MinkovskySquare.Draw(0.25, 0.25, current_depth_counter);
                label3.Text = "3/4";
                //s_fractal = false;
            }
            if (radioButton4.Checked && s_fractal)
            {
                //Draw Square with holes
                int current_depth_counter = depth_counter;
                if (depth_counter >= 7)
                {
                    current_depth_counter = 7;
                }
                SquareCutted.SetColor(100, 100, 0);
                SquareCutted.Draw(0.05, 0.05, current_depth_counter);
                label3.Text = "2";
                //s_fractal = false;
            }
            if (radioButton1.Checked && s_fractal)
            {
                //Draw Serpinsky carpet
                int current_depth_counter = depth_counter;
                if (depth_counter >= 7)
                {
                    current_depth_counter = 7;
                }
                SerpinskyCarpet.SetColor(0, 80, 255);
                SerpinskyCarpet.Draw(0.2, 0.2, 0.6, current_depth_counter, 1);

                label3.Text = "log(8)/log(3)";
                //s_fractal = false;
            }
            if (radioButton6.Checked && s_fractal)
            {
                //Draw Koch Curve
                int current_depth_counter = depth_counter;
                if (depth_counter >= 7)
                {
                    current_depth_counter = 7;
                }
                KochCurve.SetColor(0, 60, 255);
                KochCurve.Draw(0.05, 0.5, current_depth_counter);
                label3.Text = "log(4)/log(3)";
                //s_fractal = false;


            }
            #endregion

            #region ISF Fractals
            if (radioButton5.Checked && i_fractal)
            {
                //Draw IFS Fern

                Fern.SetColor(0.0, 1.0, 0.12);
                Fern.Draw(0.5, 0.0, 0.095, iterations);
                //i_fractal = false;
            }
            if (radioButton7.Checked && i_fractal)
            {
                w_fractal = true;
                i_fractal = false;
                w2_fractal = false;
            }
            if (radioButton8.Checked && i_fractal)
            {
                //Draw IFS Cobweb
                Cobweb.SetColor(0.1, 0.65, 0.75);
                Cobweb.Draw(0.5, 0.5, 0.25, iterations);
                //i_fractal = false;
            }
            if (radioButton9.Checked && i_fractal)
            {
                //Draw IFS Dragon
                Dragon.SetColor(0.98, 0.55, 0.1);
                Dragon.Draw(0.5, 0.1, 0.08, iterations);
                //i_fractal = false;
            }
            if (radioButton10.Checked && i_fractal)
            {

                //Draw IFS Star 
                Star.SetColor(0.76, 0.5, 0.89);
                Star.Draw(0.05, 0.5, 0.5, iterations);
                //i_fractal = false;
            }
            if (radioButton11.Checked && i_fractal)
            {
                //Spiral with wind code

                //Draw IFS Spiral
                //Spiral.SetColor(0.99, 0.1, 0.2);
                //Spiral.Draw(0.45, 0.5, 0.5, iterations);
                w2_fractal = true;
                i_fractal = false;
                w_fractal = false;
            }
            #endregion

            #region Julia
            if (radioButton12.Checked && j_fractal)
            {
                //z^2+C
                JuliaSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                JuliaSet.SetConstant(rec, imc);
                JuliaSet.Draw(details, details, 1, scheme);
                label3.Text = "2";
                //j_fractal = false;
            }
            if (radioButton13.Checked && j_fractal)
            {
                //z^3+C
                JuliaSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                JuliaSet.SetConstant(rec, imc);
                JuliaSet.Draw(details, details, 2, scheme);
                label3.Text = "2";
                //j_fractal = false;
            }
            if (radioButton14.Checked && j_fractal)
            {
                //z^4+C
                JuliaSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                JuliaSet.SetConstant(rec, imc);
                JuliaSet.Draw(details, details, 3, scheme);
                label3.Text = "2";
                //j_fractal = false;
            }
            if (radioButton15.Checked && j_fractal)
            {
                //z^3+Cz
                JuliaSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                JuliaSet.SetConstant(rec, imc);
                JuliaSet.Draw(details, details, 4, scheme);
                label3.Text = "2";
                //j_fractal = false;
            }
            if (radioButton16.Checked && j_fractal)
            {
                //sin(z)+c
                JuliaSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                JuliaSet.SetConstant(rec, imc);
                JuliaSet.Draw(details, details, 5, scheme);
                label3.Text = "2";
                //j_fractal = false;
            }
            if (radioButton17.Checked && j_fractal)
            {
                //e^z+c
                JuliaSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                JuliaSet.SetConstant(rec, imc);
                JuliaSet.Draw(details, details, 6, scheme);
                label3.Text = "2";
                //j_fractal = false;
            }
            #endregion

            #region Mandelbrot
            if (radioButton12.Checked && m_fractal)
            {
                //z^2+C
                ManSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                ManSet.SetConstant(rec, imc);
                ManSet.Draw(details, details, 1, scheme); 
                label3.Text = "bounders 2";
                //m_fractal = false;
            }
            if (radioButton13.Checked && m_fractal)
            {
                //z^3+C
                ManSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                ManSet.SetConstant(rec, imc);
                ManSet.Draw(details, details, 2, scheme);
                label3.Text = "bounders 2";
                //m_fractal = false;
            }
            if (radioButton14.Checked && m_fractal)
            {
                //z^4+C
                ManSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                ManSet.SetConstant(rec, imc);
                ManSet.Draw(details, details, 3, scheme);
                label3.Text = "bounders 2";
                //m_fractal = false;
            }
            if (radioButton15.Checked && m_fractal)
            {
                //z^3+Cz
                ManSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                ManSet.SetConstant(rec, imc);
                ManSet.Draw(details, details, 4, scheme);
                label3.Text = "bounders 2";
                //m_fractal = false;
            }
            if (radioButton16.Checked && m_fractal)
            {
                //sin(z)+c
                ManSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                ManSet.SetConstant(rec, imc);
                ManSet.Draw(details, details, 5, scheme);
                label3.Text = "bounders 2";
                //m_fractal = false;
            }
            if (radioButton17.Checked && m_fractal)
            {
                //e^z+c
                ManSet.SetBorders(minx, maxx, miny, maxy, jmiterations);
                ManSet.SetConstant(rec, imc);
                ManSet.Draw(details, details, 6, scheme);
                label3.Text = "bounders 2";
                //m_fractal = false;
            }
            #endregion
            Gl.glFlush();

            
        }

        //Fern with wind Timer Tick Event Handler
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (radioButton7.Checked && w_fractal)
            {
                WindFern.Tick(iterations);
               // Fractal_GL_window.Refresh();
              
                Fractal_GL_window.SwapBuffers(); 
                Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
                Gl.glShadeModel(Gl.GL_SMOOTH);
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glLoadIdentity();
                Gl.glOrtho(0.0, 1.0, 0.0, 1.0, -1.0, 1.0);
                
            }
            if (radioButton11.Checked && w2_fractal)
            {
                Spiral.Tick(iterations);

               // Fractal_GL_window.Refresh();
                Fractal_GL_window.SwapBuffers();
                Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
                Gl.glShadeModel(Gl.GL_SMOOTH);
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glLoadIdentity();
                Gl.glOrtho(0.0, 1.0, 0.0, 1.0, -1.0, 1.0);
            }
            
        }

        //Task 1 Julia and Mandelbrot Sets

        private void button4_Click(object sender, EventArgs e)
        {
            //Julia and Mandelbrot Sets button Render;

            try
            {
                jmiterations = int.Parse(textBox2.Text);
                rec = double.Parse(textBox3.Text);
                imc = double.Parse(textBox4.Text);
                details = int.Parse(textBox10.Text);

                if (jmiterations > 300 && jmiterations <= 0) { jmiterations = 100; }
                if (details > 1252 && details <= 626) { details = 626; }

                minx = double.Parse(textBox5.Text);
                maxx = double.Parse(textBox6.Text);

                if (maxx < minx)
                {
                    double temp = 0;
                    temp = minx;
                    minx = maxx;
                    maxx = temp;
                }

                miny = double.Parse(textBox8.Text);
                maxy = double.Parse(textBox9.Text);
               
               
            }
            catch (Exception mesages)
            {
                MessageBox.Show("Wrong parameters!\nUsing default.");
            }
            if (radioButton20.Checked) { scheme = 1; }
            if (radioButton21.Checked) { scheme = 2; }
            if (radioButton22.Checked) { scheme = 3; }
            if (radioButton18.Checked) { j_fractal = true; w_fractal = false; w2_fractal = false; m_fractal = false; i_fractal = false; }
            if (radioButton19.Checked) { m_fractal = true; j_fractal = false; w_fractal = false; w2_fractal = false; i_fractal = false; }
            Fractal_GL_window.Refresh();
        }

        #region Julia & Mandelbrot Controls
        private void button9_Click(object sender, EventArgs e)
        {
            //Button UP

            miny = 0.1 * scale + miny;
            maxy = 0.1 * scale + maxy;

            textBox8.Text = miny.ToString();
            textBox9.Text = maxy.ToString();
            
            if (radioButton20.Checked) { scheme = 1; }
            if (radioButton21.Checked) { scheme = 2; }
            if (radioButton22.Checked) { scheme = 3; }
            if (radioButton18.Checked) { j_fractal = true; w_fractal = false; w2_fractal = false; m_fractal = false; i_fractal = false; }
            if (radioButton19.Checked) { m_fractal = true; j_fractal = false; w_fractal = false; w2_fractal = false; i_fractal = false; }
            Fractal_GL_window.Refresh();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Button DOWN
            miny = -0.1 * scale + miny;
            maxy = -0.1 * scale + maxy;

            textBox8.Text = miny.ToString();
            textBox9.Text = maxy.ToString();

            if (radioButton20.Checked) { scheme = 1; }
            if (radioButton21.Checked) { scheme = 2; }
            if (radioButton22.Checked) { scheme = 3; }
            if (radioButton18.Checked) { j_fractal = true; w_fractal = false; w2_fractal = false; m_fractal = false; i_fractal = false; }
            if (radioButton19.Checked) { m_fractal = true; j_fractal = false; w_fractal = false; w2_fractal = false; i_fractal = false; }
            Fractal_GL_window.Refresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Button RIGHT
            minx = 0.1 * scale + minx;
            maxx = 0.1 * scale + maxx;

            textBox5.Text = minx.ToString();
            textBox6.Text = maxx.ToString();

            if (radioButton20.Checked) { scheme = 1; }
            if (radioButton21.Checked) { scheme = 2; }
            if (radioButton22.Checked) { scheme = 3; }
            if (radioButton18.Checked) { j_fractal = true; w_fractal = false; w2_fractal = false; m_fractal = false; i_fractal = false; }
            if (radioButton19.Checked) { m_fractal = true; j_fractal = false; w_fractal = false; w2_fractal = false; i_fractal = false; }
            Fractal_GL_window.Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Button LEFT
            minx = -0.1 * scale + minx;
            maxx = -0.1 * scale + maxx;

            textBox5.Text = minx.ToString();
            textBox6.Text = maxx.ToString();

            if (radioButton20.Checked) { scheme = 1; }
            if (radioButton21.Checked) { scheme = 2; }
            if (radioButton22.Checked) { scheme = 3; }
            if (radioButton18.Checked) { j_fractal = true; w_fractal = false; w2_fractal = false; m_fractal = false; i_fractal = false; }
            if (radioButton19.Checked) { m_fractal = true; j_fractal = false; w_fractal = false; w2_fractal = false; i_fractal = false; }
            Fractal_GL_window.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Button ZOOMin
            scale = 0.7*scale;
            minx = 0.7 * minx;
            maxx = 0.7 * maxx;
            miny = 0.7 * miny;
            maxy = 0.7 * maxy;

            textBox5.Text = minx.ToString();
            textBox6.Text = maxx.ToString();
            textBox8.Text = miny.ToString();
            textBox9.Text = maxy.ToString();
            if (radioButton20.Checked) { scheme = 1; }
            if (radioButton21.Checked) { scheme = 2; }
            if (radioButton22.Checked) { scheme = 3; }
            if (radioButton18.Checked) { j_fractal = true; w_fractal = false; w2_fractal = false; m_fractal = false; i_fractal = false; }
            if (radioButton19.Checked) { m_fractal = true; j_fractal = false; w_fractal = false; w2_fractal = false; i_fractal = false; }
            Fractal_GL_window.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Button ZOOMout
            scale = 10 * scale/7;
            minx = 10  * minx/7;
            maxx = 10 * maxx/7;
            miny = 10 * miny/7;
            maxy = 10  * maxy/7;

            textBox5.Text = minx.ToString();
            textBox6.Text = maxx.ToString();
            textBox8.Text = miny.ToString();
            textBox9.Text = maxy.ToString();
            if (radioButton20.Checked) { scheme = 1; }
            if (radioButton21.Checked) { scheme = 2; }
            if (radioButton22.Checked) { scheme = 3; }
            if (radioButton18.Checked) { j_fractal = true; w_fractal = false; w2_fractal = false; m_fractal = false; i_fractal = false; }
            if (radioButton19.Checked) { m_fractal = true; j_fractal = false; w_fractal = false; w2_fractal = false; i_fractal = false; }
            Fractal_GL_window.Refresh();
        }
        #endregion

        //-------------------------------------------




        //------------------ Unused ------------------

        #region Unused Task1 functions
        private void radioButton1_CheckedChanged(object sender, EventArgs e){}
        private void radioButton2_CheckedChanged(object sender, EventArgs e){} 
        private void radioButton3_CheckedChanged(object sender, EventArgs e){}   
        private void radioButton4_CheckedChanged(object sender, EventArgs e){}
        private void radioButton6_CheckedChanged(object sender, EventArgs e){}      
        private void radioButton5_CheckedChanged(object sender, EventArgs e){}
        private void radioButton7_CheckedChanged(object sender, EventArgs e) {}
        private void radioButton8_CheckedChanged(object sender, EventArgs e){}
        private void radioButton9_CheckedChanged(object sender, EventArgs e){}
        private void radioButton10_CheckedChanged(object sender, EventArgs e){}
        private void radioButton11_CheckedChanged(object sender, EventArgs e) { }     
        private void radioButton12_CheckedChanged(object sender, EventArgs e){}
        private void radioButton13_CheckedChanged(object sender, EventArgs e){}
        private void radioButton14_CheckedChanged(object sender, EventArgs e){}
        private void radioButton15_CheckedChanged(object sender, EventArgs e){}
        private void radioButton16_CheckedChanged(object sender, EventArgs e){}
        private void radioButton17_CheckedChanged(object sender, EventArgs e){}         
        private void radioButton18_CheckedChanged(object sender, EventArgs e){}
        private void radioButton19_CheckedChanged(object sender, EventArgs e){}
        private void radioButton20_CheckedChanged(object sender, EventArgs e) { }
        private void radioButton21_CheckedChanged(object sender, EventArgs e) { }
        private void radioButton22_CheckedChanged(object sender, EventArgs e) { }

        private void textBox1_TextChanged(object sender, EventArgs e) {}
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }
        private void textBox7_TextChanged(object sender, EventArgs e) { }
        private void textBox8_TextChanged(object sender, EventArgs e) { }
        private void textBox9_TextChanged(object sender, EventArgs e) { }

        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }
        private void groupBox12_Enter(object sender, EventArgs e) { }
     
        #endregion

        #region Unused Task 2 functions & About
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e){}
        private void pictureBox3_Click(object sender, EventArgs e){ }
        private void pictureBox4_Click(object sender, EventArgs e) { }
        #endregion

        #region Unused Task 3 functions
        private void VectorF_Click(object sender, EventArgs e) { }
        private void simpleOpenGlControl1_Load_1(object sender, EventArgs e) { }
        private void textBox16_TextChanged(object sender, EventArgs e) { }
        private void label20_Click(object sender, EventArgs e) { }
        private void label21_Click(object sender, EventArgs e) { }
        private void label22_Click(object sender, EventArgs e) { }
        private void groupBox19_Enter(object sender, EventArgs e) { }
        private void VectorFields_Load(object sender, EventArgs e) { }


        #endregion

        //---------------------------------------------


        //------------------ Task2 --------------------

        #region Image processing

        private void button3_Click(object sender, EventArgs e)
        {
            //Image processing load file button
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                InputImage = new Bitmap(Bitmap.FromFile(openFileDialog1.FileName), Bitmap.FromFile(openFileDialog1.FileName).Size.Width, Bitmap.FromFile(openFileDialog1.FileName).Size.Height);
                InputImage = Gray.ImageGrayscale(InputImage);

                pictureBox2.Image = InputImage;
                loadf = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (loadf)
            {
                pictureBox2.Image = BlurPicture.BlureMore(InputImage);
                InputImage = BlurPicture.BlureMore(InputImage);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (loadf)
            {
                pictureBox3.Image = Border.GetBorders(InputImage);
            }
        }

        #endregion

        //---------------------------------------------

        //------------------ Task3 --------------------
        private void InitGL3d()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glClearColor(0, 0, 0, 0);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glFrustum(-0.05, 0.05, -0.05, 0.05, 0.1, 200);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Glu.gluLookAt(0, 0, 0, 0, 0, -100, 0, 1, 0);
            Gl.glLoadIdentity();
            Gl.glTranslated(0, y_offset, -2 + z_offset);
            Gl.glRotated(x_rotated, 1, 0, 0);
            Gl.glRotated(y_rotated, 0, 1, 0);
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //Open File task 3;
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                fiload = openFileDialog2.FileName;
                
                loaded = true;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            x_rotated = 0;
            y_rotated = 0;
            y_offset = 0;
            z_offset = 0;
            RenderButton();
            VectorFields.Refresh();
        }


        private void RenderButton()
        {
            //Render          
            try
            {
                if (radioButton23.Checked) { LightONF = true; }
                if (radioButton24.Checked) { LightONF = false; }
                constant = int.Parse(textBox15.Text);
                timevalue = int.Parse(textBox14.Text);
                iX = int.Parse(textBox16.Text);
                iY = int.Parse(textBox17.Text);
                iZ = int.Parse(textBox18.Text);
            }
            catch (Exception ex)
            {
            }
            if (radioButton23.Checked) { LightONF = true; }
            if (radioButton24.Checked) { LightONF = false; }
            if (loaded)
            {
                Surface.SetParameters(iX, iY, iZ, -1, 1, -1, 1, -1, 1, constant, timevalue);
                Surface.ScanFile(fiload);
            }
            drawv = true;

           
            if (loaded)
            {
                Surface.CalculateSurface();
            }
            
        }
       
        private void RenderVectorVield()
        {
            //Gl.glScaled(0.6, 0.6, 0.6);
            Gl.glPushMatrix();
            
            Surface.DrawSurface(LightONF);
            Gl.glPopMatrix();
        }
        private void RenderCube()
        {
            //Generate coord cube
      


            Gl.glColor3f(1, 1, 1);
            
            Gl.glScaled(0.6, 0.6, 0.6);
            Gl.glPushMatrix();
            Gl.glBegin(Gl.GL_LINE_STRIP); 
           
            Gl.glVertex3f(-1, -1, -1);
            Gl.glVertex3f(-1, -1, 1);
            Gl.glVertex3f(-1, 1, 1);
            Gl.glVertex3f(-1, 1, -1);
            Gl.glVertex3f(-1, -1, -1);

            Gl.glVertex3f(1, -1, -1);
            Gl.glVertex3f(1, -1, 1);
            Gl.glVertex3f(1, 1, 1);
            Gl.glVertex3f(1, 1, -1);
            Gl.glVertex3f(1, -1, -1);
            Gl.glEnd();

            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3f(-1, -1, 1);
            Gl.glVertex3f(1, -1, 1);
            Gl.glVertex3f(-1, 1, -1);
            Gl.glVertex3f(1, 1, -1);
            Gl.glVertex3f(-1, 1, 1);
            Gl.glVertex3f(1, 1, 1);
            Gl.glEnd();


            Gl.glPopMatrix();
     
            

        }

        private void VectorFields_Paint_1(object sender, PaintEventArgs e)
        {
            InitGL3d();
            if (drawv)
            {


                RenderCube();
                if (loaded)
                {
                    RenderVectorVield();
                }
                Gl.glFlush();


            }
        }


        #region GL Mouse and Keys
        private void VectorFields_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w':
                    {
                        z_offset += 0.2;
                        VectorFields.Refresh();
                        break;
                    }
                case 's':
                    {
                        z_offset -= 0.2;
                        VectorFields.Refresh();
                        break;
                    }
                case 'd':
                    {
                        if (y_rotated == 360) { y_rotated = 0; }
                        y_rotated += 5;
                        label19.Text = y_rotated.ToString();
                        VectorFields.Refresh();
                        break;
                    }
                case 'a':
                    {
                        if (y_rotated == -360) { y_rotated = 0; }
                        y_rotated -= 5;
                        label19.Text = y_rotated.ToString();
                        VectorFields.Refresh();
                        break;
                    }
                case 'y':
                    {
                        if (x_rotated == 360) { x_rotated = 0; }
                        x_rotated+=5;
                        label18.Text = x_rotated.ToString();
                        VectorFields.Refresh();
                        break;
                    }
                case 'h':
                    {
                        if (x_rotated == -360) { x_rotated = 0; }
                        x_rotated -= 5;
                        label18.Text = x_rotated.ToString();
                        VectorFields.Refresh();
                        break;
                    }
                case '+':
                    {
                        constant += 0.1;
                        textBox15.Text = constant.ToString() ;
                        RenderButton();
                        VectorFields.Refresh();
                        break;
                    }
                case '-':
                    {
                        constant -= 0.1;
                        textBox15.Text = constant.ToString();
                        RenderButton();
                        VectorFields.Refresh();
                        break;
                    }
            }
        }
        private void VectorFields_KeyDown(object sender, KeyEventArgs e) { }
        private void VectorFields_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousex = e.X;
                mousey = e.Y;

            }
        }
        private void VectorFields_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                double dx = e.X - mousex;
                double dy = e.Y - mousey;

                if (x_rotated == -360 || x_rotated == 360) { x_rotated = 0; }
                if (y_rotated == -360 || y_rotated == 360) { y_rotated = 0; }
                x_rotated += dy;
                y_rotated += dx;
                label18.Text = x_rotated.ToString();
                label19.Text = y_rotated.ToString();
                mousey = e.Y;
                mousex = e.X;
                VectorFields.Refresh();
            }
        }
        #endregion

        #region GL Form Controls
        private void button13_Click(object sender, EventArgs e)
        {
            //Zoom in
            z_offset += 0.2;
            VectorFields.Refresh();
        }
        private void button14_Click(object sender, EventArgs e)
        {
            //Zoom out
            z_offset -= 0.2;
            VectorFields.Refresh();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            //Rotate clockwise X
            if (x_rotated == 360) { x_rotated = 0; }
            x_rotated += 5;
            label18.Text = x_rotated.ToString();
            VectorFields.Refresh();
        }
        private void button16_Click(object sender, EventArgs e)
        {
            if (x_rotated == -360) { x_rotated = 0; }
            x_rotated -= 5;
            label18.Text = x_rotated.ToString();
            VectorFields.Refresh();
        }
        private void button17_Click(object sender, EventArgs e)
        {
            //Rotate clockwise Y
            if (y_rotated == 360) { y_rotated = 0; }
            y_rotated += 5;
            label19.Text = y_rotated.ToString();
            VectorFields.Refresh();
        }
        private void button18_Click(object sender, EventArgs e)
        {
            //Rotate contrclockwise Y
            if (y_rotated == -360) { y_rotated = 0; }
            y_rotated -= 5;
            label19.Text = y_rotated.ToString();
            VectorFields.Refresh();
        }
        private void button19_Click(object sender, EventArgs e)
        {
            //Raise const
            constant += 0.1;
            textBox15.Text = constant.ToString();
            RenderButton();
            VectorFields.Refresh();
        }
        private void button20_Click(object sender, EventArgs e)
        {
            //Drop const
            constant -= 0.1;
            textBox15.Text = constant.ToString();
            RenderButton();
            VectorFields.Refresh();
        }
        private void button21_Click(object sender, EventArgs e)
        {
            //Up
            y_offset -= 0.2;
            VectorFields.Refresh();
        }
        private void button22_Click(object sender, EventArgs e)
        {
            //Down
            y_offset += 0.2;
            VectorFields.Refresh();
        }
        #endregion
    
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            //Stop redrawing if changing task
            drawv = false;
            s_fractal = false;
            i_fractal = false;
            j_fractal = false;
            m_fractal = false;
            w_fractal = false;
            w2_fractal = false;

       
        }
        private void radioButton23_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton23.Checked)
            {
                LightONF = true;
            }
            else
            {
                LightONF = false;
            }
            VectorFields.Refresh();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Fractal_GL_window.DestroyContexts();
            VectorFields.DestroyContexts();
            Fractal_GL_window.Dispose();
            VectorFields.Dispose();
        }


    }
}
