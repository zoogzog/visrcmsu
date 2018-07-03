using Tao.OpenGl;
using Tao.Platform.Windows;
using Tao.FreeGlut;
using System;
using System.Collections.Generic;
using System.IO;

using McubesTables;

namespace MCSurface
{

    public class Vertex3D
    {
        public float x, y, z;

        public Vertex3D()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        public Vertex3D(float X, float Y, float Z)
        {
            x = X;
            y = Y;
            z = Z;
        }
        public void TransVertex(double constant, Vertex3D v1, Vertex3D v2, float value1, float value2)
        {
            float trans;
            Vertex3D temporary = new Vertex3D();
            if (Math.Abs(value2 - value1) < 1e-6)
            {
                this.x = v1.x;
                this.y = v1.y;
                this.z = v1.z;
            }
            else
            {

                trans = ((float)constant - value1) / (value2 - value1);
                temporary.x = v1.x + trans * (v2.x - v1.x);
                temporary.y = v1.y + trans * (v2.y - v1.y);
                temporary.z = v1.z + trans * (v2.z - v1.z);

                this.x = temporary.x;
                this.y = temporary.y;
                this.z = temporary.z;
            }
        }
        public void NormalCount(Vertex3D v1, Vertex3D v2, Vertex3D v3)
        {
            Vertex3D a = new Vertex3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
            Vertex3D b = new Vertex3D(v1.x - v3.x, v1.y - v3.y, v1.z - v3.z);
            Vertex3D result = new Vertex3D(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
            result.Normalize();

            this.x = result.x;
            this.y = result.y;
            this.z = result.z;

        }
        public Vertex3D Normalize()
        {
            float squared = (float)Math.Sqrt(x * x + y * y + z * z);
            if (squared > 1e-5)
            {
                x = x / squared;
                y = y / squared;
                z = z / squared;
            }
            return this;
        }
        public void Set(float X, float Y, float Z)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
        }


    }



    public class Grid
    {
        private int Xsize, Ysize, Zsize;
        private float[,,] Data;
        private double minX, maxX, minY, maxY, minZ, maxZ;
        private double isoline;
        private int time;

        private int ID_List;

        private EdgeTable TabEdg;
        private PosTable TabPos;


