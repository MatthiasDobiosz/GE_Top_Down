using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        EventManager.StartListening("switchActivated", OpenDoor);
    }

    void OpenDoor(Dictionary<string, object> message)
    {
        animator.SetBool("Open", true);
    }
}
