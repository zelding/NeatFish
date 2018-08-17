using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour {

    public readonly static float mutationRate = 0.02f;

    public GameObject FisheContainer;

    public GameObject FishPrototype;
    public uint InitialPopulation = 10;

    private List<EntityManager> Fishes;

    private readonly float spawnBoundary = 50f;
    private bool IsRunning = false;

    private void Awake()
    {
        if ( null == FisheContainer ) {
            throw new System.Exception("No fishcontainer was assigned");
        }

        Fishes = new List<EntityManager>();
    }

    // Use this for initialization
    private void Start () {
        for (int i = 0; i < InitialPopulation; i++) {
            var fish = CreateNewFish();

            Fishes.Add(fish);
        }

        IsRunning = true;
	}

    // Update is called once per frame
    private void Update () {
		
	}

    private void FixedUpdate()
    {
        
    }

    private EntityManager CreateNewFish(EntityManager parent = null)
    {
        Vector3 startPosition;

        do {
            startPosition = new Vector3(Random.Range(-spawnBoundary, spawnBoundary), Random.Range(-spawnBoundary, spawnBoundary), Random.Range(-spawnBoundary, spawnBoundary));
        } while (IsCloseToOthers(startPosition));

        GameObject newFish = Instantiate(FishPrototype, startPosition, Quaternion.identity, FisheContainer.transform);

        var fishSoul = newFish.GetComponent<EntityManager>();

        if ( null != parent ) {
            fishSoul.InheritFrom(parent);
        }

        return fishSoul;
    }

    private bool IsCloseToOthers(Vector3 pos)
    {
        if (Fishes.Count == 0) return false;

        foreach (EntityManager entity in Fishes) {
            float min_x = entity.transform.position.x;
            float max_x = entity.transform.position.x + entity.transform.localScale.x;
            float min_z = entity.transform.position.z;
            float max_z = entity.transform.position.z + entity.transform.localScale.z;

            if (pos.x > min_x && pos.x < max_x && pos.z > min_z && pos.z < max_z) {
                return true;
            }
        }

        return false;
    }
}
