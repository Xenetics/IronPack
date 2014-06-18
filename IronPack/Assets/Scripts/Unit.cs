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

	public Sprite faceUp;
	public Sprite faceRight;
	public Sprite faceDown;
	public Sprite faceLeft;

	public Sprite[] walkUp;
	public Sprite[] walkRight;
	public Sprite[] walkDown;
	public Sprite[] walkLeft;

	public GameObject sensorUp;
	public GameObject sensorRight;
	public GameObject sensorDown;
	public GameObject sensorLeft;

	private Sprite[] standSprites;
	private Sprite[] [] walkSprites;

	private SpriteRenderer rend;

	private GameObject[] Sensors;
	private Sensor[] sensorScripts;

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
		standSprites = new Sprite[]{faceUp, faceRight, faceDown, faceLeft};
		walkSprites = new Sprite[][]{walkUp, walkRight, walkDown, walkLeft};

		Sensors = new GameObject[] {sensorUp, sensorRight, sensorDown, sensorLeft};
		sensorScripts = new Sensor[] {sensorUp.GetComponent<Sensor>(), sensorRight.GetComponent<Sensor>(), sensorDown.GetComponent<Sensor>(), sensorLeft.GetComponent<Sensor>()};


		rend = GetComponent<SpriteRenderer>();

		targetPositon = transform.position;
		facing = (int) dir.UP;
	}

	public void updateLoop()
	{
		//make this not exact
		if( !((targetPositon.x > transform.position.x - 0.05f && targetPositon.x < transform.position.x + 0.05f ) &&
		   (targetPositon.y > transform.position.y - 0.05f && targetPositon.y < transform.position.y + 0.05f)) )
		{
			//moving = true;
			transform.position += displacment * speed * 0.01f;
			//Debug.Log ("zooooooooooooooooom");

			//animation stuff
			Vector3 temp =  targetPositon - transform.position;
			float animPercent = 1 - Mathf.Abs( temp.y + temp.x);//hacks?
			if(animPercent > 0.95)
			{
				rend.sprite = standSprites[facing];
			}
			else if(animPercent > 0)
			{
				int animPos = Mathf.FloorToInt(walkSprites[facing].Length * animPercent);
				rend.sprite = walkSprites[facing][animPos];
			}
		}
		//this will stop the unit from being anywere but on a grid point if it is not moving
		else if ( transform.position.y != Mathf.Round(transform.position.y) || transform.position.x != Mathf.Round(transform.position.x))
		{
			//moving = false;
			Vector3 temp = transform.position;
			temp.y = Mathf.Round(temp.y);
			temp.x = Mathf.Round(temp.x);
			transform.position = temp;
			targetPositon = transform.position;
		}
	}

	public void changeDir(int d)
	{
		if ( d >= 0 && d <= 3)
		{
			facing = d;
		}
		rend.sprite = standSprites[facing];
	}

	public void move()
	{
		if((targetPositon.x > transform.position.x - 0.05f && targetPositon.x < transform.position.x + 0.05f ) &&
		   (targetPositon.y > transform.position.y - 0.05f && targetPositon.y < transform.position.y + 0.05f) &&
		   (!sensorScripts[facing].isColliding) )
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

	public int getFacing()
	{
		return facing;
	}

}
