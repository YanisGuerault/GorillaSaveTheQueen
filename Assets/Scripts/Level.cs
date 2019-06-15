using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System;
using System.Linq;
using UnityEngine.AI;

public class Level : MonoBehaviour, IEventHandler
{
    #region Variables
    [SerializeField] public Transform spawn_point;
    [SerializeField] public int Level_Music;

    private GameObject[] m_enemies;
    private SpawnPoint[] m_enemiesSpawnPoint;

    private GameObject[] m_Bonus;
    private SpawnPoint[] m_bonusSpawnPoint;

    public bool IsLast = false;

    [SerializeField] private int m_difficulty = 2;
    #endregion

    #region Event Listener
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<InstatiateLevelEvent>(LevelInstatiate);
        EventManager.Instance.AddListener<SetDifficultyEvent>(SetDifficulty);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<InstatiateLevelEvent>(LevelInstatiate);
        EventManager.Instance.RemoveListener<SetDifficultyEvent>(SetDifficulty);
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void Awake()
    {
        SubscribeEvents();
    }
    #endregion

#region Level Methods
    private void Start()
    {
        m_enemiesSpawnPoint = GetComponentsInChildren<EnemySpawnPoint>();
        m_bonusSpawnPoint = GetComponentsInChildren<BonusSpawnPoint>();
    }

    void SetDifficulty(SetDifficultyEvent e)
    {
        m_difficulty = e.eDifficulty;
    }

    void LevelInstatiate(InstatiateLevelEvent e)
    {
        if(e.eLevel == this)
        {
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
                    GameObject newEnemy = Instantiate(en.Prefab, en.transform.position, Quaternion.Euler(0, 90, 0));
                    switch(m_difficulty)
                    {
                        case 1:
                            newEnemy.GetComponent<NavMeshAgent>().speed = 2;
                            break;
                        case 2:
                            newEnemy.GetComponent<NavMeshAgent>().speed = 8;
                            break;
                        case 3:
                            newEnemy.GetComponent<NavMeshAgent>().speed = 14;
                            break;
                    }
                }
            }

            if (m_bonusSpawnPoint != null)
            {
                foreach (BonusSpawnPoint en in m_bonusSpawnPoint)
                {
                    GameObject newBonus = Instantiate(en.Prefab, en.transform.position, Quaternion.Euler(0, 0, 0));
                    newBonus.GetComponent<CaseBonus>().m_Bonus = en.m_Bonus;
                }
            }

            m_enemies = GameObject.FindGameObjectsWithTag("Enemy");
            m_Bonus = GameObject.FindGameObjectsWithTag("CaseBonus");
        }
    }
}
#endregion
