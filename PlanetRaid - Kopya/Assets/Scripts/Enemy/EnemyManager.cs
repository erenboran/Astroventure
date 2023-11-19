using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManager;

    [SerializeField]
    GameObject enemyPrefab;

    public List<EnemyAIManager> enemyAIManagers;

    [SerializeField]
    Transform[] targetPoints, spawnPoints;

    int numberofEnemy;

    int waveCount;


    private void Awake()
    {
        enemyManager = this;

    }


    void CalculateTheWave()
    {
        waveCount++;

        numberofEnemy = (int)(Mathf.Pow(waveCount, 1.5f) + 5 + Mathf.Sin(waveCount));

        int randomPoint = UnityEngine.Random.Range(0, spawnPoints.Length);

        Transform randomSpawnPoints = targetPoints[randomPoint];


        SpawnEnemy(numberofEnemy, randomSpawnPoints);

    }



    void SpawnEnemy(int _numberofEnem1, Transform _spawnPoint)
    {


        for (int i = 0; i < _numberofEnem1; i++)
        {

            GameObject newEnemy  = Instantiate(enemyPrefab, _spawnPoint.position, quaternion.identity);

            enemyAIManagers.Add(newEnemy.GetComponent<EnemyAIManager>());

            StartCoroutine(newEnemy.GetComponent<EnemyAIManager>().SetupSpawn(5, targetPoints[0]));

         

        }


    }






}
