using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour 
{
	public static Vector3 velocityThreshold = Vector3.one;
	public static Vector3 angularVelocityThreshold = Vector3.one;

	public Rigidbody rigidbody;
	public Cairn cairn;

	public static float balanceCheckDuration = 2;
	public float balanceCheckTimestamp;

	public bool isFrozen;

	void Start () 
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update () 
	{
		if (cairn != null && !isFrozen) 
		{
			if (CheckBalance()) 
			{
				if (balanceCheckTimestamp + balanceCheckDuration < Time.time) 
				{
					Debug.Log("Rock [" + transform.name + "] balanced on Cairn [" + cairn.transform.name + "] !");
					rigidbody.constraints = RigidbodyConstraints.FreezeAll;
					cairn.AddRock(this);
					isFrozen = true;
				}
			}
			else
			{
				Debug.Log("Rock [" + transform.name + "] fell off of Cairn [" + cairn.transform.name + "].");
				balanceCheckTimestamp = Time.time;
			}
		}
	}

	public bool CheckBalance()
	{
		if (rigidbody.velocity.x > velocityThreshold.x
		    || rigidbody.velocity.y > velocityThreshold.y
		    || rigidbody.velocity.z > velocityThreshold.z
			|| rigidbody.angularVelocity.x > angularVelocityThreshold.x
			|| rigidbody.angularVelocity.y > angularVelocityThreshold.y
			|| rigidbody.angularVelocity.z > angularVelocityThreshold.z) 
		{
			return false;
		}

		if (cairn == null) 
		{
			return false;
		}

		return true;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (!isFrozen) 
		{
			Rock collidingRock = collision.gameObject.GetComponent<Rock>();
			if (collidingRock != null && collidingRock.isFrozen && collidingRock == collidingRock.cairn.GetTopRock()) 
			{
				cairn = collidingRock.cairn;
				balanceCheckTimestamp = Time.time;
			}
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (!isFrozen) 
		{
			Rock collidingRock = collision.gameObject.GetComponent<Rock>();
			if (cairn != null && collidingRock != null && collidingRock.isFrozen) 
			{
				if (cairn == collidingRock.cairn && collidingRock == collidingRock.cairn.GetTopRock()) 
				{
					cairn = null;
					balanceCheckTimestamp = 0;
				}
			}
		}
	}
}
