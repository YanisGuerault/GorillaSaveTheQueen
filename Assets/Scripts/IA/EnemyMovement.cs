using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform m_player;
    private NavMeshAgent m_nav;
    private Animator m_anim;
    private bool m_hit = false;

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_nav = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
    }

    #region Follow player if see him
    private void Update()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        /*Debug.Log("Distance : "+Vector3.Distance(this.transform.position, player.position));
        Debug.Log("Angle : " + Vector3.SignedAngle(player.position - transform.position, transform.forward,Vector3.up));*/
        if (Vector3.SignedAngle(m_player.position - transform.position, transform.forward, Vector3.up) < 70 
            && Vector3.SignedAngle(m_player.position - transform.position, transform.forward, Vector3.up) > -70 
            && Vector3.Distance(this.transform.position, m_player.position) < 15)
        {
            if (!m_anim.GetCurrentAnimatorStateInfo(0).IsName("zombie_attack"))
            {
                m_nav.SetDestination(m_player.position);
            }

            m_anim.SetBool("SeePlayer", true);
        }
        else
        {
            m_anim.SetBool("SeePlayer", false);
        }
    }

    #endregion

    #region Attack & Hit Player

    private void OnCollisionStay(Collision collision)
    {
        /*Debug.Log("Player : " + collision.gameObject.CompareTag("Player") + " hit : " + hit + " Anim name : " + anim.GetCurrentAnimatorStateInfo(0).IsName("zombie_attack") + "time : " + GetCurrentAnimatorTime(anim));*/
        if (collision.gameObject.CompareTag("Player") && !m_hit && m_anim.GetCurrentAnimatorStateInfo(0).IsName("zombie_attack") && GetCurrentAnimatorTime(m_anim) > 0.3)
        {
            EventManager.Instance.Raise(new PlayerHasBeenHitEvent());
            m_hit = true;
            StartCoroutine(CoroutineHitPlayer(2));
        }
    }

    private IEnumerator CoroutineHitPlayer(float x)
    {
        yield return new WaitForSeconds(x);
        m_hit = false;     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_anim.SetTrigger("AttackPlayer");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_anim.SetTrigger("AttackPlayer");
        }
    }

    #endregion

    #region Others Functions

    public float GetCurrentAnimatorTime(Animator targetAnim, int layer = 0)
    {
        AnimatorStateInfo animState = targetAnim.GetCurrentAnimatorStateInfo(layer);
        float currentTime = animState.normalizedTime % 1;
        return currentTime;
    }

    #endregion
}
