using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public Texture backgroundTexture;

	public float menuButtonX;
	public float menuButtonY;

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),backgroundTexture);

		if (GUI.Button (new Rect (Screen.width * menuButtonX, Screen.height * menuButtonY, Screen.width * .1f, Screen.height * .1f), "Menu"))
		{
			print("Clicked Menu");
		}

	}

}
