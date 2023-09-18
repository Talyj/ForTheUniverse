using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


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


    public TMP_Text score1, score2, gold1, gold2, tour1, tour2;
    public int score1Val, score2Val, gold1Val, gold2Val, tour1Val, tour2Val;

    private List<float> x;
    private List<float> y;
    private List<float> pred;
    private IntPtr pmc;

    private List<float> xToAdd;
    private List<float> yToAdd;
    
    public Button[] zones;
    

    /**region IMPORT**/
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
    /** endregion IMPORT **/
    
    // Start is called before the first frame update
    void Start()
    {
        //LoadNativeDll("D:/Cours/PA/ForTheUniverse/FTU/Assets/Scripts/BasicAI/CppLib.dll");

        x = new List<float>();
        y = new List<float>();
        pred = new List<float>();
        xToAdd = new List<float>();
        yToAdd = new List<float>();
        
        yToAdd.AddRange(new float[]{-1,-1,-1,-1,-1,-1});
        GetNewRandomValue();
        
        try
        {
            int[] npl = new int[] {6, 6};
       
            GCHandle handle = GCHandle.Alloc(npl, GCHandleType.Pinned);
        
            IntPtr pointer = handle.AddrOfPinnedObject();
            
            pmc = createPMC(pointer, npl.Length);
            Debug.Log(pmc);
            
            //checkPMC(pmc);
            
            //print("test");

            /*float[] x = new float[] {1,1,1,1,1,1 };
            float[] y = new float[] { -1, 1,1,1,1,1 };
            
            float[] pred = new float[] {1,1,1,1,1,1};*/
            

            //savePMC(pmc, "./test.txt".ToCharArray());
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

    void GetNewRandomValue()
    {
        score1Val = Random.Range(0, 51);
        score2Val = Random.Range(0, 51);
        gold1Val = Random.Range(0, 20000);
        gold2Val = Random.Range(0, 20000);
        tour1Val = Random.Range(0, 5);
        tour2Val = Random.Range(0, 5);
        
        score1.text = score1Val.ToString();
        score2.text = score2Val.ToString();
        gold1.text = gold1Val.ToString();
        gold2.text = gold2Val.ToString();
        tour1.text = tour1Val.ToString();
        tour2.text = tour2Val.ToString();
    }
    
    public void AddToTrain()
    {
        x.Add(score1Val);
        x.Add(score2Val);
        x.Add(gold1Val);
        x.Add(gold2Val);
        x.Add(tour1Val);
        x.Add(tour2Val);
        
        y.AddRange(yToAdd);

        GetNewRandomValue();
        yToAdd.Clear();
        yToAdd.AddRange(new float[]{-1,-1,-1,-1,-1,-1});

        foreach (var b in zones)
        {
            b.GetComponent<Image>().color = Color.white;
        }
    }

    public void Train()
    {
        GCHandle handleX = GCHandle.Alloc(x.ToArray(), GCHandleType.Pinned);
        
        IntPtr pointerX = handleX.AddrOfPinnedObject();
        
        GCHandle handleY = GCHandle.Alloc(y.ToArray(), GCHandleType.Pinned);
        
        IntPtr pointerY = handleY.AddrOfPinnedObject();
        
        trainPMC(pmc, 10000, 0.01f, pointerX, pointerY, 6, x.Count/6, 6, true );
    }

    public void Predict()
    {
        GCHandle handlePred = GCHandle.Alloc(pred.ToArray(), GCHandleType.Pinned);
        
        IntPtr pointerPred = handlePred.AddrOfPinnedObject();
        
        IntPtr res = predictPMC(pmc, pointerPred, true);

        double[] prediction = new double[6];

        Marshal.Copy(res, prediction, 0, 6);
            
        Debug.Log(prediction[0]);
        Debug.Log(prediction[1]);
        Debug.Log(prediction[2]);
        Debug.Log(prediction[3]);
        Debug.Log(prediction[4]);
        Debug.Log(prediction[5]);

        for (int i = 0; i < 6; i++)
        {
            if (prediction[i] > 0)
            {
                zones[i].GetComponent<Image>().color = Color.green;
            }
        }
    }

    public void ChangeValueOfZone(int index)
    {
        var value = yToAdd[index] == 1 ? -1 : 1;

        yToAdd[index] = value;
        
        zones[index].GetComponent<Image>().color = value == 1 ? Color.green : Color.white;
        
    }
}
