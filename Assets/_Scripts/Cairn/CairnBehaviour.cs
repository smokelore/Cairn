using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CairnBehaviour : MonoBehaviour 
{
    public List<RockBehaviour> rocks;

    public void AddRock(RockBehaviour rock)
    {
        if (rocks == null)
        {
            rocks = new List<RockBehaviour>();
        }
        rocks.Add(rock);
        rock.transform.parent = transform;
    }

    public RockBehaviour GetTopRock()
    {
        if (rocks == null || rocks.Count == 0)
        {
            return null;
        }

        return rocks[rocks.Count - 1];
    }

    public bool ContainsRock(RockBehaviour rock)
    {
        return rocks.Contains(rock);
    }
}
