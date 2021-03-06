﻿/* to do
 * change functions to protected where required
 * fix animations
 */


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

	protected GameObject[] Sensors;
	protected Sensor[] sensorScripts;

	protected int facing;

	private Vector3 targetPositon;
	private Vector3 displacment;

	private Animator animController;

	public void takeDamage(int dmg)
	{
		health += dmg - (dmg * (10/defence));
	}
	
	public void ClearSensors()
	{
		for (int i = 0; i < sensorScripts.Length; i++) 
		{
			sensorScripts[i].clearTargets();
		}
	}

	public void startFunc()
	{
		standSprites = new Sprite[]{faceUp, faceRight, faceDown, faceLeft};
		walkSprites = new Sprite[][]{walkUp, walkRight, walkDown, walkLeft};
		animController = this.GetComponent<Animator>();


		Sensors = new GameObject[] {sensorUp, sensorRight, sensorDown, sensorLeft};
		sensorScripts = new Sensor[] {sensorUp.GetComponent<Sensor>(), sensorRight.GetComponent<Sensor>(), sensorDown.GetComponent<Sensor>(), sensorLeft.GetComponent<Sensor>()};


		rend = GetComponent<SpriteRenderer>();

		targetPositon = transform.position;
		facing = (int) dir.UP;
	}

	public void updateLoop()
	{
		animController.SetInteger("Direction", facing);//move this for optimization

		if(!GameManager.Instance.stateGamePlaying.IsPaused())
		{
			if( !((targetPositon.x > transform.position.x - 0.05f && targetPositon.x < transform.position.x + 0.05f ) &&
			   (targetPositon.y > transform.position.y - 0.05f && targetPositon.y < transform.position.y + 0.05f)) )
			{
				//moving = true;
				transform.position += displacment * speed * 0.01f;

				animController.SetBool("Moving", true);

				/*
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
				*/
			}
			//this will stop the unit from being anywere but on a grid point if it is not moving
			else if ( transform.position.y != Mathf.Round(transform.position.y) || transform.position.x != Mathf.Round(transform.position.x))
			{
				animController.SetBool("Moving", false);
				//moving = false;
				Vector3 temp = transform.position;
				temp.y = Mathf.Round(temp.y);
				temp.x = Mathf.Round(temp.x);
				transform.position = temp;
				targetPositon = transform.position;
			}
		}

		if(health < 0)
		{
			//play death animation
			//when its done disable script and coliders and shit
		}
	}

	public void changeDir(int d)
	{
		if(!GameManager.Instance.stateGamePlaying.IsPaused())
		{
			if( d >= 0 && d <= 3)
			{
				facing = d;
			}
			rend.sprite = standSprites[facing];
			//Lighting.Instance.resetVision();
		}
	}

	private void Die()
	{
		Destroy(this);
	}

	public void changeDirAway(Vector3 face)
	{
		//needs to be improved lots
		if( transform.position.y > face.y)
		{
			facing = 0;
		}
		else if( transform.position.x > face.x)
		{
			facing = 1;
		}
		else if( transform.position.y < face.y)
		{
			facing = 2;
		}
		else if( transform.position.x < face.x)
		{
			facing = 3;
		}

		rend.sprite = standSprites[facing];
	}

	public void changeDirFacing(Vector3 face)
	{
		//this is stil bugged. needs cases for directly north south east west.
		float deg = GetVectorDirection(face - transform.position);
		//Debug.Log(deg);
		if((deg > 315 && deg <= 360) || (deg <= 0 && deg >= 45) )
		{
			facing = 1;
			//Debug.Log ("Right");
		}
		else if(deg > 45 && deg <= 135)
		{
			facing = 0;
			//Debug.Log ("Up");
		}
		else if(deg > 135 && deg <= 225)
		{
			facing = 3;
			//Debug.Log ("Left");
		}
		else if(deg > 225 && deg <= 315)
		{
			facing = 2;
			//Debug.Log ("Down");
		}
		rend.sprite = standSprites[facing];
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

	public void move()
	{
		if(!GameManager.Instance.stateGamePlaying.IsPaused())
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
	}

	public int getFacing()
	{
		return facing;
	}

}
