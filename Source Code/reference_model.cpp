#include "stdafx.h"
#include<iostream>
#include <conio.h>
#include <windows.h>
#include <gl/glut.h>
#include<time.h>
using namespace std;

void display(void);

static double deg=0.0;
static const int FPS = 60;
int refreshMills = 50;
static int i1 = 0;

float transX;
float transY;
float transY1;
float degr=0.0;
clock_t t1, t2;

// original positions of all objects

float GTX = 2.0; 
float GTY = 1.1;

float FSA1X = 2.0;
float FSA1Y = 1.1;

float ITX =3.2;
float ITY =1.8;
float ITY1=2.5;

float OTX = 2.0;
float OTY = 1.1;
float OTY1 = 2.0;

GLUquadricObj* GTCylinderY;
GLUquadricObj* GTCylinderX;

GLUquadricObj* OTCylinderY;
GLUquadricObj* OTCylinderX;
GLUquadricObj* OTCylinderY1;

GLUquadricObj* ITCylinderY;
GLUquadricObj* ITCylinderX;
GLUquadricObj* ITCylinderY1;

GLUquadricObj* FSA1;
GLUquadricObj* FSA2;
GLUquadricObj* FSA3;
GLUquadricObj* FSA4;
GLUquadricObj* FSA5;
GLUquadricObj* FSA6;
GLUquadricObj* FSA7;
GLUquadricObj* FSA8;

// For drawing the outer container which contains the FSA's
void drawOuterContainer()
{
	GLUquadricObj* cyl;
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	glColor3f((176/255.0f), (226/255.0f), 1.0f); 
	glTranslatef(0,0.5,-8);
	cyl = gluNewQuadric();
	gluQuadricDrawStyle(cyl, GLU_SILHOUETTE);
	glColor3f(0.0f, 1.0f, 0.0f); 
	gluQuadricTexture(cyl, GL_TRUE);
	gluCylinder(cyl,2.6,2.6,5,100,20);
	glFlush();
}

//For drawing the outer container which contains the replaceable FSA's
void drawOuterContainer1()
{
	GLUquadricObj* cyl;
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	glColor3f((176/255.0f), (226/255.0f), 1.0f); 
	glTranslatef(0,-3.7,-8);
	cyl = gluNewQuadric();
	gluQuadricDrawStyle(cyl, GLU_SILHOUETTE);
	glColor3f(0.0f, 1.0f, 0.0f); 
	gluQuadricTexture(cyl, GL_TRUE);
	gluCylinder(cyl,1.5,1.5,5,100,20);
	glFlush();
}

//For drawing the Guide Tube along Y axis
void drawGuideTubeY(float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	glTranslatef(0,0,(trans+2));
	gluQuadricDrawStyle(GTCylinderY, GLU_SILHOUETTE);
	glColor3f((176/255.0f), (255/255.0f), 1.0f);
	gluCylinder(GTCylinderY,0.8,0.8,20,30,15);
	glFlush();
}

//For drawing the Guide Tube along X axis
void drawGuideTubeX( float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	glTranslatef(0,3.1,trans+2);
	glRotatef(90,1.0,0.0,0.0);
	gluQuadricDrawStyle(GTCylinderX, GLU_SILHOUETTE);
	glColor3f((176/255.0f), (255/255.0f), 1.0f);
	gluCylinder(GTCylinderX,1.75,1.75,3.9,35,10);
	glFlush();
}

//For drawing the Outer Tube along Y axis
void drawOuterTubeY( float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	glTranslatef(0,0,(trans+1.7));
	gluQuadricDrawStyle(OTCylinderY, GLU_SILHOUETTE);
	glColor3f((255/255.0f), (255/255.0f), (0/255.0f));
	gluCylinder(OTCylinderY,0.5,0.5,20,40,20);
	glFlush();
}

