using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public WaveSpawner waveSpawner; // Gán từ Inspector
    public Image Time_wave_Spawn;
    public TMP_Text ScoreZombiesKill;
    private float time_remaining;
    public int zombieScoreKill;
    public GameObject button;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
        }

        if (ScoreZombiesKill == null)
        {
            ScoreZombiesKill = GameObject.Find("Kill").GetComponent<TMP_Text>();
        }
    }
    private void Start()
    {
        if (waveSpawner == null)
        {
            Debug.LogError("WaveSpawner is not assigned in the Inspector!");
            return;
        }

        time_remaining = waveSpawner.timeBetweenWaves;

        zombieScoreKill = GameManager.instance.zombiesKillScore;

        UpdateKillScore(GameManager.instance.zombiesKillScore);
        button.GetComponent<CanvasGroup>().ignoreParentGroups = true;
    }

    private void Update()
    {
        if (waveSpawner != null)
        {
            if (time_remaining > 0)
            {
                time_remaining -= Time.deltaTime;
                Time_wave_Spawn.fillAmount = time_remaining / waveSpawner.timeBetweenWaves;
            }
            else
            {
                // Đặt lại thanh tiến trình cho wave tiếp theo
                time_remaining = waveSpawner.timeBetweenWaves;
            }
        }
    }
    public void UpdateKillScore(int NewScore)
    {
        zombieScoreKill = NewScore;
        ScoreZombiesKill.text = $"Kill: {zombieScoreKill}";
    }
    public void Exit()
    {
        Debug.Log("da an");
        SceneManager.LoadScene("MainMenu");
       
    }

}
