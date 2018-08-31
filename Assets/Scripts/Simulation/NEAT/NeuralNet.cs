using System.Collections.Generic;
using UnityEngine;
using NeatFish.Simulation.Utilities;
using Redzen.Sorting;
using Redzen.Random;
using Redzen.Numerics;

namespace NeatFish.Simulation.NEAT
{
    public class NeuralNet : IMutatable
    {
        public int Inputs { get; }
        public int Outputs { get; }

        public uint Layers = 2;

        public uint Innovation { get; protected set; }

        public List<Node> Neurons { get; protected set; }
        public List<Connection> Connections { get; protected set; }

        protected NodeIDGenerator generator;
        protected readonly IRandomSource _rng = RandomDefaults.CreateRandomSource();

        public NeuralNet(int inputs, int outputs, NodeIDGenerator nodeIDGenerator)
        {
            Inputs     = inputs;
            Outputs    = outputs;
            Innovation = 0;

            generator = nodeIDGenerator;

            Neurons     = new List<Node>();
            Connections = new List<Connection>();

            for (int i = 0; i < Inputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Input) {
                    Position = new Vector2Int(0, i),
                };

                Neurons.Add(node);
            }

            for (int i = 0; i < Outputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Output) {
                    Position = new Vector2Int(int.MaxValue, i),
                };

                Neurons.Add(node);
            }

            InitializeConnections();
        }

        public NeuralNet(NeuralNet parent)
        {
            Inputs     = parent.Inputs;
            Outputs    = parent.Outputs;
            Innovation = parent.Innovation;

            Neurons     = new List<Node>();
            Connections = new List<Connection>();

            foreach (Node x in parent.Neurons) {
                var n = new Node(x);
                Neurons.Add(n);
            }
        }

        public uint Id
        {
            get { return Innovation; }
        }

        public void Mutate()
        {
            float rate = global::Simulation.mutationRate;

            foreach (Connection c in Connections) {
                var r = _rng.NextDouble();

                if (r <= rate) {
                    c.Mutate();

                    Innovation++;
                }
            }

            var rn = _rng.NextDouble();

            if (rn <= rate) {
                // enable a connection

                Innovation++;
            } else if ( rn <= rate*2 ) {
                // add node

                Innovation++;
            }
        }

        public double[] Activate(double[] inputs)
        {
            if (inputs.Length != Inputs) {
                throw new System.IndexOutOfRangeException("Input length invalid, should be: " + Inputs.ToString());
            }

            Neurons.Sort(new Node.NodeSorter());

            for (int i = 0; i < Inputs; i++) {
                Neurons[i].AddValue(inputs[i]);
            }

            foreach(Node n in Neurons) {
                n.Fire();
            }

            var output = new double[Outputs];

            for(int o = 0; o < Outputs; o++) {
                output[o] = Neurons[o + Inputs].GetValue();
            }

            return output;
        }

        /// <summary>
        /// Sets up the initial connections for a brand new Net
        /// </summary>
        protected void InitializeConnections()
        {
            ConnectionTemplate[] conTmpl = new ConnectionTemplate[ Inputs * Outputs ];

            // create templates for all possible connections
            for (int i = 0, c = 0; i < Inputs; i++) {
                for (int o = 0; o < Outputs; o++) {
                    conTmpl[c++] = new ConnectionTemplate(generator.Next, i, o);
                }
            }
            
            // mix them up
            SortUtils.Shuffle(conTmpl, _rng);

            // make up a number between the max possible connections and zero
            int connectionCount = (int)NumericsUtils.ProbabilisticRound(conTmpl.Length * 0.075, _rng);
            connectionCount     = System.Math.Max(2, connectionCount);

            // create that many actual connections
            for(int i = 0; i < connectionCount; i++) {

                Node input  = Neurons[ i ];
                Node output = Neurons[ i + Inputs ];

                Connection con = new Connection(_rng.NextUInt(), input, output, _rng.NextDouble() * 2.0 - 1.0, _rng.NextDouble() * -5.0) {
                    Enabled = true
                };

                Connections.Add(con);
            }
        }

        /// <summary>
        /// returns whether the network is fully connected or not
        /// </summary>
        /// <returns></returns>
        protected bool IsFullyConnected()
        {
            int maxConnections = 0;
            int[] nodelsInLayers = new int[Layers];

            for(int i = 0; i < Neurons.Count; i++) {
                nodelsInLayers[Neurons[i].Position.x]++;
            }

            //for each layer the maximum amount of connections is the number in this layer * the number of nodes infront of it
            //so lets add the max for each layer together and then we will get the maximum amount of connections in the network
            for (int i = 0; i < Layers - 1; i++) {
                int nodesInFront = 0;
                //for each layer infront of this layer
                for (int j = i + 1; j < Layers; j++) {
                    //add up nodes
                    nodesInFront += nodelsInLayers[j];
                }

                maxConnections += nodelsInLayers[i] * nodesInFront;
            }

            return maxConnections == Neurons.Count;
        }

        /// <summary>
        /// A simple struct to represent a connection before actually creating ther class
        /// </summary>
        private struct ConnectionTemplate
        {
            public readonly uint innovationId;
            public readonly int inputIndex;
            public readonly int outputIndex;

            public ConnectionTemplate(uint iid, int input, int output)
            {
                innovationId = iid;
                inputIndex   = input;
                outputIndex  = output;
            }
        }
    }
}