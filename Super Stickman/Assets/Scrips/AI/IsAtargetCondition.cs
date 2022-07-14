using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class IsAtargetCondition : Conditional
{
    public float TargetX = 5f;

    private TestCharcter _testChar;

    public override void OnAwake()
    {
        _testChar = GetComponent<TestCharcter>();
    }

    public override TaskStatus OnUpdate()
    {
        if(_testChar.transform.position.x >= TargetX)
        {
            return TaskStatus.Failure;
        } 
        else
        {
            return TaskStatus.Success;
        }
    }
}
