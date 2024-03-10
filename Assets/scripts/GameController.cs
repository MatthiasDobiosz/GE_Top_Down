using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TMP_Text collectedText;
    private int collectedCount = 0;
    public int totalObjects = 4;

    public Teleporter teleporter;

    private void Start() {
        UpdateCollectedText();
    }

    public void CollectObject()
    {
        collectedCount++;
        UpdateCollectedText();

        if (collectedCount >= totalObjects)
        {
            teleporter.allKeyFragments = true;
        }
    }

    void UpdateCollectedText()
    {
        collectedText.text = collectedCount + "/" + totalObjects + " key fragments";
    }
}
