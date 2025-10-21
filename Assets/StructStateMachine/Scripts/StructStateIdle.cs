using UnityEngine;

public struct StructStateIdle : IStructState
{
    public IStructState Excute(StructStateMachine sm)
    {
        Debug.Log(nameof(StructStateIdle));

        if (sm.IsInputedChange)
        {
            return new StructStateMove();
        }

        return this;
    }
}
