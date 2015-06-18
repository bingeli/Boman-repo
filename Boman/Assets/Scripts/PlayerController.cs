using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    //public float moveTime = 0.1f;			//Time it will take object to move, in seconds.
    public LayerMask blockingLayer;			//Layer on which collision will be checked.

    private BoxCollider2D boxCollider; 		//The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;				//The Rigidbody2D component attached to this object.
    //private float inverseMoveTime;			//Used to make movement more efficient.

    public float _speedDelta;
    public float _speed;
    public int lifePoints;
    public GameObject bomb;
    public int bombRadius;
    public float maxBombs;
    public int _bombThrowsLeft;
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
    public bool _isRunning;
    public int _lookAt = 2;
    Animator anim;
    Vector3 destination;

    // Use this for initialization
    void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();

        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent<Rigidbody2D>();

        ////By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        //inverseMoveTime = 1f / moveTime;

        anim = GetComponent<Animator>();
        playerWidth = transform.localScale.x * GetComponent<CircleCollider2D>().radius;
        playerHeight = transform.localScale.y * GetComponent<CircleCollider2D>().radius;
    }
    public Vector2 v;

    public float horizontal = 0;  	//Used to store the horizontal move direction.
    public float vertical = 0;		//Used to store the vertical move direction.
    //public float horizontalRaw = 0;  	//Used to store the horizontal move direction.
    //public float verticalRaw = 0;		//Used to store the vertical move direction.
    // Update is called once per frame
    void Update()
    {

        //int horizontal = 0;  	//Used to store the horizontal move direction.
        //int vertical = 0;		//Used to store the vertical move direction.

        ////Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        //horizontal = Input.GetAxis("Horizontal");

        ////Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        //vertical = Input.GetAxis("Vertical");


        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = Input.GetAxisRaw("Horizontal");

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = Input.GetAxisRaw("Vertical");

        ////Check if moving horizontally, if so set vertical to zero.
        //if (horizontal != 0)
        //{
        //    vertical = 0;
        //}
        v = rb2D.velocity;

        if (Mathf.Abs(v.x) <= 0.1f * _speed && Mathf.Abs(v.y) <= 0.1f * _speed)
        {
            print("stand");
            _isRunning = false;
            anim.SetBool("isMoving", _isRunning);

            if (horizontal == -1)
            {
                _lookAt = 3;
            }
            else if (horizontal == 1)
            {
                _lookAt = 4;
            }

            if (vertical == -1)
            {
                _lookAt = 2;
            }
            else if (vertical == 1)
            {
                _lookAt = 1;
            }
            anim.SetInteger("direction", _lookAt);
        }
        else
        {
            _isRunning = true;
            anim.SetBool("isMoving", _isRunning);
            if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
            {
                if (v.x < 0)
                {
                    _lookAt = 3;
                }
                else if (v.x > 0)
                {
                    _lookAt = 4;
                }
            }
            else
            {
                if (v.y < 0)
                {
                    _lookAt = 2;
                }
                else if (v.y > 0)
                {
                    _lookAt = 1;
                }
            }
            anim.SetInteger("direction", _lookAt);
        }

        // move
        if (horizontal != 0)
        {
            if (Mathf.Abs(rb2D.velocity.x) <= _speed)
            {
                rb2D.AddForce(new Vector2(horizontal, 0) * _speedDelta);
            }
            else
            {
                rb2D.velocity = new Vector2(horizontal * _speed, 0);
            }
        }
        else if (vertical != 0)
        {
            if (Mathf.Abs(rb2D.velocity.y) <= _speed)
            {
                rb2D.AddForce(new Vector2(0, vertical) * _speedDelta);
            }
            else
            {
                rb2D.velocity = new Vector2(0, vertical * _speed);
            }
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

        if (maxBombs > currentBombsOnField)
        {
            if (_bombThrowsLeft > 0)
            {
                throwBomb();
            }
            plantBomb();
        }
    }


    private void plantBomb()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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

    private void throwBomb()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y), coll, LayerMask.GetMask("walkable")) > 0)
            {
                GameObject b = (GameObject)Instantiate(bomb, coll[0].transform.position, coll[0].transform.rotation);
                b.GetComponent<BombController>().explosionRadius = bombRadius;
                b.GetComponent<BombController>().fly = true;
                b.GetComponent<BombController>().direction = _lookAt;
                currentBombsOnField++;
                _bombThrowsLeft--;
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
