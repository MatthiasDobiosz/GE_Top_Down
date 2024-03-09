using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{   
    public float moveSpeed = 2f;
    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;
    public Rigidbody2D rb;

    Vector2 movementInput;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public SpriteRenderer spriteRenderer;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 5f;
    private float dashingTime = 0.1f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;
    private Animator animator;

    public Transform bulletPoint;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Eingabemethode für die Taste "V" hinzufügen
        InputAction dashAction = new InputAction(binding: "<Keyboard>/v", interactions: "press");
        dashAction.performed += context => StartDash();
        dashAction.Enable();
    }

    void FixedUpdate()
    {

        if (isDashing)
        {
            return;
        }

        if(movementInput != Vector2.zero){
            TryMove(movementInput); // Direkte Übertragung der Eingabe
        }

        // Richtung der Animation ändern
        //UpdateSpriteDirection();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetFloat("MoveX", movementInput.x);
            animator.SetFloat("MoveY", movementInput.y);
        }
    }

    public Vector2 GetMovementInput()
    {
        return movementInput;
    }

    private void UpdateSpriteDirection()
    {
        Vector3 currentScale = transform.GetChild(0).localScale;
        if (movementInput.x < 0)
        {
            transform.GetChild(0).localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        else if (movementInput.x > 0)
        {
            transform.GetChild(0).localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
    }

    private void TryMove(Vector2 direction) {
        if(direction != Vector2.zero) {
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if(count == 0){
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
        }   
    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
        if (movementInput.magnitude > 1f)
        {
            movementInput.Normalize();
        }
    }

    void StartDash()
    {
        if (canDash)
        {
            StartCoroutine(Dash(movementInput.normalized));
        }
    }

    private IEnumerator Dash(Vector2 direction)
    {
        canDash = false;
        isDashing = true;
        rb.velocity = direction * dashingPower;
        tr.emitting = true;
        
        float dashDistance = 0f;
        while (dashDistance < dashingPower)
        {
            int count = rb.Cast(direction, movementFilter, castCollisions, 0.3f);
            if (count > 0)
            {
                foreach (var hit in castCollisions)
                {
                    if (hit.collider.CompareTag("Obstacle"))
                    {
                        Debug.Log("uwbfuwbf");
                        rb.velocity = Vector2.zero;
                        isDashing = false;
                        tr.emitting = false;
                        canDash = true;
                        yield break;
                    }
                }
            }

            dashDistance += (dashingPower / dashingTime) * Time.deltaTime;
            yield return null;
        }
        
        tr.emitting = false;
        isDashing = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


}
