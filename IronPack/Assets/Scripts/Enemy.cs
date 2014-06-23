/* to do
 * make enemy target player first when attacking
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Unit {

	private List<Transform> enemies;

	// Use this for initialization
	void Start () 
	{
		startFunc();
		enemies = new List<Transform>();
	}

	private void UpdateEnemies()
	{
		List<GameObject> temp =  UnitManager.Instance.getWolves();
		temp.Add(UnitManager.Instance.getPlayer());
		temp.Reverse();//puts player first to try and go for player first, not sure if this works as intended.

		for(int i = 0; i < temp.Count; i++)
		{
			float distance = (temp[i].transform.position - transform.position).magnitude;
			
			if (distance <  visionDistance)
			{
				enemies.Add(temp[i].transform);
			}
		}
	}

	private bool TryAttack() //maybe merge this in to attack too to make it more effecnient 
	{
		//make is focus player
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

		if(!TryAttack())
		{
			if(enemies.Count > 0)
			{
				//move to enemy
				changeDirFacing(enemies[0].transform.position);
				move ();
			}
			else
			{
				//random movment
				changeDir(Random.Range(0, 4));
				move ();
			}

		}

	}
}
