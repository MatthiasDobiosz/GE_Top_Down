using UnityEngine;

/**
    Popup controller for each popup
*/
public class PopupController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayPopupAnimation()
    {
        animator.SetTrigger("ShowPopup");
    }
}