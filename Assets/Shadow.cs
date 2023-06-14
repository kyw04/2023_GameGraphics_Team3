using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public bool defaultShadow;
    public bool onShadow;
    public SpriteRenderer shadow;

    private int layerMasks;

    private void Start()
    {
        onShadow = false;
        StartCoroutine("SetDefault");
        layerMasks = ~(LayerMask.GetMask("Player") | LayerMask.GetMask("Shadow") | LayerMask.GetMask("PlayerFoot"));
    }

    private void Update()
    {
        if (shadow != null)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 50f, layerMasks);
            Vector2 minPoint = Vector2.zero;

            foreach (RaycastHit2D hit in hits)
            {
                if (minPoint == Vector2.zero || minPoint.y > hit.point.y)
                {
                    //Debug.Log(hit.collider.name);
                    minPoint = hit.point;
                }
            }

            minPoint.y += 0.35f * shadow.transform.localScale.x;
            shadow.transform.position = minPoint;

            if (shadow)
            {
                Collider2D collider = Physics2D.OverlapBox(shadow.transform.position, shadow.transform.localScale, 0f, LayerMask.GetMask("Player"));
                if (collider)
                {
                    collider.GetComponent<Player>().onChange = true;
                }
            }

            shadow.enabled = onShadow;
        }
    }

    private IEnumerator SetDefault()
    {
        while (true)
        {
            onShadow = defaultShadow;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
