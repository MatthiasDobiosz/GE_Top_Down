using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
    Fake teleporter that does not actually work but shows text
*/
public class FakeTeleporter : MonoBehaviour
{
    public TMP_Text teleporterError;

    private void OnTriggerEnter2D(Collider2D other)
    {
        teleporterError.text = "You dont have enough Keyfragments to use the teleporter!";
        teleporterError.gameObject.SetActive(true); 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        teleporterError.gameObject.SetActive(false); 
    }
}
