using UnityEngine;
using System.Collections;

public class Player : Unit{

	// Use this for initialization
	void Start () {
		startFunc();
	}
	
	// Update is called once per frame
	void Update () {
		updateLoop();

		if(Input.GetKey(KeyCode.W))
		{
			changeDir((int) dir.UP);
			move ();
			Debug.Log ("keyhit");
		}
		else if(Input.GetKey(KeyCode.D))
		{
			changeDir((int) dir.RIGHT);
			move ();
		}
		else if(Input.GetKey(KeyCode.S))
		{
			changeDir((int) dir.DOWN);
			move ();
		}
		else if(Input.GetKey(KeyCode.A))
		{
			changeDir((int) dir.LEFT);
			move ();
		}
	}
}
