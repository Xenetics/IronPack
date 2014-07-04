/* remove Debug stuff(when done)
 * 
 * room gen needs to be less liner(no clue)
 * needs doors 
 * needs auto enemy spawning
 * and other item spawning
 * 
 * make the floor collider box 1 larger in all directions(right now there is the possiblity of a one block overlap)
 * 
 * name all the game opjects made through script
 * 
 * right side door does not do the right number of checks
 * 
 * left side door starts 1 too high from the bottom after the wrap around from the top
 * 
 * change door placemnts to use raycasts if possible(remeber you can get all the things the ray hit)
 * 		maybe for this remove all of the tiles the ray hits (if its a valid door placment to check for this as long as the ray hits 6 walls then your good to place a door)
 * 		then place new tiles in the same places(could maybe just place 6? or even not remove the other tiles and replace them all to doors) 
 * 		then i need to write some sort of door script that opens and closes
 * 
 * sometimes top/bottom rooms will generate in the wrong place in the horizontal direction(maybe I fixed this?)
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {
	public static LevelGenerator Instance { get { return instance; } }
	private static LevelGenerator instance = null;

	[System.Serializable]
	public class Tiles
	{
		public Sprite wall;
		public Sprite floor;
		public Sprite doorOpen;
		public Sprite doorClose;
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
	public GameObject world;

	//private vars
	private List<Room> rooms = new List<Room>(); 

	private List<Vector3[]> debugRays = new List<Vector3[]>();

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

	// Use this for initialization
	void Start () 
	{
		//size of room can be decided before room is made 
		//this will allow rooms to be made in all directions
		world = new GameObject ("World");
		buildRoom(0, 0, new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2))));
		updateRoomPos();

		//loop through all the rooms
		for( int k = 0; k < rooms.Count; k++)
		{
			//find the tiles that are the walls and loop through them
			//decide how many doors to be built and randomly place them(this may need to be improved)
			//then in the loop try to place the door in the decided location
			//if there is nothign on the other side of the door then dont place it
			//if there are floor tiles on the other side then place it
			//otherwise try to move the door a better palce.


			//cahnge this to have a door on every wall once appropraite door checks are in place
			//int numDoors = Random.Range(0, (Mathf.CeilToInt(rooms[k].size.y * 0.1)) ); // WHY ERRORS


			//left wall
			int intDoorPlace  = Random.Range(2, (Mathf.RoundToInt(rooms[k].size.y - 3)) );
			int doorPlace = intDoorPlace;
			int counter;
			for( int i = 0; i < rooms[k].size.y; i++)//can maybe get rid of this loop? or something
			{

				counter = 0;
				if( i == intDoorPlace)
				{
					do
					{
						Vector3 tile1Pos = rooms[k].tiles[doorPlace-1].transform.position;
						Vector3 tile3Pos = rooms[k].tiles[doorPlace+1].transform.position;

						debugRays.Add(new Vector3[]{tile1Pos, tile3Pos} );
						RaycastHit2D[] result = Physics2D.LinecastAll(tile1Pos, tile3Pos); 
						if(result.Length >= 6)
						{
							BuildDoor(result, rooms[k].tiles[doorPlace].transform.position, false);
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
					}while(counter < rooms[k].size.y - 4);
				}
			}

			//bottom wall
			intDoorPlace  = Random.Range(2, (Mathf.RoundToInt(rooms[k].size.x - 3)) )* (Mathf.RoundToInt(rooms[k].size.y - 3));
			doorPlace = intDoorPlace;
			for( int i = 0; i < rooms[k].tiles.Length; i += Mathf.RoundToInt(rooms[k].size.y))
			{
				counter = 0;
				if( i == intDoorPlace)
				{
					do
					{
						Vector3 tile1Pos = rooms[k].tiles[doorPlace-Mathf.RoundToInt(rooms[k].size.y)].transform.position;
						Vector3 tile3Pos = rooms[k].tiles[doorPlace+Mathf.RoundToInt(rooms[k].size.y)].transform.position;

						debugRays.Add(new Vector3[]{tile1Pos, tile3Pos} );
						RaycastHit2D[] result = Physics2D.LinecastAll(tile1Pos, tile3Pos); 
						if(result.Length >= 6)
						{
							BuildDoor(result, rooms[k].tiles[doorPlace].transform.position, true);
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
					}while(counter < rooms[k].size.x - 4);
				}
			}

			//right not right at all <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
			int tempStart = Mathf.RoundToInt((rooms[k].size.y * rooms[k].size.x) - rooms[k].size.y);
			intDoorPlace  = Random.Range(tempStart + 2, Mathf.RoundToInt(rooms[k].size.y * rooms[k].size.x)-3 );
			doorPlace = intDoorPlace;
			for(int i = tempStart; i < rooms[k].tiles.Length; i++)//make these do whiles?
			{

				counter = Mathf.RoundToInt(rooms[k].tiles.Length - rooms[k].size.y);
				if( i == intDoorPlace)
				{
					do
					{
						Vector3 tile1Pos = rooms[k].tiles[doorPlace-1].transform.position;
						Vector3 tile3Pos = rooms[k].tiles[doorPlace+1].transform.position;

						debugRays.Add(new Vector3[]{tile1Pos, tile3Pos} );
						RaycastHit2D[] result = Physics2D.LinecastAll(tile1Pos, tile3Pos); 
						if(result.Length >= 6)
						{
							BuildDoor(result, rooms[k].tiles[doorPlace].transform.position, false);
							break;
						}
						else
						{
							if(doorPlace > Mathf.RoundToInt(rooms[k].size.y * rooms[k].size.x)-4)
							{
								doorPlace = tempStart + 3;
							}
							doorPlace++;
						}
						counter++;
					}while(counter <rooms[k].size.y - 4);
				}
			}

			//top wall
			intDoorPlace = Mathf.RoundToInt(rooms[k].size.y - 1)  +  ( Random.Range(2, Mathf.RoundToInt(rooms[k].size.x - 3) ) * (Mathf.RoundToInt(rooms[k].size.y)) );
			doorPlace = intDoorPlace;
			for( int i = Mathf.RoundToInt(rooms[k].size.y - 1); i < rooms[k].tiles.Length; i += Mathf.RoundToInt(rooms[k].size.y))
			{
				counter = 0;
				if( i == doorPlace)
				{
					do
					{
						Vector3 tile1Pos = rooms[k].tiles[doorPlace-Mathf.RoundToInt(rooms[k].size.y)].transform.position;
						Vector3 tile3Pos = rooms[k].tiles[doorPlace+Mathf.RoundToInt(rooms[k].size.y)].transform.position;

						debugRays.Add(new Vector3[]{tile1Pos, tile3Pos} );
						RaycastHit2D[] result = Physics2D.LinecastAll(tile1Pos, tile3Pos); 
						if(result.Length >= 6)
						{
							BuildDoor(result, rooms[k].tiles[doorPlace].transform.position, true);
							break;
						}
						else
						{
							if(doorPlace > (rooms[k].tiles.Length - (rooms[k].size.y * 3)) )
							{
								doorPlace = Mathf.RoundToInt(rooms[k].size.y - 1);
								doorPlace += Mathf.RoundToInt(rooms[k].size.y);
							}
							doorPlace += Mathf.RoundToInt(rooms[k].size.y);
						}
						counter++;
					}while(counter < rooms[k].size.x - 4);
				}
			}
		}

		//all doors will be open by default so that they dont get triggered by raycasts and then call a function here to close all doors
		//close them doors BB
	}

	private void BuildDoor(RaycastHit2D[] result, Vector2 pos, bool horizontal)
	{
		GameObject door = new GameObject();
		door.name = "Door";
		Door doorScript = door.AddComponent("Door") as Door;
		doorScript.doorClosed = tiles.doorClose;
		doorScript.doorOpen = tiles.doorOpen;
		doorScript.takeRaycastHit2D(result);
		for( int l = 0; l < result.Length; l++)
		{
			//result[l].collider.gameObject.GetComponent<SpriteRenderer>().sprite = tiles.door;
			result[l].collider.gameObject.transform.parent = door.transform;
		}
		/*
		BoxCollider2D sensor = door.AddComponent ("BoxCollider2D");
		if (horizontal)
			sensor.size = new Vector2 (3, 1);
		else
			sensor.size = new Vector2 (1, 3);
		sensor.center = pos;
		sensor.isTrigger = true;
		*/


		door.transform.parent = world.transform;
		//doorScript.Close();
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
		for( int i = rooms.Count -1; i >= 0 ; i--)
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
		for( int i = rooms.Count-1; i >= 0 ; i--)
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
		GameObject roomContainer = new GameObject("Room");
		room.container = roomContainer;

		//build room
		room.size = size;
		room.pos = new Vector3(xPos, yPos, -1f);
		roomContainer.transform.position = room.pos;
		room.tiles = new GameObject[Mathf.RoundToInt(room.size.x * room.size.y)];
		room.tileRenders = new SpriteRenderer[Mathf.RoundToInt(room.size.x * room.size.y)];
		for(int i = 0; i < room.size.x; i++)
		{
			for(int j = 0; j < room.size.y; j++)
			{
				//make tile and move it in to place
				GameObject tile = new GameObject("Tile " + i + " " + j );
				tile.transform.parent = room.container.transform;
				tile.transform.position = new Vector3( xPos + i, yPos + j, -1);

				//make the sprite for the tile
				SpriteRenderer tileRend;
				tileRend = tile.AddComponent("SpriteRenderer") as SpriteRenderer;

				//picks what sprite to use for the tile
				//this if determans if its a wall tile
				//this can be split up to find each wall if needed(for direction problems
				if(i == 0 || j == 0 || i == (room.size.x -1) || j == (room.size.y - 1))
				{
					tileRend.sprite = tiles.wall;
					BoxCollider2D tileCol;
					tileCol = tile.AddComponent("BoxCollider2D") as BoxCollider2D;
					tileCol.size = new Vector2 ( 0.9f, 0.9f);
				}
				//else its a floor tile
				else
				{
					tileRend.sprite = tiles.floor;
				}

				room.tiles[ Mathf.RoundToInt( (i*room.size.y) + j) ] = tile;  
				room.tileRenders[ Mathf.RoundToInt( (i*room.size.y) + j) ] = tileRend;  
			}
		}



		rooms.Add(room);
		//room.container.transform.position = room.pos;

		//update the position of the rooms
		//updateRoomPos();

		//place collider for room gen checks, this collider can be disabled after room gen?
		BoxCollider2D floorCollider = roomContainer.AddComponent("BoxCollider2D") as BoxCollider2D;
		floorCollider.size = new Vector2(room.size.x - 2, room.size.y - 2);
		floorCollider.center = (new Vector2(room.size.x - 1, room.size.y - 1)) * 0.5f;
		roomContainer.layer = 12;//floor layer

		roomContainer.transform.parent = world.transform;

		//make more rooms
		//needs elegance
		int tries = 0;
		if( rooms.Count != 1)//gives the first go more tries
			tries = -4;

		for( int i = 0; i < 2; i++)
		{
			Room builtRoom;
			Vector2 buildSize;
			float buildPosX;
			float buildPosY;
			Vector3 tile1Pos;
			Vector3 tile2Pos;
			Vector3 tile3Pos;
			Vector3 tile4Pos;

			if(rooms.Count < maxRooms)
			{
				int num = Random.Range(1, 5);
				switch(num)
				{
					//building a top side room
				case 1:
					buildSize = new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2)));
					buildPosX = xPos + Random.Range(Mathf.RoundToInt(-buildSize.x + 3), Mathf.RoundToInt(room.size.x) - 3);
					buildPosY = room.pos.y + room.size.y - 1;

					//raycast setup vectors
					tile1Pos = new Vector3(buildPosX +1, buildPosY + 1, -2.5f);
					tile2Pos = new Vector3(buildPosX +1, buildPosY + buildSize.y - 2, -2.5f);
					tile3Pos = new Vector3(buildPosX + buildSize.x -2, buildPosY + 1, -2.5f);
					tile4Pos = new Vector3(buildPosX + buildSize.x -2, buildPosY + buildSize.y - 2, -2.5f);
					
