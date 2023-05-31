using Photon.Pun;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class VoisterBehaviour : BasicAIMovement, IPunObservable
{

    protected KingsBehaviour kingVoisters;
    protected int numberOfCharges;
    protected Vector3 spawnPoint;
    protected Vector3 kingSpawnPoint;
    protected bool isNearKing;
    protected bool isTurned;
    protected bool isProtecting;
    protected Vector3 posToGo;
    private int requiredNumberOfCharge;

    NavMeshAgent _navMeshAgent;


    public enum actionState
    {
        feed,
        protect,
        patrol,
        awoken
    }
    public actionState currentState;

    #region getter
    public actionState GetCurrentState()
    {
        return currentState;
    }
    #endregion
    protected void VoisterStatsSetup()
    {
        //TODO put that in the start method of a specific voister
        //kingVoisters = FindObjectsOfType<KingsBehaviour>().Where(x => x.CompareTag(""))
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("No NavMeshAgent attached to " + gameObject.name);
        }

        numberOfCharges = 0;
        currentState = actionState.feed;
        spawnPoint = transform.position;
        kingSpawnPoint = kingVoisters.transform.position;
        isNearKing = true;
        isTurned = false;
        isProtecting = false;
        GotFirstWayPoint = false;
        posToGo = new Vector3();
        requiredNumberOfCharge = Random.Range(2, 5);

        SetEnemyType(EnemyType.voister);
        SetTeam(Team.Voister);
        SetMaxMana(500);
        SetMana(500);
        SetMaxExp(100);
        ExpRate = 1.85f;
        SetExp(0);
        SetLvl(1);
    }

    protected void VoisterBaseAction()
    {
        Regen();
    }

    protected void VoisterBaseBehaviour()
    {
        switch (currentState)
        {
            case actionState.feed:
                VoisterFeed();
                break;

            case actionState.protect:
                VoisterProtect();
                break;

            case actionState.patrol:
                VoisterPatrol();
                break;

            case actionState.awoken:
                Debug.Log("Awoken");
                break;

        }

        //TODO check the attack code
        //VoisterBasicAttack();
    }

    protected void VoisterFeed()
    {
        if (numberOfCharges >= requiredNumberOfCharge)
        {
            currentState = kingVoisters.numberOfFollower >= kingVoisters.followersMax ? actionState.patrol : actionState.protect;
        }
        if (!isNearKing && _navMeshAgent.remainingDistance <= 10)
        {
            isNearKing = true;
            numberOfCharges++;
            posToGo = spawnPoint;
        }
        else if (isNearKing && _navMeshAgent.remainingDistance <= 10)
        {
            isNearKing = false;
            posToGo = kingVoisters.gameObject.transform.position;
        }
        _navMeshAgent.SetDestination(posToGo);

    }

    protected void VoisterProtect()
    {
        if (!isProtecting)
        {
            isProtecting = true;
            kingVoisters.numberOfFollower++;
        }
        transform.RotateAround(kingVoisters.transform.position, Vector3.up, GetMoveSpeed() * Time.deltaTime * 10);
    }

    #region patrol

    [SerializeField] bool _patrolWaiting;
    [SerializeField] float _totalWaitTime = 3f;
    [SerializeField] float _switchProbability = 0.2f;

    ConnectedWaypoint _currentWaypoint;
    ConnectedWaypoint _previousWaypoint;

    bool _travelling;
    bool _waiting;
    float _waitTimer;
    int _waypointVisited;
    bool GotFirstWayPoint;

    protected void VoisterPatrol()
    {
        if (!GotFirstWayPoint)
        {
            GotFirstWayPoint = true;
            InitFirstWaypoint();
        }
        if(_travelling && _navMeshAgent.remainingDistance <= 1.0f)
        {
            _travelling = false;
            _waypointVisited++;

            if (_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                SetDestination();
            }
        }

        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            if(_waitTimer >= _totalWaitTime)
            {
                _waiting = false;
                SetDestination();
            }
        }
    }

    public void InitFirstWaypoint()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(_navMeshAgent == null)
        {
            Debug.LogError("No NavMeshAgent attached to " + gameObject.name);
        }
        else
        {
            if(_currentWaypoint == null)
            {
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("waypoint");

                if(allWaypoints.Length > 0)
                {
                    while(_currentWaypoint == null)
                    {
                        var random = Random.Range(0, allWaypoints.Length);
                        ConnectedWaypoint startWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

                        if (startWaypoint != null)
                        {
                            _currentWaypoint = startWaypoint;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("No waypoints found in the scene");
            }
            SetDestination();
        }

    }

    protected void SetDestination()
    {
        if(_waypointVisited > 0)
        {
            ConnectedWaypoint nextWaypoint = _currentWaypoint.NextWaypoint(_previousWaypoint);
            _previousWaypoint = _currentWaypoint;
            _currentWaypoint = nextWaypoint;
        }

        _navMeshAgent.SetDestination(_currentWaypoint.transform.position);
        _travelling = true;
    }

    #endregion

    protected void VoisterBasicAttack()
    {
        if (Cible == null)
        {
            isAttacking = false;
        }

        if (!isAttacking && Cible != null)
        {
            isAttacking = true;
            if(currentState != actionState.feed)
            {
                WalkToward();
            }

            if (attackType == AttackType.Melee)
            {
                StartCoroutine(AutoAttack());
            }
            if (attackType == AttackType.Ranged)
            {
                StartCoroutine(RangeAutoAttack());
            }
        }
    }

    #region training
    public float speed;
    public float rotation;
    public LayerMask raycastMask;//Mask for the sensors

    protected float[] input = new float[7];// Input to the neural network
    public NeuralNetwork network;

    public int position;//Checkpoint number on the course
    public bool collided;//To tell if the car has crashed

    public int currentLvl = 1;
    public float lastHealth;

    private float reward = 0;

    protected void MovementTraining()
    {
        if (!collided)//if the car has not collided with the wall, it uses the neural network to get an output
        {
            for (int i = 0; i < 5; i++)//draws five debug rays as inputs
            {
                Vector3 newVector = Quaternion.AngleAxis(i * 45 - 90, new Vector3(0, 1, 0)) * transform.right;//calculating angle of raycast
                RaycastHit hit;
                Ray Ray = new Ray(transform.position, newVector);
                var raycastLenght = 100;
                if (Physics.Raycast(Ray, out hit, raycastLenght, raycastMask) == true)
                {
                    input[i] = (raycastLenght - hit.distance) / raycastLenght;//return distance, 1 being close
                }
                else
                {
                    input[i] = 0;//if nothing is detected, will return 0 to network
                }
            }

            float[] output = network.FeedForward(input);//Call to network to feedforward

            //TODO find a way to make output affect the attack/skills
            transform.Rotate(0, output[0] * rotation, 0, Space.World);//controls the cars movement
            transform.position += transform.right * output[1] * speed;//controls the cars turning
        }
    }

    protected void SurviveTraining()
    {
        if (GetHealth() > 0 && !collided)
        {
            AddReward(0);
            HasLeveledUp();
            for (int i = 0; i < input.Length; i++)//draws five debug rays as inputs
            {
                Vector3 newVector = Quaternion.AngleAxis(i * 45 - 90, new Vector3(0, 1, 0)) * transform.right;//calculating angle of raycast
                RaycastHit hit;
                Ray Ray = new Ray(transform.position, newVector);
                var raycastLenght = 100;
                if (Physics.Raycast(Ray, out hit, raycastLenght, raycastMask) == true)
                {
                    var enemyDist = 0f;
                    //TODO change the input to the current health
                    //Add multiple test like ennemy distance or health
                    //input[i] = (test - hit.distance) / test;//return distance, 1 being close
                    if (hit.collider.GetComponent<IDamageable>())
                    {
                        enemyDist = -(raycastLenght - hit.distance) / raycastLenght;
                    }
                    else if(hit.collider.gameObject.CompareTag("walls"))
                    {
                        enemyDist = (raycastLenght - hit.distance) / raycastLenght;
                    }

                    input[i] = enemyDist;
                }
                else
                {
                    input[i] = 0;
                }
            }

            //input[5] = ((GetHealth() - GetMaxHealth()) / GetHealth());
            //input[6] = ((GetLvl() - 20) / GetLvl());
            //input[5] = GetHealth() * 100 / GetMaxHealth() / 100;
            input[5] = GetLvl() * 100 / 20 / 100;
            float[] output = network.FeedForward(input);//Call to network to feedforward

            //switch (Mathf.FloorToInt(output[2]))
            //{
            //    case -1:
            //        GetNearestTarget();
            //        if (Cible) VoisterBasicAttack();
            //        break;
            //    case 0:
            //        GetNearestTarget();
            //        break;
            //}
            GetNearestTarget();
            if (Cible)
            {
                //VoisterBasicAttack();
                AddReward(2);
            }
            


            transform.Rotate(0, output[0] * rotation, 0, Space.World);//controls the cars turning
            transform.position += transform.right * output[1] * speed;//controls the cars movement
        }
    }

    public void AddReward(int rewardType)
    {        
        switch (rewardType)
        {
            case 0: // Not dead & Not bonked
                if (lastHealth != GetHealth())
                {
                    lastHealth = GetHealth();
                }
                else reward += 0.000001f;
                break;
            case 1: // Lvl up
                reward += 1;
                break;
            case 2: // Dmg done
                reward += 0.7f;
                break;
        }
    }

    private void HasLeveledUp()
    {
        if(currentLvl < GetLvl())
        {
            AddReward(1);
            currentLvl = GetLvl();
        }
    }

    public void UpdateFitness()
    {
        network.fitness = reward + 0;//updates fitness of network for sorting
        var fitLvl = GetLvl() * 20 / 100;
        //var fitHealth = GetHealth() * GetMaxHealth() / 100;
        //network.fitness = fitLvl;
    }



    public void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))//check if the car passes a gate
        //{
        //    GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        //    for (int i = 0; i < checkPoints.Length; i++)
        //    {
        //        if (collision.collider.gameObject == checkPoints[i] && i == (position + 1 + checkPoints.Length) % checkPoints.Length)
        //        {
        //            position++;//if the gate is one ahead of it, it increments the position, which is used for the fitness/performance of the network
        //            break;
        //        }
        //    }
        //}
        //else
        if (collision.gameObject.CompareTag("walls"))
        {
            collided = true;//stop operation if car has collided
        }
    }
    #endregion
}

public class DataTraining
{
    public float wallDistance { get; set; }
    public float enemyDistance { get; set; }
    public float health { get; set; }
    public int lvl { get; set; }

}
