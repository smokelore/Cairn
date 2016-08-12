using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{
	public Rigidbody rigidbody;
	private Cairn cairn;
    private Rock otherRock;

	public float balanceCheckTimestamp;

	void Start ()
	{
        if (rigidbody == null)
        {
            rigidbody = GetComponentInChildren<Rigidbody>();
        }
    }

    void Update()
    {

    }

    private void SetCairn(Cairn cairn)
	{
        this.cairn = cairn;
        balanceCheckTimestamp = 0.0f;

        if (cairn != null)
        {
            cairn.AddRock(this);
        }
	}

	public bool CheckBalance()
	{
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GameObject go = new GameObject("Cairn");
            SetCairn(go.AddComponent<Cairn>());
        }
    }

    void OnCollisionStay(Collision collision)
	{
        if (cairn == null)
        {
            Rock collisionRock = collision.gameObject.GetComponent<Rock>();
            if (collisionRock != null && collisionRock.cairn != null && collisionRock.cairn.GetTopRock() == collisionRock)
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
                else if (CheckBalance())
                {
                    if (balanceCheckTimestamp + Constants.Instance.ROCK_BALANCE_CHECK_DURATION < Time.time)
                    {
                        SetCairn(otherRock.cairn);
                    }
                }
            }
        }
    }

	void OnCollisionExit(Collision collision)
	{
        Rock collisionRock = collision.gameObject.GetComponent<Rock>();
        if (collisionRock != null)
        {
            if (collisionRock == otherRock)
            {
                SetCairn(null);
            }
        }
    }
    
    public string ToHoverString()
    {
        string hoverString = "";
        if (transform != null)
        {
            hoverString = transform.name;
        }
        if (cairn != null)
        {
            hoverString = cairn.transform.name + " : " + hoverString;
        }

        return hoverString;
    }
}
