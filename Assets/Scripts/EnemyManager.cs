using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { OneHand, TwoHand, Archer }

public enum PatrolType { Linear, Random, Loop }

public class EnemyManager : Singleton<EnemyManager>
{
    public Transform[] spawnPoints;
    public string[] enemyNames;
    public GameObject[] enemyTypes;

    public List<GameObject> enemies;
    public string killCondition = "Two";

    private void Start()
    {
        //SpawnEnemies();

        //SpawnAtRandom();

        StartCoroutine(SpawnEnemiesWithDelay());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SpawnAtRandom();

        if (Input.GetKeyDown(KeyCode.K))
            KillEnemy(enemies[0]);

        if (Input.GetKeyDown(KeyCode.O))
            KillAllEnemies();
        if (Input.GetKeyDown(KeyCode.P))
            KillSpecificEnemies(killCondition);
    }

    /// <summary>
    /// Spawns an enemy at every spawn point
    /// </summary>
    void SpawnEnemies()
    {
        for (int i = 0; i <= spawnPoints.Length - 1; i++)
        {
            int rnd = Random.Range(0, enemyTypes.Length);
            GameObject enemy = Instantiate(enemyTypes[rnd], spawnPoints[i].position, spawnPoints[i].rotation);
            enemy.name = enemyNames[i];
            enemies.Add(enemy);
        }
    }

    /// <summary>
    /// Spawns an enemy every random amount of second
    /// </summary>
    IEnumerator SpawnEnemiesWithDelay()
    {
        for (int i = 0; i <= spawnPoints.Length - 1; i++)
        {
            int rnd = Random.Range(0, enemyTypes.Length);
            GameObject enemy = Instantiate(enemyTypes[rnd], spawnPoints[i].position, spawnPoints[i].rotation);
            enemy.name = enemyNames[i];
            enemies.Add(enemy);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    /// <summary>
    /// Spawns a random enemy at a random spawn point
    /// </summary>
    public void SpawnAtRandom()
    {
        int rndEnemy = Random.Range(0, enemyTypes.Length);
        int rndSpawn = Random.Range(0, spawnPoints.Length);
        int rndName = Random.Range(0, enemyNames.Length);
        GameObject enemy = Instantiate(enemyTypes[rndEnemy], spawnPoints[rndSpawn].position, spawnPoints[rndSpawn].rotation);
        //enemy.name = enemyNames[rndName];
        enemies.Add(enemy);
        ShowEnemyCount();
    }

    /// <summary>
    /// Shows the amount of enemies in the stage
    /// </summary>
    void ShowEnemyCount()
    {
        print("Number of enemies: " + enemies.Count);
    }

    /// <summary>
    /// Kills a specific enemy
    /// </summary>
    /// <param name="_enemy">The enemy we want to kill</param>
    public void KillEnemy(GameObject _enemy)
    {
        if (enemies.Count == 0)
            return;

        Destroy(_enemy);
        enemies.Remove(_enemy);
        ShowEnemyCount();
    }

    /// <summary>
    /// Kills all enemies in our scene
    /// </summary>
    void KillAllEnemies()
    {
        if (enemies.Count == 0)
            return;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            KillEnemy(enemies[i]);
        }
    }

    /// <summary>
    /// Kills all enemies with the same string condition
    /// </summary>
    /// <param name="_condition"></param>
    void KillSpecificEnemies(string _condition)
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].name.Contains(_condition))
                KillEnemy(enemies[i]);
        }
    }

    /// <summary>
    /// Get a random spawn point
    /// </summary>
    /// <returns>A random spawn point</returns>
    public Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    void Examples()
    {
        int numberRepititions = 2000;
        for (int i = 0; i <= numberRepititions; i++)
        {
            print(i);
        }


        GameObject first = Instantiate(enemyTypes[0], spawnPoints[0].position, spawnPoints[0].rotation);
        first.name = enemyNames[0];

        int lastEnemy = enemyTypes.Length - 1;
        GameObject last = Instantiate(enemyTypes[lastEnemy], spawnPoints[lastEnemy].position, spawnPoints[lastEnemy].rotation);
        last.name = enemyNames[enemyNames.Length - 1];


        //Create a loop within a loop for a wall
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; i < 10; j++)
            {
                Instantiate(wall, new Vector3(i, j, 0), transform.rotation);
            }
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDie += KillEnemy;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDie -= KillEnemy;
    }
}
