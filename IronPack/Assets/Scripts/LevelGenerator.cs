using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

	[System.Serializable]
	public class Tiles
	{
		public Sprite wall;
		public Sprite floor;
	}
	public Vector2 minRoomSize = new Vector2(4 , 4);
	public Vector2 maxRoomSize = new Vector2(20 , 20); 
	public Tiles tiles;
	private GameObject[] rooms;

	// Use this for initialization
	void Start () {
		GameObject room = new GameObject();
		Vector2 roomSize = new Vector2(Random.Range(Mathf.CeilToInt(minRoomSize.x + 2), Mathf.CeilToInt(maxRoomSize.x + 2)), Random.Range(Mathf.CeilToInt(minRoomSize.y + 2), Mathf.CeilToInt(maxRoomSize.y + 2)));

		for(int i = 0; i < roomSize.x; i++)
		{
			for(int j = 0; j < roomSize.y; j++)
			{
				GameObject tile = new GameObject();
				SpriteRenderer tileRend;
				tileRend = tile.AddComponent("SpriteRenderer") as SpriteRenderer;
				if(i == 0 || j == 0 || i == (roomSize.x -1) || j == (roomSize.y - 1))
				{
					tileRend.sprite = tiles.wall;
				}
				else
				{
					tileRend.sprite = tiles.floor;
				}
				tile.transform.position = new Vector3(i, j, -1);
				tile.transform.parent = room.transform;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
