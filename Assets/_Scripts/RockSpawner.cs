using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RockSpawner : Singleton<MonoBehaviour> 
{
    public List<GameObject> RockPrefabs;

    private Rock SpawnIndexAt(int i, Vector3 position, Quaternion rotation)
    {
        if (i < RockPrefabs.Count)
        {
            GameObject go = (GameObject) Instantiate(RockPrefabs[i], position, rotation);
            GameObject goRock = go.transform.GetChild(0).gameObject;
            goRock.transform.parent = null;
            Destroy(go);

            Rock rock = goRock.AddComponent<Rock>();
            rock.transform.name = "Rock" + Constants.Instance.ROCK_COUNT;
            
            MeshCollider mc = goRock.AddComponent<MeshCollider>();
            mc.convex = true;
            mc.sharedMesh = goRock.GetComponent<MeshFilter>().mesh;
            if (mc.sharedMesh == null)
            {
                Debug.LogWarning("Rock[ " + rock.transform.name + " ] spawned without sharedMesh.");
            }
            rock.transform.position += Vector3.forward * mc.bounds.extents.z/2;
            rock.transform.position -= Vector3.left * mc.bounds.extents.x/2;

            rock.rigidbody = goRock.AddComponent<Rigidbody>();

            Constants.Instance.ROCK_COUNT++;

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                SpawnRandomAt(hit.point + Vector3.up, Quaternion.identity);
            }
        }
    }
}
