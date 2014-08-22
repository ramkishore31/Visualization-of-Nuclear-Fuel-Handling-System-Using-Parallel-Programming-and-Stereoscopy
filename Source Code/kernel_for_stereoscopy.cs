using System;
using System.Collections.Generic;
using System.Text;

using OpenCLTemplate;
using OpenCLTemplate.CLGLInterop;
using OpenTK.Graphics.OpenGL;

namespace Stereoscopy
{
    public class Kernels
    {

        #region Preliminary tests for OpenCL
        string src1 = @"
 cl_int error,err = 0; 
cl_int max;

kernel void KernelStart()
{

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

//Memory coalescing access is the data access in a sequential pattern. 
//This means that the i-th work-item should access the i-th component of a __global vector.

__kernel void coalesced(__global double *[][] LapMatrix)
{

   i = get_global(0);
    j = get_global(1);
    k = get_global(2);

    int max = 300;

    cl_mem buffer_b = clCreateBuffer(
    context, // OpenCL context
    CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR, // Only read access from kernel,
                                             // copy data from host
    sizeof(cl_double) * LapMatrix.size(), // Buffer size in bytes
    &LapMatrix[0], // Pointer to data to copy
    &errorcode); // Return code
    if(i<max)
    {
        if(j<max)
        {
            if(k<max)
            buffer_b[i,j,k] = 0;
        }
    }
    

}

__kernel void local_sum(__global double *[][] LapMatrix)
{

// This is a parallel kernel. Each thread gets itís ID with the call to get_global_id(),
//which is used as the index into the vector.
// In contrast, a serial function would use a for or while loop. Reference : http://www.codeproject.com/Articles/110685/Part-1-OpenCL-Portable-Parallelism
    int max = 300;

   i = get_global_id(0);
    j = get_global(1);
    k = get_global(2);

    cl_mem buffer_b = clCreateBuffer(
    context, // OpenCL context
    CL_MEM_READ_ONLY | CL_MEM_COPY_HOST_PTR, // Only read access from kernel,
                                             // copy data from host
    sizeof(cl_double) * LapMatrix.size(), // Buffer size in bytes
    &LapMatrix[0], // Pointer to data to copy
    &errorcode); // Return code

    if(i<max)
    {
        if(j<max)
        {
            if(k<max)
            buffer_b[i,j,k] += buffer_b[i-x,j-y,k-z];
        }
    }

}
        
        ";
        #endregion

        // global shared variables 
                                                                                                                                                                                                                                                                       double [] m = new double[300];
        CLCalc.Program.Variable i, j, k; 
        // Creating opencl kernels as c# objects

        static CLCalc.Program.Kernel KernelStart;
        static CLCalc.Program.Kernel coalesced;

        static Kernels()
        {

            try
            {
                CLCalc.Program.Compile(src);
                CLCalc.Program.MemoryObject[] Args = new CLCalc.Program.MemoryObject[100]; ;                                                                                                                    
                int globalWorkSize = 4;

                // compile the kernels

                KernelStart = new CLCalc.Program.Kernel("KernelStart");
                coalesced = new CLCalc.Program.Kernel("coalesced");

                // run kernel start

                KernelStart.Execute(Args, globalWorkSize);
            }
            catch (NullReferenceException nre)
            {
                System.Console.WriteLine("" + nre);
            }

            //           System.Diagnostics.Debug.WriteLine("Hello");
        }


        public Kernels(CLCalc.Program.Image2D CLGLRenderImage)
        {

        }

        public Kernels()
        {
        }

    }
}

