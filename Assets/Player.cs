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
    private float dashingPower = 24f;
    private float dashingTime = 0.1f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            bool success = TryMove(new Vector2(movementInput.x, 0));

            if(!success) {
                success = TryMove(new Vector2(0, movementInput.y)); 
            }
        }

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

    private bool TryMove(Vector2 direction) {
        if(direction != Vector2.zero) {
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if(count == 0){
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
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
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash(){
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(transform.GetChild(0).localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
