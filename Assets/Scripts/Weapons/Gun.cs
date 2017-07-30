using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Equippable))]
public class Gun : MonoBehaviour {

	[System.Serializable]
	public class AudioLibrary {
		public float volume = 1f;
		public AudioClip fire;
	}

	[System.Serializable]
	public class Spread {
		public bool enabled = false;
		public float minAngle = 8f;
		public float deterioration = 1f;
		public float recoveryRate = 1f;
		public float angle { get; set; }
	}

	public Transform projectile;
	public AudioLibrary sounds = new AudioLibrary();
	public int shots = 1;			// how many shots in one fire?
	public float rateOfFire;		// how many fires in one second?
	public Spread spread = new Spread();
	public float readyTime;			// how much time to wait after equipping

	private AudioSource _audio;
	private Equippable _equip;
	private Transform _nozzle;		// spawn projectiles on the nozzle
	private bool _firing = false;
	private bool _ready = false;
	private float _lastFireTime;
	private float _nextPossibleFireTime;


	void Awake () {
		_audio = GetComponent<AudioSource> ();
		_equip = GetComponent<Equippable> ();
		_nozzle = transform.FindChild ("Nozzle");
		spread.angle = spread.minAngle;
		ObjectPool.CreatePool (projectile);
	}

	// OnEquip event from Equippable
	IEnumerator OnEquip() {
		yield return new WaitForSeconds (readyTime);
		_ready = true;
	}

	void OnDrop() {
		_ready = false;
	}

	// Update is called once per frame
	void Update () {
		//if (!_equip.equipped)
		//	return;
		//if (!_ready)
		//	return;
		if (Input.GetButton ("Fire1") && !_firing) {
			Debug.Log ("Fire!");
			StartCoroutine (Fire ());
		}
		if (spread.enabled) {
			spread.angle -= spread.recoveryRate * Time.deltaTime;
			spread.angle = Mathf.Max (spread.angle, spread.minAngle);
		}
	}

	IEnumerator Fire() {
		_firing = true;
		_lastFireTime = Time.time;
		FireProjectile ();
		if (spread.enabled) {
			spread.angle += spread.deterioration;
		}
		float wait = 1f / rateOfFire;
		_nextPossibleFireTime = Time.time + wait;
		yield return new WaitForSeconds (wait);
		_firing = false;
	}

	void FireProjectile() {
		PlaySound (sounds.fire);
		float angle = 0f;
		if (spread.enabled) {
			angle = Mathf.Deg2Rad * Mathf.Clamp (
				90f - spread.angle,
				Mathf.Epsilon,
				90f - Mathf.Epsilon);
 		}
		float distance = Mathf.Tan (angle);
		for (int i = 0; i < shots; i++) {
			Vector3 dir = _nozzle.forward;
			if (spread.enabled) {
				Vector2 pointInCircle = Random.insideUnitCircle;
				dir = new Vector3 (pointInCircle.x, pointInCircle.y, distance);
				dir = transform.rotation * dir;
			}
			Quaternion rot = Quaternion.LookRotation (dir);
			Transform p = projectile.Spawn (_nozzle.position, rot);
		}
	}

	void PlaySound(AudioClip clip, bool loop = false) {
		if (!clip)
			return;
		_audio.clip = clip;
		_audio.volume = sounds.volume;
		_audio.loop = loop;
		_audio.Play ();
	}
}
