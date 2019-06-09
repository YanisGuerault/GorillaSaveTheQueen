using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBonus : Bonus
{
    private bool m_collect = false;
    private bool m_activated = false;
    Animator m_anim;

    public bool Collect { set { m_collect = value; } }

    protected void Start()
    {
        base.Start();
        m_anim = this.GetComponent<Animator>();
        StartCoroutine(StartCoroutine());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (m_collect && other.gameObject.CompareTag("Player"))
        {
            EventManager.Instance.Raise(new PlayerGetABonus() { bonus = this });
            Destroy(this.gameObject);
        }

        if (!m_activated && !m_collect)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                m_activated = true;
                m_anim.SetTrigger("Activated");
                EventManager.Instance.Raise(new EnemyHasBeenDestroyEvent() { eEnemy = other.gameObject.GetComponent<Enemy>() });
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                m_activated = true;
                m_anim.SetTrigger("Activated");
                EventManager.Instance.Raise(new PlayerHasBeenHitEvent());
            }
        }

    }

    protected IEnumerator StartCoroutine()
    {
        yield return new WaitForSeconds(2);
        m_activated = true;
    }
}
