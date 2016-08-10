using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cairn : MonoBehaviour
{
	public List<Rock> rocks;

	void Start()
	{
		rocks = new List<Rock>();
	}

	public void AddRock(Rock rock)
	{
		rocks.Add(rock);
		rock.transform.parent = transform;
	}

	public Rock GetTopRock()
	{
		if (rocks == null || rocks.Count == 0)
		{
			return null;
		}

		return rocks[rocks.Count - 1];
	}

	public bool ContainsRock(Rock rock)
	{
		return rocks.Contains(rock);
	}
}
