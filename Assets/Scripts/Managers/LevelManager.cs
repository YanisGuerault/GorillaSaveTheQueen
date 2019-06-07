using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SDD.Events;

	public class LevelManager : Manager<LevelManager>
	{
		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
		#endregion

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

		protected override void GamePlay(GamePlayEvent e)
		{
		}

		protected override void GameMenu(GameMenuEvent e)
		{
		}

        #region Callbacks to GameManager events
        public void GoToNextLevel(GoToNextLevelEvent e)
        {

        }
        #endregion
    }