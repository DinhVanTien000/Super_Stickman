using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyAttack : Action
{
    public override void OnAwake()
    {
        EnemyControoller.instance.BtSkill1();
    }
}
