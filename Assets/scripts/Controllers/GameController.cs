using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TMP_Text collectedText;
    public int collectedCount = 0;
    public int totalObjects = 4;

    private Teleporter teleporter;
    private TeleporterMaster teleporterMaster;



    private void Start() {
        teleporter = FindObjectOfType<Teleporter>();
        teleporterMaster = FindObjectOfType<TeleporterMaster>();
        UpdateCollectedText();
    }

    public void CollectObject()
    {
        collectedCount++;
        Debug.Log(collectedCount);
        UpdateCollectedText();
        FindObjectOfType<AudioManager>().Play("CollectKey");

        if (collectedCount >= totalObjects && !teleporter.allKeyFragments)
        {
            Debug.Log("alle");
            teleporter.AllKeyFragmentsCollected();
            Debug.Log(teleporter.allKeyFragments);
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
}
