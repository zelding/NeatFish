using System.Collections.Generic;
using UnityEngine;

namespace NeatFish.Simulation.NEAT
{ 
    public class Connection : IMutatable
    {
        public Node Input { get; protected set; }

        public Node Output { get; protected set; }

        public float Weight { get; protected set; }

        public float Bias { get; protected set; }

        public bool Enabled { get; set; }

        public uint Innovation { get; protected set; }

        public Connection(Node input, Node output, float weight, float bias)
        {
            if (input.type == Node.NodeTypes.Output || output.type == Node.NodeTypes.Input || output.type == Node.NodeTypes.Bias) {
                throw new System.InvalidOperationException("Input/output node type invalid");
            }

            Input  = input;
            Output = output;
            Weight = weight;
            Bias   = bias;

            Input.AddOutputRef(Output.Id);
            Output.AddInputRef(Input.Id);

            Innovation = 0;
        }

        public void Fire()
        {
            Output.SetValue( Input.Value * Weight + Bias );
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

            Innovation++;

            Enabled = true;
        }

        public uint Id
        {
            get { return Innovation; }
        }

        public static Connection FindBetween(Node i, Node o, List<Connection> list)
        {
            foreach(Connection c in list) {
                if ( c.Input.Id == i.Id && o.Id == c.Output.Id ) {
                    return c;
                }
            }

            return null;
        }
    }
}