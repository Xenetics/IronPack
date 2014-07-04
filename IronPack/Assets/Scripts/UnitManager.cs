using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour 
{
	//singleton class stuff
	public static UnitManager Instance { get { return instance; } }
	private static UnitManager instance = null;

	//public's
	public GameObject playerPrefab;
	public GameObject grayWolfPrefab;
	public GameObject enemyPrefab;
	public int maxWolves = 8;

	//privates		heh
	private List<GameObject> enemies ;
	private GameObject player;
	private List<GameObject> wolves;

	public List<GameObject> getEnemies()
	{
		return enemies;
	}

	public List<GameObject> getWolves()
	{
		return wolves;
	}
	public GameObject BuildWolf()
	{
		if( wolves.Count < maxWolves)
		{
			Vector3 spawnPos = player.transform.position;
			spawnPos.x += 1f;
			GameObject ret = Instantiate(grayWolfPrefab, spawnPos, Quaternion.identity) as GameObject;
			wolves.Add(ret); 
			return ret;
		}
		return null;
	}

	public void ClearAllSensors()
	{
		player.GetComponent<Unit> ().ClearSensors ();
		for (int i = 0; i < enemies.Count; i ++)
		{
			enemies[i].GetComponent<Unit> ().ClearSensors ();
		}

		for (int i = 0; i < wolves.Count; i ++)
		{
			wolves[i].GetComponent<Unit> ().ClearSensors ();
		}

	}

	public GameObject getPlayer()
	{
		return player;
	}

	public GameObject SpawnPlayer(Vector3 spawnPos)
	{
		if(player == null)
		{
			player = Instantiate(playerPrefab, spawnPos, Quaternion.identity) as GameObject;

			return player;
		}
		return null;
	}

	public GameObject SpawnPlayer()
	{
		Vector3 spawnPos = new Vector3(3f, 3f, -2f);
		if(player == null)
		{
			player = Instantiate(playerPrefab, spawnPos, Quaternion.identity) as GameObject;
			
			return player;
		}
		return null;
	}

	void Awake()
	{
		//more singleton class stuff
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;        
		} 
		else 
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	void Start () 
	{
		//initialization
		wolves = new List<GameObject>();
		enemies = new List<GameObject>();
	}

	public GameObject SpawnEnemy()
	{
		Vector3 spawnPos = LevelGenerator.Instance.getRandomPosition();
		spawnPos.z = -2f;
		GameObject ret = Instantiate(enemyPrefab, spawnPos, Quaternion.identity) as GameObject;
		enemies.Add(ret); 
		return ret;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E))
		{
			SpawnEnemy();
		}
	}
}
