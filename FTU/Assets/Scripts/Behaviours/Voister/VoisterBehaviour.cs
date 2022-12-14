using Photon.Pun;
using UnityEngine;

public class VoisterBehaviour : BasicAIMovement, IPunObservable
{
    protected void VoisterStatsSetup()
    {
        SetEnemyType(EnemyType.voister);
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
    public float speed;//Speed Multiplier
    public float rotation;//Rotation multiplier
    public LayerMask raycastMask;//Mask for the sensors

    private float[] input = new float[5];//input to the neural network
    public NeuralNetwork network;

    public int position;//Checkpoint number on the course
    public bool collided;//To tell if the car has crashed

    protected void MovementTraining()
    {
        if (!collided)//if the car has not collided with the wall, it uses the neural network to get an output
        {
            for (int i = 0; i < 5; i++)//draws five debug rays as inputs
            {
                Vector3 newVector = Quaternion.AngleAxis(i * 45 - 90, new Vector3(0, 1, 0)) * transform.right;//calculating angle of raycast
                RaycastHit hit;
                Ray Ray = new Ray(transform.position, newVector);
                var test = 100;
                if (Physics.Raycast(Ray, out hit, test, raycastMask) == true)
                {
                    input[i] = (test - hit.distance) / test;//return distance, 1 being close
                }
                else
                {
                    input[i] = 0;//if nothing is detected, will return 0 to network
                }
            }

            float[] output = network.FeedForward(input);//Call to network to feedforward

            transform.Rotate(0, output[0] * rotation, 0, Space.World);//controls the cars movement
            transform.position += transform.right * output[1] * speed;//controls the cars turning
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))//check if the car passes a gate
        {
            GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
            for (int i = 0; i < checkPoints.Length; i++)
            {
                if (collision.collider.gameObject == checkPoints[i] && i == (position + 1 + checkPoints.Length) % checkPoints.Length)
                {
                    position++;//if the gate is one ahead of it, it increments the position, which is used for the fitness/performance of the network
                    break;
                }
            }
        }
        else if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Learner"))
        {
            collided = true;//stop operation if car has collided
        }
    }

    public void UpdateFitness()
    {
        network.fitness = position;//updates fitness of network for sorting
    }
    #endregion
}
