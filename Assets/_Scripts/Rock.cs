using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{
	public Rigidbody rigidbody;
	public Cairn cairn;

	public float balanceCheckTimestamp;

	public bool isFrozen;

	void Start ()
	{

	}

	private void SetCairn(Cairn newCairn)
	{

	}

	void Update ()
	{

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

		return true;
	}

    void OnCollisionEnter(Collision collision)
    {

    }

    void OnCollisionStay(Collision collision)
	{

	}

	void OnCollisionExit(Collision collision)
	{

	}
}
