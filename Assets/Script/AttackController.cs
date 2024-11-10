using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;

    public int unitDamage;

    public bool isPlayer;

    public Material idelState;
    public Material followState;
    public Material attackState;


    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (isPlayer && other.CompareTag("Zombie") && targetToAttack == null)
        {
            targetToAttack = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isPlayer && other.CompareTag("Zombie") && targetToAttack == null)
        {
            targetToAttack = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlayer && other.CompareTag("Zombie") && targetToAttack != null)
        {
            targetToAttack = null;
        }
    }

    public void SetIdleMaterial()
    {
        GetComponent<Renderer>().material = idelState;
    }
    public void SetFollowMaterial()
    {
        GetComponent<Renderer>().material = followState;
    }
    public void SetAttackMaterial()
    {
        GetComponent<Renderer>().material = attackState;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,10f*0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,1.2f);
    }
}
