using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sensor : MonoBehaviour 
{
	[HideInInspector]
	public bool isColliding = false;
	public Collider2D ignoreThis;
	public List<Collider2D> targets;

	private enum dir {UP, RIGHT, DOWN, LEFT};
	void Start()
	{
		targets = new List<Collider2D>();
	}
	
	void Update()
	{
		if(targets.Count ==0)
		{
			isColliding = false;
		}
		else
		{
			for(int c = targets.Count-1; c >= 0; c--)
			{
				if( targets[c] == null)
				{
					targets.RemoveAt(c);
				}
			}
		}
	}

	public void clearTargets()
	{
		for(int c = targets.Count-1; c >= 0; c--)
		{
			targets.RemoveAt(c);
		}
		isColliding = false;
	}

//	public int[] getCollisionSide()
//	{
//		List<int> direction = new List<int>();
//		for(int c = targets.Count-1; c >= 0; c--)
//		{
//			Vector3 difVector = transform.position - targets[c].transform.position;
//			Debug.Log(" ");
//			//
//			if( difVector.y == 1.0 && difVector.x == 0)//make not exact
//			{
//				direction.Add((int)dir.UP);
//			}
//			else if(difVector.y == 0 && difVector.x == 1.0)//make not exact
//			{
//				direction.Add((int)dir.RIGHT);
//			}
//			else if(difVector.y == -1.0 && difVector.x == 0)//make not exact
//			{
//				direction.Add((int)dir.DOWN);
//			}
//			else if(difVector.y == 0 && difVector.x == -1.0)//make not exact
//			{
//				direction.Add(dir.LEFT);
//			}
//		}
//
//		int[] ret = new int[direction.Count];
//		for(int c = direction.Count-1; c >= 0; c--)
//		{
//			ret[c] = direction[c];
//		}
//		return ret;
//	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other != ignoreThis)
		{
			isColliding = true;
			targets.Add(other);
		}
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		if(other != ignoreThis)
		{
			targets.Remove(other);
		}
	}
}
