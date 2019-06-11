using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawnPoint : SpawnPoint
{
    [SerializeField] public GameObject m_Bonus;
    // Start is called before the first frame update
    void Start()
    {
        m_Prefab.GetComponent<CaseBonus>().m_Bonus = m_Bonus;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