//For drawing the Outer Tube along X axis
void drawOuterTubeX( float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	if(i1>240 && i1<260)
	{
		glTranslatef(0,3.1,trans+7.5);
		glRotatef(90,1.0,0.0,0.0);
	}
	else if((i1>260)&&(i1<430))
	{
		glRotatef(trans,0.0,0.0,1.0);
		glTranslatef(0,2.95,OTY1+1.25);
		glRotatef(90,1.0,0.0,0.0);	
	}
	else if((i1>430)&&(i1<490))
	{
		glTranslatef(0,0.0,trans+1.0);
		glRotatef(90,1.0,0.0,0.0);
	}
	else if((i1>560)&&(i1<620))
	{
		glTranslatef(0,0.0,trans+1.0);
		glRotatef(90,1.0,0.0,0.0);
	}
	else if((i1>620)&&(i1<790))
	{
		glRotatef(trans,0.0,0.0,1.0);
		glTranslatef(0,0.0,OTY1+1.25);
		glRotatef(90,1.0,0.0,0.0);
	}
	else
	{
		glTranslatef(0,3.2,trans+1.0);
		glRotatef(90,1.0,0.0,0.0);

	}
	gluQuadricDrawStyle(OTCylinderX, GLU_SILHOUETTE);
	glColor3f((255/255.0f), (255/255.0f), (0/255.0f));
	gluCylinder(OTCylinderX,0.8,0.8,3.5,35,15);
	glFlush();
}


//For drawing the Outer Tube along Y axis 
void drawOuterTubeY1( float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	if ((i1>240)&&(i1<260))
	{
		glTranslatef(0,2.55,(trans-2));
	}
	else if((i1>260)&&(i1<430))
	{
		glRotatef(trans,0.0,0.0,1.0);
		glTranslatef(0,2.75,OTY1-2.0);	
	}
	else if((i1>430)&&(i1<490))
	{
		glTranslatef(0,-2.75,(trans-1.75));
	}
	else if((i1>560)&&(i1<620))
	{
		glTranslatef(0,-2.75,(trans-1.75));
	}
	else if((i1>620)&&(i1<790))
	{
		glRotatef(trans,0.0,0.0,1.0);
		glTranslatef(0,-3,OTY1-2.0);
	}
	else if((i1>790)&&(i1<810))
	{
		glTranslatef(0,2.5,(trans-1.75));
	}
	else
	{
		glTranslatef(0,2.55,(trans-2.25));
	}
	gluQuadricDrawStyle(OTCylinderY, GLU_SILHOUETTE);
	glColor3f((255/255.0f), (255/255.0f), (0/255.0f));
	gluCylinder(OTCylinderY,0.75,0.75,4,40,20);
	glFlush();
}

//For drawing the Inner Tube along X axis 
void drawInnerTubeX( float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	if(i1>240 && i1<260)
	{
		glTranslatef(0,0.25,trans+5);
	}
	if((i1>260)&&(i1<430))
	{
		glRotatef(trans,0.0,0.0,1.0);
		glTranslatef(0,2.85,OTY1+1.4);
		glRotatef(90,1.0,0.0,0.0);	
	}
	else if((i1>430)&&(i1<490))
	{
		glTranslatef(0,-0.3,trans + 6.15);
		glRotatef(90,1.0,0.0,0.0);
	}
	else if((i1>560)&&(i1<620))
	{
		glTranslatef(0,-0.3,trans + 6.15);	
		glRotatef(90,1.0,0.0,0.0);
	}
	else if((i1>620)&&(i1<790))
	{
		glRotatef(trans,0.0,0.0,1.0);
		glTranslatef(0,-0.25,OTY1+1.25);
		glRotatef(90,1.0,0.0,0.0);
	}
	else if((i1>790)&&(i1<810))
	{	
		glTranslatef(0,3,trans + 6.75);
		glRotatef(90,1.0,0.0,0.0);
	}
	else if((i1>810)&&(i1<885))
	{
		glTranslatef(0,2.9,trans + 6.5);
		glRotatef(90,1.0,0.0,0.0);
	}
	else
	{
		glTranslatef(0,2.75,trans);
		glRotatef(90,1.0,0.0,0.0);
	}
	gluQuadricDrawStyle(ITCylinderX, GLU_FILL);
	glColor3f((255/255.0f), (0/255.0f), (0/255.0f));
	gluCylinder(ITCylinderX,0.5,0.5,3,35,15);
	glFlush();
}

//For drawing the Inner Tube along Y axis 
void drawInnerTubeY( float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	glTranslatef(0,0,trans+1);
	glRotatef(0,1.0,0.0,0.0);
	gluQuadricDrawStyle(ITCylinderY, GLU_FILL);
	glColor3f((255/255.0f), (0/255.0f), (0/255.0f));
	gluCylinder(ITCylinderY,0.252,0.25,16,35,15);
	glFlush();
}

