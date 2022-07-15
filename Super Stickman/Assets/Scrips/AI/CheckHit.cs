using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CheckHit : Conditional
{
    private EnemyControoller enemy;

    public override void OnAwake()
    {
        base.OnAwake();
        enemy = EnemyControoller.instance;
    }
    public override TaskStatus OnUpdate()
    {
        if (enemy.hitting)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
