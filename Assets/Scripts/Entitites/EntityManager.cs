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
                double[] output = brain.Activate(new double[] { 1, 2 });

                Legs.Move(new Vector3((float)output[0], (float)output[1], (float)output[3]));
            }
            else {
                //Debug.Log("Nolegs");
            }
        }
    }
}