using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Sprite doorOpen;
	public Sprite doorClosed;

	private bool open = false;
	private SpriteRenderer[] rends;
	private BoxCollider2D[] colliders;

	public void takeRaycastHit2D(RaycastHit2D[] input)
	{
		rends = new SpriteRenderer[input.Length];
		colliders = new BoxCollider2D[input.Length];
		for (int i = 0; i < input.Length; i ++)
		{
			rends[i] = input[i].collider.gameObject.GetComponent("SpriteRenderer") as SpriteRenderer;
			colliders[i] = input[i].collider.gameObject.GetComponent("BoxCollider2D") as BoxCollider2D;
		}
		Open ();
	}
	
	public void Open()
	{
		open = true;
		for (int i = 0; i < rends.Length; i++) 
		{
			rends[i].sprite = doorOpen;
			colliders[i].enabled = false;
		}
	}

	public void Close()
	{
		open = false;
		for (int i = 0; i < rends.Length; i++) 
		{
			rends[i].sprite = doorClosed;
			colliders[i].enabled = true;
		}
	}

	public void Toggle()
	{
		if (open)
				Close ();
		else
				Open ();
	}
}
