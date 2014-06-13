using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour 
{
	protected enum dir {UP, RIGHT, DOWN, LEFT};

	public int attackPower;
	public int health;
	public int speed;
	public int defence;
	public int visionDistance;

	private int facing;

	private Vector3 targetPositon;
	private Vector3 displacment;
	private float moveCoolDown = 0;
	private bool moving = false;

	public void takeDamage(int dmg)
	{
		health -= dmg - (dmg * (10/defence));
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void startFunc()
	{
		targetPositon = transform.position;
		facing = (int) dir.DOWN;
	}

	public void updateLoop()
	{
		if( targetPositon != transform.position)
		{
			transform.position += displacment * speed * 0.01f;
			Debug.Log ("zooooooooooooooooom");
		}
	}

	public void changeDir(int d)
	{
		if ( d >= 0 && d <= 4)
		{
			facing = d;
		}
	}

	public void move()
	{
		if( targetPositon == transform.position)
		{
			switch(facing)
			{
			case (int) dir.UP:
				targetPositon.y += 1;
				displacment = targetPositon - transform.position;
				break;
			case (int) dir.RIGHT:
				targetPositon.x += 1;
				displacment = targetPositon - transform.position;
				break;
			case (int) dir.DOWN:
				targetPositon.y -= 1;
				displacment = targetPositon - transform.position;
				break;
			case (int) dir.LEFT:
				targetPositon.x -= 1;
				displacment = targetPositon - transform.position;
				break;
			}
		}
	}

}
