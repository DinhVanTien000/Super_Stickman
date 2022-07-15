using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Buff : Action
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
        enemy.Buff();
    }
    public override TaskStatus OnUpdate()
    {
        if (enemy.buffing) return TaskStatus.Running;
        return TaskStatus.Success;
    }
}
