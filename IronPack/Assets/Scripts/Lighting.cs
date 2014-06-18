using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lighting : MonoBehaviour 
{
	private enum dir {UP, RIGHT, DOWN, LEFT};

	public Sprite mask;

	public LayerMask ignoreLayers;

	public GameObject target;
	public int facing;

	private Vector2 maskSize;
	private int vision;

	private GameObject[,] maskTiles;

	// Use this for initialization
	void Start () 
	{
		//look at using draw.texture to do the texture rather then sprite shit
		maskSize = new Vector2(Mathf.Ceil(Screen.width / 64) + 4, Mathf.Ceil(Screen.height/64) + 3);
		facing = target.GetComponent<Unit>().getFacing();
		vision = target.GetComponent<Unit>().visionDistance;

		ignoreLayers = ~ignoreLayers;

		maskTiles = new GameObject[Mathf.RoundToInt(maskSize.x),Mathf.RoundToInt(maskSize.y)];

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
				//tile.SetActive(false);
				maskTiles[i,j] = tile;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if(target.transform.position.y == Mathf.Round(target.transform.position.y) && target.transform.position.x == Mathf.Round(target.transform.position.x))
		{
			resetVision();
		}

		facing = target.GetComponent<Unit>().getFacing();
		vision = target.GetComponent<Unit>().visionDistance;

		ShowTile(new Vector2(target.transform.position.x, target.transform.position.y));
		//the vision value is the distance they can see, if 45 degree angle then the number of back tiles is ((vision * 2)- 1)
		//raycast out from the player straight and also from the player to each block along the back of the vision distance

		int numRays = (vision* 2) + 1 ;
		Vector2 rayStart = target.transform.position;
		Vector2 rayEnd = rayStart;
		Vector2 rayAdd = new Vector2();

		List<RaycastHit2D> hitList = new List<RaycastHit2D>();
		switch(facing)
		{
		case (int)dir.UP:
			rayEnd.y += vision;
			rayEnd.x -= vision;
			rayAdd.x = 1f;
			break;
		case (int)dir.RIGHT:
			rayEnd.y += vision;
			rayEnd.x += vision;
			rayAdd.y = -1f;
			break;
		case (int)dir.DOWN:
			rayEnd.y -= vision;
			rayEnd.x += vision;
			rayAdd.x = -1f;
			break;
		case (int)dir.LEFT:
			rayEnd.y -= vision;
			rayEnd.x -= vision;
			rayAdd.y = 1f;
			break;
		}
		for ( int i = 0; i < numRays; i++)
		{
			hitList.Add(Physics2D.Raycast(rayStart, rayEnd, (rayEnd - rayStart).magnitude, ignoreLayers));
			Debug.DrawLine(new Vector3(rayStart.x, rayStart.y, transform.position.z), new Vector3(rayEnd.x, rayEnd.y, transform.position.z), Color.red);

			if ( hitList[i].collider == null)
			{
				bool done = false;
				Vector2 increment = (rayEnd - rayStart).normalized;
				Vector2 curentPos = rayStart;
				do
				{
					curentPos += increment;
					ShowTile(curentPos);
					if (curentPos.x > rayEnd.x - 0.05)
					{
						done =true;
					}
				}while(!done);
			}
			rayEnd += rayAdd;
		}
	}

	private void ShowTile(int posX, int posY)
	{
		Vector2 pos = new Vector2( posX, posY);
		for(int i = 0; i < maskSize.x; i++)
		{
			for(int j = 0; j < maskSize.y; j++)
			{
				Vector2 maskPos = maskTiles[i,j].transform.position;
				if( pos == maskPos)
				{
					maskTiles[i,j].SetActive(false);
				}
			}

		}
	}

	private void resetVision()
	{
		for(int i = 0; i < maskSize.x; i++)
		{
			for(int j = 0; j < maskSize.y; j++)
			{
					maskTiles[i,j].SetActive(true);				
			}
		}
	}

	private void ShowTile(Vector2 input)
	{
		Vector2 pos = new Vector2(Mathf.Round(input.x), Mathf.Round(input.y));
		for(int i = 0; i < maskSize.x; i++)
		{
			for(int j = 0; j < maskSize.y; j++)
			{
				Vector2 maskPos = maskTiles[i,j].transform.position;
				if( pos == maskPos)
				{
					maskTiles[i,j].SetActive(false);
				}
			}
			
		}
	}


	void LateUpdate ()
	{

	}
	
}
