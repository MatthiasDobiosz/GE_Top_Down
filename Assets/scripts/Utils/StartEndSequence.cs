using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEndSequence : MonoBehaviour
{
    private AudioManager audioManager;
    private CountdownTimer countdownTimer;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        countdownTimer = GetComponent<CountdownTimer>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            StartCoroutine(StartSequence());
        }
    }

    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(5);
        audioManager.Cancel("Background Music");
        yield return new WaitForSeconds(1);
        audioManager.Play("StartDestructAlarm");
        yield return new WaitForSeconds(2);
        audioManager.Play("StartDestructSequence");
        yield return new WaitForSeconds(12);
        countdownTimer.ShowTimer();
        countdownTimer.isCounting = true;
        audioManager.Play("FastPacedTheme");
    }
}