//For drawing the Inner Tube along Y axis 
void drawInnerTubeY1( float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	if((i1>260)&&(i1<430))
	{
		glRotatef(trans,0.0,0.0,1.0);
		glTranslatef(0,2.5,OTY1-2.25);
	}
	else if((i1>430)&&(i1<490))
	{
		glTranslatef(0,-2.9,trans-4.8);
	}
	else if((i1>560)&&(i1<620))
	{
		glTranslatef(0,-2.9,trans-4.8);
	}
	else if((i1>620)&&(i1<790))
	{
		glRotatef(trans,0.0,0.0,1.0);
		glTranslatef(0,-2.75,OTY1-2.25);	
	}
	else if((i1>810)&&(i1<885))
	{
		glTranslatef(0,2.5,trans-4);
	}
	else
	{
		glTranslatef(0,2.5,trans-3);
	}
	gluQuadricDrawStyle(ITCylinderY, GLU_FILL);
	glColor3f((255/255.0f), (0/255.0f), (0/255.0f));
	gluCylinder(ITCylinderY,0.45,0.45,4,35,15);
	glFlush();
}

//For drawing first FSA
void drawFSA1(float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	if((i1>490)&&(i1<560))
	{
		glTranslatef(0,-4.3,-8.75+trans);
		glRotatef(0,1.0,0.0,0.0);
		
	}
	else if(i1 == 560)
	{
		glTranslatef(0,-2.65,-8.75+trans);
		glRotatef(0,1.0,0.0,0.0);
	}
	else
	{
		glTranslatef(0,-4.3,-8.75+trans);
		glRotatef(0,1.0,0.0,0.0);
	}
	gluQuadricDrawStyle(FSA1, GLU_FILL);
	glColor3f((0/255.0f), (0/255.0f), (255/255.0f));
	gluCylinder(FSA1,0.45,0.45,6,35,15);
	glFlush();
}

//For drawing second FSA
void drawFSA2(float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	glTranslatef(0,-1.5,-8.75+trans);
	glRotatef(0,1.0,0.0,0.0);
	gluQuadricDrawStyle(FSA3, GLU_FILL);
	glColor3f((0/255.0f), (0/255.0f), (255/255.0f));
	gluCylinder(FSA3,0.45,0.45,6,35,15);
	glFlush();
}

//For drawing third FSA
void drawFSA3(float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	glTranslatef(0,-0.5,-8.75+trans);
	glRotatef(0,1.0,0.0,0.0);
	gluQuadricDrawStyle(FSA4, GLU_FILL);
	glColor3f((0/255.0f), (0/255.0f), (255/255.0f));
	gluCylinder(FSA4,0.45,0.45,6,35,15);
	glFlush();
}

//For drawing fourth FSA
void drawFSA5(float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	glTranslatef(0,0.5,-8.75+trans);
	glRotatef(0,1.0,0.0,0.0);
	gluQuadricDrawStyle(FSA5, GLU_FILL);
	glColor3f((0/255.0f), (0/255.0f), (255/255.0f));
	gluCylinder(FSA5,0.45,0.45,6,35,15);
	glFlush();
}

//For drawing fifth FSA
void drawFSA6(float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	glTranslatef(0,1.5,-8.75+trans);
	glRotatef(0,1.0,0.0,0.0);
	gluQuadricDrawStyle(FSA6, GLU_FILL);
	glColor3f((0/255.0f), (0/255.0f), (255/255.0f));
	gluCylinder(FSA6,0.45,0.45,6,35,15);
	glFlush();
}

