using UnityEngine;
using System.Collections;

public class PlayerPickup : MonoBehaviour {

	private Equippable _equipped;
	private bool _handling;

	IEnumerator OnTriggerEnter(Collider col) {
		Debug.Log (col.transform.name);
		if (_handling)
			yield break;
		_handling = true;
		Equippable equipment = col.GetComponent<Equippable> ();
		if (equipment) {
			if (equipment == _equipped)
				yield break;
			if (_equipped) {
				_equipped.Drop ();
				_equipped.transform.parent = null;
			}
			yield return new WaitForSeconds (0.5f);
			equipment.Equip ();
			equipment.transform.parent = transform;
			_equipped = equipment;
		}
		yield return new WaitForSeconds (0.5f);
		_handling = false;
	}
}