//					debugRays.Add(new Vector3[]{tile1Pos, tile2Pos} );
//					debugRays.Add(new Vector3[]{tile2Pos, tile3Pos} );
//					debugRays.Add(new Vector3[]{tile3Pos, tile4Pos} );
//					debugRays.Add(new Vector3[]{tile4Pos, tile1Pos} );
//					debugRays.Add(new Vector3[]{tile1Pos, tile3Pos} );
//					debugRays.Add(new Vector3[]{tile2Pos, tile4Pos} );

					//raycasts a box with an x throught it to try and see if it can place the room there
					if(!Physics2D.Raycast(tile1Pos, tile2Pos, ( tile1Pos - tile2Pos).magnitude )&&
					   !Physics2D.Raycast(tile2Pos, tile3Pos, ( tile2Pos - tile3Pos).magnitude )&&
					   !Physics2D.Raycast(tile3Pos, tile4Pos, ( tile3Pos - tile4Pos).magnitude )&&
					   !Physics2D.Raycast(tile4Pos, tile1Pos, ( tile4Pos - tile1Pos).magnitude )&&
					   !Physics2D.Raycast(tile1Pos, tile3Pos, ( tile1Pos - tile3Pos).magnitude )&&
					   !Physics2D.Raycast(tile2Pos, tile4Pos, ( tile2Pos - tile4Pos).magnitude )  )
					{
						builtRoom = buildRoom(Mathf.RoundToInt(buildPosX), Mathf.RoundToInt(buildPosY), buildSize);
						//Debug.Log("top side build");
					}
					else if(tries < 5)
					{
						i--;
						tries++;
					}
					break;

					//building a right side room
				case 2:
					buildSize = new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2)));
					buildPosX = room.pos.x + room.size.x - 1;
					buildPosY = yPos + Random.Range(Mathf.RoundToInt(-buildSize.y + 3), Mathf.RoundToInt(room.size.y) - 3);
					
					//raycast setup vectors
					tile1Pos = new Vector3(buildPosX +1, buildPosY + 1, -2.5f);
					tile2Pos = new Vector3(buildPosX +1, buildPosY + buildSize.y - 2, -2.5f);
					tile3Pos = new Vector3(buildPosX + buildSize.x -2, buildPosY + 1, -2.5f);
					tile4Pos = new Vector3(buildPosX + buildSize.x -2, buildPosY + buildSize.y - 2, -2.5f);