//For drawing sixth FSA
void drawFSA7(float trans)
{
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	if((i1>240)&&(i1<260))
	{
		glTranslatef(0,0,trans);	
	}
	if((i1>260)&&(i1<430))
	{
		glRotatef(trans,0.0,0.0,1.0);
		glTranslatef(0,2.5,OTY1-7.5);	
	}
	else if((i1>620)&&(i1<790))
	{
		glRotatef(trans,0.0,0.0,1.0);
		glTranslatef(0,-2.75,OTY1-7.5);
	}
	else if(((i1>430)&&(i1<490)))
	{
		glTranslatef(0,-2.9,-3.2+trans);
		glRotatef(0,1.0,0.0,0.0);
	}
	else if(((i1>810)&&(i1<885)))
	{
		glTranslatef(0,2.5,-1.45+trans);
		glRotatef(0,1.0,0.0,0.0);
	}
	else if(((i1>490)&&(i1<560)))
	{
		glTranslatef(0,-2.75,-8.75+trans);
		glRotatef(0,1.0,0.0,0.0);
	}
	else if((i1>560)&&(i1<620))
	{
		glTranslatef(0,-2.9,-3.2+trans);
		glRotatef(0,1.0,0.0,0.0);
	}
	else if(i1 == 560)
	{
		glTranslatef(0,-4.2,-8.75+trans);
		glRotatef(0,1.0,0.0,0.0);
	}
	else
	{
		glTranslatef(0,2.5,-8.75+trans);
		glRotatef(0,1.0,0.0,0.0);
	}
	gluQuadricDrawStyle(FSA7, GLU_FILL);
	glColor3f((0/255.0f), (0/255.0f), (255/255.0f));
	gluCylinder(FSA7,0.45,0.45,6,35,15);
	glFlush();
}

void drawFunction()
{
	while(i1<2000)
	{
		Sleep(2000);
		cout.flush();
		glutSwapBuffers();
		glutPostRedisplay();
		display();
		glutSwapBuffers();
		glutPostRedisplay();
		i1++;
	}
}

