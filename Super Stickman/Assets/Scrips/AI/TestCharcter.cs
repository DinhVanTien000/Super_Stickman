using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestCharcter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [HideInInspector] public bool IsMoving;
    public void MoveToRight()
    {
        IsMoving = true;

        transform.DOMoveX(5f, 1f).OnComplete(() => {
            IsMoving = false;
        });
    }
}
