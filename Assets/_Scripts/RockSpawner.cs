using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RockSpawner : Singleton<MonoBehaviour> 
{
    public List<GameObject> RockPrefabs;
    private int nRockCount;

    private Rock SpawnIndexAt(int i, Vector3 position, Quaternion rotation)
    {
        if (i < RockPrefabs.Count)
        {
            GameObject go = (GameObject) Instantiate(RockPrefabs[i], position, rotation);
            Rock rock = go.AddComponent<Rock>();
            rock.transform.name = "Rock" + nRockCount;

            GameObject rockModel = rock.transform.GetChild(0).gameObject;
            MeshCollider mc = rockModel.AddComponent<MeshCollider>();
            mc.convex = true;
            mc.sharedMesh = rockModel.GetComponent<MeshFilter>().mesh;
            if (mc.sharedMesh == null)
            {
                Debug.LogWarning("Rock[ " + rock.transform.name + " ] spawned without sharedMesh.");
            }

            rock.rigidbody = rockModel.AddComponent<Rigidbody>();

            nRockCount++;

            return rock;
        }

        return null;
    }

    public Rock SpawnRandomAt(Vector3 position, Quaternion rotation)
    {
        return SpawnIndexAt(Random.Range(0, RockPrefabs.Count), position, rotation);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SpawnRandomAt(Vector3.zero + Vector3.up * (1 + 2*nRockCount), Quaternion.identity);
        }
    }
}
