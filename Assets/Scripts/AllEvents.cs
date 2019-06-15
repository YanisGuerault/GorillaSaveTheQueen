using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

#region GameManager Events
public class GameMenuEvent : SDD.Events.Event
{
}
public class GamePlayEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class GameVictoryEvent : SDD.Events.Event
{
}

public class GameStatisticsChangedEvent : SDD.Events.Event
{
	public float eBestScore { get; set; }
	public float eScore { get; set; }
	public int eNLives { get; set; }
    public List<System.Type> eBonus { get; set; }
}
#endregion

#region MenuManager Events
public class EscapeButtonClickedEvent : SDD.Events.Event
{
}
public class PlayButtonClickedEvent : SDD.Events.Event
{
}
public class ResumeButtonClickedEvent : SDD.Events.Event
{
}

public class NextLevelButtonClickedEvent : SDD.Events.Event
{
}
public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}

public class QuitButtonClickedEvent : SDD.Events.Event
{
}

public class DifficultButtonClickedEvent : SDD.Events.Event
{
}

public class SetDifficultyEvent : SDD.Events.Event
{
    public int eDifficulty;
}
#endregion

#region Score Event
public class ScoreItemEvent : SDD.Events.Event
{
	public float eScore;
}
#endregion

#region Enemy Event
public class EnemyHasBeenDestroyEvent : SDD.Events.Event
{
    public Enemy eEnemy;
}
#endregion

#region Game Manager Additional Event

public class AskToGoToNextLevelEvent : SDD.Events.Event
{
}

public class GoToNextLevelEvent : SDD.Events.Event
{
}
#endregion

#region Player Event
public class PlayerHasBeenHitEvent: SDD.Events.Event
{
}

public class PlayerGetABonus: SDD.Events.Event
{
    public System.Type bonus;
}

public class ActiveMovingEvent : SDD.Events.Event
{
    public bool Active;
}
#endregion

#region Levels Manager Event
public class SettingCurrentLevelEvent : SDD.Events.Event
{
    public Level eLevel;
}

public class InstatiateLevelEvent : SDD.Events.Event
{
    public Level eLevel;
}

public class InitFirstLevelEvent : SDD.Events.Event
{

}

public class IsVictoryEvent : SDD.Events.Event
{
}
#endregion

#region Bonus Event
public class BonusToBePlacedEvent : SDD.Events.Event
{
    public System.Type eBonus;
}
#endregion