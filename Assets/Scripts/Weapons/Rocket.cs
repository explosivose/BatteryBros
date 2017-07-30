using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour {

	public float launchForce;
	public float acceleration;
	public float lifeTime;

	private TrailRenderer _trail;
	private Rigidbody _rigidbody;

	void Awake() {
		_rigidbody = GetComponent<Rigidbody> ();
		_trail = GetComponent<TrailRenderer> ();

	}



	void OnEnable() {
		StartCoroutine (Init ());
	}

	void OnDisable() {
		if (_trail)
			_trail.enabled = false; 
	}

	IEnumerator Init() {
		_rigidbody.isKinematic = false;
		// eliminate any kinetic energy from previous lifetime (after being recycled)
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;


		_rigidbody.AddForce (transform.forward * launchForce);

		if (_trail)
			_trail.enabled = true;
		
		yield return new WaitForSeconds (lifeTime);
		transform.Recycle ();
	}

	void FixedUpdate() {
		_rigidbody.AddForce (transform.forward * acceleration);
	}
}
