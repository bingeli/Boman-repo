using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OldEnemyController : MonoBehaviour
{


    public float speed;
    public int lifePoints;
    public GameObject bomb;
    public int bombRadius;
    public float maxBombs;
    private float enemyWidth, enemyHeight;
    private Collider2D[] coll = new Collider2D[1];
    bool movement = false;
    public int lastDir;
    int lengthOfPath = 1;

    Animator anim;
    private Vector2 nextPos;

    public int points;
    public Text score;

    // Use this for initialization
    void Start()
    {
        score = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();
        anim = GetComponent<Animator>();
        enemyWidth = transform.localScale.x * GetComponent<BoxCollider2D>().size.x;
        enemyHeight = transform.localScale.y * GetComponent<BoxCollider2D>().size.y;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("idle", !movement);
        receiveDamage();

        if (!movement)
        {
            move();
        }
        else
        {
            anim.SetInteger("direction", lastDir + 1);
            //transform.position = Vector2.MoveTowards(transform.position, nextPos, Time.deltaTime * speed);
            if (new Vector2(transform.position.x, transform.position.y) == nextPos)
            {
                movement = false;
                lengthOfPath++;
            }
        }
    }

    private void move()
    {
        List<Transform> availableDirs = new List<Transform>();
        availableDirs = getAvailableDirs(transform, enemyWidth, enemyHeight);

        plantBomb(availableDirs);
        //Debug.Log (availableDirs.Count);
        if (Random.Range(0, 100) > 75)
        {
            lastDir = Random.Range(0, 3);
        }
        if (availableDirs[lastDir] == transform)
        {
            //lastDir = Random.Range (0, 3);
            if (lastDir == 0)
            {
                nextPos = new Vector2(availableDirs[1].position.x, availableDirs[1].position.y);
                lastDir = 1;
            }
            else if (lastDir == 1)
            {
                nextPos = new Vector2(availableDirs[2].position.x, availableDirs[2].position.y);
                lastDir = 2;
            }
            else if (lastDir == 2)
            {
                nextPos = new Vector2(availableDirs[3].position.x, availableDirs[3].position.y);
                lastDir = 3;
            }
            else if (lastDir == 3)
            {
                nextPos = new Vector2(availableDirs[0].position.x, availableDirs[0].position.y);
                lastDir = 0;
            }
        }
        else
        {
            nextPos = new Vector2(availableDirs[lastDir].position.x, availableDirs[lastDir].position.y);
        }

        movement = true;


    }

    public void plantBomb(List<Transform> availablePathStart)
    {
        //	List<Transform> safePath = safePath (availablePathStart);

    }

    /*	public Transform[] safePath (List<Transform> availablePathStart) {
            List<Transform> paths = new List<Transform>();
            int i = 0;
            int j = 0;
            foreach (Transform tile in availablePathStart) {
                if (transform.position != tile.position) {
                    if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x + (i*enemyWidth), transform.position.y + (j*enemyHeight)), coll, LayerMask.GetMask("nonwalkable", "flame")) == 0) {
				
                    }
                }
            }
            return transform;
        }*/

    public void hide(Transform[] pathToSafety)
    {

    }

    private List<Transform> getAvailableDirs(Transform position, float tileWidth, float tileHeight)
    {
        List<Transform> availableDirs = new List<Transform>();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (((i == 0) || (j == 0)) && !((j == 0) && (i == 0)))
                {
                    if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x + (i * tileWidth), transform.position.y + (j * tileHeight)), coll, LayerMask.GetMask("nonwalkable", "flame")) == 0)
                    {
                        if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x + (i * tileWidth), transform.position.y + (j * tileHeight)), coll, LayerMask.GetMask("walkable")) > 0)
                        {
                            availableDirs.Add(coll[0].transform);
                        }
                        else
                        {
                            availableDirs.Add(transform);
                        }
                    }
                    else
                    {
                        availableDirs.Add(transform);
                    }
                }
            }
        }
        return availableDirs;
    }

    private void receiveDamage()
    {
        if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y), coll, LayerMask.GetMask("flame")) > 0)
        {
            die();
        }
    }

    private void die()
    {
        score.text = "" + (int.Parse(score.text) + points);
        GameObject.Destroy(gameObject);
    }

}
