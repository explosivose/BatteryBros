using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class ParticleEffect : MonoBehaviour {

	private ParticleSystem _particles;
	private AudioSource _audio;

	void Awake() {
		_particles = GetComponent<ParticleSystem> ();
		_audio = GetComponent<AudioSource> ();
	}

	void OnEnable() {
		StartCoroutine (Lifetime ());
	}

	IEnumerator Lifetime() {
		float lifetime = _particles.startLifetime + _particles.duration;
		if (_audio.clip) {
			lifetime = Mathf.Max (_particles.startLifetime + _particles.duration, _audio.clip.length);
		} 
		yield return new WaitForSeconds (lifetime + 0.1f);
		transform.Recycle ();
	}
}
