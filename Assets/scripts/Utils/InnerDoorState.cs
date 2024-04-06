using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
