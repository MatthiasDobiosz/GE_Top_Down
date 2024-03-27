using UnityEngine;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour
{
    public GameObject popupObject1;
    public GameObject popupObject2;

    private void Start()
    {
        EventManager.StartListening("ShowPopup1", ShowPopup1);
        EventManager.StartListening("ShowPopup2", ShowPopup2);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("ShowPopup1", ShowPopup1);
        EventManager.StopListening("ShowPopup2", ShowPopup2);
    }

    private void ShowPopup1(Dictionary<string, object> message)
    {
        ShowPopup(popupObject1);
    }

    private void ShowPopup2(Dictionary<string, object> message)
    {
        ShowPopup(popupObject2);
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
