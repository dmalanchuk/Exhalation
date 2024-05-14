
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;
using Random = System.Random;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Wave
    {
        public Enemy[] enemies;
        public int count;
        public float timeBtwSpawn;
    }

    [SerializeField] private Wave[] waves;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBtwWaves;
    private Wave currentwWave;
    [HideInInspector]public int currentwWaveIndex;
    private Transform player;

    private bool isSpawnFinished = false;
    private bool isFreeTime = true;
    private float currentTimeBtwWaves;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject spawnEffect;

    private void Start()
    {
        player = Player.instance.transform;

        currentTimeBtwWaves = timeBtwWaves;
        
        UpdateText();
        StartCoroutine(CallNextWave(currentwWaveIndex));
    }

    private void Update()
    {

        UpdateText();
            
        if (isSpawnFinished && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            isSpawnFinished = false;

            if (currentwWaveIndex + 1 < waves.Length)
            {
                currentwWaveIndex++;
                
                StartCoroutine(CallNextWave(currentwWaveIndex));
            }
        }
    }

    void UpdateText()
    {
        if (isFreeTime) waveText.text = "To the next wave: " + ((int)(currentTimeBtwWaves -= Time.deltaTime)).ToString();

        else waveText.text = "Current wave: " + currentwWaveIndex.ToString();
    }

    IEnumerator CallNextWave(int waveIndex)
    {
        currentTimeBtwWaves = timeBtwWaves;
        
        isFreeTime = true;
        yield return new WaitForSeconds(timeBtwWaves);
        isFreeTime = false;
        StartCoroutine(SpawnWave(waveIndex));
    }

    IEnumerator SpawnWave(int waveIndex)
    {
        currentwWave = waves[waveIndex];

        for (int i = 0; i < currentwWave.count; i++)
        {
            if (player == null) yield break;

            Enemy randomEnemy = currentwWave.enemies[UnityEngine.Random.Range(0, currentwWave.enemies.Length)];

            Transform randomSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

            Instantiate(randomEnemy, randomSpawnPoint.position, Quaternion.identity);
            Instantiate(spawnEffect, randomSpawnPoint.position, Quaternion.identity);


            if (i == currentwWave.count - 1)
            {
                isSpawnFinished = true;
            }
            else
            {
                isSpawnFinished = false;
            }

            yield return new WaitForSeconds(currentwWave.timeBtwSpawn);
        }
    }
}


