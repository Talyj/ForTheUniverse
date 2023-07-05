using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingsBehaviour : BasicAIMovement
{
    public int numberOfFollower;
    public int followersMax = 5;
    //Setup in specific class
    protected string followersTag;
    [SerializeField] protected GameObject voister;
    protected float cpt;

    public void BaseSetupKing()
    {
        numberOfFollower = 0;
        team.Code = 2;
    }

    public void BaseBehaviourKings()
    {
        HealthBehaviour();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void CheckProtector()
    {
        var targs = Physics.OverlapSphere(transform.position, 20).Where(w => w.gameObject.CompareTag(followersTag));
        Debug.Log(numberOfFollower);
        numberOfFollower = targs.Count() < numberOfFollower ? targs.Count() : numberOfFollower;
    }

    protected void SpawnVoisters(GameObject voister)
    {
        //Time.timeScale = Gamespeed;//sets gamespeed, which will increase to speed up training
        //if (voisters != null)
        //{
        //    for (int i = 0; i < voisters.Count; i++)
        //    {
        //        if (voisters[i] == null) continue;
        //        PhotonNetwork.Destroy(voisters[i].gameObject);//if there are Prefabs in the scene this will get rid of them
        //    }

        //    SortNetworks();//this sorts networks and mutates them
        //}

        //voisters = new List<VoisterBehaviour>();
        //for (int i = 0; i < populationSize; i++)
        //{
        var x = Random.Range(-150, 150);
        var z = Random.Range(-50, 50);

        VoisterBehaviour voistertemp = PhotonNetwork.Instantiate(voister.name, new Vector3(x, transform.position.y, z), Quaternion.identity).GetComponent<VoisterBehaviour>();//create botes
        voistertemp.GetComponent<VoisterBehaviour>().kingVoisters = this;
        //voistertemp.network = networks[i];//deploys network to each learner
        //voisters.Add(voistertemp);
        //}



        //var voisterTemp = PhotonNetwork.Instantiate(voister.name, new Vector3(x, king.transform.position.y, z), Quaternion.identity);
    }

    public float timeframe;
    public int populationSize = 20;//creates population size
    public GameObject prefab;//holds bot prefab

    public int[] layers = new int[3] { 6, 3, 2 };//initializing network to the right size

    [Range(0.0001f, 1f)] public float MutationChance = 0.01f;

    [Range(0f, 1f)] public float MutationStrength = 0.5f;

    [Range(0.1f, 10f)] public float Gamespeed = 1f;

    //public List<Bot> Bots;
    public List<NeuralNetwork> networks;
    private List<VoisterBehaviour> voisters;

    //public string pathToSave = "Assets/AITraining/ModelSaveMove.txt";
    public string pathToSave = "Assets/AITraining/ModelSaveSurvive.txt";

    // Start is called before the first frame update
    public void Start()
    {
        if (populationSize % 2 != 0)
            populationSize = 10;//if population size is not even, sets it to fifty

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
                if (voisters[i] == null) continue;
                PhotonNetwork.Destroy(voisters[i].gameObject);//if there are Prefabs in the scene this will get rid of them
            }

            SortNetworks();//this sorts networks and mutates them
        }

        voisters = new List<VoisterBehaviour>();
        for (int i = 0; i < populationSize; i++)
        {
            var x = Random.Range(-150, 150);
            var z = Random.Range(-50, 50);

            VoisterBehaviour voistertemp = (PhotonNetwork.Instantiate(voister.name, new Vector3(x, this.transform.position.y, z), new Quaternion(0, 0, 1, 0))).GetComponent<VoisterBehaviour>();//create botes
            voistertemp.GetComponent<VoisterBehaviour>().kingVoisters = this;
            voistertemp.network = networks[i];//deploys network to each learner
            voisters.Add(voistertemp);
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
