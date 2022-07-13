using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    [SerializeField] private float timeDestroy = 1f;
    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }
}
