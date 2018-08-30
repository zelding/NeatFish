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

        protected List<Node> Network;

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
                    Position = new Vector2(0, i),
                };

                Neurons.Add(node);
            }

            for (int i = 0; i < Outputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Output) {
                    Position = new Vector2(int.MaxValue, i),
                };

                Neurons.Add(node);
            }
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

            for(int i = 0; i < Inputs; i++) {
                Neurons[i].AddValue(inputs[i]);
            }

            foreach(Node n in Network) {
                n.Fire();
            }

            var output = new double[Outputs];

            for(int o = 0; o < Outputs; o++) {
                output[o] = Neurons[o + Inputs].GetValue();
            }

            return output;
        }

        public void GenerateNetwork()
        {
            InitializeConnections();

            Network = new List<Node>();

            for (int l = 0; l < Layers; l++) {
                foreach(Node n in Neurons) {
                    if ( n.Position.x == l ) {
                        Network.Add(n);
                    }
                }
            }
        }

        protected void InitializeConnections()
        {
            ConnectionTemplate[] conTmpl = new ConnectionTemplate[ Inputs * Outputs ];

            for (int i = 0, c = 0; i < Inputs; i++) {
                for (int o = 0; o < Outputs; o++) {
                    conTmpl[c++] = new ConnectionTemplate(generator.Next, i, o);
                }
            }
 
            SortUtils.Shuffle(conTmpl, _rng);

            int connectionCount = (int)NumericsUtils.ProbabilisticRound(conTmpl.Length * 0.05, _rng);
            connectionCount = System.Math.Max(1, connectionCount);

            for(int i = 0; i < connectionCount; i++) {

                Node input  = Neurons[ i ];
                Node output = Neurons[ i + (int)Inputs ];

                Connection con = new Connection(_rng.NextUInt(), input, output, _rng.NextDouble() * 2.0 - 1.0, _rng.NextDouble() * - 5.0);

                Connections.Add(con);
            }
        }

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