//					debugRays.Add(new Vector3[]{tile1Pos, tile2Pos} );
//					debugRays.Add(new Vector3[]{tile2Pos, tile3Pos} );
//					debugRays.Add(new Vector3[]{tile3Pos, tile4Pos} );
//					debugRays.Add(new Vector3[]{tile4Pos, tile1Pos} );
//					debugRays.Add(new Vector3[]{tile1Pos, tile3Pos} );
//					debugRays.Add(new Vector3[]{tile2Pos, tile4Pos} );

					//raycasts a box with an x throught it to try and see if it can place the room there
					if(!Physics2D.Raycast(tile1Pos, tile2Pos, ( tile1Pos - tile2Pos).magnitude )&&
					   !Physics2D.Raycast(tile2Pos, tile3Pos, ( tile2Pos - tile3Pos).magnitude )&&
					   !Physics2D.Raycast(tile3Pos, tile4Pos, ( tile3Pos - tile4Pos).magnitude )&&
					   !Physics2D.Raycast(tile4Pos, tile1Pos, ( tile4Pos - tile1Pos).magnitude )&&
					   !Physics2D.Raycast(tile1Pos, tile3Pos, ( tile1Pos - tile3Pos).magnitude )&&
					   !Physics2D.Raycast(tile2Pos, tile4Pos, ( tile2Pos - tile4Pos).magnitude )  )
					{
						builtRoom = buildRoom(Mathf.RoundToInt(buildPosX), Mathf.RoundToInt(buildPosY), buildSize);
						//Debug.Log("right side build");
					}
					else if (tries < 5)
					{
						i--;
						tries++;
					}
					break;

					//building a bottom room
				case 3:
					buildSize = new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2)));
					buildPosX = room.pos.x + Random.Range(Mathf.RoundToInt(-buildSize.x + 3), Mathf.RoundToInt(room.size.x) - 3);
					buildPosY = room.pos.y - buildSize.y + 1;
					
					//raycast setup vectors
					tile1Pos = new Vector3(buildPosX +1, buildPosY + 1, -2.5f);
					tile2Pos = new Vector3(buildPosX +1, buildPosY + buildSize.y - 2, -2.5f);
					tile3Pos = new Vector3(buildPosX + buildSize.x -2, buildPosY + 1, -2.5f);
					tile4Pos = new Vector3(buildPosX + buildSize.x -2, buildPosY + buildSize.y - 2, -2.5f);
					
