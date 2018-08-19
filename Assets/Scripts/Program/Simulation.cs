using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeatFish.Entities;
using NeatFish.Simulation.NEAT;
using NeatFish.Simulation.Utilities;
using NeatFish.Program;

public class Simulation : MonoBehaviour {

    public readonly static float mutationRate = 0.02f;

    public GameObject FisheContainer;
    public GameObject FishPrototype;

    [Range(1, 250)]
    public uint InitialPopulation = 10;

    public Vector3 SpawnBoundaryCenter { get { return spawnBoundaryCenter; } }
    public float SpawnBoundaryRaduis { get { return spawnBoundaryRaduis; } }

    protected readonly Vector3 spawnBoundaryCenter;
    protected readonly float spawnBoundaryRaduis;

    protected List<EntityManager> fishes;

    private NodeIDGenerator nodeIDGenerator;
    private SimulationManager manager;

    private void Awake()
    {
        if ( null == FisheContainer ) {
            throw new System.Exception("No fishcontainer was assigned");
        }

    }

    // Use this for initialization
    private void Start () {
        nodeIDGenerator = new NodeIDGenerator();
        manager         = new SimulationManager(InitialPopulation, nodeIDGenerator);
        fishes          = new List<EntityManager>();

        for (int i = 0; i < InitialPopulation; i++) {
            var fish = CreateNewFish();

            manager.AddBrain(fish.Brain);
        }

        manager.IsRunning = true;

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
            startPosition = new Vector3(
                Random.Range(-spawnBoundaryRaduis, spawnBoundaryRaduis), 
                Random.Range(-spawnBoundaryRaduis, spawnBoundaryRaduis), 
                Random.Range(-spawnBoundaryRaduis, spawnBoundaryRaduis)
            );

        } while (IsCloseToOthers(startPosition));

        GameObject newFish = Instantiate(FishPrototype, startPosition, Quaternion.identity, FisheContainer.transform);

        EntityManager fishSoul = newFish.AddComponent<EntityManager>();

        NeuralNet brain = null;

        if (null != parent) {
            brain = new NeuralNet(parent.Brain);
        }
        else {
            brain = new NeuralNet(10, 10, true, nodeIDGenerator);
        }

        fishSoul.assignBrain(brain);

        return fishSoul;
    }

    private bool IsCloseToOthers(Vector3 pos)
    {
        if (fishes.Count == 0) return false;

        foreach (EntityManager entity in fishes) {
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
