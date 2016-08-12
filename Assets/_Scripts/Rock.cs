using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{
	public Rigidbody rigidbody;
	private Cairn cairn;
    private Cairn emptyCairn;
    private Rock otherRock;

	public float balanceCheckTimestamp;

	void Start ()
	{
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        if (cairn == null)
        {
            if (otherRock != null && CheckBalance())
            {
                this.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, (Time.time - balanceCheckTimestamp) / Constants.Instance.ROCK_BALANCE_CHECK_DURATION);

                if (balanceCheckTimestamp + Constants.Instance.ROCK_BALANCE_CHECK_DURATION < Time.time)
                {
                    SetCairn(otherRock.cairn);
                }
            }
        }
    }

    private void SetCairn(Cairn cairn)
	{
        this.cairn = cairn;
        balanceCheckTimestamp = 0.0f;

        if (cairn != null)
        {
            cairn.AddRock(this);
            this.gameObject.GetComponent<Renderer>().material.color = Color.blue;

            if (emptyCairn != null && cairn != emptyCairn)
            {
                Destroy(emptyCairn.gameObject);
            }
        }
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

    void OnCollisionEnter(Collision collision)
    {
        
    }

    void OnCollisionStay(Collision collision)
	{
        if (collision.gameObject.tag == "Ground")
        {
            if (cairn == null && otherRock == null)
            {
                GameObject go = new GameObject("Cairn");
                go.name += Constants.Instance.CAIRN_COUNT;
                emptyCairn = go.AddComponent<Cairn>();
                SetCairn(emptyCairn);
                Constants.Instance.CAIRN_COUNT++;
            }
            else if (cairn != emptyCairn || otherRock != null)
            {
                Destroy(this.gameObject);
            }
        }
        else if (cairn == null)
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
