using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    handles the state of the inner door tiles for collision
*/
public class InnerDoorState : MonoBehaviour
{
    private bool isOpen = false;

    public void SetInnerOpen()
    {
        isOpen = true;
    }

    public void SetInnerClosed()
    {
        isOpen = false;
    }

    public bool IsInnerOpen()
    {
        return isOpen;
    }
}
