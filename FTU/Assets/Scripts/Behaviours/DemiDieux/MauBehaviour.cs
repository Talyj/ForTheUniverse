using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MauBehaviour : PlayerStats
{
    //Skill1
    public GameObject roarArea;

    public MauBehaviour Instance;

    private float baseMag;
    private float basePhys;

    [HideInInspector] public Transform templeTransform;

    


    // Start is called before the first frame update
    public void Start()
    {
        baseMag = 1;
        basePhys = 1;
        Init();
        SetHealth(1f);
        SetMaxHealth(5000f);
        SetMoveSpeed(60f);
        SetAttackRange(30f);
        SetDegMag(baseMag);
        SetDegPhys(basePhys);
        Instance = this;
        foreach (var elmt in skills)
        {
            elmt.isCooldown = false;
        }
        StartCoroutine(UseSkill());
    }

    // Update is called once per frame
    public void Update()
    {
        HealthBehaviour();
        //ExperienceBehaviour();
        Passif();
        //MovementPlayer();

        if (GetCanAct())
        {
            if (Cible == null)
            {
                GetNearestTarget();
            }
            else WalkToTarget();
            DefaultHeroBehaviourAI();
            CheckTarget();

            //Control boss as a player for TEST
            //MovementPlayer();
            //if (Input.GetKeyDown(KeyCode.Alpha1) && Cible != null && Vector3.Distance(gameObject.transform.position, Cible.transform.position) < GetAttackRange() * 4)
            //{
            //    Roar();
            //}

            //if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    Stomp();
            //}

            //if (Input.GetKeyDown(KeyCode.Alpha3)/* && GetCanUlt() == true*/)
            //{
            //    Ultime();
            //}
        }
    }

    new public void CheckTarget()
    {

        if (Cible == null)
        {          
            Cible = null;
        }
        else
        {
            var dist = Vector3.Distance(Cible.transform.position, templeTransform.position);
            if (dist >= 50) { }
        }
    }

    public IEnumerator UseSkill()
    {
        if(Cible != null)
        {
            var rdmSkill = Random.Range(0, 2);
            switch (rdmSkill)
            {
                case 0:
                    Roar();
                    break;
                case 1:
                    Stomp();
                    break;
                case 2:
                    Ultime();
                    break;
            }
        }
        yield return new WaitForSeconds(30);
        StartCoroutine(UseSkill());
    }

    new public void DefaultHeroBehaviourAI()
    {
        if (Cible == null)
        {
            isAttacking = false;
        }

        if (!isAttacking && Cible != null)
        {
            isAttacking = true;
            AttackSystemAI();
        }
    }

    public List<GameObject> GetTargetsAround(bool isAlly, float rangeMult = 1)
    {
        var enemyTargets = new List<GameObject>();
        var AllyTargets = new List<GameObject>();

        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, GetAttackRange() * rangeMult);

        if (hitColliders != null)
        {
            foreach (var col in hitColliders)
            {
                if (col.gameObject.CompareTag("Player") ||
                    col.gameObject.CompareTag("minion") ||
                    col.gameObject.CompareTag("golem"))
                {
                    if (col.gameObject.GetComponent<IDamageable>().team != team)
                    {
                        enemyTargets.Add(col.gameObject);
                    }
                    else
                    {
                        AllyTargets.Add(col.gameObject);
                    }
                }

            }
        }

        if (isAlly)
        {
            return AllyTargets;
        }
        return enemyTargets;
    }

    //Copy that in a new character file

    public void Passif()
    {
        float allyArountCount = GetTargetsAround(true).Count;
        SetDegMag(baseMag + (3f * allyArountCount));
        SetDegPhys(basePhys + (3f * allyArountCount));
    }

    //Copy that in a new character file (skill1)
    public void Roar()
    {
        if (skills[0].isCooldown == false && GetMana() >= skills[0].Cost)
        {
            SetMana(GetMana() - skills[0].Cost);
            Debug.Log(skills[0].Name + " lanc�e");
            skills[0].isCooldown = true;

            //Get direction (similar to MagicWind for mermaid)
            var targetPos = new Vector3(Cible.transform.position.x, transform.position.y, Cible.transform.position.z);
            var pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            
            Quaternion rotation = Quaternion.LookRotation(targetPos - pos);
            Vector3 direction = targetPos - pos;

            var proj = PhotonNetwork.Instantiate(roarArea.name, transform.position, rotation);
            proj.GetComponent<RoarBehaviour>().SetDamages(GetDegMag(), DamageType.magique);
            proj.GetComponent<RoarBehaviour>().direction = direction;
            proj.GetComponent<RoarBehaviour>().source = Instance;

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

    public void RoaredDamageTarget(GameObject target)
    {
        target.GetComponent<IDamageable>().TakeDamage(GetDegMag() * 2, DamageType.magique);
    }

    //Copy that in a new character file (skill2)
    public void Stomp()
    {
        if (skills[1].isCooldown == false && GetMana() >= skills[1].Cost)
        {
            //buff
            SetMana(GetMana() - skills[1].Cost);
            Debug.Log(skills[1].Name + " lanc�e");
            skills[1].isCooldown = true;

            var stompTargets = GetTargetsAround(false, 0.5f);
            foreach(var tar in stompTargets)
            {
                tar.GetComponent<IDamageable>().TakeDamage(GetDegPhys() * 2, DamageType.physique);
            }


            StartCoroutine(CoolDown(skills[1]));
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
        if (skills[2].isCooldown == false && GetMana() >= skills[2].Cost)
        {
            //buff
            SetMana(GetMana() - skills[2].Cost);
            Debug.Log(skills[2].Name + " lanc�e");

            var allies = GetTargetsAround(true);
            var degMag = GetDegMag();
            var healBonus = (degMag * allies.Count) / 100;
            SetHealth(GetHealth() + (degMag + healBonus));

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

    //Copy that in a new character file
    public void Eveil()
    {
        throw new System.NotImplementedException();
    }
}
