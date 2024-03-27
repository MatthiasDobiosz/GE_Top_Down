using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartEndSequence : MonoBehaviour
{
    public GameObject gameOverScreen;

    private AudioManager audioManager;
    private CountdownTimer countdownTimer;
    private bool isPlaying = false;
    private Image gameOverImage;
    private bool gameOver = false;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        countdownTimer = GetComponent<CountdownTimer>();

        gameOverImage = gameOverScreen.GetComponent<Image>();
    }

    void Update()
    {
        if(gameOver)
        {
            Color tempColor = gameOverImage.color;
            tempColor.a += 1f * Time.deltaTime;
            gameOverImage.color = tempColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!isPlaying)
        {
            if(collider.CompareTag("Player"))
            {
                StartCoroutine(StartSequence());
            }
        }
    }

    IEnumerator StartSequence()
    {
        isPlaying = true;
        yield return new WaitForSeconds(5);
        audioManager.ChangeVolume("Background Music", 0.01f);
        yield return new WaitForSeconds(1);
        audioManager.Play("StartDestructAlarm");
        yield return new WaitForSeconds(2);
        audioManager.Play("StartDestructSequence");
        yield return new WaitForSeconds(10);
        audioManager.Cancel("Background Music");
        countdownTimer.ShowTimer();
        countdownTimer.isCounting = true;
        audioManager.Play("FastPacedTheme");
        yield return new WaitForSeconds(55);
        audioManager.ChangeVolume("FastPacedTheme", 0.01f);
        audioManager.Play("EndCountdown");
        yield return new WaitForSeconds(5);
        audioManager.Cancel("FastPacedTheme");

        gameOver = true;
    }
}
