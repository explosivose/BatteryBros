using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Damage))]
public class ImpactBlunt : MonoBehaviour {

	public Transform impactPrefab;
	public bool destroyOnImpact;
	public float minimumVelocityForDamage = 5f;

	private bool _impact = false;
	private Damage _damage;

	void Awake() {
		ObjectPool.CreatePool (impactPrefab);
		_damage = GetComponent<Damage> ();
	}

	void OnEnable() {
		_impact = false;
	}

	IEnumerator OnCollisionEnter(Collision col) {
		if (!_impact && col.relativeVelocity.magnitude > minimumVelocityForDamage) {
			_impact = true;
			Debug.Log (col.transform.name);
			Quaternion rotation = Quaternion.LookRotation (col.contacts [0].normal);
			Transform effect = impactPrefab.Spawn (col.contacts [0].point, rotation);
			effect.parent = col.transform;
			if (destroyOnImpact) {
				transform.Recycle ();
			}
			col.transform.SendMessage ("Damage", _damage.damage, SendMessageOptions.DontRequireReceiver);
			yield return new WaitForSeconds (10f);
			effect.Recycle ();
		}
		yield return new WaitForEndOfFrame ();
	}
}
