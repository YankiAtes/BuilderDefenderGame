using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager;

    private TextMeshProUGUI waveNumberText;
    private TextMeshProUGUI waveMessageText;
    private RectTransform enemyWaveSpawnPositionIndicator;
    private RectTransform enemyClosestPositionIndicator;
    private Camera mainCamera;
    private void Awake()
    {
        waveNumberText = transform.Find("WaveNumberText").GetComponent<TextMeshProUGUI>();
        waveMessageText = transform.Find("WaveMessageText").GetComponent<TextMeshProUGUI>();
        enemyWaveSpawnPositionIndicator = transform.Find("EnemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();
        enemyClosestPositionIndicator = transform.Find("EnemyClosestPositionIndicator").GetComponent<RectTransform>();
    }
    void Start()
    {
        mainCamera = Camera.main;   
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
        SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, System.EventArgs e)
    {
        SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
    }

    void Update()
    {

        HandleNextWaveMessage();  
        HandleEnemyWaveSpawnPositionIndicator();
        HandleEnemyClosestPositionIndicator();
        
    }
        
    private void HandleNextWaveMessage()
    {
        float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
        if (nextWaveSpawnTimer <= 0f)
        {
            SetMessageText("");
        }
        else
        {
            SetMessageText("Next Wave in " + nextWaveSpawnTimer.ToString("F1") + "s");
        }
    }

    private void HandleEnemyWaveSpawnPositionIndicator()
    {

        Vector3 dirToTheNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - mainCamera.transform.position).normalized;
        enemyWaveSpawnPositionIndicator.anchoredPosition = dirToTheNextSpawnPosition * 300f;
        enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToTheNextSpawnPosition));


        float distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), mainCamera.transform.position);

        enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5);
    }

    private void HandleEnemyClosestPositionIndicator()
    {
        float targetMaxRadius = 9999f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(mainCamera.transform.position, targetMaxRadius);

        Enemy targetEnemy = null;
        foreach (Collider2D collider2d in collider2DArray)
        {
            Enemy enemy = collider2d.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }
        if (targetEnemy != null)
        {
            Vector3 dirToClosestEnemy = (targetEnemy.transform.position - mainCamera.transform.position).normalized;
            enemyClosestPositionIndicator.anchoredPosition = dirToClosestEnemy * 250f;
            enemyClosestPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToClosestEnemy));


            float distanceToClosestEnemy = Vector3.Distance(targetEnemy.transform.position, mainCamera.transform.position);

            enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > mainCamera.orthographicSize * 1.5);
        }
        else
        {
            //No enemies alive
            enemyClosestPositionIndicator.gameObject.SetActive(false);
        }
    }


    private void SetMessageText(string message)
    {
        waveMessageText.SetText(message);  
    }

    private void SetWaveNumberText(string text)
    {
        waveNumberText.SetText(text);
    }
}
