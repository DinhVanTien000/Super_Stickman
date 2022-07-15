using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AttackCondition : Conditional
{
    private EnemyControoller enemy;
    public bool downstream;

    public override void OnAwake()
    {
        base.OnAwake();
        enemy = EnemyControoller.instance;
    }
    public override TaskStatus OnUpdate()
    {
        if(!downstream)
        {
            if (enemy.touchPlayer)
            {
                return TaskStatus.Failure;
            }
            else
            {
                return TaskStatus.Success;
            }
        }
        else
        {
            if (enemy.touchPlayer)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }
}
