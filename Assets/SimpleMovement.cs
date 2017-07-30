using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour {

	public float moveSpeed;
	public float rotationSpeed;
	public Transform Head;
	public Transform Body;

	private Vector3 mousePosition;
	private Vector3 jumpLocation;
	private bool isJumping;

	// Use this for initialization
	void Start () {
		Head = transform.FindChild ("Head");
		Body = transform.FindChild ("Body");
	}
		
	// Update is called once per frame
	void Update () {

		//move with wasd
		var xDirection = Input.GetAxis ("Vertical") * Time.deltaTime * moveSpeed * Vector3.forward;
		var zDirection = Input.GetAxis ("Horizontal") * Time.deltaTime * moveSpeed * Vector3.left;
					
		transform.position += xDirection;
		transform.position -= zDirection;

		//rotate to face mouse
		var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		Debug.DrawRay (ray.origin, ray.direction);
		RaycastHit hit;
		Vector3 lookPos = transform.position + transform.forward;
		if (Physics.Raycast (ray, out hit)) {
			lookPos = hit.point;
			lookPos.y = transform.position.y;
			Debug.DrawLine (transform.position, lookPos);
		}

		//only rotate if not pointing direction at self, rotate over time with lerp on rotationSpeed
		var vector = lookPos - transform.position;
		if(vector.magnitude > 1){
			Quaternion lookDir = Quaternion.LookRotation(vector);
			transform.rotation = Quaternion.Lerp (transform.rotation, lookDir, Time.deltaTime * rotationSpeed);;
		}

		/* JUMP
		if (Input.GetKeyDown(KeyCode.Space) && isJumping != true)
		{
			jumpLocation = transform.position;
			jumpLocation.y = transform.position.y + 2.5f;
			isJumping = true;
		}
		if (isJumping){
			float step = (moveSpeed * 2) * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, jumpLocation, step);
			if(transform.position.Equals(jumpLocation))
			{
				jumpLocation.y = 0;
			}

		}
		if(Mathf.Approximately(transform.position.y, 6)){
			isJumping = false;
		}
		*/
	}
}
