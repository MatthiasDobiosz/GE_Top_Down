using System;
using TMPro;
using UnityEngine;

public class DoorAnimated : MonoBehaviour
{
    private Animator animator;
    public float interactionDistance = 2f;
    private GameObject player;
    public TMP_Text lockedText;
    private bool isOpen = false;
    private bool isPlayerNear = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        isPlayerNear = IsPlayerNearDoor();

        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            OpenDoor();
        }
        else if (!isPlayerNear)
        {
            CloseDoor();
        }

        if (isPlayerNear && !isOpen)
        {
            lockedText.gameObject.SetActive(true);
        }
        else
        {
            lockedText.gameObject.SetActive(false);
        }
    }

    bool IsPlayerNearDoor()
    {
    if (player != null)
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 doorPosition = transform.position;

        playerPosition.z = doorPosition.z;
        
        float distance = Vector3.Distance(doorPosition, playerPosition);

        return distance <= interactionDistance;
    }
    return false;
}

    public void OpenDoor()
    {
        FindObjectOfType<AudioManager>().Play("OpenDoor");
        animator.SetBool("Open", true);
        isOpen = true;
    }

    public void CloseDoor()
    {
        animator.SetBool("Open", false);
        isOpen = false;
    }
}
