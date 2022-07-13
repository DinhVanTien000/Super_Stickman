using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderOfEnemy : MonoBehaviour
{
    public EnemyControoller enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("limitH_top"))
        {
            enemy.NotMoveTop = true;
            if (enemy.superFast)
            {
                enemy.SuperSpeedPlus_V();
            }
            if(enemy.blownAway)
            {
                enemy.BounceWall_V();
            }
        }
        else if (collision.gameObject.CompareTag("limitH_below"))
        {
            enemy.NotMoveBelow = true;
            if (enemy.superFast)
            {
                enemy.SuperSpeedPlus_V();
            }
            if (enemy.blownAway)
            {
                enemy.BounceWall_V();
            }
        }
        else if (collision.gameObject.CompareTag("limitV_left"))
        {
            enemy.NotMoveleft = true;
            if (enemy.superFast)
            {
                enemy.SuperSpeedPlus_H();
            }
            if (enemy.blownAway)
            {
                enemy.BounceWall_H();
            }
        }
        else if (collision.gameObject.CompareTag("limitV_right"))
        {
            enemy.NotMoveRight = true;
            if (enemy.superFast)
            {
                enemy.SuperSpeedPlus_H();
            }
            if (enemy.blownAway)
            {
                enemy.BounceWall_H();
            }
        }
        else if(collision.gameObject.CompareTag("p_hit_normal"))
        {
            enemy.ActionHit1();
            enemy.CreateEffectHitNomal();
        }
        else if(collision.gameObject.CompareTag("player"))
        {
            Player.instance.StopMoveAttack();
        }
        else if(collision.gameObject.CompareTag("p_hit_skill1"))
        {
            DelayDestroy(collision.gameObject, 0.1f);
            enemy.CreateEffectHitSkill1();
        }
        else if (collision.gameObject.CompareTag("p_hit_skill2"))
        {
            enemy.CreateEffectHitSkill2();
        }
        else if (collision.gameObject.CompareTag("p_hit_skill3"))
        {
            DelayDestroy(collision.gameObject, 0.25f);
            enemy.CreateEffectHitSkill3();
        }
        print("tag = " + collision.gameObject.tag);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("limitH_top"))
        {
            enemy.NotMoveTop = false;
        }
        else if (collision.gameObject.CompareTag("limitH_below"))
        {
            enemy.NotMoveBelow = false;
        }
        else if (collision.gameObject.CompareTag("limitV_left"))
        {
            enemy.NotMoveleft = false;
        }
        else if (collision.gameObject.CompareTag("limitV_right"))
        {
            enemy.NotMoveRight = false;
        }
        else if (collision.gameObject.CompareTag("player"))
        {
            Player.instance.touchEnemy = false;
        }
    }

    private void DelayDestroy(GameObject g, float timeDelay)
    {
        Destroy(g, timeDelay);
    }
}
