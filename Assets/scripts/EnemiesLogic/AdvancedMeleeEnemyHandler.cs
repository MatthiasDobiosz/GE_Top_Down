using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Handle advanced normal melee enemy (Initial attack + standard attack)
*/
public class AdvancedMeleeEnemyHandler : MonoBehaviour
{
    public Transform target;
    public float minDistanceInitialAttack = 0.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isAttacking = false;
    private MeleeStandardAttack meleeStandardAttack;
    private MeleeTeleportAttack meleeTeleportAttack;
    private bool hasDoneInitialAttack = false;
    private bool hasLineOfSight = false;
    private MovePointAroundEntity movePointAroundEntityHandler;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movePointAroundEntityHandler = GetComponent<MovePointAroundEntity>();
        meleeStandardAttack = GetComponent<MeleeStandardAttack>();
        meleeTeleportAttack = GetComponent<MeleeTeleportAttack>();

        EventManager.StartListening("patrolStart", ResetInitialAttack);

        EventManager.StartListening("death", CheckForDeath);
    }

    void Update()
    {
        if(isAttacking)
        {
            // Check if attack animation is over
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                AttackFinished();
            }
        }


        if(!hasDoneInitialAttack){
            if(Vector2.Distance(rb.position, target.position) < minDistanceInitialAttack && !isAttacking && hasLineOfSight)
            {
                Vector2 direction = meleeTeleportAttack.TriggerAttackStart(rb, target);
                anim.SetFloat("XInput", direction.x);
                anim.SetFloat("YInput", direction.y);

                Vector2 facingDirection = GetCurrentFacingDirection();
                movePointAroundEntityHandler.MovePoint(facingDirection.x, facingDirection.y, 0);

                anim.SetTrigger("Attack");
                isAttacking = true;
            }
        }
        else {  
            if(!isAttacking && hasLineOfSight)
            { 
                Vector2 facingDirection = GetCurrentFacingDirection();
                movePointAroundEntityHandler.MovePoint(facingDirection.x, facingDirection.y, 1);
                bool shouldAttack = meleeStandardAttack.TriggerAttackStart(rb, target);

                if(shouldAttack)
                {
                    anim.SetTrigger("Attack");
                    isAttacking = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        CheckIfPlayerIsInLineOfSight();
    }

    public void AttackFinished()
    {
        isAttacking = false;
        if(!hasDoneInitialAttack)
        {
            meleeTeleportAttack.TriggerAttackEnd(rb);
            hasDoneInitialAttack = true;
        } else {
            meleeStandardAttack.TriggerAttackEnd(rb);
        }
    }

    void ResetInitialAttack(Dictionary<string, object> message)
    {
        if((Rigidbody2D)message["body"] == rb)
        {
            hasDoneInitialAttack = false;
        }
    }

    void CheckIfPlayerIsInLineOfSight()
    {
        RaycastHit2D ray = Physics2D.Linecast(transform.position, target.transform.position, 1 << LayerMask.NameToLayer("Obstacles"));

        if(ray.collider != null)
        {
            hasLineOfSight = false;
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
        } else 
        {
            hasLineOfSight = true;
            Debug.DrawLine(transform.position, target.transform.position, Color.green);
        }

    }

    void CheckForDeath(Dictionary<string, object> message)
    {
        if((GameObject)message["gameobject"] == transform.gameObject)
        {
            StartCoroutine(Die(1));
        }
    }

    Vector2 GetCurrentFacingDirection()
    {
        return new Vector2(anim.GetFloat("XInput"), anim.GetFloat("YInput"));
    }

    IEnumerator Die(int secs)
    {
        transform.GetComponent<EnemyAIChase>().enabled = false;
        transform.GetComponent<EnemyAIPatrol>().enabled = false;

        Destroy(transform.gameObject, secs);
        yield return new WaitForSeconds(secs);

        transform.GetComponent<EnemyAIChase>().OnDestroy();
        transform.GetComponent<EnemyAIPatrol>().OnDestroy();
        
        OnDestroy();
    }

    public void OnDestroy()
    {
        EventManager.StopListening("patrolStart", ResetInitialAttack);
        EventManager.StopListening("death", CheckForDeath);
    }
}
