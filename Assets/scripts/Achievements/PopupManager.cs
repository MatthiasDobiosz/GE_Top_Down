using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/**
    Manages all the achievement popups
*/
public class PopupManager : MonoBehaviour
{
    public GameObject stageCleared1;
    public GameObject stageCleared2;
    public GameObject stageCleared3;
    public GameObject stageCleared4;
    public GameObject firstPlayerDeath;
    public GameObject firstMonsterKill;

    public GameObject killFive;
    public GameObject killTen;
    public GameObject killFiften;
    public GameObject killTwenty;
    public GameObject killThirty;
    public GameObject killFourty;
    public GameObject killFifty;
    public GameObject deathFive;
    public GameObject deathTen;
    public GameObject deathFiften;
    public GameObject gameStart;
    public GameObject TeleportReady;

    private void Start()
    {

        EventManager.StartListening("ShowPopup1", ShowPopup1);
        EventManager.StartListening("ShowPopup2", ShowPopup2);
        EventManager.StartListening("ShowPopup3", ShowPopup3);
        EventManager.StartListening("ShowPopup4", ShowPopup4);
        EventManager.StartListening("firstPlayerDeath", ShowPopupFirstPlayerDeath);
        EventManager.StartListening("firstMonsterKill", ShowPopupFirstMonsterKill);

        EventManager.StartListening("death5", ShowPopupDeath5);
        EventManager.StartListening("death10", ShowPopupDeath10);
        EventManager.StartListening("death15", ShowPopupDeath15);

        EventManager.StartListening("kill5", ShowPopupKill5);
        EventManager.StartListening("kill10", ShowPopupKill10);
        EventManager.StartListening("kill15", ShowPopupKill15);
        EventManager.StartListening("kill20", ShowPopupKill20);
        EventManager.StartListening("kill30", ShowPopupKill30);
        EventManager.StartListening("kill40", ShowPopupKill40);
        EventManager.StartListening("kill50", ShowPopupKill50);

        EventManager.StartListening("TeleportReady", ShowPopupTeleportReady);

        StartCoroutine(ShowPopupGameStart());
    }

    private void OnDestroy()
    {
        EventManager.StopListening("ShowPopup1", ShowPopup1);
        EventManager.StopListening("ShowPopup2", ShowPopup2);
        EventManager.StopListening("ShowPopup3", ShowPopup3);
        EventManager.StopListening("ShowPopup4", ShowPopup4);
        EventManager.StopListening("firstPlayerDeath", ShowPopupFirstPlayerDeath);
        EventManager.StopListening("firstMonsterKill", ShowPopupFirstMonsterKill);

        EventManager.StopListening("death5", ShowPopupDeath5);
        EventManager.StopListening("death10", ShowPopupDeath10);
        EventManager.StopListening("death15", ShowPopupDeath15);

        EventManager.StopListening("kill5", ShowPopupKill5);
        EventManager.StopListening("kill10", ShowPopupKill10);
        EventManager.StopListening("kill15", ShowPopupKill15);
        EventManager.StopListening("kill20", ShowPopupKill20);
        EventManager.StopListening("kill30", ShowPopupKill30);
        EventManager.StopListening("kill40", ShowPopupKill40);
        EventManager.StopListening("kill50", ShowPopupKill50);

        EventManager.StopListening("TeleportReady", ShowPopupTeleportReady);
 
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

   private void ShowPopupDeath5(Dictionary<string, object> message)
    {
        ShowPopup(deathFive);
    }
   private void ShowPopupDeath10(Dictionary<string, object> message)
    {
        ShowPopup(deathTen);
    }
   private void ShowPopupDeath15(Dictionary<string, object> message)
    {
        ShowPopup(deathFiften);
    }

   private void ShowPopupKill5(Dictionary<string, object> message)
    {
        ShowPopup(killFive);
    }
   private void ShowPopupKill10(Dictionary<string, object> message)
    {
        ShowPopup(killTen);
    }
   private void ShowPopupKill15(Dictionary<string, object> message)
    {
        ShowPopup(killFiften);
    }
    private void ShowPopupKill20(Dictionary<string, object> message)
    {
        ShowPopup(killTwenty);
    }
    private void ShowPopupKill30(Dictionary<string, object> message)
    {
        ShowPopup(killThirty);
    }
    private void ShowPopupKill40(Dictionary<string, object> message)
    {
        ShowPopup(killFourty);
    }
    private void ShowPopupKill50(Dictionary<string, object> message)
    {
        ShowPopup(killFifty);
    }
   private IEnumerator ShowPopupGameStart()
    {
        yield return new WaitForSeconds(0.5f);
        ShowPopup(gameStart);
    }
   private void ShowPopupTeleportReady(Dictionary<string, object> message)
    {
        ShowPopup(TeleportReady);
    }

    private void ShowPopup(GameObject popupObject)
    {
        if (popupObject != null)
        {
            PopupController popupController = popupObject.GetComponent<PopupController>();
            if (popupController != null)
            {
                popupController.PlayPopupAnimation();
                FindObjectOfType<AudioManager>().Play("Achievement");
            }
        }
        else
        {
            Debug.LogWarning("no valid popup object");
        }
    }
}
