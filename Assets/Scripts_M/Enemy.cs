using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public int moveSpeed;
    public float observeRange = 0;
    public float distance = 0;
    public bool isChase;
    public bool isMove;
    public Player player;

    private Transform startPos;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = gameObject.transform;
    }

    void Start()
    {
        
    }

    private void Update()
    {
        Move();
        distance = (player.transform.position - transform.position).magnitude;
        
        Targeting();
    }

    private void Move()
    {
        if (isMove)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    private void Turn()
    {
        if(isMove)
            moveSpeed *= -1;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void Targeting()
    {
        if (distance <= observeRange)
        {
            if (player.transform.position.x - transform.position.x > 0)
            {
                if (moveSpeed < 0)
                {
                    moveSpeed = -moveSpeed;
                }
            }
            else
            {
                if (moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed;
                }
            }

            Debug.Log(player.transform.position.x - transform.position.x);
            if (moveSpeed > 0) spriteRenderer.flipX = false;
            else spriteRenderer.flipX = true;
        }
        else
        {
            float pointADis = pointA.position.x - transform.position.x;
            float pointBDis = pointB.position.x - transform.position.x;

            if (pointADis >= 0)
            {
                if (pointADis * moveSpeed < 0)
                    Turn();
            }
            else if (pointBDis <= 0)
            {
                if (pointBDis * moveSpeed < 0)
                    Turn();
            }
        }
        //if (isChase)
        //{


        //    else
        //    {
        //        isMove = true;
        //        isChase = false;
        //    }
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, observeRange);
    }
}
