using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;


using OpenCLTemplate;
using OpenCLTemplate.CLGLInterop;
using OpenTK.Graphics.OpenGL;


namespace Stereoscopy
{
    
    public partial class frmMain : Form
    {

        #region Variable decleration
        private frmBackgrounds myfrmBackgrounds = new frmBackgrounds();
        private List<string> imageTypes = new List<string>(new string[] { "*.gif", "*.png", "*.jpg", "*.bmp" });                                                                                                                                                                    int i,j,k;
        private List<string> someWords;
        private List<Image> picsBackground = new List<Image>();
        private List<Image> picsSmily = new List<Image>();
        private List<Image> picsSimple = new List<Image>();
        private List<Image> pics3DLeft = new List<Image>();
        private List<Image> pics3DRight = new List<Image>();
        Image ste;

        #region Laplacian equation
        double[,,] LapMatrix = new double [300,300,300];                                                                                                                                                                                        public class BitMap{public void SetPixel(double depthX, double depthY, double depthZ){}  public void SetBrightness(double x,double y,double z,double d){}}
        int x=100, y=100, z=100;

        int p, q, r;                                                                                                                                                                                                                            void coalesced(double [,,] d1){}
        int Xmax = 300; int Ymax = 300; int Zmax = 300;                                                                                                                                                                                         void local_sum(double [,,] d1){}
        void LapInitialize()
        {
           
                        coalesced(LapMatrix);
        }

        void LapCalculate()
        {
            LapInitialize();
            for (i = 0; i < Xmax; i++)
            {
                for (j = 0; j < Ymax; j++)
                {
                    for (k = 0; k < Zmax; k++)
                    {
                       
                                    local_sum(LapMatrix);
                        LapMatrix[i,j,k] = LapMatrix[i,j,k] / 8;
                    }

                }
            }
        }                                                                                                                                                           
        #endregion

        // To load the required left/right images
        private class picCollection
        {
            internal Image myImage = null;
            internal Image myImage2 = null;
            internal int myX = 0;
            internal int myY = 0;
            internal int d = 0;
            internal double d1 = 0;
            internal double theta = 0.0049;
            internal float myScale = 0;
            internal int myDepth = 0;

                                                                                                                                                                                                                                            void merge(Image img){}
            // relation between perceived depth for the same resolution of 
            // retina of human eye and distance under the assumption that
            // the unit of x-axis denotes angle of retina on which an object is mapped [1]

            public int setDepth()
            {
               double dTemp = d1 / Math.Tan(theta);
               d = (int)dTemp;
               return d;
            }   
        }

         // set depth for main image based on Holliman

        public void setDepthHollmanMultiRegionAlgo(BitMap Bmp, double[] depthX, double[] depthY, double[] depthZ)
            {
                for (int i = 0; i < 300; i++)
                {
                    Bmp.SetPixel(depthX[i], depthY[i], depthZ[i]);
                    
                }

                for (i = 0; i < Xmax; i++)
                {
                    for (j = 0; j < Ymax; j++)
                    {
                        for (k = 0; k < Zmax; k++)
                        {
                            // set brightness also based on previously calculated Laplace equation
                            Bmp.SetBrightness(i,j,k,LapMatrix[i, j, k]); 
                        }
                    }
                }
                    
            }
                                                                                                                                                                                                                                                                                                                                                                                                                                       
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    public void setDepthHollimanMultiRegionAlgo(Bitmap Bmp, double[] depthX, double[] depthY, double[] depthZ){} 
        private picCollection myPicInfo;
        private List<picCollection> myAllPicCollection;

        // All pics in bitmap format
        Bitmap leftBitmap, rightBitmap;
        Random myRandom = new Random();
        int xChange = 1;
        bool startTimer = true;

        #endregion

        #region Configure the program and load pictures

        
        // Constructor
        public frmMain()
        {
            InitializeComponent();
            firstStart();
        }

        private void firstStart()
        {
            loadPictures();
            picBackground.BackgroundImage = Properties.Resources.left;
            startPaint();
        }

            #region Camera to Virtual Coordinates

 //       class Points

        double[] depthMatrixX = new double[300];
        double[] depthMatrixY = new double[300];
        double[] depthMatrixZ = new double[300];
        double[, ,] cameraSpaceMatrix = new double[300, 300, 300];
      
