using UnityEngine;

public enum State
{
    Normal,
    Spawned,
}
public enum ButtonState
{
    Speed1,
    Speed2,
}
public class StateManager : MonoBehaviour
{
    public State currentState = State.Normal;
    public ButtonState buttonState = ButtonState.Speed1;

    public void UpdateState(State newState)
    {
        currentState = newState;
    }
    public void UpdateButtonState(ButtonState newButtonState)
    {
        buttonState = newButtonState;
    }
}
