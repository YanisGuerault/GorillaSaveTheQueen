using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBonus : Bonus
{
    private bool m_collect = false;
    private bool m_activated = false;
    private bool m_started = false;
    Animator m_anim;
    public static GameObject Prefab;
    public List<GameObject> onTriggerObject = new List<GameObject>();

    public bool Collect { get { return m_collect; } set { m_collect = value; } }

    protected void Start()
    {
        base.Start();
        Prefab = Resources.Load<GameObject>("TrapBonus");
        m_anim = this.GetComponent<Animator>();
        StartCoroutine(StartCoroutine());
    }
    private void OnTriggerEnter(Collider other)
    {
        onTriggerObject.Add(other.gameObject);
        VerifAndHit(other);

        if (m_collect && other.gameObject.CompareTag("Player"))
        {
            //m_collect = false;
            EventManager.Instance.Raise(new PlayerGetABonus() { bonus = this.GetType() });
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        VerifAndHit(other);
    }

    void VerifAndHit(Collider other)
    {

        if (!m_collect && m_started && !m_activated && (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player")))
        {
            m_activated = true;
            m_anim.SetTrigger("Activated");
            foreach (GameObject obj in onTriggerObject)
            {
                Debug.Log("Ontrigger " + obj);
                if (obj.CompareTag("Enemy"))
                {
                    EventManager.Instance.Raise(new EnemyHasBeenDestroyEvent() { eEnemy = obj.GetComponent<Enemy>() });
                }
                else if (obj.CompareTag("Player"))
                {
                    Debug.Log("Hit" + m_collect);
                    EventManager.Instance.Raise(new PlayerHasBeenHitEvent());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit "+other.gameObject);
        onTriggerObject.Remove(other.gameObject);
    }

    protected IEnumerator StartCoroutine()
    {
        yield return new WaitForSeconds(2);
        m_started = true;
    }
}
