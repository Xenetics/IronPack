using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public GameObject target;
	public float cameraSpeed = 0.08f;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Space))
		{
			if(Input.mousePosition.x < 3)
			{
				Vector3 temp = transform.position;
				temp.z -= 1;
				temp.x -= cameraSpeed;
				transform.position = temp;
			}
			if(Input.mousePosition.y < 3)
			{
				Vector3 temp = transform.position;
				temp.z -= 1;
				temp.y -= cameraSpeed;
				transform.position = temp;
			}
			if(Input.mousePosition.x > Screen.width - 3)
			{
				Vector3 temp = transform.position;
				temp.z -= 1;
				temp.x += cameraSpeed;
				transform.position = temp;
			}
			if(Input.mousePosition.y > Screen.height - 3)
			{
				Vector3 temp = transform.position;
				temp.z -= 1;
				temp.y += cameraSpeed;
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
