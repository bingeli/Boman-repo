using UnityEngine;
using System.Collections;

public class ExplosionCotroller : MonoBehaviour {

	public float lifeTime;
	private float appeared;

	public bool isFinalInRow = false;
	public bool isCenter = false;
	public bool isVertical = false;
	public int Direction = 0;

	private Animator anim;
	private float smallTime;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		appeared = Time.time;
		smallTime = Time.time;
		GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("isCenter", isCenter);
		anim.SetBool ("isFinal", isFinalInRow);
		anim.SetBool ("isVertical", isVertical);
		anim.SetInteger ("Direction", Direction);
		if (smallTime + 0.1f < Time.time) {
						GetComponent<Renderer>().enabled = true;
				}
		if ((appeared + lifeTime) < Time.time) {
			GameObject.Destroy(gameObject);
		}
		if ((isCenter == true) && !GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}
}
