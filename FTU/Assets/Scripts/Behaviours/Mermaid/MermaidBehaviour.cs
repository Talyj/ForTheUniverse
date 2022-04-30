using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermaidBehaviour : PlayerStats
{
    //Passive
    private int _passiveCounter;
    private bool isPassiveStart;

    //Skill 1
    public GameObject poissoin;

    //Skill 2
    public float speedPush;
    private float windTimerDefault = 0.2f;
    public GameObject windArea;
    public MermaidBehaviour Instance;

    //Ulti
    private float ultiTimerDefault = 1;
    private float charmSpeed;
    private List<GameObject> charmTargets;
    public GameObject charmArea;

    public new void Start()
    {
        speedPush = 3;
        charmSpeed = 5;
        charmTargets = new List<GameObject>();
        //Copy that in a new character file
        //canMove = true;
        canAct = true;
        foreach(var elmt in skills)
        {
            elmt.isCooldown = false;
        }

        Instance = this;
        isPassiveStart = false;
        _passiveCounter = 0;
    }

    //Copy that in a new character file
    public void Update()
    {
        HealthBehaviour();
        ExperienceBehaviour();
        Passif();
        if (canAct)
        {
            //Movement();
            //AttackSystem();
            if (Input.GetKeyDown(KeyCode.A) && Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < AttackRange)
            {
                Poissoin(EnemyType.minion, Cible);
            }
            if (Input.GetKeyDown(KeyCode.E) && Cible != null)
            {
                MagicWind(Cible);
            }

            if (Input.GetKeyDown(KeyCode.Space) && canUlt == true)
            {
                Ultime();
            }
        }
    }

    //Copy that in a new character file

    public void Passif()
    {
       if(_passiveCounter >= 3)
        {
            isPassiveStart = true;
            DegatsMagique *= 1.5f;
            _passiveCounter = 0;
        }
    }


    //Have to test after each skills to make sure the buff doesn't stay indefinitely
    private void CheckPassive()
    {
        if (isPassiveStart)
        {
            isPassiveStart = false;
            DegatsMagique /= 1.5f;
        }
    }

    public void AddPassive()
    {
        _passiveCounter++;
    }

    //Copy that in a new character file (skill1)
    public void Poissoin(EnemyType typeEnemy, GameObject target)
    {
        if (skills[0].isCooldown == false && Mana >= skills[0].Cost)
        {
            Mana -= skills[0].Cost;
            Debug.Log(skills[0].Name + " lancée");
            skills[0].isCooldown = true;

            float dmg = DegatsMagique;
            if (typeEnemy == EnemyType.minion /*||typeEnemy == Targetable.EnemyType.Adversaire*/)
            {
                var proj = Instantiate(poissoin, SpawnPrefab.transform.position, Quaternion.identity);
                proj.GetComponent<PoissoinProjBehaviour>().degats = dmg;
                proj.GetComponent<PoissoinProjBehaviour>().target = target;
                proj.GetComponent<PoissoinProjBehaviour>().targetSet = true;
                proj.GetComponent<PoissoinProjBehaviour>().source = Instance;
            }
            CheckPassive();   
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

    //Copy that in a new character file (skill2)
    public void MagicWind(GameObject target)
    {
        if (skills[1].isCooldown == false && Mana >= skills[1].Cost)
        {
            //buff
            Mana -= skills[1].Cost;
            Debug.Log(skills[1].Name + " lancée");
            
            Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            Vector3 direction = target.transform.position - transform.position;

            float dmg = DegatsMagique;
            var proj = Instantiate(windArea, SpawnPrefab.transform.position, rotation);
            proj.GetComponent<WindAreaBehaviour>().degats = dmg;
            proj.GetComponent<WindAreaBehaviour>().direction = direction;
            proj.GetComponent<WindAreaBehaviour>().source = Instance;

            CheckPassive();
            StartCoroutine(CoolDown(skills[1]));
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
    public void AddWindedTarget(GameObject target)
    {
        charmTargets.Add(target);
        target.GetComponent<Targetable>().canMove = false;
        StartCoroutine(GoAway(target));
    }

    public IEnumerator GoAway(GameObject target)
    {
        var timer = windTimerDefault;
        while (timer >= 0)
        {
            Vector3 direction = target.transform.position - transform.position;
            target.transform.position += direction * Time.deltaTime * speedPush;
            //target.transform.position += (transform.position + target.transform.position).normalized * charmSpeed * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        target.GetComponent<Targetable>().canMove = true;
        yield return 0;
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


    //Copy that in a new character file
    public void Ultime()
    {
        if (skills[2].isCooldown == false && Mana >= skills[2].Cost)
        {
            //buff
            Mana -= skills[2].Cost;
            Debug.Log(skills[2].Name + " lancée");
            //TODO
            float dmg = DegatsMagique;

            var area = Instantiate(charmArea, SpawnPrefab.transform.position, Quaternion.identity);
            area.GetComponent<CharmAreaBehaviour>().source = Instance;

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

    public void AddTCharmedTargets(GameObject target)
    {        
        charmTargets.Add(target);
        target.GetComponent<Targetable>().canMove = false;
        StartCoroutine(GetNear(target));
    }

    public IEnumerator GetNear(GameObject target)
    {
        var timer = ultiTimerDefault;
        while(timer >= 0)
        {
            //target.transform.position = Vector3.MoveTowards(target.transform.position, transform.position, 1.0f);
            target.transform.position += (transform.position - target.transform.position).normalized * charmSpeed * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        target.GetComponent<Targetable>().canMove = true;
        yield return 0;
    }

    //Copy that in a new character file
    public void Eveil()
    {
        throw new System.NotImplementedException();
    }

    //Copy that in a new character file
    IEnumerator CoolDown(Skills skill)
    {
        yield return new WaitForSeconds(skill.Cooldown);
        Debug.Log("fin des cd");
        skill.isCooldown = false;
    }

}
