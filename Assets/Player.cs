using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float radius;
    public float movementSpeed = 5f;
    public float jumpPower = 5f;
    public float coinCount = 0;
    public CameraMove cameraMove;

    private GameObject controlledObj;
    private Rigidbody2D controlledObjRb;
    private SpriteRenderer spriteRenderer;
    private Animator ani;
    private Rigidbody2D rb;
    private CapsuleCollider2D characterBox;
    private BoxCollider2D shadowBox;
    private Vector2 movement;
    private bool isShadow = false;
    private bool isGround = false;
    private bool isMove = true;

    private void Start()
    {
        controlledObj = null;
        controlledObjRb = null;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        characterBox = GetComponent<CapsuleCollider2D>();
        shadowBox = GetComponent<BoxCollider2D>();
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
        
        if (controlledObjRb != null)
        {
            controlledObjRb.AddForce(movement * movementSpeed, ForceMode2D.Force);
        }
        else if (controlledObj != null)
        {
            controlledObj.transform.Translate(movement * movementSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(movement * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if ((gameObject.layer == LayerMask.NameToLayer("Player") && isGround) || gameObject.layer == LayerMask.NameToLayer("Shadow"))
            {
                isMove = false;
                isShadow = !isShadow;

                characterBox.enabled = !isShadow;
                shadowBox.enabled = isShadow;
                spriteRenderer.flipY = isShadow;
                gameObject.transform.position += !isShadow ? Vector3.up : Vector3.zero;
                gameObject.layer = isShadow ? LayerMask.NameToLayer("Shadow") : LayerMask.NameToLayer("Player");
                ani.SetTrigger("Change");
            }
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
            Vector3 pos = transform.position;
            pos.y += 1f;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius, LayerMask.GetMask("Enemy") | LayerMask.GetMask("DynamicObject") | LayerMask.GetMask("Coin"));
            Collider2D selectCollider = null;
            float min = -1;

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Coin"))
                {
                    collider.GetComponent<Coin>().GetCoin(this);
                    continue;
                }

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
                    shadowBox.enabled = true;
                    rb.gravityScale = 1f;
                    spriteRenderer.enabled = true;
                    Vector3 shadowPos = controlledObj.transform.position;
                    shadowPos.y = shadowPos.y - controlledObj.transform.localScale.y / 2f;
                    transform.position = shadowPos;
                    cameraMove.target = transform;
                    controlledObjRb = null;
                    gameObject.layer = LayerMask.NameToLayer("Shadow");
                    controlledObj = null;
                }
                else if (selectCollider && selectCollider.GetComponent<Shadow>().onShadow)
                {
                    gameObject.layer = selectCollider.gameObject.layer;
                    transform.position = selectCollider.transform.position;
                    shadowBox.enabled = false;
                    rb.gravityScale = 0f;
                    rb.velocity = Vector2.zero;
                    spriteRenderer.enabled = false;
                    cameraMove.target = selectCollider.transform;
                    if (selectCollider.GetComponent<Rigidbody2D>())
                        controlledObjRb = selectCollider.GetComponent<Rigidbody2D>();
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

        if (collision.gameObject.CompareTag("End"))
        {
            SceneChangeManager.instance.ChangeScene(collision.gameObject.name);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Vector3 pos = transform.position;
        pos.y += 1f;
        Gizmos.DrawWireSphere(pos, radius);
    }
}
