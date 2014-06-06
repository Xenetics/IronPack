using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

	[System.Serializable]
	public class Tiles
	{
		public Sprite wall;
		public Sprite floor;
	}

	[System.Serializable]
	private class Room
	{
		public Vector2 pos; 
		public Vector2 size;
		public GameObject container;
		public GameObject[] tiles;
	}

	//public vars
	public Vector2 minRoomSize = new Vector2(4 , 4);
	public Vector2 maxRoomSize = new Vector2(20 , 20); 
	public Tiles tiles;

	//private vars
	private List<Room> rooms = new List<Room>();

	// Use this for initialization
	void Start () {

		rooms.Add(buildRoom());
		rooms[0].pos.x = 1;
		rooms[0].pos.y = 1;
		updateRoomPos();
	}

	private void updateRoomPos()
	{
		for( int i = 0; i < rooms.Count; i++)
		{
			rooms[i].container.transform.position = rooms[i].pos;
		}
	}

	private Room buildRoom()
	{
		//set up
		Room room = new Room();
		GameObject roomContainer = new GameObject();
		room.container = roomContainer;

		//build room
		room.size = new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2)));
		room.tiles = new GameObject[Mathf.RoundToInt(room.size.x * room.size.y)];
		for(int i = 0; i < room.size.x; i++)
		{
			for(int j = 0; j < room.size.y; j++)
			{
				GameObject tile = new GameObject();
				SpriteRenderer tileRend;
				tileRend = tile.AddComponent("SpriteRenderer") as SpriteRenderer;
				if(i == 0 || j == 0 || i == (room.size.x -1) || j == (room.size.y - 1))
				{
					tileRend.sprite = tiles.wall;
				}
				else
				{
					tileRend.sprite = tiles.floor;
				}
				tile.transform.position = new Vector3(i, j, -1);
				tile.transform.parent = room.container.transform;
				room.tiles[ Mathf.RoundToInt( (i*room.size.y) + j) ] = tile;  
			}
		}



		return room;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
