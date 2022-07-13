using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2Controller : MonoBehaviour
{
    [SerializeField] public GameObject _collider, _collider_enemy;

    public void OnColllier(bool enemy)
    {
        if(!enemy)
        {
            _collider.SetActive(true);
        }
        else
        {
            _collider_enemy.SetActive(true);
        }
        Destroy(gameObject, 0.5f);
    }
}