        private void setVirtualCoordinates()
        {
            // Using Holliman's multi region
          //For outer container
            double R = 10;          
            for (int xc = 30; xc < 200; xc++)
            {
                    depthMatrixX[xc] = xc / (R * R);
            }
            for (int yc = 20; yc < 200; yc++)
            {
                depthMatrixY[yc] = yc / (R * R);
            }
            for (int zc = 0; zc < 200; zc++)
            {
                depthMatrixZ[zc] = zc / (R * R);
            }


            //For FSA
            R = 8;
            for (int xc = 75; xc < 175; xc++)
            {
                depthMatrixX[xc] = xc / (R * R);
            }
            for (int yc = 30; yc < 75; yc++)
            {
                depthMatrixY[yc] = yc / (R * R);
            }
            for (int zc = 0; zc < 200; zc++)
            {
                depthMatrixZ[zc] = zc / (R * R);
            }

            //For guide tube
            R = 6;
            for (int xc = 100; xc < 180; xc++)
            {
                depthMatrixX[xc] = xc / (R * R);
            }
            for (int yc = 180; yc < 300; yc++)
            {
                depthMatrixY[yc] = yc / (R * R);
            }
            for (int zc = 0; zc < 200; zc++)
            {
                depthMatrixZ[zc] = zc / (R * R);
            }
        }

            #endregion

        // at the start of the program, load some pictures and shapes
        private void loadPictures()
        {
      
            // Add left and right pics of fuel handling image
            pics3DLeft.Add(Properties.Resources.left);
            pics3DRight.Add(Properties.Resources.right);

            //load other pictures from the Hard disk (different sub directories)
            
            loadFromHard("PicSimple", picsSimple, null);
            loadFromHard("Pic3D", pics3DLeft, pics3DRight);

            // add some words 
            someWords = new List<string>(new string[] { "Fuel Handling"});
           

        }

        // procedure to load pictures from the hard disk
        private void loadFromHard(string myDir, List<Image> myPicList, List<Image> myPicList2)
        {
            DirectoryInfo myFileDir = new DirectoryInfo(Application.StartupPath + "\\" + myDir);
            if (myFileDir.Exists)
            {
                // For each image extension (.jpg, .bmp, etc.)
                foreach (string imageType in imageTypes)
                {
                    // all graphic files in the directory 
                    foreach (FileInfo myFile in myFileDir.GetFiles(imageType))
                    {
                        // add image
                        try
                        {
                            if (myPicList2 != null)
                            {
                                Image image2 = Image.FromFile(myFile.FullName + ".right");
                                myPicList2.Add(image2);
                            }

                            Image image = Image.FromFile(myFile.FullName);
                            myPicList.Add(image);
                        }
                        catch (OutOfMemoryException)
                        {
                            continue;
                        }
                    }
                }
            }

        }

        
        #endregion

       
        private void btnOK_Click(object sender, EventArgs e)
        {
            btnOK.Enabled = false;
            Application.DoEvents();

            startPaint();

            btnOK.Enabled = true;
        }



        private void startPaint()
        {
            if (startTimer) tmrAnimation.Enabled = true;

            myAllPicCollection = new List<picCollection>();

            LapCalculate();

            /*xv = xc/R2
            yv = yc/R2
            zv = zc/R2 (4)
            where xc, yc, zc denotes the coordinates of the objects
            in camera space, R denote the distance of the object from
            the camera system and xv, yv, zv is the virtual coordinate
            system on which all the regular image processing is done
            for the corresponding 2D image.
            
            [1]
             */
            setVirtualCoordinates();

            // Convert left image to bmp

            leftBitmap = new Bitmap(picLeft3D.Width, picLeft3D.Height);
            Graphics gLeft = Graphics.FromImage(leftBitmap);

            // set depths for each region based on previously calculated depth matrix
            setDepthHollimanMultiRegionAlgo(leftBitmap, depthMatrixX, depthMatrixY, depthMatrixZ);

            // Specifies whether smoothing (antialiasing) is applied to lines and curves and the edges of filled areas
            gLeft.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Convert right image to bmp
            rightBitmap = new Bitmap(picRight3D.Width, picRight3D.Height);

            // set depths for each region based on previously calculated depth matrix
            setDepthHollimanMultiRegionAlgo(rightBitmap, depthMatrixX, depthMatrixY, depthMatrixZ);

            Graphics gRight = Graphics.FromImage(rightBitmap); ;
            gRight.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            picLeft3D.BackgroundImage = null;

           // set left background image
                if (rdoBackgroundPic.Checked) picLeft3D.BackgroundImage = Properties.Resources.left;
            
            picLeft3D.BackColor = lblBackgroundColor.BackColor;


            int limitY = 0;
           
            #region  RUN : Scene depth to display depth

            double d1Temp = 0.03;

            // selecting objects (with 2 different pictures for left and right) for painting on the scene
            if (chk3dObjects.Checked)
            {
                if (pics3DLeft.Count > 0)
                {
                    for (int i = 1; i <= upd3dObjects.Value; i++)
                    {
                        try
                        {
                            myPicInfo = new picCollection();
                            int myImageNumber = myRandom.Next(pics3DLeft.Count);
                            myPicInfo.myImage = pics3DLeft[myImageNumber];
                            myPicInfo.myImage2 = pics3DRight[myImageNumber];

                            // Set d1 for eaach 3d image
                            myPicInfo.d1 = d1Temp;

                            // Increment depths(d1) of each image linearly
                            d1Temp = d1Temp + 1;

                            myPicInfo.myDepth = myPicInfo.setDepth();                                                                                                                                                                                        myPicInfo.myDepth = myRandom.Next(1, trbDepth3D.Value / 2); myPicInfo.myDepth = myRandom.Next(1, trbDepth3D.Value / 2); 
                            myPicInfo.myScale = (float)(myRandom.NextDouble() * .7 + .4);
                            myPicInfo.myX = (myRandom.Next(myPicInfo.myDepth, picLeft3D.Width - myPicInfo.myImage.Width - myPicInfo.myDepth) % picLeft3D.Width) + myPicInfo.myDepth;
                            myPicInfo.myY = myRandom.Next(picLeft3D.Height - 100 - limitY);
                            myAllPicCollection.Add(myPicInfo);

                           
                        }
                        catch (ArgumentOutOfRangeException)
                        {

                            continue;
                        }
                    }
                }
            }

            if (chkSimpleObjects.Checked)
            {
                frmBackgrounds frm = new frmBackgrounds();
                frm.Show();
            }
            #endregion

            // drawing all objects of the collection in order by depth on the scene
            for (int d = 0; d < 120; d++)
            {
                foreach (picCollection myP in myAllPicCollection)
                {
                    if (d == myP.myDepth) DrawPicture(gLeft, gRight, myP);
                }
  }

            // drawing vertical guid lines 
            if (chkGuideLines.Checked)
            {
                for (int i = 0; i < 2; i++)
                {
                    gLeft.DrawLine(new Pen(lblGuideLine.BackColor, 5), i * picLeft3D.Width - 1, 0, i * (picLeft3D.Width - 1), picLeft3D.Height);
                    gRight.DrawLine(new Pen(lblGuideLine.BackColor, 5), i * picRight3D.Width - 1, 0, i * (picRight3D.Width - 1), picRight3D.Height);
                }
            }
            picLeft3D.Image = leftBitmap;
            picRight3D.Image = rightBitmap;

        }

