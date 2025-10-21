public class StructStateMachine
{
    public void Update(bool isInputedChange)
    {
        _isInputedChange = isInputedChange;
    }

    public void Excute() => _currentState = _currentState.Excute(this);

    public bool IsInputedChange => _isInputedChange;

    private bool _isInputedChange;

    private IStructState _currentState = new StructStateIdle();
}