void display(void)
{
	
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	drawOuterContainer();
	drawOuterContainer1();
	drawFSA2(0.0);
	drawFSA3(0.0);
	drawFSA5(0.0);
	drawFSA6(0.0);
	i1++;

	//For downward movement of guide tube 
	if(i1<55 || (i1>65 && i1<75))
	{
		drawFSA1(0.0);
		transX-=0.05;
		transY-=0.05;
		drawGuideTubeY(transY);
		drawGuideTubeX(transX);
		drawOuterTubeY(OTY);
		drawOuterTubeX(OTX);
		drawOuterTubeY1(OTY1);
		drawInnerTubeX(ITX);
		drawInnerTubeY(ITY);
		drawInnerTubeY1(ITY1);	
		drawFSA7(0.0);
		if(i1 == 74 || i1 == 54)
		{
			GTX = transX;
			GTY = transY;
		}
	}
	if(i1 == 75)
	{
		t2 = clock();
		float diff = ((float)t2 - (float)t1) / 1000.0F;
		cout<<"Run time = "<<diff;
		transX = 2.0;
		transY = 1.8;
		transY1=2.0;
		i1++;
	}
	if(i1>55 && i1<65)
	{
		drawGuideTubeY(GTY);
		drawGuideTubeX(GTX);	
		drawOuterTubeY(OTY);
		drawOuterTubeX(OTX);
		drawOuterTubeY1(OTY1);
		drawInnerTubeX(ITX);
		drawInnerTubeY(ITY);
		drawInnerTubeY1(ITY1);	
		drawFSA1(0.0);
		drawFSA7(0.0);
	}

	//For downward movement of Outer and Inner tube
	if((i1>75)&&(i1<100) || (i1>110)&&(i1<120))
	{
		drawFSA1(0.0);
		transX-=0.05;
		transY-=0.05;
		transY1-=0.05;
		drawOuterTubeY(transY);
		drawOuterTubeX(transX);
		drawOuterTubeY1(transY1);
		drawGuideTubeY(GTY);
		drawGuideTubeX(GTX);
		drawInnerTubeX(transX+1.25);
		drawInnerTubeY(transY+0.3);
		drawInnerTubeY1(transY1+0.3);
		drawFSA7(0.0);
		if(i1 == 119 || i1 == 99)
		{
			OTX = transX;
			OTY = transY;
			OTY1=transY1;
		}
	}
	if(i1>100 && i1<110)
	{
		drawOuterTubeY(OTY);
		drawOuterTubeX(OTX);
		drawOuterTubeY1(OTY1);
		drawGuideTubeY(GTY);
		drawGuideTubeX(GTX);
		drawInnerTubeX(OTX+1.25);
		drawInnerTubeY(OTY+0.3);
		drawInnerTubeY1(OTY1+0.3);
		drawFSA1(0.0);
		drawFSA7(0.0);
	}
	
	//For upward movement of gripper hoist with FSA
	if((i1>120)&&(i1<240))
	{
		drawFSA1(0.0);
		if(i1<240)
		{
			transX+=0.1;
			transY+=0.1;
			transY1+=0.1;
		}
		drawGuideTubeY(GTY);
		drawGuideTubeX(GTX);
		drawOuterTubeY(transY);
		drawOuterTubeX(transX);
		drawOuterTubeY1(transY1);
		drawInnerTubeX(transX+1.25);
		drawInnerTubeY(transY+0.3);
		drawInnerTubeY1(transY1+0.3);
		drawFSA7(transY+0.2);
		if(i1 < 238)
		i1++;
	}
	if(i1 == 240)
	{
		transX=0.0;
		OTY1 = transY1;
		OTX = transX;
		OTY = transY;
		ITY = transY+0.3;
		ITX = transX+1.25;
		ITY1 = transY1+0.3;
		transY=GTY;
		transX=GTX;
	}

	//For upward movement of guide tube
	if(i1>240 && i1<260)
	{
		transX+=0.05;
		transY+=0.05;
		drawFSA1(0.0);
		drawGuideTubeY(transY);
		drawGuideTubeX(transX);	
		drawOuterTubeY(OTY);
		drawOuterTubeX(OTX);
		drawOuterTubeY1(OTY1);
		drawInnerTubeX(ITX);
		drawInnerTubeY(ITY);
		drawInnerTubeY1(ITY1);	
		drawFSA7(3.5);
	}

	//For rotation og gripper hoist with FSA 
	if((i1>260)&&(i1<430))
	{
		drawFSA1(0.0);
		drawGuideTubeY(GTY);
		drawGuideTubeX(GTX);
		transX-=1;
		drawOuterTubeX(transX);
		drawOuterTubeY1(transX);
		drawOuterTubeY(OTY);
		drawInnerTubeX(transX);
		drawInnerTubeY1(transX);
		drawInnerTubeY(ITY);
		drawFSA7(transX);
	}
	if(i1 == 430)
	{
		degr = transX;
		OTY1 = OTY1 + 0.25;
		OTX = OTY1 + 0.5;
		ITY1 = OTY1 + 1.5;
		transX=0.0;
		transY=0.0;
		transY1=0.0;
	}

	//For downward movement of gripper hoist with FSA
	if((i1>430)&&(i1<490))
	{
		drawFSA1(0.0);
		transX-=0.1;
		transY-=0.1;
		transY1-=0.1;
		drawGuideTubeY(GTY);
		drawGuideTubeX(GTX);
		drawOuterTubeY(OTY + transY);
		drawOuterTubeX(transX + OTX);
		drawOuterTubeY1(transY1 + OTY1);
		drawInnerTubeX(transX+ITX+0.5);
		drawInnerTubeY(ITY + transX);
		drawInnerTubeY1(transY1+0.3 + ITY1);
		drawFSA7(transY+0.75);
	}
	if(i1 == 490)
	{
		drawFSA1(0.0);
		drawFSA7(transY+0.3);
	}

	//For replacing the used FSA with new one
	/*if((i1>490)&&(i1<560))
	{
		drawFSA1(0.0);
		drawFSA7(0.0);
	}*/
	if(i1 == 560)
	{
		drawFSA7(0.0);
		drawFSA1(0.0);	
	}
	// For upward movement of gripper hoist with FSA
	if((i1>560)&&(i1<620))
	{
		drawFSA1(0.0);
		transX+=0.1;
		transY+=0.1;
		transY1+=0.1;
		drawGuideTubeY(GTY);
		drawGuideTubeX(GTX);
		drawOuterTubeY(OTY + transY);
		drawOuterTubeX(transX + OTX);
		drawOuterTubeY1(transY1 + OTY1);
		drawInnerTubeX(transX+ITX+0.5);
		drawInnerTubeY(ITY + transX);
		drawInnerTubeY1(transY1+0.3 + ITY1);
		drawFSA7(transY+0.75);
	}
	if(i1 == 620)
		transX = 0.0;

	//For rotation og gripper hoist with FSA 
	if((i1>620)&&(i1<790))
	{
		drawFSA1(0.0);
		drawGuideTubeY(GTY);
		drawGuideTubeX(GTX);
		transX+=1;
		drawOuterTubeX(transX);
		drawOuterTubeY1(transX);
		drawOuterTubeY(OTY);
		drawInnerTubeX(transX-1.0);
		drawInnerTubeY1(transX);
		drawInnerTubeY(ITY);
		drawFSA7(transX);
	}
	if(i1==790)
	{
		transX=0.0;
	}

	//For downward movement of guide tube
	if(i1>790 && i1<810)
	{
		transX-=0.05;
		transY-=0.05;
		drawFSA1(0.0);
		drawGuideTubeY(transY);
		drawGuideTubeX(transX);
		drawOuterTubeY(OTY);
		drawOuterTubeX(OTX);
		drawOuterTubeY1(OTY1);
		drawInnerTubeX(ITX);
		drawInnerTubeY(ITY);
		drawInnerTubeY1(ITY1-1.0);	
		drawFSA7(7.5);
	}
	if(i1 == 810)
	{
		transX = 0.0;
		transY = 0.0;
		transY1=0.0;
	}

	//For downward movement of gripper hoist with FSA
	if((i1>810)&&(i1<885))
	{
		drawFSA1(0.0);
		transX-=0.1;
		transY-=0.1;
		transY1-=0.1;
		drawGuideTubeY(GTY);
		drawGuideTubeX(GTX);
		drawOuterTubeY(OTY + transY);
		drawOuterTubeX(transX + OTX);
		drawOuterTubeY1(transY1 + OTY1);
		drawInnerTubeX(transX+ITX);
		drawInnerTubeY(ITY + transX);
		drawInnerTubeY1(transY1+0.3 + ITY1);
		drawFSA7(transY+0.2);
	}
/*	if(i1 == 886)
	{
		t2 = clock();
		float diff = ((float)t2 - (float)t1) / 1000000.0F;
		cout<<"Run time = "<<diff;
	}
	*/
}

