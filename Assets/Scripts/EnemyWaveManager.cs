using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    
    public static EnemyWaveManager Instance { get; private set; }
    public event EventHandler OnWaveNumberChanged;
    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }

    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;

    private State state;
    private int waveNumber;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
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
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, 0.2f);
                        Enemy.Create(spawnPosition + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
                        remainingEnemySpawnAmount--;

                        if (remainingEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                            spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPositionTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 10f;
                        }
                    }
                }
                break;
        }   
    }

    private void SpawnWave()
    {
        remainingEnemySpawnAmount = 5 + 3*waveNumber;
        state = State.SpawningWave;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }
    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }
    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
