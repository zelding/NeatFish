using UnityEngine;
using NeatFish.Entities.NEAT;
using NeatFish.Simulation.Utilities;

namespace NeaftFish.Entities
{
    public class EntityManager : MonoBehaviour
    {
        protected NeuralNet Brain;

        protected Movement Legs;

        public bool Alive { get; protected set; }

        public uint Enegry { get; protected set; }

        public void Awake()
        {
            Movement Legs = new Movement(GetComponent<Rigidbody>(), transform);
        }

        public void assignBrain(NodeIDGenerator nodeIDGenerator)
        {
            Brain = new NeuralNet(5, 3, true, nodeIDGenerator);
        }

        public void InheritFrom(EntityManager parent)
        {
            Brain = new NeuralNet(parent.Brain);
        }

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void FixedUpdate()
        {

        }
    }
}