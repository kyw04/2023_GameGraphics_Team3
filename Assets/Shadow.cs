using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public bool defaultShadow;
    public bool onShadow;
    public SpriteRenderer shadow;

    private void Start()
    {
        onShadow = false;
        StartCoroutine("SetDefault");
    }

    private void Update()
    {
        if (shadow != null)
        {
            shadow.transform.position = new Vector3(transform.position.x, shadow.transform.position.y);
            shadow.enabled = onShadow;
        }
    }

    private IEnumerator SetDefault()
    {
        while (true)
        {
            onShadow = defaultShadow;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
