using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public delegate void SpawningEventHandler();
    public static event SpawningEventHandler OnStartedSpawning;
    public static event SpawningEventHandler OnStoppedSpawning;

    [SerializeField] private List<Wave> waves;
    [SerializeField] private float spawnInterval;

    private int enemiesSpawned = 0;
    private EnemyFactory enemyFactory;
    private List<Enemy> enemies;
    private Wave currentWave;
    private void Start()
    {
        GameManager.OnWaveStart += StartSpawning;
        GameManager.OnWaveEnd += StopSpawning;
    }
    public void Update()
    {
        currentWave = waves[GameManager.WaveNumber];
        if (GameManager.IsGameMode(GameMode.WaveInProgress))
        {
            if (enemies != null && enemiesSpawned >= enemies.Count)
                StopSpawning();
            GetComponent<Indicator>().Hide();
        }
        else if (GameManager.IsGameMode(GameMode.Build))
        {
            if (currentWave != null)
                GetComponent<Indicator>().Show(new Vector3(0, 1.0f, 0.0f));
        }
    }

    public void StartSpawning()
    {
        enemyFactory = new EnemyFactory();

        if (waves[GameManager.WaveNumber] != null)
            currentWave = waves[GameManager.WaveNumber];
        else if(GameManager.WaveNumber >= waves.Count)
            currentWave = waves[UnityEngine.Random.Range(0,waves.Count)];
            

        if(currentWave != null)
            enemies = currentWave.enemies;
        else
        {
            StopSpawning();
            return;
        }

        enemiesSpawned = 0;
        if (enemies.Count > 0)
            StartCoroutine(RepeatUntil());

        OnStartedSpawning?.Invoke();
    }

    public void StopSpawning()
    {
        OnStoppedSpawning?.Invoke();
        enemies = null;
        StopCoroutine(RepeatUntil());
    }

    IEnumerator RepeatUntil()
    {
        while (enemies != null && enemiesSpawned < enemies.Count)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        var enemy = enemyFactory.CreateInstance(enemies[enemiesSpawned]);
        enemy.transform.position = transform.position + new Vector3(0, .5f, 0);
        enemiesSpawned++;
    }
}

