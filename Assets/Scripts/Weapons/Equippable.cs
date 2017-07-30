using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
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

	public void Equip() {
		_equipped = true;
		PlaySound (sounds.equip);
		SendMessage ("OnEquip", SendMessageOptions.DontRequireReceiver);
	}

	public void Drop() {
		_equipped = false;
		PlaySound (sounds.drop);
		SendMessage ("OnDrop", SendMessageOptions.DontRequireReceiver);
	}

	void PlaySound(AudioClip clip, bool loop = false) {
		_audio.clip = clip;
		_audio.volume = sounds.volume;
		_audio.loop = loop;
		_audio.Play ();
	}


	void Awake () {
		_audio = GetComponent<AudioSource> ();
	}

}
