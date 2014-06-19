using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour 
{
	public static Options Instance { get { return instance; } }
	private static Options instance = null;

	private KeyCode pauseKey;
	private KeyCode cameraKey;
	private KeyCode moveUpKey;
	private KeyCode moveRightKey;
	private KeyCode moveDownKey;
	private KeyCode moveLeftKey;
	private KeyCode lookKey;

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
	}

	// Use this for initialization
	void Start () 
	{
		
	}

}
