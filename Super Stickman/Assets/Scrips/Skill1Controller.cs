using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1Controller : MonoBehaviour
{
    [SerializeField] private float force;
    private Rigidbody2D rb;
    private Vector3 direction;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Fire(Transform target)
    {
        direction = (target.position - transform.position).normalized;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        Destroy(gameObject, 1.5f);
    }

}
