using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public Vector3 startPoint;
    [Range(0f, 360f)]
    public float lightAngle;
    public float lightRadius;
    public float rayCount;

    private void Update()
    {
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 dir = RoateVector(startPoint, lightAngle / rayCount * i);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, lightRadius);
            if (hit)
            {
                Debug.DrawRay(transform.position, dir * hit.distance, Color.red);
                Debug.Log(hit.point);
            }
            else
            {
                Debug.DrawRay(transform.position, dir * lightRadius, Color.blue);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, lightRadius);

        Gizmos.color = Color.yellow;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 dir = RoateVector(startPoint, lightAngle / rayCount * i) * lightRadius;
            Gizmos.DrawLine(transform.position, dir + transform.position);
        }
    }

    private Vector3 RoateVector(float _angle)
    {
        float radian = _angle * Mathf.Deg2Rad;
        Vector3 a = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian));
        Vector3 b = new Vector3(-Mathf.Sin(radian), Mathf.Cos(radian));

        return (a + b).normalized;
    }

    private Vector3 RoateVector(Vector3 _point, float _angle)
    {
        float radian = _angle * Mathf.Deg2Rad;
        Vector3 a = new Vector3(_point.x * Mathf.Cos(radian), _point.x * Mathf.Sin(radian));
        Vector3 b = new Vector3(-_point.y * Mathf.Sin(radian), _point.y * Mathf.Cos(radian));

        return (a + b).normalized;
    }
}
