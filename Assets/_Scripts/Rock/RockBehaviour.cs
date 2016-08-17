using UnityEngine;
using System.Collections;

public class RockBehaviour : MonoBehaviour 
{
    public RockSpawnState SpawnState;
    public RockFallState FallState;
    public RockBalanceState BalanceState;
    public float Counter;

    StateMachine<RockBehaviour> State;
    
    void Awake()
    {
        State = new StateMachine<RockBehaviour>(this);

        StartCoroutine(State.SwitchState(BalanceState));
    }

    void Update()
    {
        State.Update();

        //if (State.CurrentState == SpawnState)
        //{
        //    StartCoroutine(State.SwitchState(FallState));
        //}
    }
}
