using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

	public class HudManager : Manager<HudManager>
	{

        [Header("HudManager")]
        #region Labels & Values
        [Header("Texts")]

        [SerializeField] private Text m_TxtScore;
        [SerializeField] private Text m_TxtNLives;

        #endregion

        #region Manager implementation
        protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
		#endregion

		#region Callbacks to GameManager events
		protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
		{
        if (e.eBonus != null)
        {
            m_TxtScore.text = e.eBonus.ToString();
        }
        else
        {
            m_TxtScore.text = "null";
        }
            m_TxtNLives.text = e.eNLives.ToString();

		}
		#endregion

	}