using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;


internal static class NativeMethods
{
    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern IntPtr LoadLibrary(
        string lpFileName
    );
}



public class IAMaster : MonoBehaviour
{
    private static IntPtr lib;
    
    public static void LoadNativeDll(string FileName)
    {
        if (lib != IntPtr.Zero)
        {
            return;
        }

        lib = NativeMethods.LoadLibrary(FileName);
        if (lib == IntPtr.Zero)
        {
            throw new Win32Exception();
        }
    }
    
    [DllImport("Assets/Scripts/BasicAI/CppLib.dll")]
    public static extern IntPtr createPMC(IntPtr npl, int L);

    [DllImport("Assets/Scripts/BasicAI/CppLib.dll")]
    public static extern void checkPMC(IntPtr pmc);


    [DllImport("Assets/Scripts/BasicAI/CppLib.dll")]
    public static extern void trainPMC(IntPtr pmc, int nb_rep, float step, IntPtr X, IntPtr Y, int xRow, int xCol,
        int yCol, bool is_classification);
    
    [DllImport("Assets/Scripts/BasicAI/CppLib.dll")]
    public static extern void savePMC(IntPtr pmc, char[] filename);

    [DllImport("Assets/Scripts/BasicAI/CppLib.dll")]
    public static extern IntPtr predictPMC(IntPtr pmc, IntPtr X, bool isClassification);
    
    // Start is called before the first frame update
    void Start()
    {
        LoadNativeDll("D:/Cours/PA/ForTheUniverse/FTU/Assets/Scripts/BasicAI/CppLib.dll");
        
        try
        {
            int[] npl = new int[] {2, 1};
       
            GCHandle handle = GCHandle.Alloc(npl, GCHandleType.Pinned);
        
            IntPtr pointer = handle.AddrOfPinnedObject();
        
        
            IntPtr pmc = createPMC(pointer, 2);
            Debug.Log(pmc);
            
            checkPMC(pmc);
            
            print("test");

            float[] x = new float[] {0,0,1,1 };
            float[] y = new float[] { -1, 1 };
            
            float[] pred = new float[] {1,1};
        
            GCHandle handleX = GCHandle.Alloc(x, GCHandleType.Pinned);
        
            IntPtr pointerX = handleX.AddrOfPinnedObject();
        
            GCHandle handleY = GCHandle.Alloc(y, GCHandleType.Pinned);
        
            IntPtr pointerY = handleY.AddrOfPinnedObject();
            
            GCHandle handlePred = GCHandle.Alloc(pred, GCHandleType.Pinned);
        
            IntPtr pointerPred = handlePred.AddrOfPinnedObject();
        
            trainPMC(pmc, 10000, 0.01f, pointerX, pointerY, 2, 2, 1, true );
            
            
            IntPtr res = predictPMC(pmc, pointerPred, true);

            double[] prediction = new double[1];

            Marshal.Copy(res, prediction, 0, 1);
            
            Debug.Log(prediction[0]);

            savePMC(pmc, "./test.txt".ToCharArray());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
