﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SDD.Events;
using System.Linq;

public enum GameState { gameMenu, gamePlay, gameNextLevel, gamePause, gameOver, gameVictory }

public class GameManager : Manager<GameManager>
{
    #region Game State
    private GameState m_GameState;
    public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }
    #endregion

    //LIVES
    #region Lives
    [Header("GameManager")]
    [SerializeField]
    private int m_NStartLives;

    private int m_NLives;
    public int NLives { get { return m_NLives; } }
    void DecrementNLives(int decrement)
    {
        if (m_NLives > 0)
        {
            SetNLives(m_NLives - decrement);
        }
    }

    void SetNLives(int nLives)
    {
        m_NLives = nLives;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives, eBonus = m_bonus });
    }
    #endregion


    #region Score
    private float m_Score;
    public float Score
    {
        get { return m_Score; }
        set
        {
            m_Score = value;
            BestScore = Mathf.Max(BestScore, value);
        }
    }

    public float BestScore
    {
        get { return PlayerPrefs.GetFloat("BEST_SCORE", 0); }
        set { PlayerPrefs.SetFloat("BEST_SCORE", value); }
    }

    void IncrementScore(float increment)
    {
        SetScore(m_Score + increment);
    }

    void SetScore(float score, bool raiseEvent = true)
    {
        Score = score;

        if (raiseEvent)
            EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives, eBonus = m_bonus });
    }
    #endregion

    #region Bonus
    private List<Bonus> m_bonus = new List<Bonus>();

    public List<Bonus> getBonusList()
    {
        return m_bonus;
    }

    public void AddABonus(Bonus bonus)
    {
        m_bonus.Add(bonus);
        Debug.Log(bonus);
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = m_Score, eNLives = m_NLives, eBonus = m_bonus });
    }

    public void removeABonus(Bonus bonus)
    {
        m_bonus.Remove(bonus);
    }
    #endregion

    #region Time
    void SetTimeScale(float newTimeScale)
		{
			Time.timeScale = newTimeScale;
		}
		#endregion


		#region Events' subscription
		public override void SubscribeEvents()
		{
			base.SubscribeEvents();
			
			//MainMenuManager
			EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
			EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
			EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
			EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
			EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);

			//Score Item
			EventManager.Instance.AddListener<ScoreItemEvent>(ScoreHasBeenGained);

            //Victory
            EventManager.Instance.AddListener<GameVictoryEvent>(WinEvent);

            //Player
            EventManager.Instance.AddListener<PlayerHasBeenHitEvent>(PlayerHasBeenHit);
            EventManager.Instance.AddListener<PlayerGetABonus>(PlayerGetABonus);

            //Enemy
            EventManager.Instance.AddListener<EnemyHasBeenDestroyEvent>(enemyDestroy);
    }

		public override void UnsubscribeEvents()
		{
			base.UnsubscribeEvents();

			//MainMenuManager
			EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
			EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
			EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
			EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
			EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);

			//Score Item
			EventManager.Instance.RemoveListener<ScoreItemEvent>(ScoreHasBeenGained);

            //Player
            EventManager.Instance.RemoveListener<PlayerHasBeenHitEvent>(PlayerHasBeenHit);
            EventManager.Instance.RemoveListener<PlayerGetABonus>(PlayerGetABonus);

            //Enemy
            EventManager.Instance.RemoveListener<EnemyHasBeenDestroyEvent>(enemyDestroy);

            //Victory
            EventManager.Instance.RemoveListener<GameVictoryEvent>(WinEvent);

    }
		#endregion

		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
			Menu();
			InitNewGame(); // essentiellement pour que les statistiques du jeu soient mise à jour en HUD
			yield break;
		}
		#endregion

		#region Game flow & Gameplay
		//Game initialization
		void InitNewGame(bool raiseStatsEvent = true)
		{
			SetScore(0);
            SetNLives(m_NStartLives);
		}
		#endregion

		#region Callbacks to events issued by Score items
		private void ScoreHasBeenGained(ScoreItemEvent e)
		{
			if (IsPlaying)
				IncrementScore(e.eScore);
		}
		#endregion

		#region Callbacks to Events issued by MenuManager
		private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
		{
			Menu();
		}

		private void PlayButtonClicked(PlayButtonClickedEvent e)
		{
			Play();
		}

		private void ResumeButtonClicked(ResumeButtonClickedEvent e)
		{
			Resume();
		}

		private void EscapeButtonClicked(EscapeButtonClickedEvent e)
		{
			if (IsPlaying) Pause();
		}

		private void QuitButtonClicked(QuitButtonClickedEvent e)
		{
			Application.Quit();
		}
		#endregion

		#region GameState methods
		private void Menu()
		{
			SetTimeScale(1);
			m_GameState = GameState.gameMenu;
			if(MusicLoopsManager.Instance)MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
			EventManager.Instance.Raise(new GameMenuEvent());
            GameObject.Find("Player").transform.position = GameObject.Find("Level_1_Spawn_Point").transform.position;
        }

		private void Play()
		{
			InitNewGame();
			SetTimeScale(1);
			m_GameState = GameState.gamePlay;

		    if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
			EventManager.Instance.Raise(new GamePlayEvent());
		}

		private void Pause()
		{
			if (!IsPlaying) return;

			SetTimeScale(0);
			m_GameState = GameState.gamePause;
			EventManager.Instance.Raise(new GamePauseEvent());
		}

		private void Resume()
		{
			if (IsPlaying) return;

			SetTimeScale(1);
			m_GameState = GameState.gamePlay;
			EventManager.Instance.Raise(new GameResumeEvent());
		}

		private void Over()
		{
			SetTimeScale(0);
			m_GameState = GameState.gameOver;
			EventManager.Instance.Raise(new GameOverEvent());
			if(SfxManager.Instance) SfxManager.Instance.PlaySfx2D(Constants.GAMEOVER_SFX);
		}

        private void Win()
        {
        Debug.Log("Bonsoir");
            m_GameState = GameState.gameVictory;
            //if (SfxManager.Instance) SfxManager.Instance.PlaySfx2D(Constants.MENU_MUSIC);
    }
    #endregion

    #region Callsbacks to events issued by Player
    private void PlayerHasBeenHit(PlayerHasBeenHitEvent e)
    {
        DecrementNLives(1);
        Debug.Log(m_NLives);

        if(m_NLives <= 0)
        {
            Over();
        }
    }

    private void PlayerGetABonus(PlayerGetABonus e)
    {
        AddABonus(e.bonus);
    }
    #endregion

    #region Callsbacks to events issued by Enemy
    private void enemyDestroy(EnemyHasBeenDestroyEvent e)
    {

    }
    #endregion

    #region Callsbacks to events issued by Win
    private void WinEvent(GameVictoryEvent e)
    {
        Debug.Log("RECEIVD");
        Win();
    }
    #endregion
}

