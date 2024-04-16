using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class IntroController : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public RenderTexture renderTexture;
    public Texture2D Tutorial;
    public string nextScene;
    private int stage = 1;
    private float timeout = 0.0f;
    //public Texture2D videoTexture;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        //key timeout
        if(timeout > 0){
            timeout-=  Time.deltaTime;
        }
    
        //key input
        if(Input.GetKeyDown(KeyCode.Space) && timeout <= 0)
        {
            //targetObject.SetActive(false);
           buttonInput();
            
        }

        //videoPlay end
        if(stage == 2){
            if (videoPlayer.frame >= (long)videoPlayer.frameCount-1)//videoPlayer.isPrepared && !videoPlayer.isPlaying)
            {
                rawImage.texture = Tutorial;
                timeout = 1.0f;
                stage = 3;
                
            }
        }
        
    }

    void buttonInput(){
        switch(stage)
        {
            case 1:
                videoPlayer.Play();
                rawImage.texture = renderTexture;
                stage = 2;
                timeout = 8.0f;
                break;
            case 2:
                rawImage.texture = Tutorial;
                timeout = 0.4f;
                stage = 3;
                break;
            case 3:
                Debug.Log("next scene");
                SceneManager.LoadScene(nextScene);
                break;
            default:
                Debug.Log("next scene");
                SceneManager.LoadScene(nextScene);
                break;
        }
        
    }
}
