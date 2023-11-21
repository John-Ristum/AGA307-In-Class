using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : GameBehaviour
{
    public static event Action<GameObject> OnEnemyHit = null;
    public static event Action<GameObject> OnEnemyDie = null;


    public PatrolType myPatrol;
    float baseSpeed = 1f;
    public float mySpeed = 1f;
    //float moveDistace = 1000f;

    int baseHealth = 100;
    int maxHealth;
    public int myHealth;
    public int myScore;
    public int myDamage = 20;
    EnemyHealthBar healthBar;

    public string myName;

    [Header("AI")]
    public EnemyType myType;
    public Transform moveToPos; //Needed for all patrols
    Transform startPos;         //Needed for loop patrol movement
    Transform endPos;           //Needed for loop patrol movement
    bool reverse;               //Needed for loop patrol movement
    int patrolPoint = 0;        //Needed for linear patrol movemnt
    public float attackDistance = 2f;
    public float detectTime = 5f;
    public float detectDistance = 10f;
    int currentWaypoint;
    NavMeshAgent agent;

    Animator anim;



    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        SetName(_EM.GetEnemyName());

        switch (myType)
        {
            case EnemyType.OneHand:
                myHealth = maxHealth = baseHealth;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Patrol;
                myScore = 100;
                myDamage = 20;
                break;
            case EnemyType.TwoHand:
                myHealth = maxHealth = baseHealth * 2;
                mySpeed = baseSpeed / 2;
                myPatrol = PatrolType.Patrol;
                myScore = 200;
                myDamage = 30;
                break;
            case EnemyType.Archer:
                myHealth = maxHealth = baseHealth / 2;
                mySpeed = baseSpeed * 2;
                myPatrol = PatrolType.Patrol;
                myScore = 300;
                myDamage = 10;
                break;

        }

        SetupAI();
        if(GetComponentInChildren<EnemyWeapon>() != null)
            GetComponentInChildren<EnemyWeapon>().damage = myDamage;
    }

    void SetupAI()
    {
        currentWaypoint = UnityEngine.Random.Range(0, _EM.spawnPoints.Length);
        agent.SetDestination(_EM.spawnPoints[currentWaypoint].position);
        ChangeSpeed(mySpeed);
    }

    void ChangeSpeed(float _speed)
    {
        agent.speed = _speed;
    }

    private void Update()
    {
        if (myPatrol == PatrolType.Die)
            return;

        //Always get the distance betweent eh player and me
        float distToPlayer = Vector3.Distance(transform.position, _PLAYER.transform.position);

        if(distToPlayer <= detectDistance && myPatrol != PatrolType.Attack)
        {
            if(myPatrol != PatrolType.Chase)
            {
                myPatrol = PatrolType.Detect;
            }
        }

        //Set the animator's speed paramter to that of my speed
        anim.SetFloat("Speed", mySpeed);

        //Switching patrol states logic
        switch (myPatrol)
        {
            case PatrolType.Patrol:
                //Get the distance between us and the current waypoint
                float distToWaypoint = Vector3.Distance(transform.position, _EM.spawnPoints[currentWaypoint].position);
                //If the ditance is close enough, get a new waypoint
                if (distToWaypoint < 1)
                    SetupAI();
                //Reset the detect time
                detectTime = 5;
                break;

            case PatrolType.Detect:
                //Set Destination to ourself, essentially stopping us
                agent.SetDestination(transform.position);
                //Stop our speed
                ChangeSpeed(0);
                //Decrement our detect time
                detectTime -= Time.deltaTime;
                if(distToPlayer <= detectDistance)
                {
                    myPatrol = PatrolType.Chase;
                    detectTime = 5;
                }
                if(detectTime < 0)
                {
                    myPatrol = PatrolType.Patrol;
                    SetupAI();
                }
                break;

            case PatrolType.Chase:
                //Set the destination to that of the player
                agent.SetDestination(_PLAYER.transform.position);
                //Increase the speed of which to chance the player
                ChangeSpeed(mySpeed * 2);
                //If the player gets outside the detect distance, go back to the detect stage
                if (distToPlayer > detectDistance)
                    myPatrol = PatrolType.Detect;
                //If we are close to the player, then attack
                if (distToPlayer <= attackDistance)
                    StartCoroutine(Attack());
                break;
        }
    }

    public void SetName(string _name)
    {
        name = _name;
        healthBar.SetName(name);
    }

    IEnumerator Test()
    {
        print("HI");
        yield return new WaitForSeconds(1f);
        print("What took you so long?");
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 5f));
        print("I miss you");
        yield return new WaitForSeconds(1f);
        print("Fine, I'm leaving");
    }


    IEnumerator Attack()
    {
        myPatrol = PatrolType.Attack;
        ChangeSpeed(0);
        PlayAnimation("Attack");
        yield return new WaitForSeconds(1);
        ChangeSpeed(mySpeed);
        myPatrol = PatrolType.Chase;
    }

    /*IEnumerator Move()
    {
        for (int i = 0; i < moveDistace; i++)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * mySpeed);
            yield return null;
        }
        transform.Rotate(Vector3.up * 180);
        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine(Move());
    }*/

    private void Hit(int _damage)
    {
        myHealth -= _damage;
        healthBar.UpdateHealthBar(myHealth, maxHealth);
        //ScaleObject(this.gameObject, transform.localScale * 1.5f);
        if (myHealth <= 0)
        {
            Die();
        }
        else
        {
            int rnd = UnityEngine.Random.Range(1, 4);
            PlayAnimation("Hit");
            OnEnemyHit?.Invoke(this.gameObject);
            //_GM.AddScore(myScore);
        }
    }

    private void Die()
    {
        myPatrol = PatrolType.Die;
        ChangeSpeed(0);
        GetComponent<Collider>().enabled = false;
        PlayAnimation("Die");
        StopAllCoroutines();
        OnEnemyDie?.Invoke(this.gameObject);
    }

    void PlayAnimation(string _animationName)
    {
        int rnd = UnityEngine.Random.Range(1, 4);
        anim.SetTrigger(_animationName + rnd);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
            Hit(collision.gameObject.GetComponent<Projectile>().damage);
            Destroy(collision.gameObject);
    }
}
