using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer shadow;
    public float radius;
    public float movementSpeed = 5f;

    private GameObject controlledObj;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private bool isShadow = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");

        transform.Translate(movement * movementSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isShadow = !isShadow;

            shadow.enabled = isShadow;
            spriteRenderer.enabled = !isShadow;
            gameObject.layer = isShadow ? LayerMask.NameToLayer("Shadow") : LayerMask.NameToLayer("Player");
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
                else if (selectCollider)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
