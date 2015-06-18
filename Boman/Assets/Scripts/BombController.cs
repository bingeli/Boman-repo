using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour
{

    public GameObject explosion;
    public float timeUntilExplosion;
    float timePlanted;
    public float bombSpeed = 30;
    public int explosionRadius;
    float bombWidth, bombHeight;
    private Collider2D[] coll = new Collider2D[1];
    public bool fly = false;
    public int direction = 0;
    Vector2 dir;

    // Use this for initialization
    void Start()
    {
        timePlanted = Time.time;
        bombWidth = transform.localScale.x * GetComponent<BoxCollider2D>().size.x;
        bombHeight = transform.localScale.y * GetComponent<BoxCollider2D>().size.y;
    }


    // Update is called once per frame
    void Update()
    {
        if (fly)
        {
            if (direction != 0)
            {
                dir = flyTo(direction);
            }
            transform.position = Vector2.MoveTowards(transform.position, dir, Time.deltaTime * bombSpeed);
            if (new Vector2(transform.position.x, transform.position.y) == dir)
            {
                fly = false;
            }
        }
        else
        {

            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y), coll, LayerMask.GetMask("flame")) > 0)
            {
                timeUntilExplosion = timeUntilExplosion * 0.5f;
            }
            if ((timePlanted + timeUntilExplosion) < Time.time)
            {
                explode();
                PlayerController temp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                if (temp)
                {
                    temp.currentBombsOnField--;
                }

                GameObject.Destroy(gameObject);
            }
        }
    }

    public Vector2 flyTo(int i)
    {
        Vector2 result = new Vector2(transform.position.x, transform.position.y);
        if (i == 1)
        {
            while (Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y + bombHeight), coll, LayerMask.GetMask("wall")) == 0)
            {
                result.y += bombHeight;
                if ((Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y), coll, LayerMask.GetMask("nonwalkable")) > 0) && (Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y + bombHeight), coll, LayerMask.GetMask("wall")) > 0))
                {
                    result.y -= bombHeight;
                    break;
                }
                else if ((Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y), coll, LayerMask.GetMask("nonwalkable")) > 0) && (Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y + bombHeight), coll, LayerMask.GetMask("nonwalkable")) == 0))
                {
                    result.y += bombHeight;
                    break;
                }
            }
        }
        else if (i == 2)
        {
            while (Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y - bombHeight), coll, LayerMask.GetMask("wall")) == 0)
            {
                result.y -= bombHeight;
                if ((Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y), coll, LayerMask.GetMask("nonwalkable")) > 0) && (Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y - bombHeight), coll, LayerMask.GetMask("wall")) > 0))
                {
                    result.y += bombHeight;
                    break;
                }
                else if ((Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y), coll, LayerMask.GetMask("nonwalkable")) > 0) && (Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y - bombHeight), coll, LayerMask.GetMask("nonwalkable")) == 0))
                {
                    result.y -= bombHeight;
                    break;
                }
            }

        }
        else if (i == 3)
        {
            while (Physics2D.OverlapPointNonAlloc(new Vector2(result.x - bombWidth, result.y), coll, LayerMask.GetMask("wall")) == 0)
            {
                result.x -= bombWidth;
                if ((Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y), coll, LayerMask.GetMask("nonwalkable")) > 0) && (Physics2D.OverlapPointNonAlloc(new Vector2(result.x - bombWidth, result.y), coll, LayerMask.GetMask("wall")) > 0))
                {
                    result.x += bombWidth;
                    break;
                }
                else if ((Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y), coll, LayerMask.GetMask("nonwalkable")) > 0) && (Physics2D.OverlapPointNonAlloc(new Vector2(result.x - bombWidth, result.y), coll, LayerMask.GetMask("nonwalkable")) == 0))
                {
                    result.x -= bombWidth;
                    break;
                }
            }
        }
        else if (i == 4)
        {
            while (Physics2D.OverlapPointNonAlloc(new Vector2(result.x + bombWidth, result.y), coll, LayerMask.GetMask("wall")) == 0)
            {
                result.x += bombWidth;
                if ((Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y), coll, LayerMask.GetMask("nonwalkable")) > 0) && (Physics2D.OverlapPointNonAlloc(new Vector2(result.x + bombWidth, result.y), coll, LayerMask.GetMask("wall")) > 0))
                {
                    result.x -= bombWidth;
                    break;
                }
                else if ((Physics2D.OverlapPointNonAlloc(new Vector2(result.x, result.y), coll, LayerMask.GetMask("nonwalkable")) > 0) && (Physics2D.OverlapPointNonAlloc(new Vector2(result.x + bombWidth, result.y), coll, LayerMask.GetMask("nonwalkable")) == 0))
                {
                    result.x += bombWidth;
                    break;
                }
            }
        }
        direction = 0;
        return result;
    }

    private void explode()
    {
        GameObject tmp = new GameObject();

        tmp = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
        tmp.GetComponent<ExplosionCotroller>().isCenter = true;

        //to top
        for (int i = 1; i <= Mathf.FloorToInt(explosionRadius / 2); i++)
        {
            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y + (bombHeight * i)), coll, LayerMask.GetMask("walkable")) > 0)
            {
                tmp = (GameObject)Instantiate(explosion, coll[0].transform.position, coll[0].transform.rotation);
                tmp.GetComponent<ExplosionCotroller>().isVertical = true;
                tmp.GetComponent<ExplosionCotroller>().Direction = 3;
                if (i == Mathf.FloorToInt(explosionRadius / 2))
                {
                    tmp.GetComponent<ExplosionCotroller>().isFinalInRow = true;
                }
            }
            else
            {
                break;
            }
        }

        //to bot
        tmp = gameObject;
        for (int i = -1; i >= Mathf.FloorToInt(-explosionRadius / 2); i--)
        {
            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y + (bombHeight * i)), coll, LayerMask.GetMask("walkable")) > 0)
            {
                tmp = (GameObject)Instantiate(explosion, coll[0].transform.position, coll[0].transform.rotation);
                tmp.GetComponent<ExplosionCotroller>().isVertical = true;
                tmp.GetComponent<ExplosionCotroller>().Direction = 2;
                if (i == Mathf.FloorToInt(-explosionRadius / 2))
                {
                    tmp.GetComponent<ExplosionCotroller>().isFinalInRow = true;
                }
            }
            else
            {
                break;
            }
        }

        //to right
        tmp = gameObject;
        for (int i = 1; i <= Mathf.FloorToInt(explosionRadius / 2); i++)
        {
            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x + (bombHeight * i), transform.position.y), coll, LayerMask.GetMask("walkable")) > 0)
            {
                tmp = (GameObject)Instantiate(explosion, coll[0].transform.position, coll[0].transform.rotation);
                tmp.GetComponent<ExplosionCotroller>().Direction = 4;
                if (i == Mathf.FloorToInt(explosionRadius / 2))
                {
                    tmp.GetComponent<ExplosionCotroller>().isFinalInRow = true;
                }
            }
            else
            {
                break;
            }
        }

        //to left
        tmp = gameObject;
        for (int i = -1; i >= Mathf.FloorToInt(-explosionRadius / 2); i--)
        {
            if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x + (bombHeight * i), transform.position.y), coll, LayerMask.GetMask("walkable")) > 0)
            {
                tmp = (GameObject)Instantiate(explosion, coll[0].transform.position, coll[0].transform.rotation);
                tmp.GetComponent<ExplosionCotroller>().Direction = 1;
                if (i == Mathf.FloorToInt(-explosionRadius / 2))
                {
                    tmp.GetComponent<ExplosionCotroller>().isFinalInRow = true;
                }
            }
            else
            {
                break;
            }
        }




        //such a good code, to baaaaad it wont get used :(
        /*	for (int i = 0; i <= (explosionRadius-1); i++) {
                for (int j = 0; j <= (explosionRadius-1); j++) {
                    if ((j == Mathf.CeilToInt(explosionRadius / 2)) || (i == Mathf.CeilToInt(explosionRadius / 2))) {
                        if (Physics2D.OverlapPointNonAlloc(new Vector2((transform.position.x + (((-1) *Mathf.FloorToInt(explosionRadius/2) *bombWidth) + (i)*bombWidth)), (transform.position.y + (((-1) * Mathf.FloorToInt(explosionRadius/2) * bombHeight)) + ((j) * bombHeight))), coll, LayerMask.GetMask("walkable")) > 0) {
                            Instantiate (explosion, coll[0].transform.position, coll[0].transform.rotation);
                        }
                    }
                }
            }*/
    }

}
	
