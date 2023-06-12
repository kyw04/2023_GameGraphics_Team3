using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;

    private float z;

    private void Start()
    {
        z = transform.position.z;
    }

    private void Update()
    {
        float y = 0;
        if (target.position.y > 0)
            y = target.position.y;

        Vector3 pos = new Vector3(target.position.x, y, z);
        transform.position = pos;
    }
}
