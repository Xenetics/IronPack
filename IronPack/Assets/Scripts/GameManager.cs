using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	//singleton class stuff
	public static GameManager Instance { get { return instance; } }
	private static GameManager instance = null;

	private GameState currentState;
	//declare game states
	public StateGamePlaying stateGamePlaying{get;set;}


	private void Awake () 
	{
		//more singleton class stuff
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;        
		} 
		else 
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);

		//initalize game states
		stateGamePlaying = new StateGamePlaying(this);
	}
	
	private void Start () 
	{
		NewGameState( stateGamePlaying );
	}
	
	private void Update () 
	{
		if (currentState != null)
		{
			currentState.StateUpdate();
		}
	}
	
	private void OnGUI () 
	{
		if (currentState != null)
		{
			currentState.StateGUI();
		}
	}
	
	public void NewGameState(GameState newState)
	{
		if( null != currentState)
		{
			currentState.OnStateExit();
		}
		currentState = newState;
		currentState.OnStateEntered();
	}
}