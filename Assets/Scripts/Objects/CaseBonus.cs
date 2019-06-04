using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseBonus : MonoBehaviour
{
    [SerializeField] private GameObject m_Bonus;
    private bool m_activated = false;
    private void OnCollisionEnter(Collision collision)
    {
        if(!m_activated && collision.gameObject.GetComponent<PlayerController>())
        {
            m_activated = true;
            foreach(Transform child in transform)
            {
                Debug.Log(child.name);
                if(child.name == "BonusSpawnPoint")
                {
                    Instantiate(m_Bonus,child.position,child.rotation);
                }
            }
            
        }
    }
}
