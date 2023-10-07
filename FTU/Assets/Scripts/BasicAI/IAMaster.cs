using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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


    public TMP_Text score1, score2, gold1, gold2, tour1, tour2, ecartKillText, ecartGoldText;
    private int score1Val, score2Val, gold1Val, gold2Val, tour1Val, tour2Val;

    private float ecartKill, ecartGold;

    private List<float> x;
    private List<float> y;
    private List<float> pred;
    private IntPtr pmc;

    private List<float> xToAdd;
    private List<float> yToAdd;
    
    public Button[] zones;

    public GameObject[] zonesCenter;
    
    //PMC Settings
    public int NbRepetitions = 10000;
    public float NbSteps = 0.01f;
    

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
        if (gold1)
        {
            GetNewRandomValue();
        }


        try
        {
            int[] npl = new int[] {2, 6};
       
            GCHandle handle = GCHandle.Alloc(npl, GCHandleType.Pinned);
        
            IntPtr pointer = handle.AddrOfPinnedObject();
            
            pmc = createPMC(pointer, npl.Length);
            //Debug.Log(pmc);
            
            //checkPMC(pmc);
            
            //print("test");

            /*float[] x = new float[] {1,1,1,1,1,1 };
            float[] y = new float[] { -1, 1,1,1,1,1 };
            
            float[] pred = new float[] {1,1,1,1,1,1};*/
            

            //savePMC(pmc, "./test.txt".ToCharArray());

            
            /*for (int i = 0; i < 3000; i++)
            {
                GetNewRandomValue();
                yToAdd.AddRange(new float[]{-1, -1, 1, -1, -1, 1});
                AddToTrain();
            }*/
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

    public void GetNewRandomValue()
    {
        score1Val = Random.Range(0, 51);
        score2Val = Random.Range(0, 51);
        gold1Val = Random.Range(0, 20000);
        gold2Val = Random.Range(0, 20000);
        tour1Val = Random.Range(0, 5);
        tour2Val = Random.Range(0, 5);
        
        /*score1Val = 37;
        score2Val = 20;
        gold1Val = 1500;
        gold2Val = 800;
        tour1Val = 0;
        tour2Val = 0;*/
        
        var widespanKill = score1Val + score2Val;
        var widespanGold = gold1Val + gold2Val;

        ecartKill = (float)score1Val / widespanKill;
        ecartGold = (float)gold1Val / widespanGold;

        Debug.Log($"score1 : {score1Val}, gold1 : {gold1Val}, tour1 : {tour1Val}\nscore2 : {score2Val}, gold2 : {gold2Val}, tour2 : {tour2Val}  ecartKill : {ecartKill}, ecartGold : {ecartGold}");

        /*ecartGold = 0.8f;
        ecartKill = 0.8f;*/
        try
        {
            score1.text = score1Val.ToString();
            score2.text = score2Val.ToString();
            gold1.text = gold1Val.ToString();
            gold2.text = gold2Val.ToString();
            tour1.text = tour1Val.ToString();
            tour2.text = tour2Val.ToString();
            ecartKillText.text = ecartKill.ToString("0.0000");
            ecartGoldText.text = ecartGold.ToString("0.0000");
        }
        catch(Exception e)
        {
            //:)
        }
    }

    public void AddToTrain()
    {
        x.Add(ecartKill);
        x.Add(ecartGold);
        
        y.AddRange(yToAdd);

        
        //Debug.Log($"{y[0]} , {y[1]} , {y[2]} , {y[3]} , {y[4]} , {y[5]}");
        //Debug.Log($"Added to training : {x.Count/2}");
        GetNewRandomValue();
        //Debug.Log($"{yToAdd[0]} , {yToAdd[1]} , {yToAdd[2]} , {yToAdd[3]} , {yToAdd[4]} , {yToAdd[5]}");
        yToAdd.Clear();
        yToAdd.AddRange(new float[]{-1,-1,-1,-1,-1,-1});
        //Debug.Log($"{yToAdd[0]} , {yToAdd[1]} , {yToAdd[2]} , {yToAdd[3]} , {yToAdd[4]} , {yToAdd[5]}");

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

        //Debug.Log($"Nb train item : {x.Count / 2}");
        //Debug.Log($"Count x : {x.Count} - Count y : {y.Count}");

        trainPMC(pmc, NbRepetitions, NbSteps, pointerX, pointerY, x.Count/2, 2, 6, true );
    }

    public Single[] Predict()
    {
        pred.Clear();

        pred.Add(ecartKill);
        pred.Add(ecartGold);

        GCHandle handlePred = GCHandle.Alloc(pred.ToArray(), GCHandleType.Pinned);
        
        IntPtr pointerPred = handlePred.AddrOfPinnedObject();
        
        IntPtr res = predictPMC(pmc, pointerPred, true);

        Single[] prediction = new Single[7];

        Marshal.Copy(res, prediction, 0, 7);

        return prediction;

        //Debug.Log($"{prediction[0]} , {prediction[1]} , {prediction[2]} , {prediction[3]} , {prediction[4]} , {prediction[5]}, {prediction[6]}");


    }


    public void PredictSceneTrain()
    {
        Single[] prediction = Predict();
        
        for (int i = 1; i < 7; i++)
        {
            if (prediction[i] > 0)
            {
                zones[i - 1].GetComponent<Image>().color = Color.green;
            }
        }
    }

    public List<GameObject> PredictZoneCenter()
    {
        Single[] prediction = Predict();

        return zonesCenter.Where(x => prediction[zonesCenter.ToList().IndexOf(x)] > 0).ToList();
        //return zonesCenter.Where(x => x.name == "Cube3" || x.name == "Cube6").ToList();
    }

    public void ChangeValueOfZone(int index)
    {
        var value = yToAdd[index] == 1 ? -1 : 1;

        yToAdd[index] = value;
        
        zones[index].GetComponent<Image>().color = value == 1 ? Color.green : Color.white;
        
    }


    public void Save()
    {
        using (StreamWriter writer = new StreamWriter("Assets/Scripts/BasicAI/test.txt"))
        {
            //Debug.Log($"Count x : {x.Count} - Count y : {y.Count}");
            
            for (int i = 0; i < x.Count / 2; i++)
            {
                var xLine = x.GetRange(2 * i, 2);
                var yLine = y.GetRange(6 * i, 6);
                
                var xString = String.Join(";", xLine.ToArray());
                var yString = String.Join(",", yLine.ToArray());
                writer.WriteLine(xString + "=" + yString);
            }
        }
    }

    public void Import()
    {
        List<string> reader = File.ReadLines("Assets/Scripts/BasicAI/test.txt").ToList<string>();
        x.Clear();
        y.Clear();

        foreach (var line in reader)
        {
            //Debug.Log(line);
            var split = line.Split("=");
            var xArray = split[0].Split(";").Select(x => float.Parse(x)).ToList();
            var yArray = split[1].Split(",").Select(x => float.Parse(x)).ToList();
            
            x.AddRange(xArray);
            y.AddRange(yArray);
        }
        
        Train();
    }
}