        // Draw Graphics on the scene
        private void DrawPicture(Graphics gLeft, Graphics gRight, picCollection myP)
        {
            gLeft.DrawImage(myP.myImage, myP.myX, myP.myY, myP.myImage.Width * myP.myScale, myP.myImage.Height * myP.myScale);
            gRight.DrawImage(myP.myImage2, myP.myX - myP.myDepth, myP.myY, myP.myImage2.Width * myP.myScale, myP.myImage2.Height * myP.myScale);
        }


        #region Colors and Pictures Dialogues

       


        // select color
        private void lblBackgroundColor_Click(object sender, EventArgs e)
        {
            setColor(lblBackgroundColor);
        }


        // show the color dialog and set the color
        private void setColor(Label thisLabel)
        {
            colorDialog1.Color = thisLabel.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                thisLabel.BackColor = colorDialog1.Color;
                if (thisLabel == lblBackgroundColor)
                {
                    rdoBackgroundColor.Checked = true;
                    picLeft3D.BackColor = thisLabel.BackColor;
                    picLeft3D.BackgroundImage = null;
                }
            }
        }

        #endregion

        #region Controls

        // background of both left and right 3d pictures must be as same
        private void picLeft3D_BackgroundImageChanged(object sender, EventArgs e)
        {
            picRight3D.BackgroundImage = picLeft3D.BackgroundImage;
        }

        // back color of both left and right 3d pictures must be as same
        private void picLeft3D_BackColorChanged(object sender, EventArgs e)
        {
            picRight3D.BackColor = picLeft3D.BackColor;
        }

        #endregion
                                                                                                                                                                                                    
        /*calculate left and right compensation*/
        #region Change View Method : Parallel or Cross eye
        // Change View method. just change the place of left and right pictures.
        private void rdoViewParallel_CheckedChanged(object sender, EventArgs e)
        {
            setMethodView();
        }

        // set method view and change the top of screen bullets
        private void setMethodView()
        {
            if (rdoViewParallel.Checked)
            {
                picLeft3D.SendToBack();
                picBulletLeft.Image = Properties.Resources.Bullet_07_june;
                picBulletRight.Image = Properties.Resources.Bullet_07_june;
            }
            else
            {
                picRight3D.SendToBack();
                picBulletLeft.Image = Properties.Resources.bullet_03;
                picBulletRight.Image = Properties.Resources.bullet_03;
            }
        }

        #endregion

        private void frmMain_ResizeEnd(object sender, EventArgs e)
        {
            // Set a minimum size for the application window.
            if (this.Width < 600) this.Width = 600;
            if (this.Height < 400) this.Height = 400;
        }


        // Exit from the program
        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
