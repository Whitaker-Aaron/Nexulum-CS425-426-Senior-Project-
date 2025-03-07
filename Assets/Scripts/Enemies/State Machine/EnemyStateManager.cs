// State manager for enemies - Aisling

using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour, IStateMachine
{
    // ----------------------------------------------
    // Adjustable in-editor settings for behaviors
    // ----------------------------------------------

    [Header("Movement Settings")]

    public float defaultMovementSpeed = 2f; // Movement speed of enemy
    public float engagementRange = 1f; // How close, from target, the enemy will get to the target (radius). Set with SetEngagementRange(float range)
    public float inaccuracyPointTolerance = 1; // Effectively the "range" surrounding a target point; if the enemy gets within this distance from the point, then it will flag itself as having reached the position
    // ^ Used in EnemySearchState to allow the enemy to 

    // Debugging
    public float currentSpeed;

    // Movement pausing
    public bool movementPaused = false;

    // Lock into idle state instead of patrol
    public bool stayInIdle = false;

    // ----------------------------------------------
    // Components
    // ----------------------------------------------

    [Header("Components")]

    public NavMeshAgent agent;
    public EnemyLOS enemyLOS;
    public EnemyFrame enemyFrame;

    // ----------------------------------------------
    // State objects and state-related variables
    // ----------------------------------------------

    [Header("Concrete States")]

    public bool useDefaultConcreteStates = true;

    // Current state
    [SerializeField] private EnemyState currentState;

    // Concrete states
    [SerializedDictionary("stateName", "stateObj(EnemyState)")]
    public SerializedDictionary<string, EnemyState> concreteStates = new SerializedDictionary<string, EnemyState>();
    
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemySearchState searchState = new EnemySearchState();
    public EnemyPatrolState patrolState = new EnemyPatrolState();

    // Debugging and status effects
    [Header("Debugging and Status Effects")]
    public bool enableStateDebugLogs = false;
    public bool isFrozen = false;

    // ----------------------------------------------
    // Methods
    // ----------------------------------------------

    public void Awake()
    {
        if (useDefaultConcreteStates)
        {
            SetDefaultState(idleState);
            AddState("Idle", idleState);
            AddState("Chase", chaseState);
            AddState("Search", searchState);
        }
        else
        {
            Debug.Log("EnemyStateManager.cs Awake() - useDefaultConcreteStates is toggled false");
        }
    }

    public void Start()
    {
        if (useDefaultConcreteStates == null)
        {
            Debug.LogError("EnemyStateManager.cs Start() - State dictionary is null");
        }

        concreteStates.TrimExcess(); // Trim wasted space in concrete state dictionary

        agent = GetComponent<NavMeshAgent>();
        enemyLOS = GetComponent<EnemyLOS>();
        enemyFrame = GetComponent<EnemyFrame>();

        if (enemyLOS == null)
        {
            Debug.LogError("enemyLOS is null");
        }

        currentSpeed = defaultMovementSpeed;

        ChangeState("Default");
    }

    public void Update()
    {
        enemyFrame = GetComponent<EnemyFrame>();
        agent.speed = currentSpeed;

        if (currentState != null)
        {
            currentState.RunState();
        }
        else
        {
            CustomDebugLog("currentState is null");
        }
    }

    public void ChangeState(string newStateName)
    {

        EnemyState stateOutput;

        if (concreteStates.TryGetValue(newStateName, out stateOutput))
        {
            if (currentState != null)
            {
                currentState.ExitState();
            }
            else
            {
                Debug.LogWarning("EnemyStateManager.cs ChangeState() - Cannot exit previous state, as it is null.");
            }
            currentState = stateOutput;
            currentState.EnterState(this);
        }
        else
        {
            Debug.LogWarning("EnemyStateManager.cs ChangeState() - Requested state '" + newStateName + "' not found in concrete states dictionary.");
        }
    }

    // Deprecated ChangeState function
    // public void ChangeState(EnemyState newState)
    // {
    //     if (currentState != null)
    //     {
    //         currentState.ExitState();
    //     }
    //     currentState = newState;
    //     currentState.EnterState(this);
    // }

    public void CustomDebugLog(string log)
    {
        if (enableStateDebugLogs == true)
        {
            Debug.Log("SM Debug: " + log);
        }
    }

    public void CustomDebugLogError(string log)
    {
        if (enableStateDebugLogs == true)
        {
            Debug.Log("SM Debug ERROR: " + log);
        }
    }

    // Move to a given Vector3 position. Bools for enabling movement with pathfinding (NavMesh) and prediction
    // Pathfinding-less and predicted movement will be supported at a later date, for now this function can only cover movement with pathfinding
    public void MoveTo(Vector3 position, bool enablePrediction = false)
    {
        CustomDebugLog("Moving to " + position);
        agent.SetDestination(position);
    }

    // Overloaded MoveTo, enforces engagement range from point
    public void MoveTo(Vector3 position, float stoppingDist, bool enablePrediction = false)
    {
        float distanceToPos = Vector3.Distance(enemyLOS.selfPos, position);
        Vector3 directionToPos = (position - enemyLOS.selfPos).normalized;

        if (distanceToPos < engagementRange) // Enemy is too close
        {
            Vector3 awayDirection = (enemyLOS.selfPos - position).normalized; // Get direction away from player
            Vector3 awayPos = (enemyLOS.selfPos + awayDirection); // Get the position, away from the player, to go to
            agent.SetDestination(awayPos);
        }
    }

    public void SetEngagementRange(float range)
    {
        engagementRange = range;
    }

    public void LookAt(GameObject lookat)
    {
        float rotationSpeed = 2f;
        Vector3 headingtolookat = lookat.transform.position - transform.position;

        var rotationtolookat = Quaternion.LookRotation(headingtolookat);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationtolookat, rotationSpeed * Time.deltaTime);
    }

    public string GetCurrentTargetTag()
    {
        return enemyLOS.currentTarget.tag;
    }

    public string TargetSpotted()
    {
        return enemyLOS.TargetSpotted();
    }

    public void ResetEnemyState()
    {
        ChangeState("Default");
    }

    public string GetCurrentStateName() // Returns name (string) of current state
    {
        return currentState.stateName;
    }

    public EnemyState GetCurrentState() // Returns the state object of the current state
    {
        return currentState;
    }

    public void PauseMovementFor(float seconds)
    {
        StartCoroutine(StopFor(seconds));
    }

    private IEnumerator StopFor(float s)
    {
        if (!movementPaused)
        {
            movementPaused = true;
            yield return new WaitForSeconds(s);
            movementPaused = false;
        }
    }

    public void AddState(string name, EnemyState state) // Add EnemyState object to dictionary
    {
        do
        {
            if (state == null)
            {
                Debug.LogError("EnemyStateManager.cs AddState() - Expected to receive EnemyState object, but received null.");
                break;
            }

            if (concreteStates.TryAdd(name, state) == false)
            {
                Debug.LogWarning("EnemyStateManager.cs AddState() - Failed to add state '" + name + "' to state dictionary--state already exists.");
                break;
            }
        } while (false);

    }

    public void RemoveState(string name)
    {
        if (concreteStates.Remove(name) == false)
        {
            Debug.LogWarning("EnemyStateManager.cs RemoveState() - State of name '" + name + "' not found in state dictionary.");
        }
    }

    public void SetDefaultState(EnemyState state) // Replace default state with given state, used to change the default state
    {
        if (!concreteStates.ContainsKey("Default"))
        {
            concreteStates.Add("Default", state);
        }
        else
        {
            concreteStates["Default"] = state;
        }
    }

    public EnemyState GetDefaultState()
    {
        EnemyState def = concreteStates["Default"];
        return def;
    }

    public string GetDefaultStateName()
    {
        EnemyState def = concreteStates["Default"];
        return def.GetName();
    }

    public void BuildStateDictionary(EnemyState defaultState, List<EnemyState> stateList)
    {
        if (defaultState != null)
        {
            SetDefaultState(defaultState);
        }
        else
        {
            Debug.LogError("EnemyStateManager.cs BuildStateDictionary() - Expected to receive EnemyState object, but received null.");
        }

        foreach (EnemyState i in stateList)
        {
            AddState(i.GetName(), i);
        }
    }
    
    public bool EnemyIsAtPosition(Vector3 position)
    {
        float distance = enemyLOS.GetDistanceToPosition(position);

        if (distance <= inaccuracyPointTolerance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}