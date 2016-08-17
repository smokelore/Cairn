using UnityEngine;
using System.Collections;

[System.Serializable]
public class RockFallState : State<RockBehaviour>
{
    public bool isFalling;

    public override IEnumerator OnEnter()
    {
        Debug.Log("FALL");
        yield return base.OnEnter();
    }

    public override void OnUpdate()
    {
        if (parentObject.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= 1.0f)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
    }
}
