using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroController : MonoBehaviour
{
    public string link;

    void Start()
    {
        StartCoroutine(EndGame());   
    }

    IEnumerator EndGame()
    {  
        yield return new WaitForSeconds(13);
        Application.OpenURL(link);
        Application.Quit();
    }
}
