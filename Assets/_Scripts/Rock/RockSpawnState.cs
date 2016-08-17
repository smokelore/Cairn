using UnityEngine;
using System.Collections;

[System.Serializable]
public class RockSpawnState : State<RockBehaviour> 
{
    public override IEnumerator OnEnter()
    {
        Debug.Log("SPAWN");
        yield return base.OnEnter();
    }

    public override void OnUpdate()
    {
        parentObject.Counter += Time.deltaTime;
    }
}
