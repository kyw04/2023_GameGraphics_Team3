using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public Transform point;
    [Range(0f, 360f)]
    public float lightAngle;
    public float lightRadius;
    public float rayCount;

    public Transform target;

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        float angle = Vector3.Angle(dir, point.up);

        RaycastHit2D hit = Physics2D.Raycast(point.position, dir, lightRadius);

        if (angle < lightAngle / 2)
        {
            if (hit)
            {
                Debug.DrawRay(point.position, dir, Color.red);
            }
        }
    }

    private Vector3 RoateVector(Vector3 _point, float _angle)
    {
        float radian = _angle / Mathf.Rad2Deg;
        Vector3 a = new Vector3(_point.x * Mathf.Cos(radian), _point.x * Mathf.Sin(radian));
        Vector3 b = new Vector3(-_point.y * Mathf.Sin(radian), _point.y * Mathf.Cos(radian));

        //Vector3 a = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian));
        //Vector3 b = new Vector3(-Mathf.Sin(radian), Mathf.Cos(radian));
        return (a + b).normalized;
    }
}
