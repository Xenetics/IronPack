using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Radial : MonoBehaviour 
{
	List<GameObject> wolves;
	public bool radialShow = false;
	Vector2 wPos;
	float mag;
	
	public Texture backgroundTexture;
	
	public float menuButtonX;
	public float menuButtonY;
	
	// Use this for initialization
	void Start () 
	{
		UpdateWolves();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 mPos = Input.mousePosition;
		//wolves[0].transform.position;

		if(Input.GetMouseButtonDown(0))
		{
			UpdateWolves();
		}

		if (Input.GetMouseButton (0)) 
		{
			UpdateWolves();
			for(int i = 0 ; i < wolves.Count ; ++i)
			{
				wPos = wolves[i].transform.position;
				if((mPos - wPos).magnitude < 0.85f)
				{
					print("GAH!");
					radialShow = true;
				}
			}
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			radialShow = false;
		}

	}

	void OnGui()
	{
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);
		if(radialShow)
		{

			GUI.Button (new Rect (Screen.width * menuButtonX, Screen.height * menuButtonY, Screen.width * .1f, Screen.height * .5f), "ATK");
			GUI.Button (new Rect (Screen.width * menuButtonX, Screen.height * menuButtonY, Screen.width * .5f, Screen.height * .1f), "Move");
			GUI.Button (new Rect (Screen.width * menuButtonX, Screen.height * menuButtonY, Screen.width * .1f, Screen.height * .5f), "Stay");
			GUI.Button (new Rect (Screen.width * menuButtonX, Screen.height * menuButtonY, Screen.width * .5f, Screen.height * .1f), "SPCL");
		}
	}

	void UpdateWolves()
	{
		if (UnitManager.Instance.getWolves () != null)
		{
			wolves = UnitManager.Instance.getWolves ();
		} 
		else 
		{
			wolves = new List<GameObject>();
		}
	}
}
