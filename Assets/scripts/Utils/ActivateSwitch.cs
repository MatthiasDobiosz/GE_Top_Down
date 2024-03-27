using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivateSwitch : MonoBehaviour
{
    public float interactionDistance = 0.5f;
    public TMP_Text doorOpenText;

    private Animator animator;
    private GameObject player;
    private bool isOn = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (IsPlayerNear())
        {
            if (!isOn && Input.GetKeyDown(KeyCode.E))
            {
                Open();
            }
        }
    }

    bool IsPlayerNear()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 doorPosition = transform.position;

            float distance = Vector3.Distance(doorPosition, playerPosition);
            return distance <= interactionDistance;
        }
        return false;
    }

    public void Open()
    {
        animator.SetTrigger("switch");
        EventManager.TriggerEvent("switchActivated", null);
        isOn = true;
        StartCoroutine(ShowDoorOpenText());
    }

    IEnumerator ShowDoorOpenText()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<AudioManager>().Play("Switch");
        doorOpenText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        doorOpenText.gameObject.SetActive(false);
    }
}
