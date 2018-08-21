using UnityEngine;
using NeatFish.Simulation.NEAT;

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
            var rb = GetComponent<Rigidbody>();

            Legs = new Movement(rb, transform);
        }

        public void AssignBrain(NeuralNet brain)
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
            if (Legs != null && brain != null) {
                Legs.Move(Random.insideUnitSphere);
            }
            else {
                Debug.Log("Nolegs");
            }
        }
    }
}