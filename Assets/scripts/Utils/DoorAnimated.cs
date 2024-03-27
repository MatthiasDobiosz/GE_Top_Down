using System.Collections.Generic;
using UnityEngine;

public class DoorAnimated : MonoBehaviour
{
    private Animator animator;
    public float interactionDistance = 2f;
    private GameObject player;
    private bool isOpen = false;
    private Bounds doorBounds;


    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 boundsCenter = new(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        Vector3 boundsSize = new(5, 5, 0);

        doorBounds = new Bounds(boundsCenter, boundsSize);
    }

    void Update()
    {
        if (IsPlayerNearDoor())
        {
            if (!isOpen && Input.GetKeyDown(KeyCode.E))
            {
                Open();
            }
        }
        else
        {
            if (isOpen)
            {
                Close();
            }
        }
    }

    bool IsPlayerNearDoor()
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
        animator.SetBool("Open", true);
        isOpen = true;

        EventManager.TriggerEvent("updateGrid", new Dictionary<string, object> {
            {"bounds", doorBounds}
        });
    }

    public void Close()
    {
        animator.SetBool("Open", false);
        isOpen = false;

        EventManager.TriggerEvent("updateGrid", new Dictionary<string, object> {
            {"bounds", doorBounds}
        });
    }
}
