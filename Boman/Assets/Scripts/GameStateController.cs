using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateController : MonoBehaviour {

	public float timeInSeconds;
	public Text clock;
	public Text lp;
	public Text EnemiesLeft;
	public Text BombCount;
	public Text BombRadius;
	public Text score;
	public Text finalScore;
	public GameObject finalPanel;
	private float gameStart;
	private float timeRemaining;
	private string formatedTime;
	private GameObject player;
	private GameObject[] enemies;
	private bool endGame = false;
//	public GameObject endStateGUI;


	// Use this for initialization
	void Start () {
		gameStart = Time.time;
		AudioListener.volume = PlayerPrefs.GetFloat ("soundVolume");

	//	endStateGUI.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (!player) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
		if (enemies == null) {
			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			EnemiesLeft.text = "0";
		} else {
			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			if (enemies.Length == 0) {
				endGame = true;
				finalScore.text = "" + Mathf.CeilToInt(int.Parse(score.text) + int.Parse(lp.text) * 500 + (timeRemaining * 15) + (int.Parse(EnemiesLeft.text) * (-250)) + (GetComponent<MapGenerator>()._mapHeight * 20) + (GetComponent<MapGenerator>()._mapWidth * 20)) ;
				finalPanel.SetActive(true);
				timeRemaining = 0;
				foreach (GameObject b in GameObject.FindGameObjectsWithTag("bomb")) {
					GameObject.Destroy(b);
				}
				player.GetComponent<PlayerController>().die();
			}
			EnemiesLeft.text = enemies.Length.ToString();
		}
		if (player && player.GetComponent<PlayerController>().lifePoints <= 0) {
			lp.text = "" + 0;
			finalScore.text = "" + Mathf.CeilToInt(int.Parse(score.text) + int.Parse(lp.text) * 200 + (timeRemaining * 25)) ;
			finalPanel.SetActive(true);
			timeRemaining = 0;
			foreach (GameObject b in GameObject.FindGameObjectsWithTag("bomb")) {
				GameObject.Destroy(b);
			}
			player.GetComponent<PlayerController>().die();
		}
		if (player) {
			timeRemaining = gameStart + timeInSeconds - Time.time;
			lp.text = player.GetComponent<PlayerController>().lifePoints.ToString();
			BombCount.text = player.GetComponent<PlayerController>().maxBombs.ToString();
			BombRadius.text = player.GetComponent<PlayerController>().bombRadius.ToString();
		}

		if (timeRemaining > 0) {
						clockUpdate ();
		} else if (!endGame) {
			endGame = true;
			finalScore.text = "" + Mathf.CeilToInt(int.Parse(score.text) + int.Parse(lp.text) * 500 + (timeRemaining * 15) + (int.Parse(EnemiesLeft.text) * (-250)) + (GetComponent<MapGenerator>()._mapHeight * 20) + (GetComponent<MapGenerator>()._mapWidth * 20)) ;
			finalPanel.SetActive(true);
			timeRemaining = 0;
			foreach (GameObject b in GameObject.FindGameObjectsWithTag("bomb")) {
				GameObject.Destroy(b);
			}
			player.GetComponent<PlayerController>().die();
		}
	}

	public void clockUpdate() {
		int minutes = Mathf.FloorToInt (timeRemaining / 60);
		int seconds =  Mathf.FloorToInt (timeRemaining - (60*(minutes)));
		formatedTime = minutes + ":" + seconds;
		clock.text = formatedTime;
	}

	public void exitClicked() {
		Application.LoadLevel (0);
	}

	public void restartClicked() {
		Application.LoadLevel (1);
	}

}
