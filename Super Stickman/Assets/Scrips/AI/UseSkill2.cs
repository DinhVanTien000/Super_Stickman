using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class UseSkill2 : Action
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
        enemy.UseSkill2();
    }
    public override TaskStatus OnUpdate()
    {
        if (enemy.useSkill) return TaskStatus.Running;
        return TaskStatus.Success;
    }
}
