using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

using System.Net.Sockets;

using OpenCLTemplate;
using OpenCLTemplate.CLGLInterop;
using OpenTK.Graphics.OpenGL;

namespace GlRender
{
    public partial class Form1 : Form
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        Stopwatch stopWatch = new Stopwatch();
        float [] receiveFloat = new float [100]; 

        public Form1()
        {
            InitializeComponent();
  /*          clientSocket.Connect("127.0.0.1", 8888);
            NetworkStream serverStream = clientSocket.GetStream();
            textBox3.Text = "Client Socket Program - Server Connected ..." + Environment.NewLine;

            int startIndex = 0;
            int n = 4;

            for (int i = 0; i < n; i++)
            {
               
                byte[] inStream = new byte[4];
                serverStream.Read(inStream, 0, 4);

                float myFloat = System.BitConverter.ToSingle(inStream, startIndex);

                receiveFloat[i] = myFloat;

                textBox3.Text += receiveFloat[i] + Environment.NewLine;
            }
 */       }

       

        String updates = "*** Sequence Begins ***\r\n";
        String title = "Transfer Arm Operation Simulation";

        int h = 400; //700;
        int w = 500; //750;

        private void Form1_Load(object sender, EventArgs e)
        {
            stopWatch.Start();
                // Maximize Window
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;

            // Set Title
            textBox2.Font = new Font(textBox1.Font.FontFamily, 22);
            textBox2.TextAlign = HorizontalAlignment.Center;
            textBox2.Text = title;

            // Set guide tube label
            label1.Font = new Font(textBox1.Font.FontFamily, 10);

            // Set outer tube label
            label2.Font = new Font(textBox1.Font.FontFamily, 10);

            // Set inner tube label
            label3.Font = new Font(textBox1.Font.FontFamily, 10);

            CLGLWindow = new GLRender(this, true, -1);
            CLGLWindow.DrawAxes = false;

            //Integer values of width and height, defined statically
            //h = 400; //700;
            //w = 500; //750;

            //reduces values of width and height proportionally (division by 8) to fit them onto the screen

            // Guide Tube Arrow

            float[] vgta = new float[] {
                0,(float)-27.5,-(float)2,
                0,(float)-27.5, (float)2,
                0,(float) 27.5, (float)2,
                0,(float) 27.5,-(float)2
            };

            //Sets texture coordinates
            // Values set below are for plain appearance
            float[] texCoord = new float[] {
                0,1,
                0,0,
                1,0,
                1,1,
            };

            // normal data indicates the orientation of the surface - here, we have used 2d so normal is set to the unused z-axis

            float[] n = new float[] {
                0,0,1,
                0,0,1,
                0,0,1,
                0,0,1
            };

            // Set colour

            float[] cgta = new float[] {
                220/255.0f, 20/255.0f, 60/255.0f, 1.0f,
                220/255.0f, 20/255.0f, 60/255.0f, 1.0f,
                220/255.0f, 20/255.0f, 60/255.0f, 1.0f,
                220/255.0f, 20/255.0f, 60/255.0f, 1.0f
            };

            // uses points 0,1,2,3 as vertexes i.e it's Sequential only
            int[] elem = new int[] { 0, 1, 2, 3 };

            GTA = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            GTA.SetVertexData(vgta);
            GTA.SetTexCoordData(texCoord);
            GTA.SetNormalData(n);
            GTA.SetElemData(elem);
            GTA.SetColorData(cgta);

            GTA.vetTransl = new Vector(0.0, 100.0, 0.0);

            // Guide Tube

            float[] vg1 = new float[] {
                0,(float)-7.5,-(float)60,
                0,(float)-7.5, (float)60,
                0,(float) 7.5, (float)60,
                0,(float) 7.5,-(float)60
            };

            //Sets texture coordinates
            // Values set below are for plain appearance
            texCoord = new float[] {
                0,1,
                0,0,
                1,0,
                1,1,
            };

            // normal data indicates the orientation of the surface - here, we have used 2d so normal is set to the unused z-axis

            n = new float[] {
                0,0,1,
                0,0,1,
                0,0,1,
                0,0,1
            };

            // Set colour

            float[] cg = new float[] {
                176/255.0f, 226/255.0f, 255/255.0f, 1.0f,
                176/255.0f, 226/255.0f, 255/255.0f, 1.0f,
                176/255.0f, 226/255.0f, 255/255.0f, 1.0f,
                176/255.0f, 226/255.0f, 255/255.0f, 1.0f
            };

            // uses points 0,1,2,3 as vertexes i.e it's Sequential only
            elem = new int[] { 0, 1, 2, 3 };

            GT1 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            GT1.SetVertexData(vg1);
            GT1.SetTexCoordData(texCoord);
            GT1.SetNormalData(n);
            GT1.SetElemData(elem);
            GT1.SetColorData(cg);

            GT1.vetTransl = new Vector(0.0, 0.0, 80.0);

            //            GT1.vetRot = new Vector(20.0, -10.0, -6.0);

            

            float [] vg2 = new float[] {
                0,(float)-16.25,-(float)10,
                0,(float)-16.25, (float)10,
                0,(float) 16.25, (float)10,
                0,(float) 16.25,-(float)10
            };

            GT2 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            GT2.SetVertexData(vg2);
            GT2.SetTexCoordData(texCoord);
            GT2.SetNormalData(n);
            GT2.SetElemData(elem);
            GT2.SetColorData(cg);

            GT2.vetTransl = new Vector(0.0, 23.0, 30.0);

            // Outer Container

            // OC1

            float[] voc1 = new float[] {
                0,(float)-45,-(float)50,
                0,(float)-45, (float)50,
                0,(float) 45, (float)50,
                0,(float) 45,-(float)50
            };

            float[] coc1 = new float[] {
                255/255.0f, 255/255.0f, 255/255.0f, 1.0f,
                255/255.0f, 255/255.0f, 255/255.0f, 1.0f,
                255/255.0f, 255/255.0f, 255/255.0f, 1.0f,
                255/255.0f, 255/255.0f, 255/255.0f, 1.0f
            };

            OC1 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OC1.SetVertexData(voc1);
            OC1.SetTexCoordData(texCoord);
            OC1.SetNormalData(n);
            OC1.SetElemData(elem);
            OC1.SetColorData(coc1);

            OC1.vetTransl = new Vector(0.0, 5.0, -12.5);

            CLGLWindow.Models.Add(OC1);

            // OC2 - rotated

            float[] voc2 = new float[] {
                0,(float)-10,-(float)40,
                0,(float)-10, (float)40,
                0,(float) 10, (float)40,
                0,(float) 10,-(float)40
            };

            OC2 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OC2.SetVertexData(voc2);
            OC2.SetTexCoordData(texCoord);
            OC2.SetNormalData(n);
            OC2.SetElemData(elem);
            OC2.SetColorData(coc1);

//            OC2.vetRot = new Vector(-10.0, 0.0, 0.0);

            OC2.vetRot = new Vector(0.0, 10.5, 0.0);
            OC2.vetTransl = new Vector(0.0, 64.0, 0.0);

            CLGLWindow.Models.Add(OC2);

            // OC3

            float[] voc3 = new float[] {
                0,(float)-45,-(float)3,
                0,(float)-45, (float)3,
                0,(float) 45, (float)3,
                0,(float) 45,-(float)3
            };

            float[] coc2 = new float[] {
                205/255.0f, 105/255.0f, 201/255.0f, 1.0f,
                205/255.0f, 105/255.0f, 201/255.0f, 1.0f,
                205/255.0f, 105/255.0f, 201/255.0f, 1.0f,
                205/255.0f, 105/255.0f, 201/255.0f, 1.0f
            };

            OC3 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OC3.SetVertexData(voc3);
            OC3.SetTexCoordData(texCoord);
            OC3.SetNormalData(n);
            OC3.SetElemData(elem);
            OC3.SetColorData(coc2);

            OC3.vetTransl = new Vector(0.0, 5.0, 40.5);

            CLGLWindow.Models.Add(OC3);

            // OC4

            float[] voc4 = new float[] {
                0,(float)-12,-(float)3,
                0,(float)-12, (float)3,
                0,(float) 12, (float)3,
                0,(float) 12,-(float)3
            };

            OC4 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OC4.SetVertexData(voc4);
            OC4.SetTexCoordData(texCoord);
            OC4.SetNormalData(n);
            OC4.SetElemData(elem);
            OC4.SetColorData(coc2);

            OC4.vetTransl = new Vector(0.0, 76.7, 25.5);

            CLGLWindow.Models.Add(OC4);
            CLGLWindow.Models.Add(GT1);
            CLGLWindow.Models.Add(GT2);

            // Outer Shield

            // OS1

            float[] vos1 = new float[] {
                0,(float)-2.0,-(float)8,
                0,(float)-2.0, (float)8,
                0,(float) 2.0, (float)8,
                0,(float) 2.0,-(float)8
            };

            float[] cos = new float[] {
                0/255.0f, 255/255.0f, 0/255.0f, 1.0f,
                0/255.0f, 255/255.0f, 0/255.0f, 1.0f,
                0/255.0f, 255/255.0f, 0/255.0f, 1.0f,
                0/255.0f, 255/255.0f, 0/255.0f, 1.0f
            };

            OS1 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OS1.SetVertexData(vos1);
            OS1.SetTexCoordData(texCoord);
            OS1.SetNormalData(n);
            OS1.SetElemData(elem);
            OS1.SetColorData(cos);

            OS1.vetTransl = new Vector(0.0, 9.6, 51.6);
            CLGLWindow.Models.Add(OS1);

            // OS2


            OS2 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OS2.SetVertexData(vos1);
            OS2.SetTexCoordData(texCoord);
            OS2.SetNormalData(n);
            OS2.SetElemData(elem);
            OS2.SetColorData(cos);

            OS2.vetTransl = new Vector(0.0, -9.6, 51.6);
            CLGLWindow.Models.Add(OS2);

            // OS3


            float[] vos2 = new float[] {
                0,(float)-8.0,-(float)1.5,
                0,(float)-8.0, (float)1.5,
                0,(float) 8.0, (float)1.5,
                0,(float) 8.0,-(float)1.5
            };

            OS3 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OS3.SetVertexData(vos2);
            OS3.SetTexCoordData(texCoord);
            OS3.SetNormalData(n);
            OS3.SetElemData(elem);
            OS3.SetColorData(cos);

            OS3.vetTransl = new Vector(0.0, -15.6, 45.0);
            CLGLWindow.Models.Add(OS3);

            // OS4


            OS4 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OS4.SetVertexData(vos2);
            OS4.SetTexCoordData(texCoord);
            OS4.SetNormalData(n);
            OS4.SetElemData(elem);
            OS4.SetColorData(cos);

            OS4.vetTransl = new Vector(0.0, 15.6, 45.0);
            CLGLWindow.Models.Add(OS4);

            // OS5


            float[] vos3 = new float[] {
                0,(float)-2.5,-(float)1.5,
                0,(float)-2.5, (float)1.5,
                0,(float) 2.5, (float)1.5,
                0,(float) 2.5,-(float)1.5
            };

            OS5 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OS5.SetVertexData(vos3);
            OS5.SetTexCoordData(texCoord);
            OS5.SetNormalData(n);
            OS5.SetElemData(elem);
            OS5.SetColorData(cos);

            OS5.vetTransl = new Vector(0.0, -14.15, 48.1);
            CLGLWindow.Models.Add(OS5);

            // OS6


            OS6 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OS6.SetVertexData(vos3);
            OS6.SetTexCoordData(texCoord);
            OS6.SetNormalData(n);
            OS6.SetElemData(elem);
            OS6.SetColorData(cos);

            OS6.vetTransl = new Vector(0.0, 14.15, 48.1);
            CLGLWindow.Models.Add(OS6);

            // OS7


            float[] vos4 = new float[] {
                0,(float)-1.5,-(float)1.0,
                0,(float)-1.5, (float)1.0,
                0,(float) 1.5, (float)1.0,
                0,(float) 1.5,-(float)1.0
            };

            OS7 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OS7.SetVertexData(vos4);
            OS7.SetTexCoordData(texCoord);
            OS7.SetNormalData(n);
            OS7.SetElemData(elem);
            OS7.SetColorData(cos);

            OS7.vetTransl = new Vector(0.0, -13.15, 58.5);
            CLGLWindow.Models.Add(OS7);

            // OS8


            OS8 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OS8.SetVertexData(vos4);
            OS8.SetTexCoordData(texCoord);
            OS8.SetNormalData(n);
            OS8.SetElemData(elem);
            OS8.SetColorData(cos);

            OS8.vetTransl = new Vector(0.0, 13.15, 58.5);
            CLGLWindow.Models.Add(OS8);


            //FSAs

            // FSA0

            float[] vfsa = new float[] {
                0,(float)-3,-(float)18,
                0,(float)-3, (float)18,
                0,(float) 3, (float)18,
                0,(float) 3,-(float)18
            };

            float[] cfsa = new float[] {
                255/255.0f, 0/255.0f, 0/255.0f, 1.0f,
                255/255.0f, 0/255.0f, 0/255.0f, 1.0f,
                255/255.0f, 0/255.0f, 0/255.0f, 1.0f,
                255/255.0f, 0/255.0f, 0/255.0f, 1.0f
            };

            FSA0 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSA0.SetVertexData(vfsa);
            FSA0.SetTexCoordData(texCoord);
            FSA0.SetNormalData(n);
            FSA0.SetElemData(elem);
            FSA0.SetColorData(cfsa);

            FSA0.vetTransl = new Vector(0.0, 29.0, -40.0);
            CLGLWindow.Models.Add(FSA0);

            // FSA1

            
            FSA1 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSA1.SetVertexData(vfsa);
            FSA1.SetTexCoordData(texCoord);
            FSA1.SetNormalData(n);
            FSA1.SetElemData(elem);
            FSA1.SetColorData(cfsa);

            FSA1.vetTransl = new Vector(0.0, 22.5, -40.0);
            CLGLWindow.Models.Add(FSA1);

            // FSA2


            FSA2 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSA2.SetVertexData(vfsa);
            FSA2.SetTexCoordData(texCoord);
            FSA2.SetNormalData(n);
            FSA2.SetElemData(elem);
            FSA2.SetColorData(cfsa);

            FSA2.vetTransl = new Vector(0.0, 16.0, -40.0);
            CLGLWindow.Models.Add(FSA2);

            // FSA3


            FSA3 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSA3.SetVertexData(vfsa);
            FSA3.SetTexCoordData(texCoord);
            FSA3.SetNormalData(n);
            FSA3.SetElemData(elem);
            FSA3.SetColorData(cfsa);

            FSA3.vetTransl = new Vector(0.0, 9.5, -40.0);
            CLGLWindow.Models.Add(FSA3);

            // FSA4


            FSA4 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSA4.SetVertexData(vfsa);
            FSA4.SetTexCoordData(texCoord);
            FSA4.SetNormalData(n);
            FSA4.SetElemData(elem);
            FSA4.SetColorData(cfsa);

            FSA4.vetTransl = new Vector(0.0, 3.0, -40.0);
            CLGLWindow.Models.Add(FSA4);

            // FSA5


            FSA5 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSA5.SetVertexData(vfsa);
            FSA5.SetTexCoordData(texCoord);
            FSA5.SetNormalData(n);
            FSA5.SetElemData(elem);
            FSA5.SetColorData(cfsa);

            FSA5.vetTransl = new Vector(0.0, -3.5, -40.0);
            CLGLWindow.Models.Add(FSA5);

            // FSA6


            FSA6 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSA6.SetVertexData(vfsa);
            FSA6.SetTexCoordData(texCoord);
            FSA6.SetNormalData(n);
            FSA6.SetElemData(elem);
            FSA6.SetColorData(cfsa);

            FSA6.vetTransl = new Vector(0.0, -10.0, -40.0);
            CLGLWindow.Models.Add(FSA6);

            // FSA7


            FSA7 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSA7.SetVertexData(vfsa);
            FSA7.SetTexCoordData(texCoord);
            FSA7.SetNormalData(n);
            FSA7.SetElemData(elem);
            FSA7.SetColorData(cfsa);

            FSA7.vetTransl = new Vector(0.0, -16.5, -40.0);
            CLGLWindow.Models.Add(FSA7);

            // FSA8


            FSA8 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSA8.SetVertexData(vfsa);
            FSA8.SetTexCoordData(texCoord);
            FSA8.SetNormalData(n);
            FSA8.SetElemData(elem);
            FSA8.SetColorData(cfsa);

            FSA8.vetTransl = new Vector(0.0, -54.5, -37.0);
            CLGLWindow.Models.Add(FSA8);

 
            // FSA CAPS

            // FSACAP0

            float[] vfsacap = new float[] {
                0,(float)-3,-(float)3,
                0,(float)-3, (float)3,
                0,(float) 3, (float)3,
                0,(float) 3,-(float)3
            };

            float[] cfsacap = new float[] {
                205/255.0f, 105/255.0f, 201/255.0f, 1.0f,
                205/255.0f, 105/255.0f, 201/255.0f, 1.0f,
                205/255.0f, 105/255.0f, 201/255.0f, 1.0f,
                205/255.0f, 105/255.0f, 201/255.0f, 1.0f
            };


            FSACAP0 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSACAP0.SetVertexData(vfsacap);
            FSACAP0.SetTexCoordData(texCoord);
            FSACAP0.SetNormalData(n);
            FSACAP0.SetElemData(elem);
            FSACAP0.SetColorData(cfsacap);

            FSACAP0.vetTransl = new Vector(0.0, 29.0, -19.0);
            CLGLWindow.Models.Add(FSACAP0);

            // FSACAP1


            FSACAP1 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSACAP1.SetVertexData(vfsacap);
            FSACAP1.SetTexCoordData(texCoord);
            FSACAP1.SetNormalData(n);
            FSACAP1.SetElemData(elem);
            FSACAP1.SetColorData(cfsacap);

            FSACAP1.vetTransl = new Vector(0.0, 22.5, -19.0);
            CLGLWindow.Models.Add(FSACAP1);

            // FSACAP2


            FSACAP2 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSACAP2.SetVertexData(vfsacap);
            FSACAP2.SetTexCoordData(texCoord);
            FSACAP2.SetNormalData(n);
            FSACAP2.SetElemData(elem);
            FSACAP2.SetColorData(cfsacap);

            FSACAP2.vetTransl = new Vector(0.0, 16.0, -19.0);
            CLGLWindow.Models.Add(FSACAP2);

            // FSACAP3


            FSACAP3 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSACAP3.SetVertexData(vfsacap);
            FSACAP3.SetTexCoordData(texCoord);
            FSACAP3.SetNormalData(n);
            FSACAP3.SetElemData(elem);
            FSACAP3.SetColorData(cfsacap);

            FSACAP3.vetTransl = new Vector(0.0, 9.5, -19.0);
            CLGLWindow.Models.Add(FSACAP3);

            // FSACAP4


            FSACAP4 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSACAP4.SetVertexData(vfsacap);
            FSACAP4.SetTexCoordData(texCoord);
            FSACAP4.SetNormalData(n);
            FSACAP4.SetElemData(elem);
            FSACAP4.SetColorData(cfsacap);

            FSACAP4.vetTransl = new Vector(0.0, 3.0, -19.0);
            CLGLWindow.Models.Add(FSACAP4);

            // FSACAP5


            FSACAP5 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSACAP5.SetVertexData(vfsacap);
            FSACAP5.SetTexCoordData(texCoord);
            FSACAP5.SetNormalData(n);
            FSACAP5.SetElemData(elem);
            FSACAP5.SetColorData(cfsacap);

            FSACAP5.vetTransl = new Vector(0.0, -3.5, -19.0);
            CLGLWindow.Models.Add(FSACAP5);

            // FSACAP6


            FSACAP6 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSACAP6.SetVertexData(vfsacap);
            FSACAP6.SetTexCoordData(texCoord);
            FSACAP6.SetNormalData(n);
            FSACAP6.SetElemData(elem);
            FSACAP6.SetColorData(cfsacap);

            FSACAP6.vetTransl = new Vector(0.0, -10.0, -19.0);
            CLGLWindow.Models.Add(FSACAP6);

            // FSACAP7


            FSACAP7 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSACAP7.SetVertexData(vfsacap);
            FSACAP7.SetTexCoordData(texCoord);
            FSACAP7.SetNormalData(n);
            FSACAP7.SetElemData(elem);
            FSACAP7.SetColorData(cfsacap);

            FSACAP7.vetTransl = new Vector(0.0, -16.5, -19.0);
            CLGLWindow.Models.Add(FSACAP7);

            // FSACAP8


            FSACAP8 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FSACAP8.SetVertexData(vfsacap);
            FSACAP8.SetTexCoordData(texCoord);
            FSACAP8.SetNormalData(n);
            FSACAP8.SetElemData(elem);
            FSACAP8.SetColorData(cfsacap);

            FSACAP8.vetTransl = new Vector(0.0, -54.5, -17.0);
            CLGLWindow.Models.Add(FSACAP8);


            // Outer tube code

            // OT1

            float[] vo1 = new float[] {
                0,(float)-4.2,-(float)47,
                0,(float)-4.2, (float)47,
                0,(float) 4.2, (float)47,
                0,(float) 4.2,-(float)47
            };

            float[] co = new float[] {
                255/255.0f, 255/255.0f, 0/255.0f, 1.0f,
                255/255.0f, 255/255.0f, 0/255.0f, 1.0f,
                255/255.0f, 255/255.0f, 0/255.0f, 1.0f,
                255/255.0f, 255/255.0f, 0/255.0f, 1.0f
            };

            OT1 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OT1.SetVertexData(vo1);
            OT1.SetTexCoordData(texCoord);
            OT1.SetNormalData(n);
            OT1.SetElemData(elem);
            OT1.SetColorData(co);

            OT1.vetTransl = new Vector(0.0, 0.0, 78.5);
            CLGLWindow.Models.Add(OT1);


            // OT2

            float[] vo2 = new float[] {
                0,(float)-15.25,-(float)3,
                0,(float)-15.25, (float)3,
                0,(float) 15.25, (float)3,
                0,(float) 15.25,-(float)3
            };

            OT2 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OT2.SetVertexData(vo2);
            OT2.SetTexCoordData(texCoord);
            OT2.SetNormalData(n);
            OT2.SetElemData(elem);
            OT2.SetColorData(co);

            OT2.vetTransl = new Vector(0.0, 17.5, 34.5);
            CLGLWindow.Models.Add(OT2);

            // OT3

            float[] vo3 = new float[] {
                0,(float)-4,-(float)6,
                0,(float)-4, (float)6,
                0,(float) 4, (float)6,
                0,(float) 4,-(float)6
            };

            OT3 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            OT3.SetVertexData(vo3);
            OT3.SetTexCoordData(texCoord);
            OT3.SetNormalData(n);
            OT3.SetElemData(elem);
            OT3.SetColorData(co);

            OT3.vetTransl = new Vector(0.0, 28.7, 27.0);
            CLGLWindow.Models.Add(OT3);

            // Inner tube code

            // IT1

            float[] vi1 = new float[] {
                0,(float)-1.9,-(float)50,
                0,(float)-1.9, (float)50,
                0,(float) 1.9, (float)50,
                0,(float) 1.9,-(float)50
            };

            float[] ci = new float[] {
                238/255.0f, 162/255.0f, 193/255.0f, 1.0f,
                238/255.0f, 162/255.0f, 193/255.0f, 1.0f,
                238/255.0f, 162/255.0f, 193/255.0f, 1.0f,
                238/255.0f, 162/255.0f, 193/255.0f, 1.0f
            };

            IT1 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            IT1.SetVertexData(vi1);
            IT1.SetTexCoordData(texCoord);
            IT1.SetNormalData(n);
            IT1.SetElemData(elem);
            IT1.SetColorData(ci);

            IT1.vetTransl = new Vector(0.0, 0.0, 76.5);
            CLGLWindow.Models.Add(IT1);

            // IT2

            float[] vi2 = new float[] {
                0,(float)-13.25,-(float)1.5,
                0,(float)-13.25, (float)1.5,
                0,(float) 13.25, (float)1.5,
                0,(float) 13.25,-(float)1.5
            };

            IT2 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            IT2.SetVertexData(vi2);
            IT2.SetTexCoordData(texCoord);
            IT2.SetNormalData(n);
            IT2.SetElemData(elem);
            IT2.SetColorData(ci);

            IT2.vetTransl = new Vector(0.0, 14.5, 28);
            CLGLWindow.Models.Add(IT2);

            // IT3

            float[] vi3 = new float[] {
                0,(float)-2,-(float)4.2,
                0,(float)-2, (float)4.2,
                0,(float) 2, (float)4.2,
                0,(float) 2,-(float)4.2
            };

            IT3 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            IT3.SetVertexData(vi3);
            IT3.SetTexCoordData(texCoord);
            IT3.SetNormalData(n);
            IT3.SetElemData(elem);
            IT3.SetColorData(ci);

            IT3.vetTransl = new Vector(0.0, 28.7, 25.25);
            CLGLWindow.Models.Add(IT3);

            // Finger open code

            // FO1

            float[] fo1 = new float[] {
                0,(float)-1.5,-(float)0.8,
                0,(float)-1.5, (float)0.8,
                0,(float) 1.5, (float)0.8,
                0,(float) 1.5,-(float)0.8
            };

            float[] cfo = new float[] {
                0/255.0f, 255/255.0f, 0/255.0f, 1.0f,
                0/255.0f, 255/255.0f, 0/255.0f, 1.0f,
                0/255.0f, 255/255.0f, 0/255.0f, 1.0f,
                0/255.0f, 255/255.0f, 0/255.0f, 1.0f
            };


            FO1 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
            FO1.SetVertexData(fo1);
            FO1.SetTexCoordData(texCoord);
            FO1.SetNormalData(n);
            FO1.SetElemData(elem);
            FO1.SetColorData(cfo);

            //FO2

            FO2 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.Quads);
         //   FO2 = new GLRender.GLVBOModel(OpenTK.Graphics.OpenGL.BeginMode.);
            FO2.SetVertexData(fo1);
            FO2.SetTexCoordData(texCoord);
            FO2.SetNormalData(n);
            FO2.SetElemData(elem);
            FO2.SetColorData(cfo);

