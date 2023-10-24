using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PatrolType myPatrol;
    float baseSpeed = 1f;
    public float mySpeed = 1f;
    //float moveDistace = 1000f;

    int baseHealth = 100;
    public int myHealth;

    [Header("AI")]
    public EnemyType myType;
    public Transform moveToPos; //Needed for all patrols
    public EnemyManager _EM;
    Transform startPos;         //Needed for loop patrol movement
    Transform endPos;           //Needed for loop patrol movement
    bool reverse;               //Needed for loop patrol movement
    int patrolPoint = 0;        //Needed for linear patrol movemnt



    void Start()
    {
        _EM = FindObjectOfType<EnemyManager>();

        switch (myType)
        {
            case EnemyType.OneHand:
                myHealth = baseHealth;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Linear;
                break;
            case EnemyType.TwoHand:
                myHealth = baseHealth * 2;
                mySpeed = baseSpeed / 2;
                myPatrol = PatrolType.Random;
                break;
            case EnemyType.Archer:
                myHealth = baseHealth / 2;
                mySpeed = baseSpeed * 2;
                myPatrol = PatrolType.Loop;
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

    IEnumerator Test()
    {
        print("HI");
        yield return new WaitForSeconds(1f);
        print("What took you so long?");
        yield return new WaitForSeconds(Random.Range(2f, 5f));
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
}
