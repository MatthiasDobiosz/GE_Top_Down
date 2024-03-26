using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSwitch : MonoBehaviour
{
    private Animator animator;
    public float interactionDistance = 0.5f;
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
        // animator.SetBool("Open", true);
        EventManager.TriggerEvent("switchActivated", null);
        isOn = true;
    }
}
