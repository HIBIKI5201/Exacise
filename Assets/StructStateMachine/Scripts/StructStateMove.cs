using UnityEngine;

public struct StructStateMove : IStructState
{
    public IStructState Excute(StructStateMachine sm)
    {
        Debug.Log(nameof(StructStateMove));

        if (sm.IsInputedChange)
        {
            return new StructStateIdle();
        }
        return this;
    }
}
