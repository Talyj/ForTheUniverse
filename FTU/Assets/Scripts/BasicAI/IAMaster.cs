using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class IAMaster : MonoBehaviour
{
    [DllImport("CppLib.dll",  SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr createPMC(IntPtr npl, int L);

    [DllImport("CppLib.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern void checkPMC(IntPtr pmc);


    [DllImport("CppLib.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern void trainPMC(IntPtr pmc, int nb_rep, float step, IntPtr X, IntPtr Y, int xRow, int xCol,
        int yCol, bool is_classification);
    
    // Start is called before the first frame update
    void Start()
    {
        int[] npl = new int[] {2, 2};
       
        GCHandle handle = GCHandle.Alloc(npl, GCHandleType.Pinned);
        
        IntPtr pointer = handle.AddrOfPinnedObject();
        
        
        IntPtr pmc = createPMC(pointer, 2);
        
        trainPMC(pmc, 10000, 0.01f, );
        
        //checkPMC(pmc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
