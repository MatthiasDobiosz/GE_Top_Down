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
    public GameObject[] enemies;
    public GameObject controlsImage;

    private TeleporterMaster teleporterMaster;
    private TeleporterToFinal teleporterToFinal;
    private bool isOnFinalTeleporter = false;
    private Teleporter currentTeleporter;

    private void Start() {
        currentTeleporter = teleporters[0].GetComponent<Teleporter>();
        teleporterToFinal = FindObjectOfType<TeleporterToFinal>();
        teleporterMaster = FindObjectOfType<TeleporterMaster>();
        UpdateCollectedText();

        EventManager.StartListening("teleport", UpdateCurrentTeleporter);
        EventManager.StartListening("teleportLast", UpdateToLastTeleporter);
        EventManager.StartListening("teleportFinal", UpdateToNoTeleporter);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!controlsImage.activeSelf)
            {
                controlsImage.SetActive(true);
                Time.timeScale = 0;
            } else 
            {
                controlsImage.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void CollectObject()
    {
        collectedCount++;
        UpdateCollectedText();
        FindObjectOfType<AudioManager>().Play("CollectKey");

        if(isOnFinalTeleporter)
        {
            if (collectedCount >= totalObjects && !teleporterToFinal.allKeyFragments)
            {
                teleporterToFinal.AllKeyFragmentsCollected();
            }
        }
        else
        {
            if (collectedCount >= totalObjects && !currentTeleporter.allKeyFragments)
            {
                currentTeleporter.AllKeyFragmentsCollected();
            }
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
        enemies[(int)message["layer"]].SetActive(true);
        LogManager.Instance.AddStageReached();
        Destroy(enemies[(int)(message)["layer"]-1]);
        currentTeleporter = teleporters[(int)message["layer"]].GetComponent<Teleporter>();   
    }

    void UpdateToLastTeleporter(Dictionary<string, object> message)
    {
        isOnFinalTeleporter = true;
        LogManager.Instance.AddStageReached();

        enemies[3].SetActive(true);
        Destroy(enemies[2]);

        if (collectedCount >= totalObjects && !teleporterToFinal.allKeyFragments)
        {
            teleporterToFinal.AllKeyFragmentsCollected();
        }
    }

    void UpdateToNoTeleporter(Dictionary<string, object> message)
    {
        LogManager.Instance.AddStageReached();
        EventManager.TriggerEvent("teleportToStage", new Dictionary<string, object>{
            {"stage", 5},
        });

        enemies[4].SetActive(true);
        Destroy(enemies[3]);

        foreach(GameObject teleporterObject in teleporters)
        {
            teleporterObject.SetActive(false);
        }
    }
}
