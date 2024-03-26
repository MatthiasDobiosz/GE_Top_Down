using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CloseIntro : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private bool videoEnded = false;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd; 
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoEnded = true;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && videoEnded)
        {
            CloseIntroManually();
        }
    }

    public void CloseIntroManually()
    {
        videoPlayer.Stop();
        gameObject.SetActive(false);
    }
}
