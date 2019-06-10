using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform m_player;
    private NavMeshAgent m_nav;
    private Animator m_anim;
    private bool m_hit = false;
    private Transform m_Ground;
    private List<Collider> m_collisions = new List<Collider>();
    private bool dead = false;

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<EnemyHasBeenDestroyEvent>(EnemyHasBeenDestroy);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<EnemyHasBeenDestroyEvent>(EnemyHasBeenDestroy);
    }

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_nav = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    #region Follow player if see him
    private void Update()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        float angleVision = Vector3.SignedAngle(m_player.position - transform.position, transform.forward, Vector3.up);
        float distanceWithPlayer = Vector3.Distance(this.transform.position, m_player.position);
        //Debug.Log("Distance : " + distanceWithPlayer + " angle : "+angleVision);
        //Debug.Log("Dead : " + dead);
        if (!dead && (angleVision < 70 && angleVision > -70 && distanceWithPlayer < 15) 
            || ((angleVision > 70 && angleVision < 180) || (angleVision < -70 && angleVision > -180) && distanceWithPlayer < 5))
        {
            if (m_nav != null && !m_anim.GetCurrentAnimatorStateInfo(0).IsName("zombie_attack"))
            {
                m_nav.SetDestination(m_player.position);
            }

            m_anim.SetBool("SeePlayer", true);
        }
        else
        {
            m_anim.SetBool("SeePlayer", false);
        }

        if (m_Ground != null && transform.position.y < m_Ground.position.y - 10)
        {
            EventManager.Instance.Raise(new EnemyHasBeenDestroyEvent() { eEnemy = this });
        }
    }

    #endregion

    #region Attack & Hit Player

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionStay(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        /*Debug.Log("Player : " + collision.gameObject.CompareTag("Player") + " hit : " + hit + " Anim name : " + anim.GetCurrentAnimatorStateInfo(0).IsName("zombie_attack") + "time : " + GetCurrentAnimatorTime(anim));*/
        if (!dead && collision.gameObject.CompareTag("Player") && !m_hit && m_anim.GetCurrentAnimatorStateInfo(0).IsName("zombie_attack") && GetCurrentAnimatorTime(m_anim) > 0.3)
        {
            EventManager.Instance.Raise(new PlayerHasBeenHitEvent());
            m_hit = true;
            StartCoroutine(CoroutineHitPlayer(2));
        }

        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                m_Ground = collision.transform;
            }
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }

        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
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

    private void EnemyHasBeenDestroy(EnemyHasBeenDestroyEvent e)
    {
        if(e.eEnemy == this)
        {
            StartCoroutine(CoroutineDead());
        }
    }

    private IEnumerator CoroutineDead()
    {
        yield return new WaitForSeconds(0.5f);
        m_anim.SetTrigger("Dead");
        dead = true;
        Destroy(this.GetComponent<NavMeshAgent>());
        Destroy(this.GetComponent<CapsuleCollider>());
    }

    #endregion
}
