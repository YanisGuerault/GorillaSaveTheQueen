using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    NavMeshAgent nav;
    Animator anim;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("JE TE VOIS !");
        if (other.CompareTag("Player"))
        {
            nav.SetDestination(player.position);
            anim.SetBool("SeePlayer", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("SeePlayer", false);
        }
    }
}
