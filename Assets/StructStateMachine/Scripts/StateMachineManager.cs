using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    [SerializeField]
    private KeyCode _nextStateKey = KeyCode.Space;

    private StructStateMachine _structStateMachine = new();

    private void Update()
    {
        _structStateMachine.Update(Input.GetKeyDown(_nextStateKey));
        _structStateMachine.Excute();
    }
}
