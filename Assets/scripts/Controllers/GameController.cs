using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TMP_Text collectedText;
    public int collectedCount = 0;
    public int totalObjects = 4;
    public GameObject[] teleporters;

    private TeleporterMaster teleporterMaster;
    private Teleporter currentTeleporter;

    private void Start() {
        currentTeleporter = teleporters[0].GetComponent<Teleporter>();
        teleporterMaster = FindObjectOfType<TeleporterMaster>();
        UpdateCollectedText();

        EventManager.StartListening("teleport", UpdateCurrentTeleporter);
    }

    public void CollectObject()
    {
        collectedCount++;
        UpdateCollectedText();
        FindObjectOfType<AudioManager>().Play("CollectKey");

        if (collectedCount >= totalObjects && !currentTeleporter.allKeyFragments)
        {
            currentTeleporter.AllKeyFragmentsCollected();
        }
    }

    public void CollectMasterObject()
    {
        teleporterMaster.hasMasterKey = true;
        FindObjectOfType<AudioManager>().Play("CollectKey");
    }

    public void UpdateCollectedText()
    {
        collectedText.text = collectedCount + "/" + totalObjects + " key fragments";
    }

    void UpdateCurrentTeleporter(Dictionary<string, object> message)
    {
        currentTeleporter = teleporters[(int)message["layer"]].GetComponent<Teleporter>();
    }
}
