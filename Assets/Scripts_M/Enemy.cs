using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int moveSpeed;
    public float observeRange = 0;
    public float distance = 0;
    public bool isChase;
    public bool isMove;
    public Player player;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    private void Update()
    {
        Move();
        Targeting();
        distance = (player.transform.position - transform.position).magnitude;
    }

    private void Move()
    {
        if (isMove)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            if (distance <= observeRange)
            {
                float distance = (player.transform.position.x - transform.position.x);
                int direction = distance > 0 ? 1 : -1;
                moveSpeed *= direction;

                isChase = true;
                isMove = false;
            }
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
        if (isChase)
        {
            if (distance <= observeRange)
            {
                moveSpeed = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
                if (moveSpeed > 0) spriteRenderer.flipX = false;
                else spriteRenderer.flipX = true;
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            else
            {
                isMove = true;
                isChase = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Turn") Turn();
    }
}
