using UnityEngine;

namespace NeatFish.Simulation.NEAT
{ 
    public class Connection : IMutatable
    {
        public Node Input { get; protected set; }

        public Node Output { get; protected set; }

        public double Weight { get; protected set; }

        public double Bias { get; protected set; }

        public bool Enabled { get; set; }

        public uint Innovation { get; protected set; }

        public Connection(uint innovation, Node input, Node output, double weight, double bias)
        {
            if (input.type == Node.NodeTypes.Output || output.type == Node.NodeTypes.Input || output.type == Node.NodeTypes.Bias) {
                throw new System.InvalidOperationException("Input/output node type invalid");
            }

            Input  = input;
            Output = output;
            Weight = weight;
            Bias   = bias;

            Innovation = innovation;

            Enabled = false;

            Input.RegisterConnection(this);
        }

        public void Mutate()
        {
            var r = Random.Range(0f, 1f);

            if ( r <= (1f / 6f) ) {
                Weight += Random.Range(-0.5f, 0.5f);
            }
            else if ( r <= (2f / 6f) ) {
                Weight *= Random.Range(-1f, 1f);
            }
            else if (r <= 0.5) {
                Weight = Random.Range(-1f, 1f);
            }
            else if (r <= (2f / 3f)) {
                Bias += Random.Range(-0.5f, 0.5f);
            }
            else if (r <= (5f /6f) ) {
                Bias *= Random.Range(-1f, 1f);
            }
            else {
                Bias += Random.Range(-1f, 1f);
            }

            Enabled = true;
        }

        public uint Id
        {
            get { return Innovation; }
        }

    }
}