using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBehaviour : PlayerStats
{
    //Passive
    private float dmgMultiplier;

    //Skill2
    public GameObject stickTP;
    public int step;
    public float defaultTimer = 10;
    public bool isTouched;

    //Ulti
    public GameObject bigStick;

    public SunBehaviour Instance;

    //Passif
    private enum Sticks
    {
        AuraStick = 0,
        SpiritSitck = 1
    }
    private Sticks currentStick;

    public new void Start()
    {
        currentStick = Sticks.AuraStick;
        canMove = true;
        canAct = true;
        step = 0;
        Instance = this;
        foreach (var elmt in skills)
        {
            elmt.isCooldown = false;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        HealthBehaviour();
        ExperienceBehaviour();
        if(Cible != null)
        {
            Passif();
        }
        if (canAct)
        {
            if (isAI)
            {
                if (Cible == null)
                {
                    GetNearestTarget();
                }
                CheckTarget();
                DefaultHeroBehaviourAI();
            }
            else
            {
                MovementPlayer();
                AttackSystemPlayer();
                if (Input.GetKeyDown(KeyCode.A) && Cible != null)
                {
                    if(Vector3.Distance(Cible.transform.position, transform.position) <= AttackRange)
                    {
                        Swap(Cible);
                    }
                }
                if (Input.GetKeyDown(KeyCode.E) && Cible != null && Vector3.Distance(Cible.transform.position, transform.position) <= AttackRange * 3)
                {
                    StickTeleportation(Cible);
                }

                if (Input.GetKeyDown(KeyCode.Space) && canUlt == true)
                {
                    Ultime();
                }
            }
        }
    }

    public void Passif()
    {
        switch (currentStick)
        {
            case Sticks.AuraStick:
                if(Cible.GetComponent<IDamageable>().enemyType == EnemyType.voister)
                {
                    damageSupp = DamageMultiplier(DegatsPhysique, 0.5f);
                }
                break;
            case Sticks.SpiritSitck:
                if (Cible.GetComponent<IDamageable>().enemyType == EnemyType.joueur)
                {
                    damageSupp = DamageMultiplier(DegatsMagique, 0.5f);
                }
                break;
        }
    }

    private void SwitchStick()
    {
        if(currentStick == Sticks.AuraStick)
        {
            currentStick = Sticks.SpiritSitck;
        }
        else
        {
            currentStick = Sticks.AuraStick;
        }
    }

    public void Swap(GameObject target)
    {
        if (skills[0].isCooldown == false && Mana >= skills[0].Cost)
        {
            Mana -= skills[0].Cost;
            Debug.Log(skills[0].Name + " lancée");
            skills[0].isCooldown = true;

            SwitchStick();
            if(currentStick == Sticks.AuraStick && target.GetComponent<IDamageable>().enemyType == EnemyType.voister)
            {
                target.GetComponent<IDamageable>().canAct = false;
                StartCoroutine(SwapCooldown(target));
            }
            else if(currentStick == Sticks.SpiritSitck && target.GetComponent<IDamageable>().enemyType == EnemyType.joueur)
            {
                var speedTemp = target.GetComponent<IDamageable>().MoveSpeed;
                target.GetComponent<IDamageable>().MoveSpeed /= 2;
                StartCoroutine(SwapCooldown(target, speedTemp));
            }

            StartCoroutine(CoolDown(skills[0]));
        }
        else if (skills[0].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if (Mana < skills[0].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    public IEnumerator SwapCooldown(GameObject target, float defaultSpeed = 0)
    {
        yield return new WaitForSeconds(3);
        if(defaultSpeed != 0)
        {
            target.GetComponent<IDamageable>().MoveSpeed = defaultSpeed;
        }
        else
        {
            target.GetComponent<IDamageable>().canAct = true;
        }
        
    }

    public void StickTeleportation(GameObject target)
    {
        if (skills[1].isCooldown == false && Mana >= skills[1].Cost)
        {
            if(step == 0)
            {
                //buff
                Mana -= skills[1].Cost;
                Debug.Log(skills[1].Name + " lancée");

                Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                Vector3 direction = target.transform.position - transform.position;

                float dmg = DegatsPhysique;

                var proj = Instantiate(stickTP, transform.position, Quaternion.identity);
                proj.GetComponent<ThrowStickBehaviour>().degats = 0;
                proj.GetComponent<ThrowStickBehaviour>().target = target;
                proj.GetComponent<ThrowStickBehaviour>().targetSet = true;
                proj.GetComponent<ThrowStickBehaviour>().source = Instance;

                StartCoroutine(StickTeleportationSecondPart(proj, target));
                step = 1;
            }
        }
        else if (skills[0].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if (Mana < skills[0].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    public IEnumerator StickTeleportationSecondPart(GameObject proj, GameObject target)
    {
        var timer = defaultTimer;
        while(timer >= 0)
        {
            timer -= Time.deltaTime;
            //if (Input.GetKeyDown(KeyCode.E) && isTouched && step == 1)
            if (isTouched && step == 1)
            {                
                //if(Input.GetKeyDown(KeyCode.E))
                //{
                transform.position = target.transform.position;
                target.GetComponent<IDamageable>().TakeDamage(DegatsPhysique, "Physique");
                break;
                //}
            }
            yield return new WaitForEndOfFrame();
        }
        skills[0].isCooldown = true;
        step = 0;
        isTouched = false;
        StartCoroutine(CoolDown(skills[1]));
    }

    IEnumerator Buff(Skills skill)
    {
        //while(Time.deltaTime != skill.CastTime)
        //{
        //    ResistanceMagique = ResistanceMagique * 1.25f;
        //}

        yield return new WaitForSeconds(skill.Cooldown);
        Debug.Log("fin des cd");
        skill.isCooldown = false;
    }

    public void Ultime()
    {
        if (skills[2].isCooldown == false && Mana >= skills[2].Cost)
        {
            //buff
            Mana -= skills[2].Cost;
            Debug.Log(skills[2].Name + " lancée");
            //TODO

            Quaternion rotation = Quaternion.LookRotation(Cible.transform.position - transform.position);
            Vector3 direction = Cible.transform.position - transform.position;

            //TODO
            var proj = Instantiate(bigStick, transform.position, rotation);
            proj.GetComponent<BigStickBehaviour>().degats = DegatsMagique;
            proj.GetComponent<BigStickBehaviour>().direction = direction;

            StartCoroutine(CoolDown(skills[2]));
        }
        else if (skills[2].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if (Mana < skills[2].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    public void Eveil()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator CoolDown(Skills skill)
    {
        yield return new WaitForSeconds(skill.Cooldown);
        Debug.Log("fin des cd");
        skill.isCooldown = false;
    }
}
