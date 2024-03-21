using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseMelee : EnemyChase
{

    protected override void Update()
    {
        if (path == null || !currentlyChasing || currentlyAttacking)
        {
            if(!isPatroling)
            {
                anim.SetBool("Moving", false);
            }
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
        /**else 
        {
            reachedEndOfPath = false;
        }*/
        

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
}
