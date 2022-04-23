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
    private void Start()
    {
        GameManager.OnWaveStart += StartSpawning;
        GameManager.OnWaveEnd += StopSpawning;
    }
    public void Update()
    {
        if (GameManager.IsGameMode(GameMode.WaveInProgress))
        {
            if (enemies != null && enemiesSpawned >= enemies.Count)
                StopSpawning();
            GetComponent<Indicator>().Hide();
        }
        else if (GameManager.IsGameMode(GameMode.Build))
        {
            if (waves[GameManager.WaveNumber] != null)
                GetComponent<Indicator>().Show(new Vector3(0, 1.0f, 0.0f));
        }
    }

    public void StartSpawning()
    {
        if (GameManager.WaveNumber >= waves.Count)
            return;

        Debug.Log("StartSpawning");
        enemyFactory = new EnemyFactory();

        if (waves[GameManager.WaveNumber] != null)
            enemies = waves[GameManager.WaveNumber].enemies;
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
        Debug.Log("Stopped Spawning");
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

