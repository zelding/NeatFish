using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeatFish.Entities;
using NeatFish.Simulation.NEAT;
using NeatFish.Simulation.Utilities;
using NeatFish.Program;

public class Simulation : MonoBehaviour {

    public readonly static float mutationRate = 0.92f;

    public GameObject FisheContainer;
    public GameObject FishPrototype;

    public GameObject Inspector;

    [Range(1, 250)]
    public uint InitialPopulation = 1;

    public Vector3 SpawnBoundaryCenter { get { return spawnBoundaryCenter; } }
    public float SpawnBoundaryRaduis { get { return spawnBoundaryRaduis; } }

    protected Vector3 spawnBoundaryCenter;
    protected float spawnBoundaryRaduis;

    protected List<EntityManager> fishManagers;

    protected Inspector inspector;

    private NodeIDGenerator nodeIDGenerator;
    private SimulationManager manager;

    private void Awake()
    {
        if ( null == FisheContainer ) {
            throw new System.Exception("No fishcontainer was assigned");
        }

        inspector = Inspector.GetComponent<Inspector>();
        inspector.enabled = false;
    }

    // Use this for initialization
    private void Start () {
        nodeIDGenerator = new NodeIDGenerator();
        manager         = new SimulationManager(InitialPopulation, nodeIDGenerator);
        fishManagers    = new List<EntityManager>();

        spawnBoundaryCenter = new Vector3(0, 0, 0);
        spawnBoundaryRaduis = 67f;

        for (int i = 0; i < InitialPopulation; i++) {
            var fish = CreateNewFish();

            fishManagers.Add(fish);
            manager.AddBrain(fish.Brain);
        }

        manager.IsRunning = true;

        inspector.Brain = fishManagers[0].Brain;
        inspector.enabled = true;
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
            ) + spawnBoundaryCenter;

        } while (IsCloseToOthers(startPosition));

        GameObject newFish = Instantiate(FishPrototype, startPosition, Quaternion.identity, FisheContainer.transform);

        EntityManager fishManager = newFish.GetComponent<EntityManager>();

        NeuralNet brain = manager.CreateNewBrain(parent != null ? parent.Brain : null);

        fishManager.AssignBrain(brain);

        return fishManager;
    }

    private bool IsCloseToOthers(Vector3 pos)
    {
        if (fishManagers.Count == 0) return false;

        foreach (EntityManager entity in fishManagers) {
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
