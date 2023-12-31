using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text enemyCountText;

    [Header("Game Data Stuff")]
    public TMP_Text totalKillText;
    public TMP_Text totalPlayedTimeText;

    private void Start()
    {
        UpdateScore(0);
        UpdateTime(0f);
        UpdateEnemyCount(0);
        UpdateKillTotal();
        UpdatePlayTimeTotal();
    }

    public void UpdateScore(int _score)
    {
        scoreText.text = "Score: " + _score.ToString();
    }

    public void UpdateTime(float _time)
    {
        timeText.text = _time.ToString("F2");
    }

    public void UpdateEnemyCount(int _enemyCount)
    {
        enemyCountText.text = "Enemy Count: " + _enemyCount.ToString();
    }

    public void UpdateKillTotal()
    {
        totalKillText.text = "Total Kills: " + _DATA.GetEnemyKillCount();
    }

    public void UpdatePlayTimeTotal()
    {
        totalPlayedTimeText.text = "Total Playtime: " + _DATA.GetTimeFormatted();
    }
}
