using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class MoveToPlayer : Action
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
        enemy.MoveToPlayer();
    }
    public override TaskStatus OnUpdate()
    {
        if (enemy.touchPlayer || enemy.hitting == false) return TaskStatus.Success;
        return TaskStatus.Running;
    }
}
