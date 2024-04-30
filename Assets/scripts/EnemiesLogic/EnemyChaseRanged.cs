using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Extended chase logic for ranged enemies
*/
public class EnemyChaseRanged : EnemyChase
{
    public float thresholdDistance = 1f;

    private bool isInThresholdDistance = false;

    protected override void Start()
    {
        base.Start();

        GetRangedPosition();
    }


    protected override void Update()
    {
        GetRangedPosition();

        base.Update();

        if(currentlyChasing)
        {
            if(currentlyAttacking || isInThresholdDistance)
            {
                anim.SetBool("Moving", false);
            }
            else 
            {
                anim.SetBool("Moving", true);
            }
        } else 
        {
            if(!isPatroling)
            {
                anim.SetBool("Moving", false);
            }
            else 
            {
                anim.SetBool("Moving", true);
            }
        }

        if (path == null || !currentlyChasing || currentlyAttacking || isInThresholdDistance)
        {
            return;
        }
            

        if (Vector2.Distance(rb.position, target.position) > chaseEndDistance)
        {
            EndChase();
        }

        
        if(currentWaypoint >= path.vectorPath.Count)
        {
            //reachedEndOfPath = true;
            return;
        } 

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 velocity = direction * speed;

        rb.position += velocity * Time.deltaTime;
        // rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(Math.Abs(path.vectorPath[currentWaypoint].x - rb.position.x) > 0.1f)
        {
            if(Math.Abs(velocity.y) < 0.1f)
            {   
                anim.SetFloat("YInput", 0);
            }
            anim.SetFloat("XInput", velocity.x);
        }
        if(Math.Abs(path.vectorPath[currentWaypoint].y - rb.position.y) > 0.1f)
        {
            if(Math.Abs(velocity.x) < 0.1f)
            {   
                anim.SetFloat("XInput", 0);
            }
            anim.SetFloat("YInput", velocity.y);
        }

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    
    void GetRangedPosition()
    {
        float currentDistance = Vector2.Distance(target.position, rb.position);
        bool isCurrentlyInRangeDistance = true;

        // if the enemy is not at the threshold distance or has no LOS than it should chase the player target point
        if(currentDistance > thresholdDistance  || !hasLineOfSight && isPlayerDead)
        {
            isCurrentlyInRangeDistance = false;
        }

        if(isInThresholdDistance != isCurrentlyInRangeDistance)
        {
            if(isCurrentlyInRangeDistance)
            {
                isInThresholdDistance = true;
                EventManager.TriggerEvent("canAttackRanged", new Dictionary<string, object> {
                    {"body", rb}
                 });
            }
            else 
            {
                isInThresholdDistance = false;
                EventManager.TriggerEvent("canNotAttackRanged", new Dictionary<string, object> {
                    {"body", rb}
                });
            }
        }
    }

}
