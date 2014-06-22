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
