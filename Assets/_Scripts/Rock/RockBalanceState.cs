using UnityEngine;
using System.Collections;

[System.Serializable]
public class RockBalanceState : MonoState<RockBehaviour>
{
    public new Rigidbody rigidbody;
    public CairnBehaviour cairn;
    private CairnBehaviour emptyCairn;
    private RockBalanceState otherRock;
    
    public float balanceCheckTimestamp;

    public override IEnumerator OnEnter()
    {
        Debug.Log("BALANCE");

        if (rigidbody == null)
        {
            rigidbody = parentObject.gameObject.GetComponent<Rigidbody>();
        }

        yield return base.OnEnter();
    }

    public override void OnUpdate()
    {
        if (cairn == null && CheckBalance())
        {
            if (balanceCheckTimestamp + Constants.Instance.ROCK_BALANCE_CHECK_DURATION < Time.time)
            {
                SetCairn(otherRock.cairn);
            }
        }

        UpdateColor();
    }

    public bool CheckBalance()
    {
        if (rigidbody == null)
        {
            return false;
        }

        if (rigidbody.velocity.x > Constants.Instance.ROCK_VEL_THRESHOLD.x
            || rigidbody.velocity.y > Constants.Instance.ROCK_VEL_THRESHOLD.y
            || rigidbody.velocity.z > Constants.Instance.ROCK_VEL_THRESHOLD.z
            || rigidbody.angularVelocity.x > Constants.Instance.ROCK_ANGVEL_THRESHOLD.x
            || rigidbody.angularVelocity.y > Constants.Instance.ROCK_ANGVEL_THRESHOLD.y
            || rigidbody.angularVelocity.z > Constants.Instance.ROCK_ANGVEL_THRESHOLD.z)
        {
            return false;
        }

        if (otherRock == null || otherRock.cairn == null)
        {
            return false;
        }

        return true;
    }

    private void SetCairn(CairnBehaviour cairn)
    {
        this.cairn = cairn;

        if (cairn != null)
        {
            cairn.AddRock(parentObject.GetComponent<RockBehaviour>());

            if (emptyCairn != null && cairn != emptyCairn)
            {
                Destroy(emptyCairn.gameObject);
            }
        }
    }

    public void UpdateColor()
    {
        Color newColor = Color.white;

        if (cairn == null)
        {
            if (CheckBalance())
            {
                newColor = Color.Lerp(Color.white, Color.red, (Time.time - balanceCheckTimestamp) / Constants.Instance.ROCK_BALANCE_CHECK_DURATION);
            }
            else
            {
                newColor = Color.yellow;
            }
        }
        else
        {
            newColor = Color.blue;
        }

        parentObject.GetComponent<Renderer>().material.color = newColor;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (cairn == null && otherRock == null)
            {
                if (emptyCairn == null)
                {
                    GameObject go = new GameObject("Cairn");
                    go.name += Constants.Instance.CAIRN_COUNT;
                    emptyCairn = go.AddComponent<CairnBehaviour>();
                }
                SetCairn(emptyCairn);
                Constants.Instance.CAIRN_COUNT++;
            }
            else if (cairn != emptyCairn || otherRock != null)
            {
                Destroy(parentObject);
            }
        }
        else if (cairn == null)
        {
            RockBalanceState collisionRock = collision.gameObject.GetComponent<RockBalanceState>();
            if (collisionRock != null && collisionRock.cairn != null)
            {
                if (otherRock == null)
                {
                    otherRock = collisionRock;
                    balanceCheckTimestamp = Time.time;
                }
                else if (collisionRock != otherRock)
                {
                    SetCairn(null);
                    otherRock = collisionRock;
                    balanceCheckTimestamp = Time.time;
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        RockBalanceState collisionRock = collision.gameObject.GetComponent<RockBalanceState>();
        if (collisionRock != null)
        {
            if (collisionRock == otherRock)
            {
                SetCairn(null);
            }
        }
    }
}
