using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class StaticShadow : MonoBehaviour
{
    private SpriteRenderer shadow;
    private BoxCollider2D box;
    private int layerMasks;

    private void Start()
    {
        if (transform.parent == null)
            return;

        shadow = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        layerMasks = ~(LayerMask.GetMask("Player") | LayerMask.GetMask("Shadow") | LayerMask.GetMask("PlayerFoot"));

        Vector2 colliderSize = box.size;
        Vector2 colliderOffset = Vector2.zero;

        if (shadow != null)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 50f, layerMasks);
            Vector2 minPoint = Vector2.zero;

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject == transform.parent.gameObject)
                    continue;

                if (minPoint == Vector2.zero || minPoint.y > hit.point.y)
                {
                    Debug.Log(hit.point);
                    //Debug.Log(hit.collider.name);
                    colliderSize.y = hit.distance / transform.lossyScale.y;
                    colliderOffset.y = colliderSize.y / 2f;
                    minPoint = hit.point;
                }
            }

            transform.position = minPoint;
            box.size = colliderSize;
            box.offset = colliderOffset;
            box.isTrigger = true;

            shadow.enabled = true;
        }
    }
}
