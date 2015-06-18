using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	public Text height;
	public Text width;

	public int h;
	public int w;

    public RectTransform _levels;
    bool _isLevelMoving;

	public Slider sound;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

	public void playClick() {
		PlayerPrefs.SetFloat ("soundVolume", sound.value);
		if (int.TryParse (height.text, out h) && int.TryParse (width.text, out w) && (h > 3) && (w > 3)) {	
					PlayerPrefs.SetInt ("mapHeight", h);
					PlayerPrefs.SetInt ("mapWidth", w);
				} else { 
						PlayerPrefs.SetInt ("mapHeight", 15);
						PlayerPrefs.SetInt ("mapWidth", 25);
				}
		Application.LoadLevel (1);
	}


    public void OnClick_LeftLevel()
    {
        if (_isLevelMoving == false)
        {
            StartCoroutine("MoveLevelTo", new Vector2(_levels.position.x + 400f, _levels.position.y));
        }
    }

    public void OnClick_RightLevel()
    {
        if (_isLevelMoving == false)
        {
            StartCoroutine("MoveLevelTo", new Vector2(_levels.position.x - 400f, _levels.position.y));
        }
    }

    IEnumerator MoveLevelTo(Vector2 v)
    {
        _isLevelMoving = true;
        while (Mathf.Abs(_levels.position.x - v.x)>=10f)
        {
            _levels.position = Vector2.MoveTowards(_levels.position, v, 10f);
            yield return null;
        }
        _isLevelMoving = false;
    }

	public void changeVolume() {
		AudioListener.volume = sound.value;
	}

	public void ExitGame() {
		Application.Quit ();
	}
}
