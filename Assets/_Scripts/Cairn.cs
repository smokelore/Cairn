using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cairn : MonoBehaviour
{
	public List<Rock> rocks;

	void Start()
	{
		
	}

	public void AddRock(Rock rock)
	{
		rocks.Add(rock);
	}

	public Rock GetTopRock()
	{
		return rocks[rocks.Count - 1];
	}
}
