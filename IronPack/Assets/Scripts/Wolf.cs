using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wolf : Unit
{

	public enum State{
		aggressive, defensive, passiveAggressive, passive
	}

	public State currentState;
	public int cutOffDistance = 3; //rename this varable

	private Transform player;

	private List<Transform> enemies;

	// Use this for initialization
	void Start () 
	{
		enemies = new List<Transform>();
		player = UnitManager.Instance.getPlayer().transform;
	}

	private void updateEnemies()
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

	private bool isEnemyInRange() //maybe merge this in to attack too to make it more effecnient 
	{
		//if all empty then return flase
		if (!sensorScripts[0].isColliding && !sensorScripts[1].isColliding && !sensorScripts[2].isColliding && !sensorScripts[3].isColliding)
		{
			return false;
		}

		for(int i = 0; i < enemies.Count; i++)
		{
			if ( sensorScripts[0].targets[0].transform == enemies[i] || sensorScripts[1].targets[1].transform == enemies[i] || sensorScripts[2].targets[2].transform == enemies[i] || sensorScripts[3].targets[3].transform == enemies[i])
			{
				return true;
			}
		}
		return false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(true/* no command present */)
		{
			switch((int)currentState)
			{
			case (int)State.aggressive:
				if(isEnemyInRange())
				{
					//attack enemy
				}
				else if(enemies.Count > 0)
				{
					changeDir(enemies[0].transform.position);
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
				if( (player.transform.position - transform.position).magnitude > cutOffDistance)
				{
					changeDir(player.position);
					move ();
				}
				else if(isEnemyInRange())
				{
					//attack
				}
				else
				{
					//move ahead of player
				}
				break;

			case (int)State.passive:
				//does nothing
				//maybe move away from enemy
				break;

			case (int)State.passiveAggressive:
				if(isEnemyInRange())
				{
					//attack
				}
				break;
			}
		}
	}

}
