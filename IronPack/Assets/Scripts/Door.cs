//this class is a little slopy and could be improved

//doors can be opened from anywhere

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour {

	public Sprite doorOpen;
	public Sprite doorClosed;

	private bool open = false;
	private List<SpriteRenderer> rends;
	private List<BoxCollider2D> colliders;

	public void takeRaycastHit2D(RaycastHit2D[] input)
	{
		rends = new List<SpriteRenderer>();
		colliders = new List<BoxCollider2D>();
		for (int i = 0; i < input.Length; i ++)
		{
			if(input[i].collider.gameObject.name != "Room")//maybe change this to a better compare
			{
				rends.Add (input[i].collider.gameObject.GetComponent("SpriteRenderer") as SpriteRenderer);
				colliders.Add (input[i].collider.gameObject.GetComponent("BoxCollider2D") as BoxCollider2D);
			}
		}
		for (int i = 0; i < rends.Count; i++) 
		{
			rends[i].sprite = doorClosed;
		}

	}

	void Update()
	{
		if(Input.GetMouseButtonUp(0))
		{
			Vector2 mPos = GetMouseInWorld (-2);
			Vector2 temp;
			mPos.x = Mathf.Round(mPos.x);
			mPos.y = Mathf.Round(mPos.y);
			for (int i = 0; i < rends.Count; i++) 
			{
				temp = rends[i].transform.position;
				if(temp == mPos )
				{
					Toggle();
					break;
				}
			}
		}
	}

	private Vector2 GetMouseInWorld( float z)
	{
		Vector3 v3 = Input.mousePosition;
		v3.z = z;
		v3 = Camera.main.ScreenToWorldPoint(v3);
		
		return v3;
	}
	
	public void Open()
	{
		open = true;
		for (int i = 0; i < rends.Count; i++) 
		{
			rends[i].sprite = doorOpen;
			colliders[i].enabled = false;
		}
		UnitManager.Instance.ClearAllSensors ();
	}

	public void Close()
	{
		open = false;
		for (int i = 0; i < rends.Count; i++) 
		{
			rends[i].sprite = doorClosed;
			colliders[i].enabled = true;
		}
		UnitManager.Instance.ClearAllSensors ();
	}

	public void Toggle()
	{
		if (open)
				Close ();
		else
				Open ();
	}
}
