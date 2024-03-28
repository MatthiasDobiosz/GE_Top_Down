using UnityEngine;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour
{
    public GameObject stageCleared1;
    public GameObject stageCleared2;
    public GameObject stageCleared3;
    public GameObject stageCleared4;
    public GameObject firstPlayerDeath;
    public GameObject firstMonsterKill;

    private void Start()
    {
        EventManager.StartListening("ShowPopup1", ShowPopup1);
        EventManager.StartListening("ShowPopup2", ShowPopup2);
        EventManager.StartListening("ShowPopup3", ShowPopup3);
        EventManager.StartListening("ShowPopup4", ShowPopup4);
        EventManager.StartListening("firstPlayerDeath", ShowPopupFirstPlayerDeath);
        EventManager.StartListening("firstMonsterKill", ShowPopupFirstMonsterKill);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("ShowPopup1", ShowPopup1);
        EventManager.StopListening("ShowPopup2", ShowPopup2);
        EventManager.StopListening("ShowPopup3", ShowPopup3);
        EventManager.StopListening("ShowPopup4", ShowPopup4);
        EventManager.StopListening("firstPlayerDeath", ShowPopupFirstPlayerDeath);
        EventManager.StopListening("firstMonsterKill", ShowPopupFirstMonsterKill);
    }

    private void ShowPopup1(Dictionary<string, object> message)
    {
        ShowPopup(stageCleared1);
    }

    private void ShowPopup2(Dictionary<string, object> message)
    {
        ShowPopup(stageCleared2);
    }

    private void ShowPopup3(Dictionary<string, object> message)
    {
        ShowPopup(stageCleared3);
    }

    private void ShowPopup4(Dictionary<string, object> message)
    {
        ShowPopup(stageCleared4);
    }

    private void ShowPopupFirstPlayerDeath(Dictionary<string, object> message)
    {
        ShowPopup(firstPlayerDeath);
    }

    private void ShowPopupFirstMonsterKill(Dictionary<string, object> message)
    {
        ShowPopup(firstMonsterKill);
    }

    private void ShowPopup(GameObject popupObject)
    {
        if (popupObject != null)
        {
            PopupController popupController = popupObject.GetComponent<PopupController>();
            if (popupController != null)
            {
                popupController.PlayPopupAnimation();
            }
        }
        else
        {
            Debug.LogWarning("bruh");
        }
    }
}
