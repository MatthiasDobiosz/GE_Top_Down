using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Basic ranged enemy handler: only executes one standard attack
*/
public class BasicRangedEnemyHandler : Enemy
{
    public float minDistanceAttack = 0.3f;
    public float attackInterval = 3f;
    public float immobileTime = 1.5f;
    
    private Animator anim;
    private bool isAttacking = false;
    private RangedStandardAttack rangedStandardAttack;
    private bool isInAttackPosition = false;
    private float attackTimer;
    private MovePointAroundEntity movePointAroundEntityHandler;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        rangedStandardAttack = GetComponent<RangedStandardAttack>();
        attackTimer = attackInterval;
        movePointAroundEntityHandler = GetComponent<MovePointAroundEntity>();

        EventManager.StartListening("canAttackRanged", StartAttacking);
        EventManager.StartListening("canNotAttackRanged", StopAttacking);
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

 
        if(!isAttacking && hasLineOfSight && isInAttackPosition && !isPlayerDead)
        { 
            Vector2 direction = rangedStandardAttack.getPlayerDirection(rb, target);

            movePointAroundEntityHandler.MovePoint(direction.x, direction.y, 0);
            anim.SetFloat("XInput", direction.x);
            anim.SetFloat("YInput", direction.y);

            rangedStandardAttack.TriggerAttackStart(rb, direction);
            
            isAttacking = true;
        }

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

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.StopListening("canAttackRanged", StartAttacking);
        EventManager.StopListening("canNotAttackRanged", StopAttacking);
    }
}
