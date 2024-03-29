using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static WorldState;

public class VoisterBehaviour : BasicAIMovement, IPunObservable
{

    public KingsBehaviour kingVoisters;
    protected Vector3 spawnPoint;
    protected bool isNearKing;
    protected bool isTurned;
    protected bool isProtecting;

    //Somewhere add the number of enemy around

    public VoisterAction currentState;
    public VoisterAction previousState;

    #region getter
    public VoisterAction GetCurrentState()
    {
        return currentState;
    }
    #endregion
    protected void VoisterStatsSetup()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("No NavMeshAgent attached to " + gameObject.name);
        }

        spawnPoint = transform.position;
        isNearKing = Vector3.Distance(transform.position, kingVoisters.transform.position) >= 5 ? false : true;
        isTurned = false;
        isProtecting = false;
        GotFirstWayPoint = false;
        posToGo = new Vector3();

        SetEnemyType(EnemyType.voister);
        SetTeam(2);
        SetMaxHealth(500);
        SetHealth(GetMaxHealth());
        SetMaxMana(500);
        SetMana(GetMaxMana());
        SetMaxExp(100);
        ExpRate = 1.85f;
        SetExp(0);
        SetLvl(1);
    }

    protected void VoisterBaseAction()
    {
        //Regen();
    }

    protected IEnumerator VoisterBaseBehaviour()
    {
        while(GetHealth() > 0)
        {
            //MovementTraining();
            //SurviveTraining();
            if (Cible)
            {
                previousState = currentState;
                currentState = VoisterAction.ATTACK;
            }
            else if(currentState == VoisterAction.ATTACK)
            {
                currentState = previousState;
            }
            switch (currentState)
            {
                case VoisterAction.FEED:
                    VoisterFeed();
                    break;

                case VoisterAction.GARD:
                    VoisterProtect();
                    break;

                case VoisterAction.PATROL:
                    VoisterPatrol();
                    break;

                case VoisterAction.ATTACK:
                    //VoisterBasicAttack();
                    Debug.Log("ATTACK");
                    break;

            }
            yield return new WaitForSeconds(0.001f);
        }
    }

    public new void HealthBehaviour()
    {
        //Debug.Log("toto");
        if (GetHealth() >= GetMaxHealth())
        {
            SetHealth(GetMaxHealth());
        }
        if (GetMana() >= GetMaxMana())
        {
            SetMana(GetMaxMana());
        }
    }
    #region BasicBehavior
    protected void VoisterFeed()
    {
        //try
        //{
        //    if (!_navMeshAgent.isOnNavMesh) return;
        //    if (numberOfCharges >= requiredNumberOfCharge && _navMeshAgent.remainingDistance <= 5)
        //    {
        //        previousState = currentState;
        //        currentState = kingVoisters.numberOfFollower >= kingVoisters.followersMax ? VoisterAction.patrol : VoisterAction.protect;
        //    }
        //    if (!isNearKing && _navMeshAgent.remainingDistance <= 5)
        //    {
        //        isNearKing = true;
        //        numberOfCharges++;
        //        posToGo = spawnPoint;
        //    }
        //    else if (isNearKing && _navMeshAgent.remainingDistance <= 5)
        //    {
        //        isNearKing = false;
        //        posToGo = kingVoisters.gameObject.transform.position;
        //    }
        //    _navMeshAgent.SetDestination(posToGo);
        //}
        //catch(MissingReferenceException e)
        //{
        //    currentState = VoisterAction.patrol;
        //}
        if (PhotonNetwork.IsMasterClient)
        {
            try
            {
                if (!_navMeshAgent.isOnNavMesh) return;
                if (!isNearKing && _navMeshAgent.remainingDistance <= 5)
                {
                    isNearKing = true;
                    //kingVoisters.ws.IncrementValue((int)WorldState.VoisterAction.FOOD);
                    posToGo = spawnPoint;
                }
                else if (isNearKing && _navMeshAgent.remainingDistance <= 5)
                {
                    isNearKing = false;
                    posToGo = kingVoisters.gameObject.transform.position;
                }
                _navMeshAgent.SetDestination(posToGo);
            }
            catch (MissingReferenceException e)
            {
                currentState = VoisterAction.PATROL;
            }
        }
    }

    protected void VoisterProtect()
    {
        //try
        //{
        //    if (!isProtecting)
        //    {
        //        isProtecting = true;
        //        kingVoisters.numberOfFollower++;
        //        _navMeshAgent.ResetPath();
        //    }
        //    transform.RotateAround(kingVoisters.transform.position, Vector3.up, GetMoveSpeed() * Time.deltaTime);            
        //}
        //catch(MissingReferenceException e)
        //{
        //    previousState = currentState;
        //    currentState = VoisterAction.patrol;
        //}

        if (!isProtecting)
        {
            isProtecting = true;
            _navMeshAgent.ResetPath();
        }
        transform.RotateAround(kingVoisters.transform.position, Vector3.up, 30 * Time.deltaTime);

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
        if (!_navMeshAgent.isOnNavMesh) return;
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
            StartCoroutine(WalkToward());

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
    #endregion

    #region training
    public float speed;
    public float rotation;
    public LayerMask raycastMask;//Mask for the sensors

    protected float[] input = new float[6];// Input to the neural network
    public NeuralNetwork network;

    public int position;//Checkpoint number on the course
    public bool collided;//To tell if the car has crashed

    public int currentLvl = 1;
    public float lastHealth;

    private float reward = 0;

    protected void MovementTraining()
    {
        if (!collided)
        {
            for (int i = 0; i < 5; i++)//draws five debug rays as inputs
            {
                Vector3 newVector = Quaternion.AngleAxis(i * 45 - 90, new Vector3(0, 1, 0)) * transform.forward;//calculating angle of raycast
                RaycastHit hit;
                Ray Ray = new Ray(transform.position, newVector);
                var raycastLenght = 100;
                Debug.DrawRay(Ray.origin, Ray.direction * raycastLenght);
                if (Physics.Raycast(Ray, out hit, raycastLenght, raycastMask) == true)
                {
                    if (hit.collider.GetComponent<KingsBehaviour>())
                    {
                        input[i] = (raycastLenght - hit.distance) / raycastLenght;//return distance, 1 being close
                        continue;
                    }
                    input[i] = 0;
                }
                else
                {
                    input[i] = 0;//if nothing is detected, will return 0 to network
                }
            }

            float[] output = network.FeedForward(input);//Call to network to feedforward

            //TODO find a way to make output affect the attack/skills
            transform.Rotate(0, output[0] * rotation, 0, Space.World);
            transform.position += transform.forward * output[1] * 1;
            _navMeshAgent.SetDestination(transform.position);
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
