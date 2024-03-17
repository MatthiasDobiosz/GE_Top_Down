using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Basic enemy handler: Only exceute one standard melee attack
*/
public class BasicMeleeEnemyHandler : MonoBehaviour
{
    public Transform target;
    public float minDistanceAttack = 0.3f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isAttacking = false;
    private MeleeStandardAttack meleeStandardAttack;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        meleeStandardAttack = GetComponent<MeleeStandardAttack>();

        EventManager.StartListening("death", CheckForDeath);
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

 
        if(!isAttacking)
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

    void CheckForDeath(Dictionary<string, object> message)
    {
        if((GameObject)message["gameobject"] == transform.gameObject)
        {
            StartCoroutine(Die(1));
        }
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
        EventManager.StopListening("death", CheckForDeath);
    }
}
