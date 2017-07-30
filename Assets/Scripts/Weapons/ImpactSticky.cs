using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Damage))]
public class ImpactSticky : MonoBehaviour {

	public Transform impactPrefab;
	public bool destroyOnImpact;
	public float lifetime = 300f;

	private Damage _damage;
	private bool _impact;
	private Rigidbody _rigidbody;
	private AudioSource _audio;

	void Awake() {
		ObjectPool.CreatePool (impactPrefab);
		_damage = GetComponent<Damage> ();
		_rigidbody = GetComponent<Rigidbody> ();
		_audio = GetComponent<AudioSource> ();
	}

	void OnEnable() {
		_impact = false;
		_rigidbody.isKinematic = false;
	}

	IEnumerator OnCollisionEnter(Collision col) {
		if (!_impact) {
			_impact = true;
			if (_audio)
				_audio.Stop ();
			Quaternion rotation = Quaternion.LookRotation (col.contacts [0].normal);
			impactPrefab.Spawn (col.contacts [0].point, rotation);
			_rigidbody.angularVelocity = Vector3.zero;
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.isKinematic = true;
			if (destroyOnImpact) {
				transform.Recycle ();
			} else {
				yield return new WaitForSeconds (lifetime);
			}
		}
		yield return new WaitForEndOfFrame ();
	}
}
