using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyAttack : Action
{
    private EnemyControoller enemy;

    public override void OnAwake()
    {
        base.OnAwake();
        enemy = EnemyControoller.instance;
    }
    public override void OnStart()
    {
        base.OnStart();
        enemy.FightNormal();
    }
    public override TaskStatus OnUpdate()
    {
        if (enemy.attacking) return TaskStatus.Running;
        return TaskStatus.Success;
    }
}
