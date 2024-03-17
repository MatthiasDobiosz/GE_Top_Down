using System;
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
    public Vector2 lastMovementInput;

    public GameController gameController;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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
            TryMove(movementInput); 
            lastMovementInput = movementInput;
            UpdateAnimator(false);
        } else{
            UpdateAnimator(true);
        }

    }

    private void UpdateAnimator(bool idle)
    {
        if (animator != null)
        {
            animator.SetBool("Idle", idle);
            animator.SetFloat("MoveX", lastMovementInput.x);
            animator.SetFloat("MoveY", lastMovementInput.y);
        }
    }

    public Vector2 GetMovementInput()
    {
        return movementInput;
    }

    public Vector2 GetLastMovementInput()
    {
        return lastMovementInput;
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
        } else if (movementInput.magnitude < 1f) 
        {
            movementInput = Vector2.zero;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            gameController.CollectObject();

            other.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 pushbackDirection = (transform.position - collision.transform.position).normalized;

            rb.AddForce(pushbackDirection * 0.5f, ForceMode2D.Impulse);

            if(collision.gameObject.TryGetComponent<Rigidbody2D>(out var enemyRb))
            {
                enemyRb.AddForce(-pushbackDirection * 0.5f, ForceMode2D.Impulse);
            }
        }
    }


}
