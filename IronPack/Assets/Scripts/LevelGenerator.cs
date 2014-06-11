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
		public Sprite blank;
	}

	[System.Serializable]
	private class Room
	{
		public Vector2 pos; 
		public Vector2 size;
		public GameObject container;
		public GameObject[] tiles;
		public SpriteRenderer[] tileRenders;
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

		//loop through all the rooms
		for( int k = 0; k < rooms.Count; k++)//sould make this tile search thing a function(maybe)
		{
			//find the tiles that are the walls and loop through them
			//decide how many doors to be built and randomly place them(this may need to be improved)
			//then in the loop try to place the door in the decided location
			//if there is nothign on the other side of the door then dont place it
			//if there are floor tiles on the other side then place it
			//otherwise try to move the door a better palce.

			//left wall
			//cahnge this to have a door on every wall once appropraite door checks are in place
			//int numDoors = Random.Range(0, (Mathf.CeilToInt(rooms[k].size.y * 0.1)) ); // WHY ERRORS

			int doorPlace  = Random.Range(2, (Mathf.RoundToInt(rooms[k].size.y - 3)) );
			int counter;
			for( int i = 0; i < rooms[k].size.y; i++)
			{
				counter = 0;
				if( i == doorPlace)
				{
					do
					{
						Vector3 tile1Pos = rooms[k].tiles[doorPlace-1].transform.position;
						Vector3 tile2Pos = rooms[k].tiles[doorPlace].transform.position;
						Vector3 tile3Pos = rooms[k].tiles[doorPlace+1].transform.position;
						tile1Pos.x -= 1;
						tile2Pos.x -= 1;
						tile3Pos.x -= 1;

						SpriteRenderer text1 = findTileRender(tile1Pos);
						SpriteRenderer text2 = findTileRender(tile2Pos);
						SpriteRenderer text3 = findTileRender(tile3Pos);

						if(text1 != null && text1.sprite == tiles.floor &&
						   text2 != null && text2.sprite == tiles.floor &&
						   text3 != null && text3.sprite == tiles.floor)
						{
							rooms[k].tileRenders[doorPlace-1].sprite = tiles.door;
							rooms[k].tileRenders[doorPlace].sprite = tiles.door;
							rooms[k].tileRenders[doorPlace+1].sprite = tiles.door;
							//Debug.Log ("door made");
							break;
						}
						else
						{
							if(doorPlace > rooms[k].size.y-4)
							{
								doorPlace = 2;
							}
							doorPlace++;
						}
						counter++;
					}while(counter < rooms[k].size.y - 5);
				}

			}
			//bottom wall
			doorPlace  = Random.Range(2, (Mathf.RoundToInt(rooms[k].size.y - 3)) )* (Mathf.RoundToInt(rooms[k].size.y - 3));
			for( int i = 0; i < rooms[k].tiles.Length; i += Mathf.RoundToInt(rooms[k].size.y))
			{
				counter = 0;
				if( i == doorPlace)
				{
					do
					{
						Vector3 tile1Pos = rooms[k].tiles[doorPlace-Mathf.RoundToInt(rooms[k].size.y)].transform.position;
						Vector3 tile2Pos = rooms[k].tiles[doorPlace].transform.position;
						Vector3 tile3Pos = rooms[k].tiles[doorPlace+Mathf.RoundToInt(rooms[k].size.y)].transform.position;
						tile1Pos.y -= 1;
						tile2Pos.y -= 1;
						tile3Pos.y -= 1;

						SpriteRenderer text1 = findTileRender(tile1Pos);
						SpriteRenderer text2 = findTileRender(tile2Pos);
						SpriteRenderer text3 = findTileRender(tile3Pos);

						if(text1 != null && text1.sprite == tiles.floor &&
						   text2 != null && text2.sprite == tiles.floor &&
						   text3 != null && text3.sprite == tiles.floor)
						{
							rooms[k].tileRenders[doorPlace-Mathf.RoundToInt(rooms[k].size.y)].sprite = tiles.door;
							rooms[k].tileRenders[doorPlace].sprite = tiles.door;
							rooms[k].tileRenders[doorPlace+Mathf.RoundToInt(rooms[k].size.y)].sprite = tiles.door;
							//Debug.Log ("door made");
							break;
						}
						else
						{
							if(doorPlace > (rooms[k].size.y * (rooms[k].size.x - 4)) )
							{
								doorPlace = Mathf.RoundToInt(rooms[k].size.y)* 2;
							}
							doorPlace += Mathf.RoundToInt(rooms[k].size.y);
						}
						counter++;
					}while(counter < rooms[k].size.y - 5);
				}
			}

			//right
			int tempStart = Mathf.RoundToInt((rooms[k].size.y * rooms[k].size.x) - rooms[k].size.y);
			doorPlace  = Random.Range(tempStart + 2, Mathf.RoundToInt(rooms[k].size.y * rooms[k].size.x)-3 );
			for(int i = tempStart; i < rooms[k].tiles.Length; i++)
			{
				counter = 0;
				if( i == doorPlace)
				{
					do
					{
						Vector3 tile1Pos = rooms[k].tiles[doorPlace-1].transform.position;
						Vector3 tile2Pos = rooms[k].tiles[doorPlace].transform.position;
						Vector3 tile3Pos = rooms[k].tiles[doorPlace+1].transform.position;
						tile1Pos.x += 1;
						tile2Pos.x += 1;
						tile3Pos.x += 1;
						
						SpriteRenderer text1 = findTileRender(tile1Pos);
						SpriteRenderer text2 = findTileRender(tile2Pos);
						SpriteRenderer text3 = findTileRender(tile3Pos);
						
						if(text1 != null && text1.sprite == tiles.floor &&
						   text2 != null && text2.sprite == tiles.floor &&
						   text3 != null && text3.sprite == tiles.floor)
						{
							rooms[k].tileRenders[doorPlace-1].sprite = tiles.door;
							rooms[k].tileRenders[doorPlace].sprite = tiles.door;
							rooms[k].tileRenders[doorPlace+1].sprite = tiles.door;
							//Debug.Log ("door made");
							break;
						}
						else
						{
							if(doorPlace > Mathf.RoundToInt(rooms[k].size.y * rooms[k].size.x)-4)
							{
								doorPlace = tempStart + 2;
							}
							doorPlace++;
						}
						counter++;
							}while(counter < Mathf.RoundToInt(rooms[k].size.y * rooms[k].size.x) - 5);
				}
			}

			//top wall
			for( int i = Mathf.RoundToInt(rooms[k].size.y - 1); i < rooms[k].tiles.Length; i += Mathf.RoundToInt(rooms[k].size.y))
			{
				//rooms[k].tileRenders[i].sprite = tiles.door;
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

	private GameObject findTile(Vector3 v)
	{
		for( int i = rooms.Count -1; i >= 0 ; i--)//sould make this tile search thing a function(maybe)
		{
			for(int j = 0; j < rooms[i].tiles.Length; j++)
			{
				if(rooms[i].tiles[j].transform.position == v)
				{
					return rooms[i].tiles[j];
				}
			}
		}
		GameObject ret = new GameObject();
		return ret;
	}

	private SpriteRenderer findTileRender(Vector3 v)
	{
		for( int i = rooms.Count-1; i >= 0 ; i--)//sould make this tile search thing a function(maybe)
		{
			for(int j = 0; j < rooms[i].tiles.Length; j++)
			{
				if(rooms[i].tiles[j].transform.position == v)
				{
					return rooms[i].tileRenders[j];
				}
			}
		}
		return null;
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
		room.tileRenders = new SpriteRenderer[Mathf.RoundToInt(room.size.x * room.size.y)];
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
				room.tileRenders[ Mathf.RoundToInt( (i*room.size.y) + j) ] = tileRend;  
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
				int num = Random.Range(1, 5);
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

					builtRoom = buildRoom(xPos + Random.Range(Mathf.RoundToInt(-buildSize.x + 3), Mathf.RoundToInt(room.size.x) - 3),    -Mathf.RoundToInt(room.pos.y + buildSize.y - yPos - 1), buildSize);
					break;
				case 4:
					buildSize = new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2)));
					
					builtRoom = buildRoom(-Mathf.RoundToInt(room.pos.x + buildSize.x - xPos - 1), yPos + Random.Range(Mathf.RoundToInt(-buildSize.y + 3), Mathf.RoundToInt(room.size.y) - 3), buildSize);
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
