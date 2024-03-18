using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Basic enemy handler: Only exceute one standard melee attack
*/
public class BasicMeleeEnemyHandler : Enemy
{
    public float minDistanceAttack = 0.3f;

    private Animator anim;
    private bool isAttacking = false;
    private MeleeStandardAttack meleeStandardAttack;


    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        meleeStandardAttack = GetComponent<MeleeStandardAttack>();
    }

    void Update()
    {
        if(isAttacking)
        {
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                AttackFinished();
            }
        }

 
        if(!isAttacking && hasLineOfSight)
        { 
            bool shouldAttack = meleeStandardAttack.TriggerAttackStart(rb, target);

            if(shouldAttack)
            {
                //Vector2 facingDirection = GetCurrentFacingDirection();
                // movePointAroundEntityHandler.MovePoint(facingDirection.x, facingDirection.y, 0);
                //anim.SetTrigger("Attack");
                isAttacking = true;
            }
        }

    }

    public void AttackFinished()
    {
        isAttacking = false;
        meleeStandardAttack.TriggerAttackEnd(rb);
    }

    /**
    IEnumerator Dieold(int secs)
    {
        //transform.GetComponent<EnemyAIChase>().enabled = false;
        //transform.GetComponent<EnemyAIPatrol>().enabled = false;

        //Destroy(transform.gameObject, secs);

        yield return new WaitForSeconds(secs);
        Color objectColor = spriteRenderer.material.color;
        originalColor = objectColor;
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 0);
        spriteRenderer.material.color = objectColor;
        transform.gameObject.SetActive(false);
        //transform.GetComponent<EnemyAIChase>().OnDestroy();
        //transform.GetComponent<EnemyAIPatrol>().OnDestroy();
        
        //OnDestroy();
    }

    public void OnDestroy()
    {
        EventManager.StopListening("death", CheckForDeath);
    }
    */
}
