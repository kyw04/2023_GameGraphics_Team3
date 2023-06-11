using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DrawLight))]
public class Light : MonoBehaviour
{
    public Vector3 startPoint;
    [Range(0f, 360f)]
    public float lightAngle;
    public float lightRadius;
    public int rayCount;

    private DrawLight draw;

    private void Start()
    {
        draw = GetComponent<DrawLight>();
    }
    
    private void Update()
    {
        draw.Clear();
        draw.AddVertice(transform.position);
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 dir = RoateVector(startPoint, lightAngle / rayCount * i);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, lightRadius, ~LayerMask.GetMask("Shadow"));
            if (hit)
            {
                if (hit.collider.GetComponent<Shadow>())
                {
                    hit.collider.GetComponent<Shadow>().onShadow = true;
                }

                draw.AddVertice(dir * hit.distance + transform.position);
                Debug.DrawRay(transform.position, dir * hit.distance, Color.red);
            }
            else
            {
                draw.AddVertice(dir * lightRadius + transform.position);
                Debug.DrawRay(transform.position, dir * lightRadius, Color.blue);
            }
        }
        draw.DrawMesh();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, lightRadius);

        Gizmos.color = Color.yellow;
        Vector3 startDir = RoateVector(startPoint, 0) * lightRadius;
        Gizmos.DrawLine(transform.position, startDir + transform.position);
        Vector3 endDir = RoateVector(startPoint, lightAngle) * lightRadius;
        Gizmos.DrawLine(transform.position, endDir + transform.position);

        //for (int i = 0; i <= rayCount; i++)
        //{
        //    Vector3 dir = RoateVector(startPoint, lightAngle / rayCount * i) * lightRadius;
        //    Gizmos.DrawLine(transform.position, dir + transform.position);
        //}
    }

    private Vector3 RoateVector(Vector3 _point, float _angle)
    {
        float radian = _angle * Mathf.Deg2Rad;
        Vector3 a = new Vector3(_point.x * Mathf.Cos(radian), _point.x * Mathf.Sin(radian));
        Vector3 b = new Vector3(-_point.y * Mathf.Sin(radian), _point.y * Mathf.Cos(radian));

        return (a + b).normalized;
    }
}