//					debugRays.Add(new Vector3[]{tile1Pos, tile2Pos} );
//					debugRays.Add(new Vector3[]{tile2Pos, tile3Pos} );
//					debugRays.Add(new Vector3[]{tile3Pos, tile4Pos} );
//					debugRays.Add(new Vector3[]{tile4Pos, tile1Pos} );
//					debugRays.Add(new Vector3[]{tile1Pos, tile3Pos} );
//					debugRays.Add(new Vector3[]{tile2Pos, tile4Pos} );

					//raycasts a box with an x throught it to try and see if it can place the room there
					if(!Physics2D.Raycast(tile1Pos, tile2Pos, ( tile1Pos - tile2Pos).magnitude )&&
					   !Physics2D.Raycast(tile2Pos, tile3Pos, ( tile2Pos - tile3Pos).magnitude )&&
					   !Physics2D.Raycast(tile3Pos, tile4Pos, ( tile3Pos - tile4Pos).magnitude )&&
					   !Physics2D.Raycast(tile4Pos, tile1Pos, ( tile4Pos - tile1Pos).magnitude )&&
					   !Physics2D.Raycast(tile1Pos, tile3Pos, ( tile1Pos - tile3Pos).magnitude )&&
					   !Physics2D.Raycast(tile2Pos, tile4Pos, ( tile2Pos - tile4Pos).magnitude )  )
					{
						builtRoom = buildRoom(Mathf.RoundToInt(buildPosX), Mathf.RoundToInt(buildPosY), buildSize);
						//Debug.Log("bottom side build");
					}
					else if(tries < 5)
					{
						i--;
						tries++;
					}
					break;

					//left side room
				case 4:
					buildSize = new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2)));
					buildPosX = room.pos.x - buildSize.x  + 1;
					buildPosY = room.pos.y + Random.Range(Mathf.RoundToInt(-buildSize.y + 3), Mathf.RoundToInt(room.size.y) - 3);
					
					//raycast setup vectors
					tile1Pos = new Vector3(buildPosX +1, buildPosY + 1, -2.5f);
					tile2Pos = new Vector3(buildPosX +1, buildPosY + buildSize.y - 2, -2.5f);
					tile3Pos = new Vector3(buildPosX + buildSize.x -2, buildPosY + 1, -2.5f);
					tile4Pos = new Vector3(buildPosX + buildSize.x -2, buildPosY + buildSize.y - 2, -2.5f);
					
