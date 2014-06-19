using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject target;
	private float cameraSpeed = 1f;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.LeftAlt))
		{
			if(Input.mousePosition.x < 3)
			{
				Vector3 temp = transform.position;
				temp.z -= 1;
				temp.x -= cameraSpeed;
				transform.position = temp;
			}
			else if(Input.mousePosition.y < 3)
			{
				Vector3 temp = transform.position;
				temp.z -= 1;
				temp.y -= cameraSpeed;
				transform.position = temp;
			}
			else if(Input.mousePosition.x > Screen.width - 3)
			{
				Vector3 temp = transform.position;
				temp.z -= 1;
				temp.x += cameraSpeed;
				transform.position = temp;
			}
			else if(Input.mousePosition.y > Screen.height - 3)
			{
				Vector3 temp = transform.position;
				temp.z -= 1;
				temp.y += cameraSpeed;
				transform.position = temp;
			}
			else if ( transform.position.y != Mathf.Round(transform.position.y) || transform.position.x != Mathf.Round(transform.position.x))
			{
				//moving = false;
				Vector3 temp = transform.position;
				temp.y = Mathf.Round(temp.y);
				temp.x = Mathf.Round(temp.x);
				transform.position = temp;
			}
		}
		else
		{
			Vector3 temp = target.transform.position;
			temp.z -= 1;
			transform.position = temp;
		}



	}
}
