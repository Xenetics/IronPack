using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

	[System.Serializable]
	public class Tiles
	{
		public Sprite wall;
		public Sprite floor;
		public Sprite door;
	}

	[System.Serializable]
	private class Room
	{
		public Vector2 pos; 
		public Vector2 size;
		public GameObject container;
		public GameObject[] tiles;
	}

	enum Direction{NORTH, EAST, SOUTH, WEST};

	//public vars
	public Vector2 minRoomSize = new Vector2(4 , 4);
	public Vector2 maxRoomSize = new Vector2(20 , 20); 
	public int maxRooms = 20;
	public Tiles tiles;

	//private vars
	private List<Room> rooms = new List<Room>();

	// Use this for initialization
	void Start () 
	{
		//size of room can be decided before room is made 
		//this will allow rooms to be made in all directions

		buildRoom(0, 0, new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2))));
		updateRoomPos();

		//search to see if there is another wall tile in the same place
		//if there is make it a door tile
		for( int k = 0; k < rooms.Count; k++)//sould make this tile search thing a function(maybe)
		{
			for(int l = 0; l < rooms[k].tiles.Length; l++)
			{

			}
		}
	}

	private void updateRoomPos()
	{
		for( int i = 0; i < rooms.Count; i++)
		{
			rooms[i].container.transform.position = rooms[i].pos;
		}
	}

	private Room buildRoom(int xPos, int yPos, Vector2 size)
	{
		//set up
		Room room = new Room();
		GameObject roomContainer = new GameObject();
		room.container = roomContainer;

		//build room
		room.size = size;
		room.tiles = new GameObject[Mathf.RoundToInt(room.size.x * room.size.y)];
		for(int i = 0; i < room.size.x; i++)
		{
			for(int j = 0; j < room.size.y; j++)
			{
				//make tile and move it in to place
				GameObject tile = new GameObject();
				tile.transform.position = new Vector3(xPos + i, yPos + j, -1);

				//make the sprite for the tile
				SpriteRenderer tileRend;
				tileRend = tile.AddComponent("SpriteRenderer") as SpriteRenderer;

				//picks what sprite to use for the tile
				//this if determans if its a wall tile
				//this can be split up to find each wall if needed(for direction problems
				if(i == 0 || j == 0 || i == (room.size.x -1) || j == (room.size.y - 1))
				{
					tileRend.sprite = tiles.wall;
				}
				//else its a floor tile
				else
				{
					tileRend.sprite = tiles.floor;
				}
				tile.transform.parent = room.container.transform;
				room.tiles[ Mathf.RoundToInt( (i*room.size.y) + j) ] = tile;  
			}
		}
		rooms.Add(room);

		//make more rooms

		for( int i = 0; i < 2; i++)
		{
			Room builtRoom;
			Vector2 buildSize;
			if(rooms.Count < maxRooms)
			{
				int num = 3;//Random.Range(1, 3);
				switch(num)
				{
					//building a top side room
				case 1:
					buildSize = new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2)));

					builtRoom = buildRoom(xPos + Random.Range(Mathf.RoundToInt(-buildSize.x + 3), Mathf.RoundToInt(room.size.x) - 3), Mathf.RoundToInt(room.pos.y + room.size.y + yPos - 1), buildSize);
					break;

					//building a right side room
				case 2:
					buildSize = new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2)));

					builtRoom = buildRoom(Mathf.RoundToInt(room.pos.x + room.size.x + xPos - 1), yPos + Random.Range(Mathf.RoundToInt(-buildSize.y + 3), Mathf.RoundToInt(room.size.y) - 3), buildSize);
					break;

					//building a bottom room
				case 3:
					buildSize = new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2)));
					//FIXXXXXXXXXXXXXXXXX THIIIIIIIIIIIS SHHHHHHHIIIIIIIIIIIT
					builtRoom = buildRoom(xPos + Random.Range(Mathf.RoundToInt(-buildSize.x + 3), Mathf.RoundToInt(room.size.x) - 3),    -Mathf.RoundToInt(room.pos.y +room.size.y + buildSize.y + yPos - 1), buildSize);
					break;

				}
			}
		}

		return room;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