//					debugRays.Add(new Vector3[]{tile1Pos, tile2Pos} );
//					debugRays.Add(new Vector3[]{tile2Pos, tile3Pos} );
//					debugRays.Add(new Vector3[]{tile3Pos, tile4Pos} );
//					debugRays.Add(new Vector3[]{tile4Pos, tile1Pos} );
//					debugRays.Add(new Vector3[]{tile1Pos, tile3Pos} );
//					debugRays.Add(new Vector3[]{tile2Pos, tile4Pos} );

					//raycasts a box with an x throught it to try and see if it can place the room there
					if(!Physics2D.Raycast(tile1Pos, tile2Pos, ( tile1Pos - tile2Pos).magnitude )&&
					   !Physics2D.Raycast(tile2Pos, tile3Pos, ( tile2Pos - tile3Pos).magnitude )&&
					   !Physics2D.Raycast(tile3Pos, tile4Pos, ( tile3Pos - tile4Pos).magnitude )&&
					   !Physics2D.Raycast(tile4Pos, tile1Pos, ( tile4Pos - tile1Pos).magnitude )&&
					   !Physics2D.Raycast(tile1Pos, tile3Pos, ( tile1Pos - tile3Pos).magnitude )&&
					   !Physics2D.Raycast(tile2Pos, tile4Pos, ( tile2Pos - tile4Pos).magnitude )  )
					{
						builtRoom = buildRoom(Mathf.RoundToInt(buildPosX), Mathf.RoundToInt(buildPosY), buildSize);
						//Debug.Log("left side build");
					}
					else if(tries < 5)
					{
						tries++;
						i--;
					}
					break;
				}
			}
		}

		return room;
	}

	private void DrawDebugRays()
	{
		for( int i = 0; i < debugRays.Count; i++)
		{
			Vector3 rayStart= (Vector3)(debugRays[i].GetValue(0));
			Vector3 rayEnd = (Vector3) (debugRays[i].GetValue(1));
			rayStart.z += i * 0.1f;
			rayEnd.z += i * 0.1f;
			Debug.DrawLine(rayStart, rayEnd , Color.red) ;
		}
	}

	void Update()
	{
		//DrawDebugRays();
	}
	
	public Vector3 getRandomPosition()
	{
		Vector3 ret;
		int room;
		int tile;
		do
		{
			room = Random.Range(0, rooms.Count);
			
			tile = Random.Range(0, rooms[room].tiles.Length);
			ret = (rooms[room].tiles[tile].transform.position);
			
		}while(ret.x == rooms[room].pos.x ||
		       ret.y == rooms[room].pos.y ||
		       ret.x == rooms[room].pos.x + rooms[room].size.x -1 ||
		       ret.y == rooms[room].pos.y + rooms[room].size.y -1 );
		return ret;
	}
}
