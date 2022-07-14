using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Action
{
    private TestCharcter _testChar;

    public override void OnAwake()
    {
        base.OnAwake();
        _testChar = GetComponent<TestCharcter>();
    }
    public override void OnStart()
    {
        base.OnStart();

        _testChar.MoveToRight();
    }

    public override TaskStatus OnUpdate()
    {
        if (_testChar.IsMoving) return TaskStatus.Running;
        return TaskStatus.Success;
    }
}
