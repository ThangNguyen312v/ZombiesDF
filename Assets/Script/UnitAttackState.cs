using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;

    public float stopAttackingDistance = 1.2f;
    private float attackTime;
    private float attackRate = 2f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        attackController.SetAttackMaterial();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            LookAtTarget();

            agent.SetDestination(attackController.targetToAttack.position);
            if (attackTime <= 0)
            {
                //Attack();
                attackTime = 1f / attackRate;
            }
            else
            {
                attackTime -= Time.deltaTime;
            }

            float distanceFormTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
            if (distanceFormTarget > stopAttackingDistance || attackController.targetToAttack == null)
            {
                animator.SetBool("isAttack", false);
            }else
            {
                animator.SetBool("isAttack", false);
            }
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    //public void Attack()
    //{
    //    var damageToInflict = attackController.unitDamage;

    //    attackController.targetToAttack.GetComponent<Unit>().TakeDmage(damageToInflict);
    //}
}
