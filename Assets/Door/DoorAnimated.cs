using System;
using UnityEngine;

public class DoorAnimated : MonoBehaviour
{
    private Animator animator;
    public float interactionDistance = 2f;
    private GameObject player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (IsPlayerNearDoor() && Input.GetKeyDown(KeyCode.E))
        {
            OpenDoor();
        }
        else if (!IsPlayerNearDoor())
        {
            CloseDoor();
        }
    }

    bool IsPlayerNearDoor()
    {   
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            return distance <= interactionDistance;
        }
        return false;
    }

    public void OpenDoor()
    {
        animator.SetBool("Open", true);
    }

    public void CloseDoor()
    {
        animator.SetBool("Open", false);
    }
}