void init (void)
{
	glClearColor (0.5, 0.5, 0.5, 0.0);
	glEnable(GL_DEPTH_TEST); 
	glMatrixMode (GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(35.0, 1.0, 1.0, 100.0);
	glMatrixMode (GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt (30.0, 0.0, 10.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);
	transX = 2.0;
	transY = 1.8;
	transY1=2.0;

	//initialization of all the objects
	GTCylinderY = gluNewQuadric();
	GTCylinderX = gluNewQuadric();
	OTCylinderY = gluNewQuadric();
	OTCylinderX = gluNewQuadric();
	OTCylinderY1 = gluNewQuadric();
	ITCylinderY =  gluNewQuadric();
	ITCylinderX =  gluNewQuadric();
	ITCylinderY1 = gluNewQuadric();
	FSA1 =  gluNewQuadric();
	FSA2  =  gluNewQuadric();
	FSA3 =  gluNewQuadric();
	FSA4 =  gluNewQuadric();
	FSA5 =  gluNewQuadric();
	FSA6 =  gluNewQuadric();
	FSA7 =  gluNewQuadric();
	FSA8 =  gluNewQuadric();
	drawFSA2(0.0);
	drawFSA3(0.0);
	drawFSA5(0.0);
	drawFSA6(0.0);
	drawFSA7(0.0);
	display();
}

// Called back when timer expired 
void Timer(int value) {
	if(i1<900)
	{
		glutPostRedisplay();      // Post re-paint request to activate display()
		glutTimerFunc(refreshMills, Timer, 0); // next Timer call milliseconds later
	}
	else
	{
		glutTimerFunc(refreshMills, Timer, 0); // next Timer call milliseconds later
	}
}

int main(int argc, char** argv)
{
	
	t1 = clock();
	glutInit(&argc, argv);
	glutInitDisplayMode (GLUT_SINGLE | GLUT_RGB);
	glutInitWindowSize (600, 1200);
	glutInitWindowPosition (100, 100);
	glutCreateWindow ("Rotation...");
	init ();
	glutTimerFunc(0, Timer, 0);
	glutDisplayFunc(display);
	glutMainLoop();
	
	return 0;
}

