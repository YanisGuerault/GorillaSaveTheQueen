﻿using System;
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
    }

    private void Reset()
    {
        Start();
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
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);
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
        m_CurrentLevel = m_LevelsPrefabs[m_CurrentLevelIndex].GetComponent<Level>();
        EventManager.Instance.Raise(new SettingCurrentLevelEvent() { eLevel = m_CurrentLevel });
    }
    #endregion
}
