using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SimpleMovement))]
public class AnimateLegs : MonoBehaviour {
	public float enthusiasm;
	public float timeBetweenSteps;
	private Transform leftArm, rightArm, leftFoot, rightFoot;

	private SimpleMovement _movement;
	// Use this for initialization
	void Start () {
		_movement = GetComponent<SimpleMovement> ();

		leftArm = transform.FindChild ("LeftArm");
		rightArm = transform.FindChild ("RightArm");
		leftFoot = transform.FindChild ("LeftLeg").FindChild("LeftFoot");
		rightFoot = transform.FindChild ("RightLeg").FindChild("RightFoot");

		StartCoroutine (Animate ());
	}
	
	IEnumerator Animate() {
		while (true) {
			leftFoot.GetComponent<Rigidbody> ().AddForce (leftFoot.forward * enthusiasm);
			rightFoot.GetComponent<Rigidbody> ().AddForce (-rightFoot.forward * enthusiasm);
			yield return new WaitForSeconds (timeBetweenSteps);

			rightFoot.GetComponent<Rigidbody> ().AddForce (rightFoot.forward * enthusiasm);
			leftFoot.GetComponent<Rigidbody> ().AddForce (-leftFoot.forward * enthusiasm);	
			yield return new WaitForSeconds (timeBetweenSteps);
		}
	}
}
