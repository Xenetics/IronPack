using UnityEngine;
using System.Collections;

public class Lighting : MonoBehaviour 
{
	private enum dir {UP, RIGHT, DOWN, LEFT};

	public Sprite mask;

	public GameObject target;
	public int facing;

	private Vector2 maskSize;

	private GameObject[] [] maskTiles;

	// Use this for initialization
	void Start () 
	{
		//look at using draw.texture to do the texture rather then sprite shit
		maskSize = new Vector2(Mathf.Ceil(Screen.width / 64) + 4, Mathf.Ceil(Screen.height/64) + 3);
		facing = target.GetComponent<Unit>().getFacing();

		maskTiles = new GameObject[Mathf.RoundToInt(maskSize.x-1)] [Mathf.RoundToInt(maskSize.y-1)];

		for(int i = 0; i < maskSize.x; i++)
		{
			for(int j = 0; j < maskSize.y; j++)
			{
				//make tile and move it in to place
				GameObject tile = new GameObject();
				tile.transform.position = new Vector3(transform.position.x + i - Mathf.Floor(maskSize.x * 0.5f), transform.position.y + j - Mathf.Floor(maskSize.y * 0.5f), transform.position.z + 0.5f);
				
				//make the sprite for the tile
				SpriteRenderer tileRend;
				tileRend = tile.AddComponent("SpriteRenderer") as SpriteRenderer;
				tileRend.sprite = mask;
				tile.transform.parent = transform;
				maskTiles[i][j] = tile;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//the vision value is the distance they can see, if 45 degree angle then the number of back tiles is ((vision * 2)- 1)
		//raycast out from the player straight and also from the player to each block along the back of the vision distance
	}

	void LateUpdate ()
	{

	}
	
}
