using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenCLTemplate;
using OpenCLTemplate.CLGLInterop;
using OpenTK.Graphics.OpenGL;

namespace DemoCLGL
{
    public class Kernels
    {

        //CLGL shared texture

        OpenCLTemplate.CLCalc.Program.Variable CLimg1;

        #region Preliminary tests for OpenCL
        string src1 = @"
 cl_int error,err = 0; 
cl_int max;

kernel void KernelStart()
{

  int x = get_global_id(0);
  int y = get_global_id(1);
  int2 coords = (int2)(x,y);
    int numDevices;
  
  float4 val = (float4)(0.0f,0.0f,1.0f,1.0f);
 

            // Used to handle error codes 

            cl_platform_id platform; 
            cl_context context; 
            cl_command_queue queue; 
            cl_device_id device; 
            cl_device_id deviceUsed; 

            // Platform 

            error = oclGetPlatformID(&platform); 
            if (error != CL_SUCCESS) 
            {  
                exit(error); 
            }
 
            // Device

            err = clGetDeviceIDs(platform, CL_DEVICE_TYPE_GPU, 0, NULL, &numDevices);
            // initialize device
            device = new cl_device_id [numDevices];
            // Check if obtained device is configured proprely
            err = clGetDeviceIDs(platform, CL_DEVICE_TYPE_GPU, numDevices, devices, NULL);
            if (err != CL_SUCCESS) 
            {
               exit(error); 
            } 
            
            // Context - shared between devices
            
            context = clCreateContext(0, 1, &device, NULL, NULL, &error); 
            
            if (error != CL_SUCCESS)
            { 
                cexit(error);
            }
            
            // Command-queue to check errors while executing kernels from our c# code
            
            queue = clCreateCommandQueue(context, device, 0, &error);
            
            if (error != CL_SUCCESS) 
            {
               
                exit(error);
            }

            // Create input and output arrays in device memory for calculation

            float* input = clCreateBuffer(context,CL_MEM_READ_ONLY, sizeof(float * dimensions[0], NULL, NULL));

            float* output = clCreateBuffer(context,CL_MEM_WRITE_ONLY, sizeof(float * dimensions[0], NULL, NULL));

            if(!input || !output)
            {
                //Failed to allocate device memory
                exit(1);
                }
    // Set the arguments to compute kernel
    err = 0;

    cl_kernel kernel;

    error = clSetKernelArg(kernel,0,sizeof(cl_mem),&input);

    err |= clSetKernelArg(kernel,1,sizeof(cl_mem),&output);

    if((err != CL_SUCCESS)&&(error != CL_SUCCESS))
    {
        // Failed to set args
        exit(1);
    }
    
    // Get the max workgroup size for executing the kernel on the device

    max = clGetKernelWorkGroupInfo(kernel, device_id, CL_KERNEL_WORKGROUP_SIZE,sizeof(err), &err, NULL);

    // we use only a max of 3 groups in our code
    if(max<3)
    {
        exit(1);
    }

    
}

";

        #endregion


        #region OpenCL source
        static string src = @"

__kernel void vetTransl(__write_only image2d_t img,
                        constant int       *[][] dimensions,
                        __constant float      Z,
                        __constant float      x,
                        __constant float      y)
{

    // Gives the central position of current object/model
    // current position
    int x1 = img.get_global_index(0);
    int y1 = img.get_global_index(1); 
    //dim[x,y] = dim[y + img.Height*x]

    // Central value
    int ind = x*y;
    

    // Translate by 1 position

        float transValueX = dimensions[x  + dimensions[0][0][x]];
        float transValueY = dimensions[y + dimensions[0][y][0]];
        float transValueZ = dimensions[z + dimensions[z][0][0]];
        float uxx = x1 + transValueX;
        float uyy = y1 + transValueY;
   
        float val = 0.0;
        
        // retain original colour
        float4 pixColor = getColor(img, val);

    // optimization - using int2, int4 wherever possible
    //modifies image
// 1 - colour
        write_imagef(img, (int2)(uxx,uyy), 1, pixColor);
}


        
        ";
        #endregion

        // Creating opencl kernels as c# objects

        static CLCalc.Program.Kernel KernelStart;
        static CLCalc.Program.Kernel vetTransl;
        static Kernels()
        {
          
            try
            {
                CLCalc.Program.Compile(src);
                CLCalc.Program.MemoryObject[] Args = new CLCalc.Program.MemoryObject[100]; ;
                int globalWorkSize = 4;

                // compile the kernels

                KernelStart = new CLCalc.Program.Kernel("KernelStart");
                vetTransl = new CLCalc.Program.Kernel("vetTransl");

                // run kernel start

                KernelStart.Execute(Args, globalWorkSize);
            }
            catch (NullReferenceException nre)
            {
                System.Console.WriteLine("" + nre);
            }

 //           System.Diagnostics.Debug.WriteLine("Hello");
        }

       
        CLCalc.Program.Image2D CLimg;

        CLCalc.Program.Variable CLDimensions;
        int[] dimensions;

        CLCalc.Program.Variable CLX;
        int[] x;

        CLCalc.Program.Variable CLY;
        int[] y;

        CLCalc.Program.Variable CLZ;
        int[] z;

        public Kernels(CLCalc.Program.Image2D CLGLRenderImage)
        {
            this.CLimg = CLGLRenderImage;
            dimensions = new int[CLimg.Width * CLimg.Height];

            for (int y = 0; y < CLimg.Height; y++)
            {
                CLY.VarSize += 100;
            }

            for (int x = 0; x < CLimg.Width; x++)
            {
                CLX.VarSize += dimensions[CLimg.Height - 1 + CLimg.Height * x];
                CLZ.VarSize += dimensions[CLimg.Height * x] = 240;
            }
            CLDimensions = new CLCalc.Program.Variable(dimensions);
        }

        public Kernels()
        {
        }

        public int Step(int nSteps)
        {
            //grabs the Stereoscopic parameters
            dimensions[0] = CLX.VarSize;
            dimensions[1] = CLY.VarSize;
            dimensions[2] = CLZ.VarSize;
            CLDimensions.WriteToDevice(dimensions);

            //steps forward; 
            for (int k = 0; k < nSteps - 1; k++)
            {
                vetTransl.Execute(new CLCalc.Program.MemoryObject[] { CLimg, CLDimensions, CLX, CLY, CLZ }, new int[] { dimensions[0] - 2, dimensions[1] - 2 });
            }

            vetTransl.Execute(new CLCalc.Program.MemoryObject[] { CLimg, CLDimensions, CLX, CLY, CLZ }, new int[] { dimensions[0] - 2, dimensions[1] - 2 });

            return nSteps << 1;
        }

    }
}

