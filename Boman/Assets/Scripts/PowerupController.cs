using UnityEngine;
using System.Collections;

public class PowerupController : MonoBehaviour {

	/* 1 - bomb radius +,
	 * 2 - bomb count +,
	 * 3 - speed +,
	 * 4 - bomb throw,
	 * 5 - power bomb,
	 * 6 - walk through soft blocks,
	 * 7 - life point +
	 * 8 - speed down
	 */
	public int type;

	private float objWidth, objHeight;
	private Collider2D[] coll = new Collider2D[1];


	// Use this for initialization
	void Start () {
		if (type == 1) {
			if (GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().bombRadius >= 15) {
				GameObject.Destroy (gameObject);
			}
		} else if (type == 2) {
			if (GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().maxBombs >= 5) {
				GameObject.Destroy (gameObject);
			}
		} else if (type == 3) {
			if (GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ()._speed >= 20) {
				GameObject.Destroy (gameObject);
			}
		} else if (type == 4) {
		} else if (type == 5) {
		} else if (type == 6) {
			if (GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().isGhost) {
				GameObject.Destroy (gameObject);
			}
		} else if (type == 7) {
			if (GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().lifePoints > 5) {
				GameObject.Destroy (gameObject);
			}
		} else if (type == 8) {
			if (GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ()._speed <= 4) {
				GameObject.Destroy (gameObject);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics2D.OverlapPointNonAlloc (new Vector2 (transform.position.x, transform.position.y), coll, LayerMask.GetMask ("flame")) > 0) {
			GameObject.Destroy(gameObject);		
		}
		if (Physics2D.OverlapPointNonAlloc (new Vector2 (transform.position.x, transform.position.y), coll, LayerMask.GetMask ("player")) > 0) {
			if (type == 1) {
				coll[0].GetComponent<PlayerController>().bombRadius += 2;
			} else if (type == 2) {
				coll[0].GetComponent<PlayerController>().maxBombs++;
			} else if (type == 3) {
				coll[0].GetComponent<PlayerController>()._speed += 4;
			} else if (type == 4) {
				coll[0].GetComponent<PlayerController>()._bombThrowsLeft += 1;
			} else if (type == 5) {
				coll[0].GetComponent<PlayerController>().powerBomb++;
			} else if (type == 6) {
				coll[0].GetComponent<PlayerController>().isGhost = true;
			} else if (type == 7) {
				coll[0].GetComponent<PlayerController>().lifePoints++;
			} else if (type == 8) {
				coll[0].GetComponent<PlayerController>()._speed -= 4;
			}
			GameObject.Destroy(gameObject);
		}
	}
}
