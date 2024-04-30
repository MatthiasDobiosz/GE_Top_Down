using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
    Countdown logic for the end-sequence
*/
public class CountdownTimer : MonoBehaviour
{
    public float timeLeft;
    public bool isCounting = false;
    public TMP_Text TimeText;
    private float overallTime;

    void Start()
    {
        HideTimer();
    }

    void Update()
    {
        overallTime += Time.deltaTime;
        if(isCounting)
        {
            if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            } else 
            {
                timeLeft = 0;
                isCounting = false;
                HideTimer();
                LogManager.Instance.Log("Tode ingesamt: " + FindObjectOfType<PlayerRespawn>().deathCount);
                LogManager.Instance.Log("Zeit insgesamt: " + overallTime);
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimeText.text = string.Format("{0:00}: {1:00}", minutes, seconds);
    }

    public void HideTimer()
    {
        TimeText.alpha = 0;
    }

    public void ShowTimer()
    {
        TimeText.alpha = 1;
    }
}
