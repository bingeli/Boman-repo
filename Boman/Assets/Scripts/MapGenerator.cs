using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public GameObject thickTile;
	public GameObject crackedTile;
	public GameObject grassTile;
	public GameObject thickTileSide;
	public int _mapWidth;
	public int _mapHeight;
	public int startX;
	public int startY;
	public int probOfCracked;
	public GameObject enemy;
	public int countOfEnemies;
	public GameObject player;
	public float timeToWin;

	private float tileWidth;
	private float tileHeight;
	private float currentX;
	private float currentY;

    public float _width;
    public float _height;

	private Collider2D[] coll = new Collider2D[1];
	private float[,] availableCoords = new float[100, 2];
	// Use this for initialization
	void Start () {
        //_mapWidth = PlayerPrefs.GetInt ("mapWidth");
        //_mapHeight = PlayerPrefs.GetInt ("mapHeight");
		availableCoords [99, 0] = 0;
		availableCoords [99, 1] = 0;
		tileWidth = thickTile.transform.localScale.x * thickTile.GetComponent<BoxCollider2D>().size.x;
		tileHeight = thickTile.transform.localScale.y * thickTile.GetComponent<BoxCollider2D>().size.y;
        _width = _mapWidth * tileWidth;
        _height = _mapHeight * tileHeight;

		currentX = startX;
		currentY = startY;
		for (int i = 0; i < _mapWidth; i++) {
			for (int j = 0; j < _mapHeight; j++) {
				if ((i == 1) && (j == 1)) {

					Instantiate(player, new Vector2(currentX, currentY*(-1)), Quaternion.Euler(new Vector3(0, 0, 0)));
					Instantiate(grassTile, new Vector2(currentX, currentY*(-1)), Quaternion.Euler(new Vector3(0, 0, 0)));
				} else if ((i == 0) || (j == 0) || (i == (_mapWidth - 1)) || (j == (_mapHeight - 1)) || ((i % 2) == 0) && ((j %2) == 0)) {
					if ((i == 0) || ((i+1) == _mapWidth)) { 
						Instantiate(thickTileSide, new Vector2(currentX, currentY*(-1)), Quaternion.Euler(new Vector3(0, 0, 0)));
					} else {
						Instantiate(thickTile, new Vector2(currentX, currentY*(-1)), Quaternion.Euler(new Vector3(0, 0, 0)));
					}
				} else if (((i != 1) || (j != 1)) && ((i != 1) || (j != 2)) && ((i != 2) || (j != 1))) {
					if (Random.Range(0, 100) < probOfCracked) {
						Instantiate(crackedTile, new Vector2(currentX, currentY*(-1)), Quaternion.Euler(new Vector3(0, 0, 0)));
					} else {
						Instantiate(grassTile, new Vector2(currentX, currentY*(-1)), Quaternion.Euler(new Vector3(0, 0, 0)));
					}
				} else {
					Instantiate(grassTile, new Vector2(currentX, currentY*(-1)), Quaternion.Euler(new Vector3(0, 0, 0)));
				}
				currentY += tileHeight;
			}
			currentX += tileWidth;
			currentY = startY;
		}
		int lastVal = 0;
		for (int i = _mapWidth; i >= 1; i--) {
			currentX = i * tileWidth;
			for (int j = _mapHeight; j >= 1; j--) {
				currentY = j * tileHeight;
				if (Physics2D.OverlapPointNonAlloc(new Vector2(currentX, currentY*(-1)), coll, LayerMask.GetMask("walkable")) > 0) {
					if (lastVal < 99){
						availableCoords[lastVal, 0] = coll[0].transform.position.x;
						availableCoords[lastVal, 1] = coll[0].transform.position.y;
						lastVal++;
					}
				}
			}
		}
		int currentEnemies = 0;
		int randomizedIndex = 0;
		while (countOfEnemies > currentEnemies) {
			randomizedIndex = Random.Range(0, lastVal-1);
			if (availableCoords[randomizedIndex, 0] != 0) {
				Instantiate(enemy, new Vector2(availableCoords[randomizedIndex, 0], availableCoords[randomizedIndex, 1]), transform.rotation);
				currentEnemies++;
			}
			for (int i = randomizedIndex; i < lastVal; i++) {
				availableCoords[i, 0] = availableCoords [i+1, 0];
				availableCoords[i, 1] = availableCoords [i+1, 1];
			}
			lastVal--;

				
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
