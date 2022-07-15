using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class UseSkill3 : Action
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
        enemy.UseSkill3();
    }
    public override TaskStatus OnUpdate()
    {
        if (enemy.useSkill) return TaskStatus.Running;
        return TaskStatus.Success;
    }
}
