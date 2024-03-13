using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Basic ranged enemy handler: only executes one standard attack
*/
public class BasicRangedEnemyHandler : MonoBehaviour
{
    public Transform target;
    public float minDistanceAttack = 0.3f;
    public float attackInterval = 3f;
    public float immobileTime = 1.5f;

    private Rigidbody2D rb;
    
    private Animator anim;
    private bool isAttacking = false;
    private RangedStandardAttack rangedStandardAttack;
    private bool hasLineOfSight = false;
    private bool isInAttackPosition = false;
    private float attackTimer;
    private bool inDeathAnimation = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rangedStandardAttack = GetComponent<RangedStandardAttack>();
        attackTimer = attackInterval;

        EventManager.StartListening("canAttackRanged", StartAttacking);
        EventManager.StartListening("canNotAttackRanged", StopAttacking);
        EventManager.StartListening("death", CheckForDeath);
    }

    void Update()
    {
        if(isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer <= attackInterval - immobileTime)
            {
                AttackFinished();
            }
            if(attackTimer <= 0.0f)
            {
                RestartAttack();
            }
        }

 
        if(!isAttacking && hasLineOfSight && isInAttackPosition)
        { 
            rangedStandardAttack.TriggerAttackStart(rb, target);
            isAttacking = true;
        }

    }

    void FixedUpdate()
    {
        CheckIfPlayerIsInLineOfSight();
    }

    public void AttackFinished()
    {
        rangedStandardAttack.TriggerAttackEnd(rb);
    }

    public void RestartAttack()
    {
        isAttacking = false;
        attackTimer = attackInterval;
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

    void StopAttacking(Dictionary<string, object> message)
    {
        if((Rigidbody2D)message["body"] == rb)
        {
            isInAttackPosition = false;
        }
    }

    void StartAttacking(Dictionary<string, object> message)
    {
        if((Rigidbody2D)message["body"] == rb)
        {
            isInAttackPosition = true;
        }
    }

    void CheckForDeath(Dictionary<string, object> message)
    {
        if((GameObject)message["gameobject"] == transform.gameObject && !inDeathAnimation)
        {
            anim.SetTrigger("Death");
            inDeathAnimation = true;
            StartCoroutine(Die(1));
        }
    }

    IEnumerator Die(int secs)
    {
        transform.GetComponent<EnemyAIChaseRanged>().enabled = false;
        transform.GetComponent<EnemyAIPatrol>().enabled = false;
        
        Destroy(transform.gameObject, secs);
        yield return new WaitForSeconds(secs);

        transform.GetComponent<EnemyAIChaseRanged>().OnDestroy();
        transform.GetComponent<EnemyAIPatrol>().OnDestroy();

        OnDestroy();
    }
    public void OnDestroy()
    {
        EventManager.StopListening("canAttackRanged", StartAttacking);
        EventManager.StopListening("canNotAttackRanged", StopAttacking);
        EventManager.StopListening("death", CheckForDeath);
    }
}
