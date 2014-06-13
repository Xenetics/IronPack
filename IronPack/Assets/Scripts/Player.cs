﻿using UnityEngine;
using System.Collections;

public class Player : Unit{

	// Use this for initialization
	void Start () {
		startFunc();
	}
	
	// Update is called once per frame
	void Update () {
		updateLoop();

		if(Input.GetKeyDown(KeyCode.W))
		{
			changeDir((int) dir.UP);
		}
		else if(Input.GetKeyDown(KeyCode.D))
		{
			changeDir((int) dir.RIGHT);
		}
		else if(Input.GetKeyDown(KeyCode.S))
		{
			changeDir((int) dir.DOWN);
		}
		else if(Input.GetKeyDown(KeyCode.A))
		{
			changeDir((int) dir.LEFT);
		}


		if(Input.GetKey(KeyCode.W))
		{
			move ();
		}
		else if(Input.GetKey(KeyCode.D))
		{
			move ();
		}
		else if(Input.GetKey(KeyCode.S))
		{
			move ();
		}
		else if(Input.GetKey(KeyCode.A))
		{
			move ();
		}
	}
}
