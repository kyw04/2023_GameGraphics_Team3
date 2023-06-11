using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public bool onShadow;
    public GameObject shadow;

    private void Start()
    {
        onShadow = false;
    }

    private void Update()
    {
        shadow.SetActive(onShadow);
        onShadow = false;
    }
}
