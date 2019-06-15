using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class LevelManager : Manager<LevelManager>
{

    [Header("LevelsManager")]
    #region levels & current level management
    [SerializeField] GameObject[] m_LevelsPrefabs;
    private int m_CurrentLevelIndex;
    private Level m_CurrentLevel;
    public Level CurrentLevel { get { return m_CurrentLevel; } }
    #endregion

    #region Manager implementation
    private void Start()
    {
        if(m_LevelsPrefabs[0] != null)
        {
            m_CurrentLevelIndex = 0;
            m_CurrentLevel = m_LevelsPrefabs[0].GetComponent<Level>();
            EventManager.Instance.Raise(new SettingCurrentLevelEvent() { eLevel = m_CurrentLevel });
        }
        m_LevelsPrefabs[m_LevelsPrefabs.Length - 1].GetComponent<Level>().IsLast = true;
    }

    private void Reset()
    {
        EventManager.Instance.Raise(new SettingCurrentLevelEvent() { eLevel = m_CurrentLevel });
    }

    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }
    #endregion

    #region Events' subscription
    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        EventManager.Instance.AddListener<GoToNextLevelEvent>(GoToNextLevel);
        EventManager.Instance.AddListener<InitFirstLevelEvent>(InitFirstLevel);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);
        EventManager.Instance.RemoveListener<InitFirstLevelEvent>(InitFirstLevel);
    }
    #endregion

    #region Callbacks to GameManager events
    protected override void GameMenu(GameMenuEvent e)
    {
        Reset();
    }
    protected override void GamePlay(GamePlayEvent e)
    {
        Reset();
    }

    public void GoToNextLevel(GoToNextLevelEvent e)
    {
        m_CurrentLevelIndex++;
        if (m_CurrentLevelIndex >= m_LevelsPrefabs.Length)
        {
            EventManager.Instance.Raise(new GameVictoryEvent());
        }
        else
        {
            m_CurrentLevel = m_LevelsPrefabs[m_CurrentLevelIndex].GetComponent<Level>();
            EventManager.Instance.Raise(new SettingCurrentLevelEvent() { eLevel = m_CurrentLevel });
            EventManager.Instance.Raise(new InstatiateLevelEvent() { eLevel = m_CurrentLevel });
        }
    }

    private void InitFirstLevel(InitFirstLevelEvent e)
    {
        Start();
    }
    #endregion
}
