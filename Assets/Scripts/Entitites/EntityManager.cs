using UnityEngine;
using NeatFish.Simulation.NEAT;
using NeatFish.Simulation.Utilities;

namespace NeatFish.Entities
{
    public class EntityManager : MonoBehaviour
    {
        protected NeuralNet brain;

        protected Movement Legs;

        public NeuralNet Brain { get { return brain; } }

        public bool Alive { get; protected set; }

        public uint Enegry { get; protected set; }

        public void Awake()
        {
            Movement Legs = new Movement(GetComponent<Rigidbody>(), transform);
        }

        public void assignBrain(NeuralNet brain)
        {
            this.brain = brain;
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