        public Grid()
        {
            TabEdg = new EdgeTable();
            TabPos = new PosTable();
        }
        public void SetParameters(int xsize, int ysize, int zsize, double minx, double maxx, double miny, double maxy, double minz, double maxz, double cons, int t)
        {
            Xsize = xsize;
            Ysize = ysize;
            Zsize = zsize;
            minX = minx;
            maxX = maxx;
            minY = miny;
            maxY = maxy;
            minZ = minz;
            maxZ = maxz;
            Data = new float[Xsize+1, Ysize+1, Zsize+1];
            isoline = cons;
            time = t;
            ID_List = 1;
        }
        public void ScanFile (string filename)
        {

            if (filename.Contains(".res") || filename.Contains(".RES"))
            {
                FileStream Stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader BReader = new BinaryReader(Stream);


                //FileStream Stream1 = new FileStream("in2.txt", FileMode.Create, FileAccess.Write);
                //StreamWriter Reader1 = new StreamWriter(Stream1);

                for (int u = 0; u < time; u++)
                {
                    float temp = 0;
                    for (int i = 0; i < Xsize; i++)
                    {
                        for (int j = 0; j < Ysize; j++)
                        {
                            for (int m = 0; m < Zsize; m++)
                            {
                                temp = BReader.ReadSingle();
                                Data[i, j, m] = temp;
                                //Reader1.Write(temp + " ");
                            }
                        }
                    }
                }

                //Reader1.Close();
                //Stream1.Close();

                BReader.Close();
                Stream.Close();
            }
            if (filename.Contains(".txt") || filename.Contains(".TXT"))
            {
                FileStream Stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                StreamReader Reader = new StreamReader(Stream);

         
                float temp=0;

                for (int i=0; i<Xsize; i++)
                {
                    for (int j=0; j<Ysize; j++)
                    {
                        for (int m=0; m<Zsize; m++)
                        {
                            temp=Reader.Read();
                            Data[i,j,m]=temp;
                        }
                    }
                }

                Reader.Close();
                Stream.Close();
            }
        }
        private float getVertexValueX(int index)
        {
            return (float)(minX + index * (maxX - minX) / Xsize);
        }
        private float getVertexValueY(int index)
        {
            return (float)(minY + index * (maxY - minY) / Ysize);
        }
        private float getVertexValueZ(int index)
        {
            return (float)(minZ + index * (maxZ - minZ) / Zsize);
        }
        public void CalculateSurface()
        {
            float[] cube_cell = new float[8];
            Vertex3D[] cube_vertex3d = new Vertex3D[8];
            Vertex3D[] list_vertex3d = new Vertex3D[12];
            Vertex3D normal = new Vertex3D();
            for (int counter = 0; counter <= 7; counter++){cube_vertex3d[counter] = new Vertex3D();}
            for (int counter = 0; counter <= 11; counter++) { list_vertex3d[counter] = new Vertex3D(); }
                
            


            int temp1;
            
            Gl.glNewList(ID_List, Gl.GL_COMPILE);

            for (int i = 0; i < Xsize-1; i++)
            {
                for (int j = 0; j < Ysize-1; j++)
                {
                    for (int k = 0; k < Zsize-1; k++)
                    {

                        

                        cube_cell[0]=Data[i  , j  , k  ];
                        cube_cell[1]=Data[i+1, j  , k  ];
                        cube_cell[2]=Data[i+1, j+1, k  ];
                        cube_cell[3]=Data[i  , j+1, k  ];
                        cube_cell[4]=Data[i  , j  , k+1];
                        cube_cell[5]=Data[i+1, j  , k+1];
                        cube_cell[6]=Data[i+1, j+1, k+1];
                        cube_cell[7]=Data[i  , j+1, k+1];
                        
                        
                                
                        cube_vertex3d[0].Set(this.getVertexValueX(i),     this.getVertexValueY(j),     this.getVertexValueZ(k));
                        cube_vertex3d[1].Set(this.getVertexValueX(i + 1), this.getVertexValueY(j), this.getVertexValueZ(k));
                        cube_vertex3d[2].Set(this.getVertexValueX(i + 1), this.getVertexValueY(j + 1), this.getVertexValueZ(k));
                        cube_vertex3d[3].Set(this.getVertexValueX(i), this.getVertexValueY(j + 1), this.getVertexValueZ(k));
                        cube_vertex3d[4].Set(this.getVertexValueZ(i), this.getVertexValueY(j), this.getVertexValueZ(k + 1));
                        cube_vertex3d[5].Set(this.getVertexValueX(i + 1), this.getVertexValueY(j), this.getVertexValueZ(k + 1));
                        cube_vertex3d[6].Set(this.getVertexValueX(i + 1), this.getVertexValueY(j + 1), this.getVertexValueZ(k + 1));
                        cube_vertex3d[7].Set(this.getVertexValueX(i), this.getVertexValueY(j + 1), this.getVertexValueZ(k + 1));
                        
                        int cube_index = 0;
                        if (cube_cell[0] < isoline) { cube_index |= 1; }
                        if (cube_cell[1] < isoline) { cube_index |= 2; }
                        if (cube_cell[2] < isoline) { cube_index |= 4; }
                        if (cube_cell[3] < isoline) { cube_index |= 8; }
                        if (cube_cell[4] < isoline) { cube_index |= 16; }
                        if (cube_cell[5] < isoline) { cube_index |= 32; }
                        if (cube_cell[6] < isoline) { cube_index |= 64; }
                        if (cube_cell[7] < isoline) { cube_index |= 128; }
                        
                        if (TabEdg.Table[cube_index] != 0)
                        {
                            //Finding intersections 1-cube & surface

                            if ((TabEdg.Table[cube_index] & 1)!=0)    { list_vertex3d [0].TransVertex (isoline, cube_vertex3d[0], cube_vertex3d[1], cube_cell[0], cube_cell[1]); }
                            if ((TabEdg.Table[cube_index] & 2)!=0)    { list_vertex3d [1].TransVertex(isoline, cube_vertex3d[1], cube_vertex3d[2], cube_cell[1], cube_cell[2]); }
                            if ((TabEdg.Table[cube_index] & 4)!=0)    { list_vertex3d [2].TransVertex(isoline, cube_vertex3d[2], cube_vertex3d[3], cube_cell[2], cube_cell[3]); }
                            if ((TabEdg.Table[cube_index] & 8)!=0)    { list_vertex3d [3].TransVertex(isoline, cube_vertex3d[3], cube_vertex3d[0], cube_cell[3], cube_cell[0]); }
                            if ((TabEdg.Table[cube_index] & 16)!=0)   { list_vertex3d [4].TransVertex(isoline, cube_vertex3d[4], cube_vertex3d[5], cube_cell[4], cube_cell[5]); }
                            if ((TabEdg.Table[cube_index] & 32)!=0)   { list_vertex3d [5].TransVertex(isoline, cube_vertex3d[5], cube_vertex3d[6], cube_cell[5], cube_cell[6]); }
                            if ((TabEdg.Table[cube_index] & 64)!=0)   { list_vertex3d [6].TransVertex(isoline, cube_vertex3d[6], cube_vertex3d[7], cube_cell[6], cube_cell[7]); }
                            if ((TabEdg.Table[cube_index] & 128)!=0)  { list_vertex3d [7].TransVertex(isoline, cube_vertex3d[7], cube_vertex3d[4], cube_cell[7], cube_cell[4]); }
                            if ((TabEdg.Table[cube_index] & 256)!=0)  { list_vertex3d [8].TransVertex(isoline, cube_vertex3d[0], cube_vertex3d[4], cube_cell[0], cube_cell[4]); }
                            if ((TabEdg.Table[cube_index] & 512)!=0)  { list_vertex3d [9].TransVertex(isoline, cube_vertex3d[1], cube_vertex3d[5], cube_cell[1], cube_cell[5]); }
                            if ((TabEdg.Table[cube_index] & 1024)!=0) { list_vertex3d [10].TransVertex(isoline, cube_vertex3d[2], cube_vertex3d[6], cube_cell[2], cube_cell[6]); }
                            if ((TabEdg.Table[cube_index] & 2048)!=0) { list_vertex3d [11].TransVertex(isoline, cube_vertex3d[3], cube_vertex3d[7], cube_cell[3], cube_cell[7]); }

                            temp1=0;
                            Gl.glColor3f(1, 1, 0);
                            for (int q=0; TabPos.Matrix[cube_index,q]!=-1; q+=3)
                            {
                                float mult=1.0f;
                                normal.NormalCount(list_vertex3d[TabPos.Matrix[cube_index,q]],list_vertex3d[TabPos.Matrix[cube_index,q+1]],list_vertex3d[TabPos.Matrix[cube_index,q+2]]);
                                float x, y, z;
                                float x1, y1, z1;
                                float x2, y2, z2;
                                float x3, y3, z3;
                                float nx, ny, nz;
                              
                                x=normal.x*mult;
                                y=normal.y*mult;
                                z=normal.z*mult;

                                nx = x; ny = y; nz = z;

                                x=list_vertex3d[TabPos.Matrix[cube_index,q]].x;
                                y=list_vertex3d[TabPos.Matrix[cube_index,q]].y;
                                z=list_vertex3d[TabPos.Matrix[cube_index,q]].z;
                                x1 = x; y1 = y; z1 = z;

                                x = list_vertex3d[TabPos.Matrix[cube_index, q + 1]].x;
                                y = list_vertex3d[TabPos.Matrix[cube_index, q + 1]].y;
                                z = list_vertex3d[TabPos.Matrix[cube_index, q + 1]].z;
                                x2 = x; y2 = y; z2 = z;

                                x = list_vertex3d[TabPos.Matrix[cube_index, q + 2]].x;
                                y = list_vertex3d[TabPos.Matrix[cube_index, q + 2]].y;
                                z = list_vertex3d[TabPos.Matrix[cube_index, q + 2]].z;
                                x3 = x; y3 = y; z3 = z;
                                                               
                                Gl.glBegin(Gl.GL_TRIANGLES);

                                Gl.glNormal3f(nx, ny, nz);
                                Gl.glVertex3f(x1, y1, z1);
                                Gl.glVertex3f(x2, y2, z2);
                                Gl.glVertex3f(x3, y3, z3);

                                Gl.glEnd();

                                temp1++;
                                
                            
                            }
                        }
                        
                    }
                }
            }


           Gl.glEndList();

        }
        public void DrawSurface(bool enabled)
        {
            if (enabled)
            {
                Gl.glEnable(Gl.GL_LIGHTING);
                Gl.glEnable(Gl.GL_LIGHT0);

                float[] diffuse0 = new float[4] { 0.2f, 0.5f, 0.9f, 1 };
                float[] specular0 = new float[4] { 1, 0, 0, 1 };
                float[] ambient0 = new float[4] { 0.5f, 0.5f, 0.5f, 1 };
                //float[] lightpos0 = new float[4] { 10, 10, 10, 0 };
                //Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lightpos0);
                Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, diffuse0);
                Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, specular0);
                Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, ambient0);
            }
                
            Gl.glCallList(ID_List);

            if (enabled)
            {
                Gl.glDisable(Gl.GL_LIGHT0);
                Gl.glDisable(Gl.GL_LIGHTING);
            }

        }

    }
}