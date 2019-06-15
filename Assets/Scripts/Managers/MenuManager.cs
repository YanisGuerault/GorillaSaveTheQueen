

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

	public class MenuManager : Manager<MenuManager>
	{

		[Header("MenuManager")]

		#region Panels
		[Header("Panels")]
		[SerializeField] GameObject m_PanelMainMenu;
		[SerializeField] GameObject m_PanelInGameMenu;
		[SerializeField] GameObject m_PanelGameOver;
        [SerializeField] GameObject m_PanelWin;
        [SerializeField] GameObject m_PanelNextLevel;
        [SerializeField] GameObject m_PanelDifficulty;
        [SerializeField] GameObject m_PanelCredits;

    List<GameObject> m_AllPanels;
		#endregion

		#region Events' subscription
		public override void SubscribeEvents()
		{
            EventManager.Instance.AddListener<AskToGoToNextLevelEvent>(AskToGoToNextLevel);
            EventManager.Instance.AddListener<GoToNextLevelEvent>(GoToNextLevel);
            EventManager.Instance.AddListener<DifficultButtonClickedEvent>(DifficultySelect);
            base.SubscribeEvents();
		}

		public override void UnsubscribeEvents()
		{
            EventManager.Instance.RemoveListener<AskToGoToNextLevelEvent>(AskToGoToNextLevel);
            EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);
            EventManager.Instance.RemoveListener<DifficultButtonClickedEvent>(DifficultySelect);
            base.UnsubscribeEvents();
		}
		#endregion

		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
		#endregion

		#region Monobehaviour lifecycle
		protected override void Awake()
		{
			base.Awake();
			RegisterPanels();
		}

		private void Update()
		{
			if (Input.GetButton("Cancel"))
			{
				EscapeButtonHasBeenClicked();
			}
		}
		#endregion

		#region Panel Methods
		void RegisterPanels()
		{
			m_AllPanels = new List<GameObject>();
			m_AllPanels.Add(m_PanelMainMenu);
			m_AllPanels.Add(m_PanelInGameMenu);
			m_AllPanels.Add(m_PanelGameOver);
            m_AllPanels.Add(m_PanelWin);
            m_AllPanels.Add(m_PanelNextLevel);
            m_AllPanels.Add(m_PanelDifficulty);
            m_AllPanels.Add(m_PanelCredits);
        }

		void OpenPanel(GameObject panel)
		{
			foreach (var item in m_AllPanels)
				if (item) item.SetActive(item == panel);
		}
		#endregion

		#region UI OnClick Events
		public void EscapeButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new EscapeButtonClickedEvent());
		}

		public void PlayButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new PlayButtonClickedEvent());
		}

		public void ResumeButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new ResumeButtonClickedEvent());
		}

		public void MainMenuButtonHasBeenClicked()
		{
        Debug.Log("Yes");
			EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
		}

		public void QuitButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new QuitButtonClickedEvent());
		}

        public void NextLevelButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new NextLevelButtonClickedEvent());
        }

        public void DifficultyButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new DifficultButtonClickedEvent());
        }

        public void EasyDifficultyButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new SetDifficultyEvent() { eDifficulty = 1 });
            OpenPanel(m_PanelMainMenu);
        }

        public void MediumDifficultyButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new SetDifficultyEvent() { eDifficulty = 2 });
            OpenPanel(m_PanelMainMenu);
        }

        public void HardDifficultyButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new SetDifficultyEvent() { eDifficulty = 3 });
            OpenPanel(m_PanelMainMenu);
        }

        public void CreditButtonHasBeenClicked()
        {
            OpenPanel(m_PanelCredits);
        }

    #endregion

    #region Callbacks to GameManager events
    protected override void GameMenu(GameMenuEvent e)
		{
			OpenPanel(m_PanelMainMenu);
		}

		protected override void GamePlay(GamePlayEvent e)
		{
			OpenPanel(null);
		}

		protected override void GamePause(GamePauseEvent e)
		{
			OpenPanel(m_PanelInGameMenu);
		}

		protected override void GameResume(GameResumeEvent e)
		{
			OpenPanel(null);
		}

		protected override void GameOver(GameOverEvent e)
		{
			OpenPanel(m_PanelGameOver);
		}

        protected override void GameVictory(GameVictoryEvent e)
        {
            OpenPanel(m_PanelWin);
        }

        private void AskToGoToNextLevel(AskToGoToNextLevelEvent e)
        {
            OpenPanel(m_PanelNextLevel);
        }

        private void GoToNextLevel(GoToNextLevelEvent e)
        {
            OpenPanel(null);
        }

        private void DifficultySelect(DifficultButtonClickedEvent e)
        {
            OpenPanel(m_PanelDifficulty);
        }
    #endregion
}
