using UnityEngine;

public enum State
{
    Normal,
    Spawned,
}
public class StateManager : MonoBehaviour
{
    public State currentState = State.Normal;

    public void UpdateState(State newState)
    {
        currentState = newState;
    }
}
