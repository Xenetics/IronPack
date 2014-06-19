using UnityEngine;
using System.Collections;
public class StateGamePlaying : GameState 
{
	private bool isPaused = false;
	
	public StateGamePlaying(GameManager manager):base(manager){	}
	
	public override void OnStateEntered(){}
	public override void OnStateExit(){}
	
	public override void StateUpdate() 
	{
		if (Input.GetKeyDown(KeyCode.Space)) 
		{
			if (isPaused)
			{
				ResumeGameMode();
			}
			else
			{
				PauseGameMode();
			}
		}
	}
	
	public override void StateGUI() 
	{	
		if(isPaused)
		{
			GUILayout.Label("state: GAME PLAYING  **PAUSED**");
		}
		else
		{
			GUILayout.Label("state: GAME PLAYING");
		}
	}
	
	private void ResumeGameMode() 
	{
		Time.timeScale = 1.0f;
		isPaused = false;
	}
	
	private void PauseGameMode() 
	{
		Time.timeScale = 0.0000001f;
		isPaused = true;
	}

	public bool IsPaused()
	{
		return isPaused;
	}
}