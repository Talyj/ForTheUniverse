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
    protected float cpt = 5f;
    protected float delaySpawn = 5f;

    private List<VoisterBehaviour> allVoisters = new List<VoisterBehaviour>();
    private VoisterBehaviour lastSpawned;

    //Utility AI
    [HideInInspector] public VoisterManager voisterManager;
    [HideInInspector] public WorldState ws;
    List<Action> allActions = new List<Action>();
    Action feed;
    Action gard;
    Action patrol;
    Action attack;

    public void BaseSetupKing()
    {
        numberOfFollower = 0;
        team.Code = 2;
        populationSize = 20;
        timeframe = 60f;

        #region utilityAI setup
        ws = new WorldState(voisterManager.food);

        //Action
        feed = new Action("FEED", WorldState.VoisterAction.FEED);
        gard = new Action("GARD", WorldState.VoisterAction.GARD);
        patrol = new Action("PATROL", WorldState.VoisterAction.PATROL);
        attack = new Action("ASSAULT", WorldState.VoisterAction.ATTACK);

        allActions = new List<Action>() { feed, gard, patrol, attack };
        #endregion

        //VoisterBehaviour voistertemp = PhotonNetwork.Instantiate(voister.name, new Vector3(75, transform.position.y, -25), Quaternion.identity).GetComponent<VoisterBehaviour>();
        //voistertemp.GetComponent<VoisterBehaviour>().kingVoisters = this;
        //voistertemp.currentState = WorldState.VoisterAction.FEED;
        //allVoisters.Add(voistertemp);
        //ws.currentQty[(int)WorldState.VoisterAction.FEED] += 1;

        //if (populationSize % 2 != 0)
        //    populationSize = 10;//if population size is not even, sets it to fifty

        //InitNetworks();
        //InvokeRepeating(nameof(CreateVoisters), 0.1f, timeframe);//repeating function
    }

    public void BaseBehaviourKings()
    {
        HealthBehaviour();
        //ws.food = voisterManager.food;
    }

    protected void MainKing()
    {
        BaseBehaviourKings();

        if (PhotonNetwork.IsMasterClient)
        {
            cpt -= Time.deltaTime;
            if (cpt <= 0)
            {
                cpt = delaySpawn;
                SpawnVoisters(voister);
            }
            UpdateListVoister(lastSpawned);
        }
    }

    private void UpdateListVoister(VoisterBehaviour voisterTemp)
    {
        var listTemp = new List<VoisterBehaviour>();
        foreach (var voi in allVoisters)
        {
            if (voi == null) continue;
            if(voi.GetHealth() <= 0)
            {
                foreach (var meshRenderer in voi.GetComponentsInChildren<MeshRenderer>())
                {
                    meshRenderer.material = dissolveMaterial;

                    var animator = meshRenderer.gameObject.GetComponent<Animator>();
                    if (animator == null)
                    {
                        animator = meshRenderer.gameObject.AddComponent<Animator>();
                    }

                    animator.runtimeAnimatorController = dissolveController;
                }
                StartCoroutine(DissolveEffect(voi.gameObject));
                ws.currentQty[(int)voi.currentState]--;
                continue;
            }
            listTemp.Add(voi);
        }

        if (allVoisters.Contains(lastSpawned)) return;
        allVoisters = listTemp;
        allVoisters.Add(voisterTemp);
    }

    private int GetMaxFromList(List<float> valueList, List<Action> returnObject)
    {
        float maxValue = 0;
        int res = 0;
        for(int i = 0; i < valueList.Count; i++)
        {
            if (valueList[i] > maxValue)
            {
                maxValue = valueList[i];
                res = (int)returnObject[i].action;
            }
        }
        return res;
    }

    private int UtilityAIMain()
    {
        bool canDoSomething = false;
        var res = 999;
        if (ws.GetValue((int)WorldState.VoisterAction.FEED) < WorldState.OBJECTIVEQTY[(int)WorldState.VoisterAction.FEED] |
            ws.GetValue((int)WorldState.VoisterAction.GARD) < WorldState.OBJECTIVEQTY[(int)WorldState.VoisterAction.GARD] |
            ws.GetValue((int)WorldState.VoisterAction.PATROL) < WorldState.OBJECTIVEQTY[(int)WorldState.VoisterAction.PATROL] |
            ws.GetValue((int)WorldState.VoisterAction.ATTACK) < WorldState.OBJECTIVEQTY[(int)WorldState.VoisterAction.ATTACK])
        {
            List<float> utilityScore = new List<float>();

            for(int i = 0; i < allActions.Count; i++)
            {
                utilityScore.Add(allActions[i].UpdateValue(ws, allActions[i].action));
                if (allActions[i].CanDo(ws) && ws.GetValue((int)allActions[i].action) < WorldState.OBJECTIVEQTY[(int)allActions[i].action])
                {
                    ws.IncrementValue((int)allActions[i].action);
                    canDoSomething = true;
                    break;
                }
            }
            Debug.Log($"food : {ws.food}, feed : {ws.GetValue(0)}, gard : {ws.GetValue(1)}, patrol : {ws.GetValue(2)}, attack : {ws.GetValue(3)}");
            if (canDoSomething)
            {
                res = GetMaxFromList(utilityScore, allActions);
            }
            return res;
        }
        return 999;
    }

    protected void SpawnVoisters(GameObject voister)
    {
        #region machinelearning
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

        //voistertemp.network = networks[i];//deploys network to each learner
        //voisters.Add(voistertemp);
        //}
        #endregion

        float x, z;

        var voisterAction = UtilityAIMain();

        if (voisterAction >= 999) return;

        if (voisterAction == 1)
        {
            x = transform.position.x + 15;
            z = transform.position.z;
        }
        else
        {
            x = Random.Range(-150, 150);
            z = Random.Range(-50, 50);
        }

        lastSpawned = PhotonNetwork.Instantiate(voister.name, new Vector3(x, transform.position.y, z), Quaternion.identity).GetComponent<VoisterBehaviour>();
        lastSpawned.GetComponent<VoisterBehaviour>().kingVoisters = this;
        lastSpawned.currentState = allActions[voisterAction].action;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<VoisterBehaviour>())
        {
            if (other.gameObject.GetComponentInChildren<VoisterBehaviour>().currentState == WorldState.VoisterAction.FEED)
            {
                ws.food += Random.Range(3, 50);
            }
        }
    }

    #region machinelearning

    public float timeframe;
    public int populationSize;//creates population size
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
    #endregion
}
