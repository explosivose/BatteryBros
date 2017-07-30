using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class Equippable : MonoBehaviour {

	[System.Serializable]
	public class AudioLibrary {
		public float volume = 1f;
		public AudioClip equip;
		public AudioClip drop;
	}

	public Vector3 equippedPosition;
	public AudioLibrary sounds = new AudioLibrary();

	public bool equipped { get { return _equipped; } }

	private bool _equipped;
	private AudioSource _audio;
	private Rigidbody _rigidbody;

	public void Equip() {
		_equipped = true;
		PlaySound (sounds.equip);
		_rigidbody.isKinematic = true;
		SendMessage ("OnEquip", SendMessageOptions.DontRequireReceiver);
	}

	public void Drop() {
		_equipped = false;
		PlaySound (sounds.drop);
		SendMessage ("OnDrop", SendMessageOptions.DontRequireReceiver);
		_rigidbody.isKinematic = false;
	}

	void PlaySound(AudioClip clip, bool loop = false) {
		_audio.clip = clip;
		_audio.volume = sounds.volume;
		_audio.loop = loop;
		_audio.Play ();
	}

	void Awake () {
		_audio = GetComponent<AudioSource> ();
		_rigidbody = GetComponent<Rigidbody> ();
	}

	void Update() {
		if (_equipped) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, equippedPosition, Time.deltaTime * 2f);
			transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.identity, Time.deltaTime * 4f);
		}
	}

}
