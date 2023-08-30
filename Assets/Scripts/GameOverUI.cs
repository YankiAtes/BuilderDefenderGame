using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{

    public static GameOverUI Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        transform.Find("RestartButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });

        transform.Find("MainMenuButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        transform.Find("WavesSurvivedText").GetComponent<TextMeshProUGUI>().
            SetText("You Survived " + EnemyWaveManager.Instance.GetWaveNumber() + " Waves"); 
    }

    private void Hide()
    {
        gameObject.SetActive(false);    
    }
}
