// State machine interface - Aisling
// Concrete implementations should also be MonoBehaviours

public interface IStateMachine
{
    // Needs a holder for the current state (generalized), and concrete states
    // EnemyState currentState;
    
    // Enemies should exit their currentState, set currentState to newState, then enter their currentState
    void ChangeState(string newStateName);

    // Used to reset the state of enemies during level loading, implementation should simply change the state to whatever you want the 'default' state to be
    void ResetEnemyState();
}
