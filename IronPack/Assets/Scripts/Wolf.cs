//this eniter thing is pretty messy as it was build in peices
//can be cleaned up big time

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wolf : Unit
{

	public enum State{
		aggressive, defensive, passiveAggressive, passive
	}

	public State currentState;
	public int followDistance = 3; //rename this varable
	public bool commandPresent = false;
	
	private Vector2 commandLocation;
	private string currentCommand;
	private Transform player;

	private List<Transform> enemies;

	// Use this for initialization
	void Start () 
	{
		startFunc();
		enemies = new List<Transform>();
		player = UnitManager.Instance.getPlayer().transform;
	}

	private void UpdateEnemies()
	{
		List<GameObject> temp =  UnitManager.Instance.getEnemies();
		for(int i = 0; i < temp.Count; i++)
		{
			float distance = (temp[i].transform.position - transform.position).magnitude;

			if (distance <  visionDistance)
			{
				enemies.Add(temp[i].transform);
			}
		}
	}

	public void GiveCommand(string s, Vector2 pos)
	{
		currentCommand = s;
		commandLocation = pos;
		commandPresent = true;
	}

	private bool tryAttack()
	{
		//if all empty then return flase
		if (!sensorScripts[0].isColliding && !sensorScripts[1].isColliding && !sensorScripts[2].isColliding && !sensorScripts[3].isColliding)
		{
			return false;
		}

		for(int i = 0; i < enemies.Count; i++)
		{
			if (sensorScripts[0].targets.Count > 0 && sensorScripts[0].targets[0].transform == enemies[i])
			{
				enemies[i].gameObject.GetComponent<Unit>().takeDamage(attackPower);
				return true;
			}
			else if (sensorScripts[1].targets.Count > 0 && sensorScripts[1].targets[0].transform == enemies[i])
			{
				enemies[i].gameObject.GetComponent<Unit>().takeDamage(attackPower);
				return true;
			}
			else if (sensorScripts[2].targets.Count > 0 && sensorScripts[2].targets[0].transform == enemies[i])
			{
				enemies[i].gameObject.GetComponent<Unit>().takeDamage(attackPower);
				return true;
			}
			else if (sensorScripts[3].targets.Count > 0 && sensorScripts[3].targets[0].transform == enemies[i])
			{
				enemies[i].gameObject.GetComponent<Unit>().takeDamage(attackPower);
				return true;
			}
		}
		return false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		updateLoop();

		//move this to an optimized place
		UpdateEnemies();

		if(!commandPresent)
		{
			switch((int)currentState)
			{
			case (int)State.aggressive:
				if(tryAttack())
				{}
				if(enemies.Count > 0)
				{
					changeDirFacing(enemies[0].transform.position);
					move ();
				}
				else
				{
					changeDir(Random.Range(0, 4));
					move ();
					//ranom direction?
					//maybe moves away from player untill a ceritan range
				}
				break;

			case (int)State.defensive:
				if( Mathf.Abs((player.transform.position - transform.position).magnitude) > followDistance)
				{
					changeDirFacing(player.position);
					move ();
				}
				else if(tryAttack())
				{}
				else
				{

				}
				break;

			case (int)State.passive:
				//does nothing
				//maybe move away from enemy 
				//for some reason this makes thewolf run away
				if( (enemies[0].position - transform.position).magnitude < followDistance)
				{
					changeDirAway(enemies[0].position);
					move ();
				}
				break;

			case (int)State.passiveAggressive:
				if(tryAttack())
				{}
				break;
			}
		}
		else if(commandPresent)
		{
			if(currentCommand == ("move"))
			{
				changeDirFacing(new Vector3(commandLocation.x, commandLocation.y, transform.position.z) );
				move();
			}
			else if(currentCommand == ("special"))
			{
				Debug.Log("do shit bb S");
			}
			else if(currentCommand == ("stay"))
			{
				//yeah does nothing I guess
				//maybe add in to try attack here
			}
			else if(currentCommand == ("attack"))
			{
				Debug.Log("do shit bb A");
			}
		}
	}

}
