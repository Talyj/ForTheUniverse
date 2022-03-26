using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermaidBehaviour : PlayerStats
{
    private int _passiveCounter;
    private bool isPassiveStart;

    public GameObject poissoin;

    private float ultiTimerDefault = 1;
    private float charmSpeed;
    private List<GameObject> charmTargets;
    public GameObject charmArea;

    public MermaidBehaviour Instance;

    public void Start()
    {
        charmSpeed = 5;
        charmTargets = new List<GameObject>();
        canMove = true;
        Instance = this;
        isPassiveStart = false;
        _passiveCounter = 0;
        foreach(var elmt in skills)
        {
            elmt.isCooldown = false;
        }
    }

    public void Update()
    {
        Movement();
        HealthBehaviour();
        ExperienceBehaviour();
        AttackSystem();
        Passif();
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < AttackRange)
            {
                Poissoin(Targetable.EnemyType.minion, Cible);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Skill2();
        }

        if (Input.GetKeyDown(KeyCode.Space)/* && canUlt == true*/)
        {
            Ultime();
        }
        Debug.Log(charmTargets);
    }

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

    public void Poissoin(Targetable.EnemyType typeEnemy, GameObject Target)
    {
        if (skills[0].isCooldown == false && Mana >= skills[0].Cost)
        {
            Mana -= skills[0].Cost;
            Debug.Log(skills[0].Name + " lancée");
            skills[0].isCooldown = true;

            //TODO
            float dmg = DegatsMagique;
            if (typeEnemy == Targetable.EnemyType.minion /*||typeEnemy == Targetable.EnemyType.Adversaire*/)
            {
                var proj = Instantiate(poissoin, SpawnPrefab.transform.position, Quaternion.identity);
                proj.GetComponent<PoissoinProjBehaviour>().degats = dmg;
                proj.GetComponent<PoissoinProjBehaviour>().target = Target;
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

    public void Skill2()
    {
        if (skills[1].isCooldown == false && Mana >= skills[1].Cost)
        {
            //buff
            Mana -= skills[1].Cost;
            Debug.Log(skills[1].Name + " lancée");
            //TODO
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
