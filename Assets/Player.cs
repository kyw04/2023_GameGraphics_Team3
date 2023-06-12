using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float radius;
    public float movementSpeed = 5f;
    public float jumpPower = 5f;
    public float coinCount = 0;

    private GameObject controlledObj;
    private SpriteRenderer spriteRenderer;
    private Animator ani;
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private Vector2 movement;
    private bool isShadow = false;
    private bool isGround = false;
    private bool isMove = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (!isMove)
            return;

        movement.x = Input.GetAxisRaw("Horizontal");
        
        if (movement.x == 0f)
        {
            ani.SetBool("isRun", false);
        }
        else
        {
            if (movement.x > 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
            ani.SetBool("isRun", true);
        }

        transform.Translate(movement * movementSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isMove = false;
            isShadow = !isShadow;
            gameObject.layer = isShadow ? LayerMask.NameToLayer("Shadow") : LayerMask.NameToLayer("Player");
            ani.SetTrigger("Change");
        }

        if (rb.velocity.y != 0)
        {
            isGround = false;
            ani.SetBool("isGround", false);
        }
        if (Input.GetButtonDown("Jump") && isGround && !isShadow)
        {
            OnJump();
        }

        if (isShadow)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));
            Collider2D selectCollider = null;
            float min = -1;

            foreach (Collider2D collider in colliders)
            {
                float dis = Mathf.Abs(Vector2.Distance(collider.transform.position, transform.position));
                if (min == -1 || min > dis)
                {
                    min = dis;
                    selectCollider = collider;
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (controlledObj != null)
                {
                    controlledObj.transform.parent = null;
                    gameObject.layer = LayerMask.NameToLayer("Shadow");
                    controlledObj = null;
                }
                else if (selectCollider && selectCollider.GetComponent<Shadow>().onShadow)
                {
                    gameObject.layer = selectCollider.gameObject.layer;
                    transform.position = selectCollider.transform.position;
                    selectCollider.transform.parent = this.transform;
                    controlledObj = selectCollider.gameObject;
                }
            }
        }
        else if (controlledObj != null)
        {
            controlledObj.transform.parent = null;
            gameObject.layer = LayerMask.NameToLayer("Player");
            controlledObj = null;
        }
    }

    private void OnJump()
    {
        isGround = false;
        ani.SetTrigger("doJump");
        ani.SetBool("isGround", false);
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    public void OnMove()
    {
        isMove = true;
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isShadow)
            return;

        if (collision.gameObject.CompareTag("Floor"))
        {
            ani.SetBool("isGround", true);
            isGround = true;
        }

        if (collision.gameObject.CompareTag("EnemyHead"))
        {
            OnJump();
            collision.GetComponentInParent<Enemy>().Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isMove = false;
            ani.SetTrigger("doDie");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
