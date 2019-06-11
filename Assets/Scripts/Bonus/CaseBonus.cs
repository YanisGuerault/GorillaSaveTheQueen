using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseBonus : MonoBehaviour
{
    [SerializeField] public GameObject m_Bonus;
    [SerializeField] private Material m_texture;

    private bool m_activated = false;
    private void OnCollisionEnter(Collision collision)
    {
        if(!m_activated && collision.gameObject.CompareTag("Player"))
        {
            m_activated = true;
            foreach(Transform child in transform)
            {
                if(child.name == "BonusSpawnPoint")
                {
                    Bonus bonus = Instantiate(m_Bonus,child.position,m_Bonus.transform.rotation).GetComponent<Bonus>();
                    if(bonus.GetType() == typeof(TrapBonus))
                    {
                        TrapBonus bonus2 = bonus.GetComponent<TrapBonus>();
                        bonus2.Collect = true;
                        bonus2.GetComponent<Rigidbody>().isKinematic = false;
                    }
                    GetComponent<Renderer>().material = m_texture;
                }
            }
            
        }
    }
}
