using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private BoxCollider2D bc;
    private Animator anim;
    private AutoMove autoMove;
    private Level2DGameManager gameManager;

    private bool aplastado;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        autoMove = GetComponent<AutoMove>();
        gameManager = GameObject.Find("GameManager").GetComponent<Level2DGameManager>();
    }


    void Update()
    {
        if (autoMove != null && autoMove.enabled)
            anim.SetFloat("velX", Mathf.Abs(autoMove.speed * autoMove.direction.normalized.x));
    }

    //private void OnCollisionEnter2D(Collision2D col)
    private void OnCollisionStay2D(Collision2D col)
    {
        var obj = col.gameObject;
        if (obj.CompareTag("Player") && !GameObject.Find("Meta").GetComponent<Meta>().win)
        {
            //if above, crush me
            if (obj.GetComponent<Player>().bc.bounds.min.y >= bc.bounds.max.y)
            {
                if (!aplastado)
                {
                    aplastado = true;

                    transform.localScale = new Vector3(transform.localScale.x * 1.3f, transform.localScale.y / 2, transform.localScale.z);

                    obj.GetComponent<Player>().jump();

                    gameManager.UpdateScore(300);
                    Destroy(gameObject, 0.5f);
                }
            }
            //if from the sides or below, damage player
            else // if (obj.GetComponent<Player>().bc.bounds.min.x <= bc.bounds.max.x || obj.GetComponent<Player>().bc.bounds.max.x <= bc.bounds.min.x)
            {
                obj.GetComponent<Player>().takeDamage(transform.position - obj.transform.position);
            }
        }
    }
}