            TimeSpan ts = stopWatch.Elapsed;


            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }
        
        double transX;
        double transY;
        double transZ;

        GLRender CLGLWindow;
        GLRender.GLVBOModel GT1;
        GLRender.GLVBOModel GT2;
        GLRender.GLVBOModel OT1;
        GLRender.GLVBOModel OT2;
        GLRender.GLVBOModel OT3;
        GLRender.GLVBOModel IT1;
        GLRender.GLVBOModel IT2;
        GLRender.GLVBOModel IT3;
        GLRender.GLVBOModel OC1;
        GLRender.GLVBOModel OC2;
        GLRender.GLVBOModel OC3;
        GLRender.GLVBOModel OC4;
        GLRender.GLVBOModel OS1;
        GLRender.GLVBOModel OS2;
        GLRender.GLVBOModel OS3;
        GLRender.GLVBOModel OS4;
        GLRender.GLVBOModel OS5;
        GLRender.GLVBOModel OS6;
        GLRender.GLVBOModel OS7;
        GLRender.GLVBOModel OS8;
        GLRender.GLVBOModel FSA0;
        GLRender.GLVBOModel FSA1;
        GLRender.GLVBOModel FSA2;
        GLRender.GLVBOModel FSA3;
        GLRender.GLVBOModel FSA4;
        GLRender.GLVBOModel FSA5;
        GLRender.GLVBOModel FSA6;
        GLRender.GLVBOModel FSA7;
        GLRender.GLVBOModel FSA8;
        GLRender.GLVBOModel FSACAP0;
        GLRender.GLVBOModel FSACAP1;
        GLRender.GLVBOModel FSACAP2;
        GLRender.GLVBOModel FSACAP3;
        GLRender.GLVBOModel FSACAP4;
        GLRender.GLVBOModel FSACAP5;
        GLRender.GLVBOModel FSACAP6;
        GLRender.GLVBOModel FSACAP7;
        GLRender.GLVBOModel FSACAP8;
        GLRender.GLVBOModel FO1;
        GLRender.GLVBOModel FO2;
        GLRender.GLVBOModel GTA;
        GLRender.GLVBOModel OTA;
        GLRender.GLVBOModel ITA;


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            transZ = 80.0;
            for (int i = 0; i < 200; i++)
            {
                Application.DoEvents(); 
                                                                                                                                                                                           Thread.Sleep(20);
                transZ = transZ - 0.1;
                try
                {
                    GT1.vetTransl = new Vector(0.0, 0.0, (20.0 + transZ));
                    GT2.vetTransl = new Vector(0.0, 23.0, transZ);
                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            transZ = 27.0;
            for (int i = 0; i < 200; i++)
            {
                Application.DoEvents();
                                                                                                                                                                                            Thread.Sleep(20);
                transZ = transZ - 0.1;
                try
                {
                    OT1.vetTransl = new Vector(0.0, 0.0, (51.0 + transZ));
                    OT2.vetTransl = new Vector(0.0, 17.5, (7.0 + transZ));
                    OT3.vetTransl = new Vector(0.0, 28.7, transZ);
                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            transZ = 25.5;
            for (int i = 0; i < 200; i++)
            {
                Application.DoEvents();                                                                                                                                                   Thread.Sleep(20);
                transZ = transZ - 0.1;
                try
                {
                    IT1.vetTransl = new Vector(0.0, 0.0, (51.25 + transZ));
                    IT2.vetTransl = new Vector(0.0, 14.5, (2.75 + transZ));
                    IT3.vetTransl = new Vector(0.0, 28.7, transZ);
                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }
        }

        System.Windows.Forms.ToolTip ToolTip4 = new System.Windows.Forms.ToolTip();
       

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

            transZ = 30.0;
            textBox1.Font = new Font(textBox1.Font.FontFamily, 16);

            textBox1.Text = updates;
            Thread.Sleep(800);
            
            // GT down by 3900
            for (int i = 0; i < 330; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ - 0.1;
                try
                {
                    GT1.vetTransl = new Vector(0.0, 0.0, (50.0 + transZ));
                    GT2.vetTransl = new Vector(0.0, 23.0, transZ);
                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(800);
            updates += "1. Guide Tube down by 3900\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // GT down by 75
            for (int i = 0; i < 20; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ - 0.1;
                try
                {
                    GT1.vetTransl = new Vector(0.0, 0.0, (50.0 + transZ));
                    GT2.vetTransl = new Vector(0.0, 23.0, transZ);
                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            double GtTransZ = transZ;

            Thread.Sleep(800);
            updates += "2. Guide Tube down by 75\r\n";
            textBox1.Text = updates;

            // Gripper Assembly down by 3900

            transY = 25.5;
            transZ = 27.0;

            for (int i = 0; i < 360; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ - 0.1;
                transY = transY - 0.1;
                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (51.0 + transZ));
                    OT2.vetTransl = new Vector(0.0, 17.5, (7.0 + transZ));
                    OT3.vetTransl = new Vector(0.0, 28.7, transZ);

                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (51.25 + transY));
                    IT2.vetTransl = new Vector(0.0, 14.5, (2.75 + transY));
                    IT3.vetTransl = new Vector(0.0, 28.7, transY);

                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(800);
            updates += "3. Gripper Assembly down by 3900\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // Gripper Assembly down by 256

            for (int i = 0; i < 45; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ - 0.1;
                transY = transY - 0.1;
                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (51.0 + transZ));
                    OT2.vetTransl = new Vector(0.0, 17.5, (7.0 + transZ));
                    OT3.vetTransl = new Vector(0.0, 28.7, transZ);

                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (51.25 + transY));
                    IT2.vetTransl = new Vector(0.0, 14.5, (2.75 + transY));
                    IT3.vetTransl = new Vector(0.0, 28.7, transY);

                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(800);
            updates += "4. Gripper Assembly down by 256\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // Finger open

            double fo_double = transY - 3.5;

            FO1.vetTransl = new Vector(0.0, 31.7, fo_double);
            CLGLWindow.Models.Add(FO1);

            FO2.vetTransl = new Vector(0.0, 25.7, fo_double);
            CLGLWindow.Models.Add(FO2);
            CLGLWindow.ReDraw();

            Thread.Sleep(800);
            updates += "5. Finger open\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // Lift FSA

            transX = -38.0;

            for (int i = 0; i < 460; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ + 0.1;
                transY = transY + 0.1;
                transX = transX + 0.1;
                fo_double = fo_double + 0.1;
                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (51.0 + transZ));
                    OT2.vetTransl = new Vector(0.0, 17.5, (7.0 + transZ));
                    OT3.vetTransl = new Vector(0.0, 28.7, transZ);

                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (51.25 + transY));
                    IT2.vetTransl = new Vector(0.0, 14.5, (2.75 + transY));
                    IT3.vetTransl = new Vector(0.0, 28.7, transY);

                    // FSA translation

                    FSA0.vetTransl = new Vector(0.0, 29.0, transX);
                    FSACAP0.vetTransl = new Vector(0.0, 29.0, (transX + 20));

                    // Finger translation

                    FO1.vetTransl = new Vector(0.0, 31.7, fo_double);
                    FO2.vetTransl = new Vector(0.0, 25.7, fo_double);

                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            double Ot2Height = (7.0 + transZ);
            double It2Height = (2.75 + transY);
            double Ot3Height = (0.0 + transZ);
            double It3Height = (0.0 + transY);
            double FoHeight = fo_double;
            double Fsa0Height = transX;
            double Fsacap0Height = (transX + 20);


            Thread.Sleep(800);
            updates += "6. Gripper lifts the FSA by 4600\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // Guide Tube up by 75 for Free Rotation

            for (int i = 0; i < 20; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                GtTransZ = GtTransZ + 0.1;
                try
                {
                    GT1.vetTransl = new Vector(0.0, 0.0, (50.0 + GtTransZ));
                    GT2.vetTransl = new Vector(0.0, 23.0, GtTransZ);
                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(800);
            updates += "7. Guide Tube up by 75 for Free Rotation\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // TA CW Rotation

            // Guide tube, OT rotation, FO rotation, FSA rotation, FSA CAP rotation

            // Guide tube rotation

            double vg2w = 16.25;
            double ot2w = 15.25;
            double it2w = 13.25;
            double GtHeight = GtTransZ;
            double GtTransY = 0.0;
            double OtTransY = 0.0;
            double Ot3TransY = 0.0;
            double ItTransY = 0.0;
            double It3TransY = 0.0;
            double FoTransY = 0.0;
            double FsaTransY = 0.0;
            double FsacapTransY = 0.0;

            for (int i = 0; i < 144; i++)
            {
                Application.DoEvents();
                Thread.Sleep(10);

                // Guide Tube

                vg2w = vg2w - 0.1;
                GtTransY = GtTransY - 0.1;

                float[] vg2 = new float[] {
                0,(float)-vg2w,-(float)10,
                0,(float)-vg2w, (float)10,
                0,(float) vg2w, (float)10,
                0,(float) vg2w,-(float)10
                };

                GT2.SetVertexData(vg2);
                GT2.vetTransl = new Vector(0.0, (23.0 + GtTransY), GtHeight);

                // OT2, OT3

                ot2w = ot2w - 0.1;
                OtTransY = OtTransY - 0.1;
                Ot3TransY = Ot3TransY - 0.1;

                float[] vo2 = new float[] {
                0,(float)-ot2w,-(float)3,
                0,(float)-ot2w, (float)3,
                0,(float) ot2w, (float)3,
                0,(float) ot2w,-(float)3
                };
           
                OT2.SetVertexData(vo2);
                OT2.vetTransl = new Vector(0.0, (17.5 + OtTransY), Ot2Height);

                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);
                
                // IT2, IT3

                it2w = it2w - 0.1;
                ItTransY = ItTransY - 0.1;
                It3TransY = It3TransY - 0.1;

                float[] vi2 = new float[] {
                0,(float)-it2w,-(float)1.5,
                0,(float)-it2w, (float)1.5,
                0,(float) it2w, (float)1.5,
                0,(float) it2w,-(float)1.5
                };

                IT2.SetVertexData(vi2);
                IT2.vetTransl = new Vector(0.0, (14.5 + ItTransY), It2Height);

                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                // FO1, FO2

                FoTransY = FoTransY - 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                // FSA, FSACAP

                FsaTransY = FsaTransY - 0.1;
                FsacapTransY = FsacapTransY - 0.1;

                FSA0.vetTransl = new Vector(0.0, (29.0 + FsaTransY), Fsa0Height);
                FSACAP0.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), Fsacap0Height);

                // slow shift

                Thread.Sleep(10);
                Ot3TransY = Ot3TransY - 0.1;
                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);

                It3TransY = It3TransY - 0.1;
                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                FoTransY = FoTransY - 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                FsaTransY = FsaTransY - 0.1;
                FsacapTransY = FsacapTransY - 0.1;

                FSA0.vetTransl = new Vector(0.0, (29.0 + FsaTransY), Fsa0Height);
                FSACAP0.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), Fsacap0Height);

                CLGLWindow.ReDraw();

            }

            // Rotation- right side - CW

            for (int i = 0; i < 194; i++)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                vg2w = vg2w + 0.1;
                GtTransY = GtTransY - 0.1;

                float[] vg2 = new float[] {
                0,(float)-vg2w,-(float)10,
                0,(float)-vg2w, (float)10,
                0,(float) vg2w, (float)10,
                0,(float) vg2w,-(float)10
                };

                GT2.SetVertexData(vg2);
                GT2.vetTransl = new Vector(0.0, (17.0 + GtTransY), GtHeight);

                // OT2, OT3

                ot2w = ot2w + 0.1;
                OtTransY = OtTransY - 0.1;
                Ot3TransY = Ot3TransY - 0.1;

                float[] vo2 = new float[] {
                0,(float)-ot2w,-(float)3,
                0,(float)-ot2w, (float)3,
                0,(float) ot2w, (float)3,
                0,(float) ot2w,-(float)3
                };

                OT2.SetVertexData(vo2);
                OT2.vetTransl = new Vector(0.0, (11.0 + OtTransY), Ot2Height);

                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);

                // IT2, IT3

                it2w = it2w + 0.1;
                ItTransY = ItTransY - 0.1;
                It3TransY = It3TransY - 0.1;

                float[] vi2 = new float[] {
                0,(float)-it2w,-(float)1.5,
                0,(float)-it2w, (float)1.5,
                0,(float) it2w, (float)1.5,
                0,(float) it2w,-(float)1.5
                };

                IT2.SetVertexData(vi2);
                IT2.vetTransl = new Vector(0.0, (15.0 + ItTransY), It2Height);

                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                // FO1, FO2

                FoTransY = FoTransY - 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                // FSA, FSACAP

                FsaTransY = FsaTransY - 0.1;
                FsacapTransY = FsacapTransY - 0.1;

                FSA0.vetTransl = new Vector(0.0, (29.0 + FsaTransY), Fsa0Height);
                FSACAP0.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), Fsacap0Height);

                // slow shift

                Thread.Sleep(10);
                Ot3TransY = Ot3TransY - 0.1;
                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);

                It3TransY = It3TransY - 0.1;
                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                FoTransY = FoTransY - 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                FsaTransY = FsaTransY - 0.1;
                FsacapTransY = FsacapTransY - 0.1;

                FSA0.vetTransl = new Vector(0.0, (29.0 + FsaTransY), Fsa0Height);
                FSACAP0.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), Fsacap0Height);

                OT2.SetVertexData(vo2);

                CLGLWindow.ReDraw();

            }

            // Rotate everything except g.t

            for (int i = 0; i < 30; i++)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                
                // OT2, OT3

                ot2w = ot2w + 0.1;
                OtTransY = OtTransY - 0.1;
                Ot3TransY = Ot3TransY - 0.1;

                float[] vo2 = new float[] {
                0,(float)-ot2w,-(float)3,
                0,(float)-ot2w, (float)3,
                0,(float) ot2w, (float)3,
                0,(float) ot2w,-(float)3
                };

                OT2.SetVertexData(vo2);
                OT2.vetTransl = new Vector(0.0, (11.0 + OtTransY), Ot2Height);

                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);

                // IT2, IT3

                it2w = it2w + 0.1;
                ItTransY = ItTransY - 0.1;
                It3TransY = It3TransY - 0.1;

                float[] vi2 = new float[] {
                0,(float)-it2w,-(float)1.5,
                0,(float)-it2w, (float)1.5,
                0,(float) it2w, (float)1.5,
                0,(float) it2w,-(float)1.5
                };

                IT2.SetVertexData(vi2);
                IT2.vetTransl = new Vector(0.0, (15.0 + ItTransY), It2Height);

                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                // FO1, FO2

                FoTransY = FoTransY - 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                // FSA, FSACAP

                FsaTransY = FsaTransY - 0.1;
                FsacapTransY = FsacapTransY - 0.1;

                FSA0.vetTransl = new Vector(0.0, (29.0 + FsaTransY), Fsa0Height);
                FSACAP0.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), Fsacap0Height);

                // slow shift

                Thread.Sleep(10);
                Ot3TransY = Ot3TransY - 0.1;
                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);

                It3TransY = It3TransY - 0.1;
                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                FoTransY = FoTransY - 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                FsaTransY = FsaTransY - 0.1;
                FsacapTransY = FsacapTransY - 0.1;

                FSA0.vetTransl = new Vector(0.0, (29.0 + FsaTransY), Fsa0Height);
                FSACAP0.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), Fsacap0Height);

                OT2.SetVertexData(vo2);

                CLGLWindow.ReDraw();

            }


            Thread.Sleep(800);
            updates += "8. TA CW Rotation\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // Lower down to collect new fuel rod

            transZ = 0.0;
            transY = 0.0;
            transX = -38.0;

            for (int i = 0; i < 450; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ - 0.1;
                transY = transY - 0.1;
                transX = transX - 0.1;
                fo_double = fo_double - 0.1;
                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (83.5 + transZ));
                    OT2.vetTransl = new Vector(0.0, (11.2 + OtTransY), (Ot2Height + transZ));
                    OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), (Ot3Height + transZ));


                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (82.25 + transY));
                    IT2.vetTransl = new Vector(0.0, (15.0 + ItTransY), (It2Height + transY));
                    IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), (It3Height + transY));

                    // FO1, FO2

                    FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), (FoHeight + transY));
                    FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), (FoHeight + transY));


                    // FSA translation

                    FSA0.vetTransl = new Vector(0.0, (29.0 + FsaTransY), (Fsa0Height + transY));
                    FSACAP0.vetTransl = new Vector(0.0, (29.0 + FsaTransY), (Fsacap0Height + transY));

                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(1000);
            updates += "9. Lower Fuel Rod\r\n";
            textBox1.Text = updates;
            Thread.Sleep(1000);

            double Fsa0HeightTemp = (Fsa0Height + transY);
            double Fsacap0HeightTemp = (Fsacap0Height + transY);


            for (int i = 0; i < 130; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ + 0.1;
                transY = transY + 0.1;
                transX = transX + 0.1;
                fo_double = fo_double + 0.1;
                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (83.5 + transZ));
                    OT2.vetTransl = new Vector(0.0, (11.2 + OtTransY), (Ot2Height + transZ));
                    OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), (Ot3Height + transZ));


                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (82.25 + transY));
                    IT2.vetTransl = new Vector(0.0, (15.0 + ItTransY), (It2Height + transY));
                    IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), (It3Height + transY));

                    // FO1, FO2

                    FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), (FoHeight + transY));
                    FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), (FoHeight + transY));

                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }
            Thread.Sleep(800);

            // rotate old and new rods

            double Fsa0Position = 29.0 + FsaTransY;
            double Fsa8Position = -54.5;
            double Fsa8PositionTemp = -54.5;

            while(Fsa0Position > Fsa8Position)
            {
                Application.DoEvents();
                Thread.Sleep(20);

                Fsa0Position--;
                Fsa8PositionTemp++;

                // FSA0 translation

                FSA0.vetTransl = new Vector(0.0, Fsa0Position, Fsa0HeightTemp);
                FSACAP0.vetTransl = new Vector(0.0, Fsa0Position, Fsacap0HeightTemp);

                    // FSA8 translation

                    FSA8.vetTransl = new Vector(0.0, Fsa8PositionTemp, Fsa0HeightTemp);
                    FSACAP8.vetTransl = new Vector(0.0, Fsa8PositionTemp, Fsacap0HeightTemp);

                    CLGLWindow.ReDraw();
            }

            Thread.Sleep(800);

            double FSA8Height = Fsa0HeightTemp;
            double FSACAP8Height = Fsacap0HeightTemp;


            // lower gripper hoist drive

            for (int i = 0; i < 130; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ - 0.1;
                transY = transY - 0.1;
                transX = transX - 0.1;
                fo_double = fo_double - 0.1;
                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (83.5 + transZ));
                    OT2.vetTransl = new Vector(0.0, (11.2 + OtTransY), (Ot2Height + transZ));
                    OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), (Ot3Height + transZ));


                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (82.25 + transY));
                    IT2.vetTransl = new Vector(0.0, (15.0 + ItTransY), (It2Height + transY));
                    IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), (It3Height + transY));

                    // FO1, FO2

                    FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), (FoHeight + transY));
                    FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), (FoHeight + transY));


                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }
            Thread.Sleep(800);

            updates += "10. Replace spent fuel rod with a fresh one\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // Rise the new fuel rod
            for (int i = 0; i < 450; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ + 0.1;
                transY = transY + 0.1;
                transX = transX + 0.1;
                fo_double = fo_double + 0.1;
                FSA8Height = FSA8Height + 0.1;
                FSACAP8Height = FSACAP8Height + 0.1;

                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (83.5 + transZ));
                    OT2.vetTransl = new Vector(0.0, (11.2 + OtTransY), (Ot2Height + transZ));
                    OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), (Ot3Height + transZ));


                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (82.25 + transY));
                    IT2.vetTransl = new Vector(0.0, (15.0 + ItTransY), (It2Height + transY));
                    IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), (It3Height + transY));

                    // FO1, FO2

                    FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), (FoHeight + transY));
                    FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), (FoHeight + transY));


                    // FSA translation

                    FSA8.vetTransl = new Vector(0.0, (29.0 + FsaTransY), (FSA8Height));
                    FSACAP8.vetTransl = new Vector(0.0, (29.0 + FsaTransY), (FSACAP8Height));

                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(800);
            updates += "11. Rise Fuel Rod\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // Rotate everything except g.t - ACW

            for (int i = 0; i < 30; i++)
            {
                Application.DoEvents();
                Thread.Sleep(10);

                // OT2, OT3

                ot2w = ot2w - 0.1;
                OtTransY = OtTransY + 0.1;
                Ot3TransY = Ot3TransY + 0.1;

                float[] vo2 = new float[] {
                0,(float)-ot2w,-(float)3,
                0,(float)-ot2w, (float)3,
                0,(float) ot2w, (float)3,
                0,(float) ot2w,-(float)3
                };

                OT2.SetVertexData(vo2);
                OT2.vetTransl = new Vector(0.0, (11.0 + OtTransY), Ot2Height);

                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);

                // IT2, IT3

                it2w = it2w - 0.1;
                ItTransY = ItTransY + 0.1;
                It3TransY = It3TransY + 0.1;

                float[] vi2 = new float[] {
                0,(float)-it2w,-(float)1.5,
                0,(float)-it2w, (float)1.5,
                0,(float) it2w, (float)1.5,
                0,(float) it2w,-(float)1.5
                };

                IT2.SetVertexData(vi2);
                IT2.vetTransl = new Vector(0.0, (15.0 + ItTransY), It2Height);

                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                // FO1, FO2

                FoTransY = FoTransY + 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                // FSA, FSACAP

                FsaTransY = FsaTransY + 0.1;
                FsacapTransY = FsacapTransY + 0.1;

                FSA8.vetTransl = new Vector(0.0, (29.0 + FsaTransY), FSA8Height);
                FSACAP8.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), FSACAP8Height);

                // slow shift

                Thread.Sleep(10);
                Ot3TransY = Ot3TransY + 0.1;
                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);

                It3TransY = It3TransY + 0.1;
                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                FoTransY = FoTransY + 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                FsaTransY = FsaTransY + 0.1;
                FsacapTransY = FsacapTransY + 0.1;

                FSA8.vetTransl = new Vector(0.0, (29.0 + FsaTransY), FSA8Height);
                FSACAP8.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), FSACAP8Height);

                OT2.SetVertexData(vo2);

                CLGLWindow.ReDraw();

            }

            // Rotation- right side - ACW

            for (int i = 0; i < 194; i++)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                vg2w = vg2w - 0.1;
                GtTransY = GtTransY + 0.1;

                float[] vg2 = new float[] {
                0,(float)-vg2w,-(float)10,
                0,(float)-vg2w, (float)10,
                0,(float) vg2w, (float)10,
                0,(float) vg2w,-(float)10
                };

                GT2.SetVertexData(vg2);
                GT2.vetTransl = new Vector(0.0, (17.0 + GtTransY), GtHeight);

                // OT2, OT3

                ot2w = ot2w - 0.1;
                OtTransY = OtTransY + 0.1;
                Ot3TransY = Ot3TransY + 0.1;

                float[] vo2 = new float[] {
                0,(float)-ot2w,-(float)3,
                0,(float)-ot2w, (float)3,
                0,(float) ot2w, (float)3,
                0,(float) ot2w,-(float)3
                };

                OT2.SetVertexData(vo2);
                OT2.vetTransl = new Vector(0.0, (11.0 + OtTransY), Ot2Height);

                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);

                // IT2, IT3

                it2w = it2w - 0.1;
                ItTransY = ItTransY + 0.1;
                It3TransY = It3TransY + 0.1;

                float[] vi2 = new float[] {
                0,(float)-it2w,-(float)1.5,
                0,(float)-it2w, (float)1.5,
                0,(float) it2w, (float)1.5,
                0,(float) it2w,-(float)1.5
                };

                IT2.SetVertexData(vi2);
                IT2.vetTransl = new Vector(0.0, (15.0 + ItTransY), It2Height);

                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                // FO1, FO2

                FoTransY = FoTransY + 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                // FSA, FSACAP

                FsaTransY = FsaTransY + 0.1;
                FsacapTransY = FsacapTransY + 0.1;

                FSA8.vetTransl = new Vector(0.0, (29.0 + FsaTransY), FSA8Height);
                FSACAP8.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), FSACAP8Height);

                // slow shift

                Thread.Sleep(10);
                Ot3TransY = Ot3TransY + 0.1;
                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);

                It3TransY = It3TransY + 0.1;
                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                FoTransY = FoTransY + 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                FsaTransY = FsaTransY + 0.1;
                FsacapTransY = FsacapTransY + 0.1;

                FSA8.vetTransl = new Vector(0.0, (29.0 + FsaTransY), FSA8Height);
                FSACAP8.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), FSACAP8Height);

                OT2.SetVertexData(vo2);

                CLGLWindow.ReDraw();

            }

            // TA ACW Rotation

            // Guide tube, OT rotation, FO rotation, FSA rotation, FSA CAP rotation

            // Guide tube rotation


            for (int i = 0; i < 144; i++)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                vg2w = vg2w + 0.1;
                GtTransY = GtTransY + 0.1;

                float[] vg2 = new float[] {
                0,(float)-vg2w,-(float)10,
                0,(float)-vg2w, (float)10,
                0,(float) vg2w, (float)10,
                0,(float) vg2w,-(float)10
                };

                GT2.SetVertexData(vg2);
                GT2.vetTransl = new Vector(0.0, (23.0 + GtTransY), GtHeight);

                // OT2, OT3

                ot2w = ot2w + 0.1;
                OtTransY = OtTransY + 0.1;
                Ot3TransY = Ot3TransY + 0.1;

                float[] vo2 = new float[] {
                0,(float)-ot2w,-(float)3,
                0,(float)-ot2w, (float)3,
                0,(float) ot2w, (float)3,
                0,(float) ot2w,-(float)3
                };

                OT2.SetVertexData(vo2);
                OT2.vetTransl = new Vector(0.0, (17.5 + OtTransY), Ot2Height);

                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);

                // IT2, IT3

                it2w = it2w + 0.1;
                ItTransY = ItTransY + 0.1;
                It3TransY = It3TransY + 0.1;

                float[] vi2 = new float[] {
                0,(float)-it2w,-(float)1.5,
                0,(float)-it2w, (float)1.5,
                0,(float) it2w, (float)1.5,
                0,(float) it2w,-(float)1.5
                };

                IT2.SetVertexData(vi2);
                IT2.vetTransl = new Vector(0.0, (14.5 + ItTransY), It2Height);

                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                // FO1, FO2

                FoTransY = FoTransY + 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                // FSA, FSACAP

                FsaTransY = FsaTransY + 0.1;
                FsacapTransY = FsacapTransY + 0.1;

                FSA8.vetTransl = new Vector(0.0, (29.0 + FsaTransY), FSA8Height);
                FSACAP8.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), FSACAP8Height);

                // slow shift

                Thread.Sleep(10);
                Ot3TransY = Ot3TransY + 0.1;
                OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), Ot3Height);

                It3TransY = It3TransY + 0.1;
                IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), It3Height);

                FoTransY = FoTransY + 0.1;

                FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), FoHeight);
                FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), FoHeight);

                FsaTransY = FsaTransY + 0.1;
                FsacapTransY = FsacapTransY + 0.1;

                FSA8.vetTransl = new Vector(0.0, (29.0 + FsaTransY), FSA8Height);
                FSACAP8.vetTransl = new Vector(0.0, (29.0 + FsacapTransY), FSACAP8Height);

                CLGLWindow.ReDraw();

            }


            Thread.Sleep(800);
            updates += "12. TA ACW Rotation\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // Guide Tube down by 75

            for (int i = 0; i < 20; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                GtTransZ = GtTransZ - 0.1;
                try
                {
                    GT1.vetTransl = new Vector(0.0, 0.0, (50.0 + GtTransZ));
                    GT2.vetTransl = new Vector(0.0, 23.0, GtTransZ);
                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(800);
            updates += "13. Guide Tube down by 75\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // Lower FSA back to original position

            transX = -38.0;

            transZ = 0.0;
            transY = 0.0;
            transX = -38.0;

            for (int i = 0; i < 480; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ - 0.1;
                transY = transY - 0.1;
                transX = transX - 0.1;
                fo_double = fo_double - 0.1;
                FSA8Height = FSA8Height - 0.1;
                FSACAP8Height = FSACAP8Height - 0.1;

                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (83.5 + transZ));
                    OT2.vetTransl = new Vector(0.0, (17.5 + OtTransY), (Ot2Height + transZ));
                    OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), (Ot3Height + transZ));


                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (82.25 + transY));
                    IT2.vetTransl = new Vector(0.0, (14.5 + ItTransY), (It2Height + transY));
                    IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), (It3Height + transY));

                    // FO1, FO2

                    FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), (FoHeight + transY));
                    FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), (FoHeight + transY));


                    // FSA translation

                    FSA8.vetTransl = new Vector(0.0, (29.0 + FsaTransY), (FSA8Height));
                    FSACAP8.vetTransl = new Vector(0.0, (29.0 + FsaTransY), (FSACAP8Height));

                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            FSACAP8.vetTransl = new Vector(0.0, (29.0 + FsaTransY), (FSACAP8Height + 1.0));

            Thread.Sleep(800);
            updates += "14. Lower FSA back - 4600\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // Finger close

            CLGLWindow.Models.Remove(FO1);
            CLGLWindow.Models.Remove(FO2);

            Thread.Sleep(800);
            updates += "15. Finger close\r\n";
            textBox1.Text = updates;
            Application.DoEvents();

            Thread.Sleep(800);

            // Gripper Assembly up by 256 for seat check-up

            for (int i = 0; i < 40; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ + 0.1;
                transY = transY + 0.1;
                transX = transX + 0.1;
                fo_double = fo_double + 0.1;
                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (83.5 + transZ));
                    OT2.vetTransl = new Vector(0.0, (17.5 + OtTransY), (Ot2Height + transZ));
                    OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), (Ot3Height + transZ));


                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (82.25 + transY));
                    IT2.vetTransl = new Vector(0.0, (14.5 + ItTransY), (It2Height + transY));
                    IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), (It3Height + transY));
                   
                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(400);
            updates += "16. Gripper Assembly up by 256 for seat check-up\r\n";
            textBox1.Text = updates;

            Thread.Sleep(800);

            // Finger full open for seat check up

            CLGLWindow.Models.Add(FO1);
            CLGLWindow.Models.Add(FO2);

            FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), (FoHeight + transY));
            FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), (FoHeight + transY));

            Thread.Sleep(800);
            updates += "17. Finger open for seat check up\r\n";
            textBox1.Text = updates;
            Application.DoEvents();
            Thread.Sleep(800);

            // Gripper Assembly down by 187 for seat check-up

            for (int i = 0; i < 30; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ - 0.1;
                transY = transY - 0.1;
                transX = transX - 0.1;
                fo_double = fo_double - 0.1;
                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (83.5 + transZ));
                    OT2.vetTransl = new Vector(0.0, (17.5 + OtTransY), (Ot2Height + transZ));
                    OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), (Ot3Height + transZ));


                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (82.25 + transY));
                    IT2.vetTransl = new Vector(0.0, (14.5 + ItTransY), (It2Height + transY));
                    IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), (It3Height + transY));

                    // FO1, FO2

                    FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), (FoHeight + transY));
                    FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), (FoHeight + transY));

                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(800);
            updates += "18. Gripper Assembly down by 187 for seat check-up\r\n";
            textBox1.Text = updates;
            Application.DoEvents();
            Thread.Sleep(800);

            // Gripper Assembly up by 187 for seat check-up

            for (int i = 0; i < 30; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ + 0.1;
                transY = transY + 0.1;
                transX = transX + 0.1;
                fo_double = fo_double + 0.1;
                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (83.5 + transZ));
                    OT2.vetTransl = new Vector(0.0, (17.5 + OtTransY), (Ot2Height + transZ));
                    OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), (Ot3Height + transZ));


                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (82.25 + transY));
                    IT2.vetTransl = new Vector(0.0, (14.5 + ItTransY), (It2Height + transY));
                    IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), (It3Height + transY));


                    // FO1, FO2

                    FO1.vetTransl = new Vector(0.0, (31.7 + FoTransY), (FoHeight + transY));
                    FO2.vetTransl = new Vector(0.0, (25.7 + FoTransY), (FoHeight + transY));

                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(800);
            updates += "19. Gripper Assembly up by 187\r\n";
            textBox1.Text = updates;
            Application.DoEvents();
            Thread.Sleep(800);

            // Finger close

            CLGLWindow.Models.Remove(FO1);
            CLGLWindow.Models.Remove(FO2);

            Thread.Sleep(800);
            updates += "20. Finger close\r\n";
            textBox1.Text = updates;
            Application.DoEvents();

            Thread.Sleep(800);

            // Gripper Assembly up by 3900 for seat check-up

            for (int i = 0; i < 370; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                transZ = transZ + 0.1;
                transY = transY + 0.1;
                transX = transX + 0.1;
                fo_double = fo_double + 0.1;
                try
                {
                    // OT translation
                    OT1.vetTransl = new Vector(0.0, 0.0, (83.5 + transZ));
                    OT2.vetTransl = new Vector(0.0, (17.5 + OtTransY), (Ot2Height + transZ));
                    OT3.vetTransl = new Vector(0.0, (28.7 + Ot3TransY), (Ot3Height + transZ));


                    // IT translation
                    IT1.vetTransl = new Vector(0.0, 0.0, (82.25 + transY));
                    IT2.vetTransl = new Vector(0.0, (14.5 + ItTransY), (It2Height + transY));
                    IT3.vetTransl = new Vector(0.0, (28.7 + It3TransY), (It3Height + transY));

                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(800);
            updates += "21. Gripper Assembly up by 3900\r\n";
            textBox1.Text = updates;
            Application.DoEvents();
            Thread.Sleep(800);

            // Guide Tube up by 75

            for (int i = 0; i < 20; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                GtTransZ = GtTransZ + 0.1;
                try
                {
                    GT1.vetTransl = new Vector(0.0, 0.0, (50.0 + GtTransZ));
                    GT2.vetTransl = new Vector(0.0, 23.0, GtTransZ);
                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(800);
            updates += "22. Guide Tube up by 75\r\n";
            textBox1.Text = updates;
            Application.DoEvents();

            Thread.Sleep(800);

            // Guide Tube up by 3900

            for (int i = 0; i < 317; i++)
            {
                Application.DoEvents();
                Thread.Sleep(20);
                GtTransZ = GtTransZ + 0.1;
                try
                {
                    GT1.vetTransl = new Vector(0.0, 0.0, (50.0 + GtTransZ));
                    GT2.vetTransl = new Vector(0.0, 23.0, GtTransZ);
                    CLGLWindow.ReDraw();
                }
                catch (NullReferenceException nre)
                {
                }
            }

            Thread.Sleep(800);
            updates += "23. Guide Tube up by 3900\r\n";
            textBox1.Text = updates;
            Application.DoEvents();

            Thread.Sleep(800);
            updates += "*** Sequence Over ***\r\n";
            textBox1.Text = updates;
            Application.DoEvents();

            Thread.Sleep(800);


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            Process objProcess = new Process();
            objProcess.StartInfo.FileName = "C:/Users/Madhu/Documents/Visual Studio 2008/Projects/Stereoscopy/Stereoscopy/bin/Debug/Stereoscopy.exe";
            objProcess.Start(); 
        }


        
    }
}

