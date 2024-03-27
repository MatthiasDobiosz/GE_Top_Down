using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public float timeLeft;
    public bool isCounting = false;
    public TMP_Text TimeText;

    void Start()
    {
        HideTimer();
    }

    void Update()
    {
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
