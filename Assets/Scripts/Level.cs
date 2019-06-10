using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System;
using System.Linq;

public class Level : MonoBehaviour, IEventHandler
{
    [SerializeField] public Transform spawn_point;

    private GameObject[] m_enemies;
    private SpawnPoint[] m_enemiesSpawnPoint;

    private GameObject[] m_Bonus;
    private SpawnPoint[] m_bonusSpawnPoint;

    public bool IsLast = false;

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<InstatiateLevelEvent>(LevelInstatiate);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<InstatiateLevelEvent>(LevelInstatiate);
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void Awake()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        m_enemiesSpawnPoint = GetComponentsInChildren<EnemySpawnPoint>();
        m_bonusSpawnPoint = GetComponentsInChildren<BonusSpawnPoint>();
    }

    void LevelInstatiate(InstatiateLevelEvent e)
    {
        if(e.eLevel == this)
        {
            Debug.Log(e.eLevel);
            if (m_enemies != null)
            {
                foreach (GameObject en in m_enemies)
                {
                    Destroy(en);
                }
            }

            if (m_Bonus != null)
            {
                foreach (GameObject en in m_Bonus)
                {
                    Destroy(en);
                }
            }

            if (m_enemiesSpawnPoint != null)
            {
                foreach (EnemySpawnPoint en in m_enemiesSpawnPoint)
                {
                    Instantiate(en.Prefab, en.transform.position, Quaternion.Euler(0, 90, 0));
                }
            }

            if (m_bonusSpawnPoint != null)
            {
                foreach (BonusSpawnPoint en in m_bonusSpawnPoint)
                {
                    Instantiate(en.Prefab, en.transform.position, Quaternion.Euler(0, 0, 0));
                }
            }

            m_enemies = GameObject.FindGameObjectsWithTag("Enemy");
            m_Bonus = GameObject.FindGameObjectsWithTag("CaseBonus");
        }
    }
}
