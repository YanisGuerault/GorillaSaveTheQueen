using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseBonus : MonoBehaviour
{
    [SerializeField] private GameObject m_Bonus;
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
                    Instantiate(m_Bonus,child.position,child.rotation);
                    GetComponent<Renderer>().material = m_texture;
                }
            }
            
        }
    }
}
