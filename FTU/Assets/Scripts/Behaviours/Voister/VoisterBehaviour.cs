using Photon.Pun;
using UnityEngine;

public class VoisterBehaviour : BasicAIMovement, IPunObservable
{
    protected void VoisterStatsSetup()
    {
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

    protected void VoisterMovement()
    {
        if (!pathDone && !isAttacking && Cible == null)
        {
            if (way == Way.up)
            {
                MovementAI(whichTeam(targetsUp));
            }
            else MovementAI(whichTeam(targetsDown));
        }
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
            BasicAttackIA();
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
                        enemyDist = (raycastLenght - hit.distance) / raycastLenght;
                    }

                    input[i] = enemyDist;
                }
                else
                {
                    input[i] = 0;
                }
            }

            input[5] = -((GetHealth() - GetMaxHealth()) / GetHealth());
            input[6] = -((GetLvl() - 20) / GetLvl());
            float[] output = network.FeedForward(input);//Call to network to feedforward

            switch (Mathf.FloorToInt(output[2]))
            {
                case -1:
                    GetNearestTarget();
                    if (Cible) VoisterBasicAttack();
                    break;
                case 0:
                    GetNearestTarget();
                    break;
            }
            


            transform.Rotate(0, output[0] * rotation, 0, Space.World);//controls the cars movement
            transform.position += transform.right * output[1] * speed;//controls the cars turning
        }
    }

    public void AddReward(int rewardType)
    {
        switch (rewardType)
        {
            case 0: // Not dead & Not bonked
                reward += 0.00000001f;
                break;
            case 1: // Lvl up
                reward += 20;
                break;
            case 2: // 
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
