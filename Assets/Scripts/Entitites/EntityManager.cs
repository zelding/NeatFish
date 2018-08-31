using UnityEngine;
using NeatFish.Simulation.NEAT;

namespace NeatFish.Entities
{
    public class EntityManager : MonoBehaviour
    {
        public double Speed = 0.0;

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
                Vector2 asd = Random.insideUnitCircle * Random.Range(-2, 2);

                double[] input = new double[2] {asd .x, asd.y };

                double[] output = brain.Activate(input);

                Legs.Move(new Vector3((float)output[0], (float)output[1], (float)output[2]));
            }
            else {
                //Debug.Log("Nolegs");
            }
        }
    }
}