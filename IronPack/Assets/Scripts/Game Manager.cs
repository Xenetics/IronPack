/*
 * Game Manager for Iron Pack
 * Author: Alexander Burton
 * Date: June 4th 2014
 */

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager Instance {get {return Instance;} }
	public static GameManager instance = null;


	 void Awake()
	{
		if ( instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
