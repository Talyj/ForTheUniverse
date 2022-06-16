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

    public void Start()
    {
        Init();
        SetMoveSpeed(60f);
        SetAttackRange(20f);
        CameraWork();
        currentStick = Sticks.AuraStick;
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
        if (Cible != null)
        {
            Passif();
        }

        #region test

        // test des touches
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(100, DamageType.physique);

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetHealth(GetMaxHealth());
            SetMana(GetMaxMana());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(100, DamageType.magique);
        }
        if (Input.GetKeyDown(KeyCode.L))//execute methode
        {
            TakeDamage(9999, DamageType.brut);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            SetExp(GetMaxExp() + 1);

        }
        #endregion

        if (GetCanAct())
        {
            if (isAI)
            {
                if (Cible == null)
                {
                    GetNearestTarget();
                }
                else WalkToTarget();
                DefaultHeroBehaviourAI();
                CheckTarget();
            }
            else if (!isAttacking)
            {
                MovementPlayer();
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Swap(Cible);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) && Cible != null && Vector3.Distance(Cible.transform.position, transform.position) <= GetAttackRange() * 3)
                {
                    StickTeleportation(Cible);
                }

                if (Input.GetKeyDown(KeyCode.Alpha3) && GetCanUlt() == true)
                {
                    Ultime();
                }
            }
        }
    }

    public new void Passif()
    {
        switch (currentStick)
        {
            case Sticks.AuraStick:
                if(Cible.GetComponent<PlayerStats>().enemyType == EnemyType.voister)
                {
                    damageSupp = DamageMultiplier(GetDegPhys(), 0.5f);
                }
                break;
            case Sticks.SpiritSitck:
                if (Cible.GetComponent<PlayerStats>().enemyType == EnemyType.joueur)
                {
                    damageSupp = DamageMultiplier(GetDegMag(), 0.5f);
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
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost)
        {
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");
            skills[0].isCooldown = true;

            SwitchStick();
            if(currentStick == Sticks.AuraStick && target.GetComponent<IDamageable>().enemyType == EnemyType.voister)
            {
                target.GetComponent<IDamageable>().SetCanAct(false);
                StartCoroutine(SwapCooldown(target));
            }
            else if(currentStick == Sticks.SpiritSitck && target.GetComponent<IDamageable>().enemyType == EnemyType.joueur)
            {
                var speedTemp = target.GetComponent<IDamageable>().GetMoveSpeed();
                target.GetComponent<IDamageable>().SetMoveSpeed(GetMoveSpeed()/2);
                StartCoroutine(SwapCooldown(target, speedTemp));
            }

            StartCoroutine(CoolDown(skills[0]));
        }
        else if (skills[0].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if (GetMana() < skills[0].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    public IEnumerator SwapCooldown(GameObject target, float defaultSpeed = 0)
    {
        yield return new WaitForSeconds(3);
        if(defaultSpeed != 0)
        {
            target.GetComponent<IDamageable>().SetMoveSpeed(defaultSpeed);
        }
        else
        {
            target.GetComponent<IDamageable>().SetCanAct(true);
        }
        
    }

    public void StickTeleportation(GameObject target)
    {
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost)
        {
            if(step == 0)
            {
                //buff
                SetMana(GetMana() - skills[1].Cost);
                Debug.Log(skills[1].Name + " lanc�e");

                Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                Vector3 direction = target.transform.position - transform.position;

                var proj = Instantiate(stickTP, transform.position, Quaternion.identity);
                proj.GetComponent<ThrowStickBehaviour>().SetDamages(GetDegMag(), DamageType.magique);
                proj.GetComponent<ThrowStickBehaviour>().target = target;
                proj.GetComponent<ThrowStickBehaviour>().targetSet = true;
                proj.GetComponent<ThrowStickBehaviour>().source = Instance;

                StartCoroutine(StickTeleportationSecondPart(proj, target));
                step = 1;
            }
        }
        else if (skills[1].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if (GetMana() < skills[1].Cost)
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
                target.GetComponent<IDamageable>().TakeDamage(GetDegMag(), DamageType.physique);
                break;
                //}
            }
            yield return new WaitForEndOfFrame();
        }
        skills[1].isCooldown = true;
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
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            //buff
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lanc�e");
            //TODO

            Quaternion rotation = Quaternion.LookRotation(Cible.transform.position - transform.position);
            Vector3 direction = Cible.transform.position - transform.position;

            //TODO
            var proj = Instantiate(bigStick, transform.position, rotation);
            proj.GetComponent<BigStickBehaviour>().SetDamages(GetDegMag(), DamageType.magique);
            proj.GetComponent<BigStickBehaviour>().direction = direction;

            StartCoroutine(CoolDown(skills[2]));
        }
        else if (skills[2].isCooldown == true)
        {
            Debug.Log("en cd");
        }
        else if (GetMana() < skills[2].Cost)
        {
            Debug.Log("pas assez de mana");
        }
    }

    public void Eveil()
    {
        throw new System.NotImplementedException();
    }
}
