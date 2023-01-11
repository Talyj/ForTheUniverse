using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    public float timeframe;
    public int populationSize;//creates population size
    public GameObject prefab;//holds bot prefab

    public int[] layers = new int[3] { 7, 3, 3 };//initializing network to the right size

    [Range(0.0001f, 1f)] public float MutationChance = 0.01f;

    [Range(0f, 1f)] public float MutationStrength = 0.5f;

    [Range(0.1f, 10f)] public float Gamespeed = 1f;

    //public List<Bot> Bots;
    public List<NeuralNetwork> networks;
    private List<VoisterBehaviour> voisters;
    public GameObject spawnPos;

    //public string pathToSave = "Assets/AITraining/ModelSaveMove.txt";
    public string pathToSave = "Assets/AITraining/ModelSaveSurvive.txt";

    // Start is called before the first frame update
    public void Start()
    {
        if (populationSize % 2 != 0)
            populationSize = 50;//if population size is not even, sets it to fifty

        InitNetworks();
        InvokeRepeating("CreateVoisters", 0.1f, timeframe);//repeating function
    }

    public void InitNetworks()
    {
        networks = new List<NeuralNetwork>();
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Load(pathToSave);//on start load the network save
            networks.Add(net);
        }
    }

    public void CreateVoisters()
    {
        Time.timeScale = Gamespeed;//sets gamespeed, which will increase to speed up training
        if (voisters != null)
        {
            for (int i = 0; i < voisters.Count; i++)
            {
                if (voisters[i].GetHealth() <= 0) continue;
                PhotonNetwork.Destroy(voisters[i].gameObject);//if there are Prefabs in the scene this will get rid of them
            }

            SortNetworks();//this sorts networks and mutates them
        }

        voisters = new List<VoisterBehaviour>();
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 randomPos = new Vector3(spawnPos.transform.position.x + Random.Range(-20, 20), spawnPos.transform.position.y + 3, spawnPos.transform.position.z + Random.Range(-75, 75));
            //Vector3 randomPos = spawnPos.transform.position;

            VoisterBehaviour voister = (PhotonNetwork.Instantiate(prefab.name, randomPos, new Quaternion(0, 0, 1, 0))).GetComponent<VoisterBehaviour>();//create botes
            voister.network = networks[i];//deploys network to each learner
            voisters.Add(voister);
        }

    }

    public void SortNetworks()
    {
        for (int i = 0; i < populationSize; i++)
        {
            voisters[i].UpdateFitness();//gets bots to set their corrosponding networks fitness
        }
        networks.Sort();
        networks[populationSize - 1].Save(pathToSave);//saves networks weights and biases to file, to preserve network performance
        for (int i = 0; i < populationSize / 2; i++)
        {
            networks[i] = networks[i + populationSize / 2].copy(new NeuralNetwork(layers));
            networks[i].Mutate((int)(1 / MutationChance), MutationStrength);
        }
    }
}
