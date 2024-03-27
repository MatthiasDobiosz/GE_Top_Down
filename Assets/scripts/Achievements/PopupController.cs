using UnityEngine;

public class PopupController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayPopupAnimation()
    {
        Debug.Log("hier");
        //FindObjectOfType<AudioManager>().Play("Achievement");
        animator.SetTrigger("ShowPopup");
    }
}