using System;
using System.Collections;
using UnityEngine;

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
    EnemyHealthBar healthBar;

    public string myName;

    [Header("AI")]
    public EnemyType myType;
    public Transform moveToPos; //Needed for all patrols
    Transform startPos;         //Needed for loop patrol movement
    Transform endPos;           //Needed for loop patrol movement
    bool reverse;               //Needed for loop patrol movement
    int patrolPoint = 0;        //Needed for linear patrol movemnt



    void Start()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        SetName(_EM.GetEnemyName());

        switch (myType)
        {
            case EnemyType.OneHand:
                myHealth = maxHealth = baseHealth;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Linear;
                myScore = 100;
                break;
            case EnemyType.TwoHand:
                myHealth = maxHealth = baseHealth * 2;
                mySpeed = baseSpeed / 2;
                myPatrol = PatrolType.Random;
                myScore = 200;
                break;
            case EnemyType.Archer:
                myHealth = maxHealth = baseHealth / 2;
                mySpeed = baseSpeed * 2;
                myPatrol = PatrolType.Loop;
                myScore = 300;
                break;

        }

        SetUpAI();
    }

    void SetUpAI()
    {
        startPos = Instantiate(new GameObject(), transform.position, transform.rotation).transform;
        endPos = _EM.GetRandomSpawnPoint();
        moveToPos = _EM.GetRandomSpawnPoint();
        StartCoroutine(Move());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            StopAllCoroutines();
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

    IEnumerator Move()
    {
        switch (myPatrol)
        {
            case PatrolType.Linear:
                moveToPos = _EM.spawnPoints[patrolPoint];
                patrolPoint = patrolPoint != _EM.spawnPoints.Length ? patrolPoint + 1 : 0;
                break;
            case PatrolType.Random:
                moveToPos = _EM.GetRandomSpawnPoint();
                break;
            case PatrolType.Loop:
                // if reverse is equal to startPos, moveToPos = endPos
                moveToPos = reverse ? startPos : endPos;
                reverse = !reverse;
                break;
        }
        
        transform.LookAt(moveToPos);
        while(Vector3.Distance(transform.position, moveToPos.position) > 0.3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPos.position, Time.deltaTime * mySpeed);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(Move());
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
        ScaleObject(this.gameObject, transform.localScale * 1.5f);
        if (myHealth <= 0)
        {
            Die();
        }
        else
        {
            OnEnemyHit?.Invoke(this.gameObject);
            //_GM.AddScore(myScore);
        }
    }

    private void Die()
    {
        StopAllCoroutines();
        OnEnemyDie?.Invoke(this.gameObject);
        //_GM.AddScore(myScore * 2);
        //_EM.KillEnemy(this.gameObject);
        //Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
            Hit(collision.gameObject.GetComponent<Projectile>().damage);
            Destroy(collision.gameObject);
    }
}
