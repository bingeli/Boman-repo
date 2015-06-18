using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OldPlayerController : MonoBehaviour
{

    public float speed;
    public int lifePoints;
    public GameObject bomb;
    public int bombRadius;
    public float maxBombs;
    public int bombThrowsLeft;
    public int powerBomb = 0;
    public bool isGhost = false;
    public int currentBombsOnField = 0;

    // 0 - doesnt move, 1 - top, 2 - down, 3 - left, 4 - right
    public int movementDir = 0;

    private float playerWidth, playerHeight;

    private Collider2D[] coll = new Collider2D[1];
    private bool isImmortal = false;
    private float defaultImmortality = 2f;
    private float flashing = 0.2f;
    private float timeOfFlash;
    private float timeStarted;
    public int lookAt;
    Animator anim;
    Vector3 destination;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        playerWidth = transform.localScale.x * GetComponent<BoxCollider2D>().size.x/2;
        playerHeight = transform.localScale.y * GetComponent<BoxCollider2D>().size.y/2;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger("lookAt", lookAt);

        //	Debug.Log (Physics2D.OverlapCircleNonAlloc (transform.position, coll, LayerMask.NameToLayer("walkable")));
        //	Debug.Log (coll[7].ToString);
        if (speed > 20)
        {
            speed = 20;
        }
        else if (speed < 4)
        {
            speed = 4;
        }
        if (maxBombs > 5)
        {
            maxBombs = 5;
        }

        if (lifePoints > 5)
        {
            lifePoints = 5;
        }
        if (isImmortal)
        {
            if (timeOfFlash + flashing < Time.time)
            {
                if (GetComponent<Renderer>().enabled)
                {
                    GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    GetComponent<Renderer>().enabled = true;
                }
                timeOfFlash = Time.time;
            }
        }
        if (timeStarted + defaultImmortality < Time.time)
        {
            isImmortal = false;
            GetComponent<Renderer>().enabled = true;
        }
        if (!isImmortal)
        {
            receiveDamage();
        }
        if (movementDir == 0)
        {
            move();
            anim.SetInteger("movementDir", movementDir);
        }
        else
        {
            lookAt = movementDir;
            transform.position = destination;
            //transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * speed);
            //if (transform.position == destination)
            {
                lookAt = movementDir;
                movementDir = 0;
                anim.SetInteger("movementDir", movementDir);
            }
        }
        if (maxBombs > currentBombsOnField)
        {
            if (bombThrowsLeft > 0)
            {
                throwBomb();
            }
            plantBomb();
        }
    }

    private void move()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            lookAt = 1;
            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y + playerHeight), coll, LayerMask.GetMask("nonwalkable")) == 0)
            {
                if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y + playerHeight), coll, LayerMask.GetMask("walkable")) > 0)
                {
                    movementDir = 1;
                    destination = new Vector3(coll[0].transform.position.x, coll[0].transform.position.y, 0);
                }
            }
            else if (isGhost && (coll[0].gameObject.tag == "crackedWall"))
            {
                movementDir = 1;
                destination = new Vector3(coll[0].transform.position.x, coll[0].transform.position.y, 0);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            lookAt = 2;
            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y - playerHeight), coll, LayerMask.GetMask("nonwalkable")) == 0)
            {
                if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y - playerHeight), coll, LayerMask.GetMask("walkable")) > 0)
                {
                    movementDir = 2;
                    destination = new Vector3(coll[0].transform.position.x, coll[0].transform.position.y, 0);
                }
            }
            else if (isGhost && (coll[0].gameObject.tag == "crackedWall"))
            {
                movementDir = 2;
                destination = new Vector3(coll[0].transform.position.x, coll[0].transform.position.y, 0);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            lookAt = 3;
            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x - playerWidth, transform.position.y), coll, LayerMask.GetMask("nonwalkable")) == 0)
            {
                if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x - playerWidth, transform.position.y), coll, LayerMask.GetMask("walkable")) > 0)
                {
                    movementDir = 3;
                    destination = new Vector3(coll[0].transform.position.x, coll[0].transform.position.y, 0);
                }
            }
            else if (isGhost && (coll[0].gameObject.tag == "crackedWall"))
            {
                movementDir = 3;
                destination = new Vector3(coll[0].transform.position.x, coll[0].transform.position.y, 0);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            lookAt = 4;
            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x + playerHeight, transform.position.y), coll, LayerMask.GetMask("nonwalkable")) == 0)
            {
                if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x + playerHeight, transform.position.y), coll, LayerMask.GetMask("walkable")) > 0)
                {
                    movementDir = 4;
                    //destination = new Vector3(coll[0].transform.position.x, coll[0].transform.position.y, 0);
                    destination = new Vector3(transform.position.x + speed/60,transform.position.y,0);
                }
            }
            else if (isGhost && (coll[0].gameObject.tag == "crackedWall"))
            {
                movementDir = 4;
                destination = new Vector3(coll[0].transform.position.x, coll[0].transform.position.y, 0);
            }
        }
    }

    private void plantBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y), coll, LayerMask.GetMask("nonwalkable")) == 0)
            {
                if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y), coll, LayerMask.GetMask("walkable")) > 0)
                {
                    GameObject b = (GameObject)Instantiate(bomb, coll[0].transform.position, coll[0].transform.rotation);
                    if (powerBomb > 0)
                    {
                        b.GetComponent<BombController>().explosionRadius = 15;
                        powerBomb--;
                    }
                    else
                    {
                        b.GetComponent<BombController>().explosionRadius = bombRadius;
                    }
                    currentBombsOnField++;
                }
            }
        }
    }

    private void throwBomb()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y), coll, LayerMask.GetMask("walkable")) > 0)
            {
                GameObject b = (GameObject)Instantiate(bomb, coll[0].transform.position, coll[0].transform.rotation);
                b.GetComponent<BombController>().explosionRadius = bombRadius;
                b.GetComponent<BombController>().fly = true;
                b.GetComponent<BombController>().direction = lookAt;
                currentBombsOnField++;
                bombThrowsLeft--;
            }
        }
    }

    private void receiveDamage()
    {
        if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y), coll, LayerMask.GetMask("flame", "enemy")) > 0)
        {
            lifePoints--;
            isImmortal = true; //only for a bit;
            timeStarted = Time.time;
        }
    }

    public void die()
    {
        GameObject.Destroy(gameObject);
    }
}
