using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DestroyableObjectsController : MonoBehaviour
{

    public GameObject placeHolder;
    private float objWidth, objHeight;
    private Collider2D[] coll = new Collider2D[5];
    private bool willExplode = false;
    private bool tmp = false;
    private bool spawnedGrass = false;

    public float chanceForPowerup;
    public List<GameObject> powerups = new List<GameObject>();

    public int points;
    public Text score;

    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        score = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();

        objWidth = transform.localScale.x * GetComponent<BoxCollider2D>().size.x;
        objHeight = transform.localScale.y * GetComponent<BoxCollider2D>().size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!willExplode)
        {
            destroySelf();
        }
        anim.SetBool("shouldCollapse", false);
        if (willExplode)
        {
            anim.SetBool("shouldCollapse", true);
            if (!spawnedGrass)
            {
                Instantiate(placeHolder, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(new Vector3(0, 0, 0)));
                spawnedGrass = true;
            }
            if (!tmp)
            {
                tmp = true;
                if ((Random.Range(1, 100) < chanceForPowerup))
                {
                    spawnPowerUp();
                }
            }

            if (!GetComponent<Renderer>().enabled)
            {
                score.text = "" + (int.Parse(score.text) + points);
                GameObject.Destroy(gameObject);
            }
        }
    }

    private void destroySelf()
    {
        if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y + objHeight), coll, LayerMask.GetMask("flame")) > 0)
        {
            if ((!coll[0].GetComponent<ExplosionCotroller>().isFinalInRow) && (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y + objHeight + objHeight), coll, LayerMask.GetMask("flame")) > 0))
            {
                willExplode = true;
            }
            else
            {
                foreach (Collider2D a in coll)
                {
                    if ((a != null) && a.GetComponent<ExplosionCotroller>().isCenter)
                    {
                        willExplode = true;
                    }
                }

            }
        }
        else if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y - objHeight), coll, LayerMask.GetMask("flame")) > 0)
        {
            if ((!coll[0].GetComponent<ExplosionCotroller>().isFinalInRow) && (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x, transform.position.y - objHeight - objHeight), coll, LayerMask.GetMask("flame")) > 0))
            {
                willExplode = true;
            }
            else
            {
                foreach (Collider2D a in coll)
                {
                    if ((a != null) && (a.GetComponent<ExplosionCotroller>().isCenter))
                    {
                        willExplode = true;
                    }
                }
            }
        }
        else if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x - objWidth, transform.position.y), coll, LayerMask.GetMask("flame")) > 0)
        {
            if ((!coll[0].GetComponent<ExplosionCotroller>().isFinalInRow) && (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x - objWidth - objWidth, transform.position.y), coll, LayerMask.GetMask("flame")) > 0))
            {
                willExplode = true;
            }
            else
            {
                foreach (Collider2D a in coll)
                {
                    if ((a != null) && a.GetComponent<ExplosionCotroller>().isCenter)
                    {
                        willExplode = true;
                    }
                }
            }
        }
        else if (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x + objWidth, transform.position.y), coll, LayerMask.GetMask("flame")) > 0)
        {
            if ((!coll[0].GetComponent<ExplosionCotroller>().isFinalInRow) && (Physics2D.OverlapPointNonAlloc(new Vector2(transform.position.x + objWidth + objWidth, transform.position.y), coll, LayerMask.GetMask("flame")) > 0))
            {
                willExplode = true;
            }
            else
            {
                foreach (Collider2D a in coll)
                {
                    if ((a != null) && a.GetComponent<ExplosionCotroller>().isCenter)
                    {
                        willExplode = true;
                    }
                }
            }
        }
    }

    private void spawnPowerUp()
    {
        if (powerups.Count > 0)
        {
            Instantiate(powerups[Random.Range(0, powerups.Count)], new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
}
