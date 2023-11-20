using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManager;

    [SerializeField]
    GameObject enemyPrefab;

    public List<EnemyAIManager> enemyAIManagers;

    [SerializeField]
    Transform[] spawnPoints;

    int numberofEnemy;

    int waveCount = 0;

    [SerializeField]
    Transform playerBase;

    [SerializeField] TMP_Text waveCoolDownText;


    private void Awake()
    {
        enemyManager = this;

    }

    private void OnEnable()
    {
        GameEvents.Instance.OnEnemySpawnWave += CreateNewWave;

    }

    private void OnDisable()
    {
        GameEvents.Instance.OnEnemySpawnWave -= CreateNewWave;
    }



    void CreateNewWave(int time)
    {
        StartCoroutine(WaveCoolDown(time));

    }


    IEnumerator WaveCoolDown(int time)
    {
        while (time > 0)
        {
            if (time < 120)
            {
                waveCoolDownText.text = "Düşman Dalgasının Saldırıya Geçmesine "+time.ToString()+ " Saniye";
            }

            yield return new WaitForSeconds(1);

            time--;
        }

        StartCoroutine(CalculateTheWave());

        waveCoolDownText.text = "Düşmanlar Geliyor";

        yield return new WaitForSeconds(30);
        waveCoolDownText.text = "";



    }


    IEnumerator CalculateTheWave()
    {
        yield return null;

        waveCount++;

        numberofEnemy = (int)(Mathf.Pow(waveCount, 1.5f) + 5 + Mathf.Sin(waveCount));

        int randomPoint = UnityEngine.Random.Range(0, spawnPoints.Length);

        Transform randomSpawnPoints = spawnPoints[randomPoint];


        SpawnEnemy(numberofEnemy, randomSpawnPoints);

    }



    void SpawnEnemy(int _numberofEnem1, Transform _spawnPoint)
    {
        Vector3 offset = UnityEngine.Random.insideUnitCircle * 5;

        for (int i = 0; i < _numberofEnem1; i++)
        {

            offset = UnityEngine.Random.insideUnitCircle * 5;

            GameObject newEnemy = Instantiate(enemyPrefab, _spawnPoint.position + offset, quaternion.identity);

            enemyAIManagers.Add(newEnemy.GetComponent<EnemyAIManager>());

            StartCoroutine(newEnemy.GetComponent<EnemyAIManager>().SetupSpawn(UnityEngine.Random.Range(10, 15), playerBase));
        }


        GameEvents.Instance.OnEnemySpawnWave?.Invoke(360);


    }






}
