using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    private ParticleSystem particleSys;

    public float fillSpeed = 0.2f;
    private float targetProgress = 0;

    private void Awake() {
        slider = gameObject.GetComponent<Slider>();
        particleSys = GameObject.Find("ProgressBarParticles").GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if(slider.value < targetProgress){
            slider.value += fillSpeed * Time.deltaTime;
            if(!particleSys.isPlaying){
                particleSys.Play();
            }
        }
        else{
            particleSys.Stop();
        }
    }

    public void IncrementProgress(float newProgress){
        targetProgress = slider.value + newProgress;
    }
}
