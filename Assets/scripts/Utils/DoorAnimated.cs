using UnityEngine;

public class DoorAnimated : MonoBehaviour
{
    private Animator animator;
    public float interactionDistance = 2f;
    private GameObject player;
    private bool isOpen = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
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
    }

    public void Close()
    {
        animator.SetBool("Open", false);
        isOpen = false;
    }
}
