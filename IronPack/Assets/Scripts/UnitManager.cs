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

	//privates		heh
	private List<GameObject> enemys;
	private GameObject player;
	private List<GameObject> wolfs;

	public GameObject SpawnPlayer()
	{
		if(player == null)
		{
			player = Instantiate(playerPrefab, new Vector3(3f, 3f, -2f), Quaternion.identity) as GameObject;

			return player;
		}
		return null;
	}

	void Start () {
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

		//initialization
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}
