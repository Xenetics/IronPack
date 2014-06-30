using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Radial : MonoBehaviour 
{
	List<Transform> wolves;
	public bool radialShow = false;

	private Vector2 wPos;
	private Vector2 wolfPos;
	private float mag;
	
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
		Vector2 mPos = GetMouseInWorld(-2f);
		//wolves[0].transform.position;

		if(Input.GetMouseButtonDown(0))
		{
			UpdateWolves();
			//find the wolf you clicked on
		}

		if (Input.GetMouseButton (0) && !radialShow) 
		{
			UpdateWolves();
			for(int i = 0 ; i < wolves.Count ; ++i)
			{
				wolfPos = wolves[i].position;
				if( (wolfPos - mPos).magnitude < 0.8f) 
				{
					radialShow = true;
					menuButtonX = Input.mousePosition.x;
					menuButtonY = Input.mousePosition.y;
				}
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			if (radialShow == true)
			{
				float dir = GetVectorDirection(mPos - wolfPos);
				if(dir > 0 && dir < 90)
				{
					Debug.Log ("move");
					//command for pointing top right (move)
				}
				else if(dir > 90 && dir < 180)
				{
					Debug.Log ("special");
					//command for pointing top left(special)
				}
				else if(dir > 180 && dir < 270)
				{
					Debug.Log ("stay");
					//command for pointing bottom left(stay)
				}
				else if(dir > 270 && dir < 360)
				{
					Debug.Log ("attack");
					//command for pointing bottom right(attack)           
				}
			}
			radialShow = false;
		}
	}

	void OnGUI()
	{
		//GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);
		if(radialShow)
		{
			GUI.Button (new Rect (menuButtonX + 100, menuButtonY + 100, Screen.width * .1f, Screen.height * .1f), "ATK");
			GUI.Button (new Rect (menuButtonX + 100, menuButtonY - 100, Screen.width * .1f, Screen.height * .1f), "Move");
			GUI.Button (new Rect (menuButtonX - 100, menuButtonY + 100, Screen.width * .1f, Screen.height * .1f), "Stay");
			GUI.Button (new Rect (menuButtonX - 100, menuButtonY - 100, Screen.width * .1f, Screen.height * .1f), "SPCL");
		}
	}

	void UpdateWolves()
	{
		if (UnitManager.Instance.getWolves () != null)
		{
			List<GameObject> temp = UnitManager.Instance.getWolves ();
			for(int i = 0 ; i < temp.Count ; ++i)
			{
				wolves.Add(temp[i].transform);
			}
		} 
		else 
		{
			wolves = new List<Transform>();
		}
	}

	private Vector2 GetMouseInWorld( float z)
	{
		Vector3 v3 = Input.mousePosition;
		v3.z = z;
		v3 = Camera.main.ScreenToWorldPoint(v3);

		return v3;
	}

	private float GetVectorDirection(Vector2 v)
	{
		float ret = 0;
		if(v.x >= 0 && v.y > 0) //Q1
		{
			ret =  Mathf.Rad2Deg * (Mathf.Atan(v.y/v.x));
		}
		else if(v.x >= 0 &&  v.y < 0) //Q2
		{
			ret = 360 + Mathf.Rad2Deg * (Mathf.Atan(v.y/v.x));
		}
		else if(v.x <= 0 &&  v.y < 0) //Q3
		{
			ret = 180 + Mathf.Rad2Deg * (Mathf.Atan(v.y/v.x));
		}
		else if(v.x <= 0 &&  v.y > 0)//Q4
		{
			ret = 90 - (-Mathf.Rad2Deg * (Mathf.Atan(v.y/v.x)) - 90);
		}
		else if ( v.y == 0 && v.x < 0)
		{
			ret = 180;
		}
		else if ( v.y == 0 && v.x > 0)
		{
			ret = 0;
		}
		else
		{
			//the vector is 0,0
		}
		return ret;
	}

}
