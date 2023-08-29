using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }

    [SerializeField] private List<Transform> spawnPositionTransformList;
    private State state;
    private int waveNumber;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;
    void Start()
    {
        spawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
        state = State.WaitingToSpawnNextWave;
        nextWaveSpawnTimer = 3f;
    }

    void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnNextWave:

                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer <= 0)
                {
                    SpawnWave();
                }
                break;

            case State.SpawningWave:

                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer <= 0)
                    {
                        nextEnemySpawnTimer = Random.Range(0f, 0.2f);
                        Enemy.Create(spawnPosition + UtilsClass.GetRandomDir() * Random.Range(0f, 10f));
                        remainingEnemySpawnAmount--;

                        if (remainingEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                            spawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
                        }
                    }
                }
                break;
        }   
    }

    private void SpawnWave()
    {
        nextWaveSpawnTimer = 10f;
        remainingEnemySpawnAmount = 5 + 3*waveNumber;
        state = State.SpawningWave;
        waveNumber++;
    }
}
