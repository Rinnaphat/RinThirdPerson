using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Spider : MonoBehaviour
{
    public Transform[] targetPoint;
    public int currentPoint;

    public NavMeshAgent agent;
    public Animator animator;

    public float waitAtPoint = 2f;
    private float waitCounter;
    public enum AIState
    {
        isDead, isSeekTargetPoint, isSeekPlyer, isAttack
    }
     public AIState state;

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoint;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

        if (!PlayerController.instance.isDead)
        {
            if (distanceToPlayer >= 4 && distanceToPlayer <= 8f)
            {
                state = AIState.isSeekPlyer;
            }
            else if (distanceToPlayer > 8)
            {
                state = AIState.isSeekTargetPoint;
            }
            else
            {
                state = AIState.isAttack;
            }
        }
        else 
        {
            state = AIState.isSeekTargetPoint;
            animator.SetBool("Attack", false);
            animator.SetBool("Run", true);
        }
        

        switch (state)
        {
            case AIState.isDead:
                break;
            case AIState.isSeekPlyer:
                
                agent.SetDestination(PlayerController.instance.transform.position);
                animator.SetBool("Run", true);
                animator.SetBool("Attack", false);
                break;
            case AIState.isSeekTargetPoint:
                agent.SetDestination(targetPoint[currentPoint].position);
                agent.stoppingDistance = 0;
                animator.SetBool("Run", true);
                if (agent.remainingDistance <= .2f)
                {
                    if (waitCounter > 0)
                    {
                        waitCounter -= Time.deltaTime;
                        animator.SetBool("Run", false);
                    }
                    else
                    {
                        currentPoint++;
                        waitCounter = waitAtPoint;
                        animator.SetBool("Run", true);
                    }


                    if (currentPoint >= targetPoint.Length)
                    {
                        currentPoint = 0;
                    }
                    agent.SetDestination(targetPoint[currentPoint].position);
                }
                break;

            case AIState.isAttack:

                RotateTowardPlayer();
                agent.stoppingDistance = 4;
                animator.SetBool("Attack", true);
                animator.SetBool("Run", false);
                break;
        }      
    }

    void RotateTowardPlayer()
    {
        Vector3 direction = (PlayerController.instance.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
    }
}
