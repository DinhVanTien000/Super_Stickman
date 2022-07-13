using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 direction;
    [SerializeField] private float force;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Fire(Transform target)
    {
        direction = (target.position - transform.position).normalized;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        Destroy(gameObject, 2f);
    }
}
