using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public bool onShadow;
    public SpriteRenderer shadow;

    private void Start()
    {
        onShadow = false;
    }

    private void Update()
    {
        shadow.transform.position = new Vector3(transform.position.x, shadow.transform.position.y);
        shadow.enabled = onShadow;
        onShadow = false;
    }
}
