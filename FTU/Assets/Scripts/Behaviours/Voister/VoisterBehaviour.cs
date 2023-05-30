using Photon.Pun;
using System.Collections;
using System.Linq;
using UnityEngine;

public class VoisterBehaviour : BasicAIMovement, IPunObservable
{

    protected KingsBehaviour kingVoisters;
    protected int numberOfCharges;
    protected Vector3 spawnPoint;
    protected Vector3 kingSpawnPoint;
    protected bool isNearKing;
    protected bool isTurned;
    protected Vector3 posToGo;

    protected enum actionState
    {
        feed,
        protect,
        patrol,
        awoken
    }
    protected actionState curentState;
    protected void VoisterStatsSetup()
    {
        //TODO put that in the start method of a specific voister
        //kingVoisters = FindObjectsOfType<KingsBehaviour>().Where(x => x.CompareTag(""))

        numberOfCharges = 0;
        curentState = actionState.feed;
        spawnPoint = transform.position;
        kingSpawnPoint = kingVoisters.transform.position;
        isNearKing = true;
        isTurned = false;
        posToGo = new Vector3();

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
        switch (curentState)
        {
            case actionState.feed:
                //TODO dynamic pathfinding (dodge player at first when feeding the king)
                VoisterFeed();
                break;

            case actionState.protect:
                transform.RotateAround(kingVoisters.transform.position, Vector3.up, GetMoveSpeed() * Time.deltaTime * 10);
                break;

            case actionState.patrol:
                VoisterPatrol();
                break;

            case actionState.awoken:
                Debug.Log("Awoken");
                break;

        }

        //TODO check the attack code
        VoisterBasicAttack();

        //if (!pathDone && !isAttacking && Cible == null)
        //{
        //    if (way == Way.up)
        //    {
        //        MovementAI(whichTeam(targetsUp));
        //    }
        //    else MovementAI(whichTeam(targetsDown));
        //}
    }

    protected void VoisterFeed()
    {
        if (numberOfCharges >= 3)
        {
            curentState = kingVoisters.numberOfFollower >= 0 ? actionState.patrol : actionState.protect;
        }
        if (!isNearKing && Vector3.Distance(gameObject.transform.position, kingSpawnPoint) <= 10)
        {
            isNearKing = true;
            numberOfCharges++;
            posToGo = spawnPoint;
        }
        else if (isNearKing && Vector3.Distance(gameObject.transform.position, spawnPoint) <= 10)
        {
            isNearKing = false;
            posToGo = kingVoisters.gameObject.transform.position;
        }

        //TODO dynamic pathfinding (dodge player at first when feeding the king)
        transform.position = Vector3.MoveTowards(transform.position, posToGo, GetMoveSpeed() * Time.deltaTime);

    }

    protected void VoisterPatrol()
    {
        if (!isTurned)
        {
            isTurned = true;
            transform.Rotate(0, 180, 0, Space.Self);
        }
        if (Vector3.Distance(transform.position, posToGo) <= 5)
        {
            var z = Random.Range(-45, 45);
            var x = Random.Range(-45, 45);
            posToGo = transform.position + transform.forward + new Vector3(x, 0, z);
        }
        transform.position = Vector3.MoveTowards(transform.position, posToGo, GetMoveSpeed() * Time.deltaTime);
    }

    protected void VoisterBasicAttack()
    {
        if (Cible == null)
        {
            isAttacking = false;
        }

        if (!isAttacking && Cible != null)
        {
            isAttacking = true;
            if(curentState != actionState.feed)
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
