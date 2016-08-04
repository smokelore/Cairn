using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour 
{
	public static Vector3 velocityThreshold = Vector3.one/50;
	public static Vector3 angularVelocityThreshold = Vector3.one/50;

	public Rigidbody rigidbody;
	public Cairn cairn;

	public static float balanceCheckDuration = 2;
	public float balanceCheckTimestamp;

	public bool isFrozen;

	void Start () 
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	private void SetCairn(Cairn newCairn)
	{
		Debug.Log("Rock [" + transform.name + "] balanced on Cairn [" + cairn.transform.name + "] !");
		cairn = newCairn;
		cairn.AddRock (this);
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
		isFrozen = true;
	}

	void Update () 
	{
		if (cairn != null && !isFrozen) 
		{
			if (CheckBalance()) 
			{
				if (balanceCheckTimestamp + balanceCheckDuration < Time.time) 
				{
					SetCairn (cairn);
				} 
				else 
				{
					this.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, (Time.time - balanceCheckTimestamp)/balanceCheckDuration);
				}
			}
			else
			{
				Debug.Log("Rock [" + transform.name + "] fell off of Cairn [" + cairn.transform.name + "].");
				balanceCheckTimestamp = Time.time;
				this.gameObject.GetComponent<Renderer>().material.color = Color.white;
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
