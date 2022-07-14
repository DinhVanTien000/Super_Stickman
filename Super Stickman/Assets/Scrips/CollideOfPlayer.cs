using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideOfPlayer : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("limitH_top"))
        {
            player.NotMoveTop = true;
            if(player.superFast)
            {
                player.SuperSpeedPlus_V();
            }
            if (player.blownAway)
            {
                player.BounceWall_V();
            }
        }
        else if(collision.gameObject.CompareTag("limitH_below"))
        {
            player.NotMoveBelow = true;
            if (player.superFast)
            {
                player.SuperSpeedPlus_V();
            }
            if (player.blownAway)
            {
                player.BounceWall_V();
            }
        }
        else if(collision.gameObject.CompareTag("limitV_left"))
        {
            player.NotMoveleft = true;
            if (player.superFast)
            {
                player.SuperSpeedPlus_H();
            }
            if (player.blownAway)
            {
                player.BounceWall_H();
            }
        }
        else if(collision.gameObject.CompareTag("limitV_right"))
        {
            player.NotMoveRight = true;
            if(player.superFast)
            {
                player.SuperSpeedPlus_H();
            }
            if (player.blownAway)
            {
                player.BounceWall_H();
            }
        }
        else if (collision.gameObject.CompareTag("e_hit_normal"))
        {
            player.ActionHit1();
            player.CreateEffectHitNomal();
        }
        else if (collision.gameObject.CompareTag("e_hit_skill1"))
        {
            DelayDestroy(collision.gameObject, 0.1f);
            player.CreateEffectHitSkill1();
        }
        else if (collision.gameObject.CompareTag("e_hit_skill2"))
        {
            player.CreateEffectHitSkill2();
        }
        else if (collision.gameObject.CompareTag("e_hit_skill3"))
        {
            DelayDestroy(collision.gameObject, 0.25f);
            player.CreateEffectHitSkill3();
        }
        else if(collision.gameObject.CompareTag("enemy"))
        {
            player.StopMoveAttack();
        }
        //print("tag = " + collision.gameObject.tag);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("limitH_top"))
        {
            player.NotMoveTop = false;
        }
        else if (collision.gameObject.CompareTag("limitH_below"))
        {
            player.NotMoveBelow = false;
        }
        else if (collision.gameObject.CompareTag("limitV_left"))
        {
            player.NotMoveleft = false;
        }
        else if (collision.gameObject.CompareTag("limitV_right"))
        {
            player.NotMoveRight = false;
        }
    }
    private void DelayDestroy(GameObject g, float timeDelay)
    {
        Destroy(g, timeDelay);
    }
}
