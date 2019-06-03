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
            Vector3 pos = transform.position + new Vector3(0,0.5f,-0.3f);
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            Instantiate(m_Bonus, pos,rot);
            
        }
    }
}
