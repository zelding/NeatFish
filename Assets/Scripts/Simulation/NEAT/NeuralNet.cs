using System.Collections.Generic;
using UnityEngine;
using NeatFish.Simulation.Utilities;
using Redzen.Sorting;
using Redzen.Random;

namespace NeatFish.Simulation.NEAT
{
    public class NeuralNet : IMutatable
    {
        public uint Inputs { get; protected set; }
        public uint Outputs { get; protected set; }

        public uint Innovation { get; protected set; }

        public Node[] InputNeurons { get; protected set; }
        public Node[] OutputNeurons { get; protected set; }

        public Dictionary<uint, Vector2> NodeMatrix;

        public List<Node> Neurons { get; protected set; }
        public List<Connection> Connections { get; protected set; }

        protected NodeIDGenerator generator;
        protected readonly IRandomSource _rng = RandomDefaults.CreateRandomSource();

        public NeuralNet(uint inputs, uint outputs, bool bias, NodeIDGenerator nodeIDGenerator)
        {
            Inputs     = inputs;
            Outputs    = outputs;
            Innovation = 0;

            generator = nodeIDGenerator;

            InputNeurons  = new Node[inputs + (bias ? 1 : 0) ];
            OutputNeurons = new Node[outputs];

            NodeMatrix  = new Dictionary<uint, Vector2>();
            Neurons     = new List<Node>();
            Connections = new List<Connection>();

            if (bias) {
                var node = new Node(0, Node.NodeTypes.Bias) {
                    Position = new Vector2(0, 0),
                };

                InputNeurons[0] = node;
                NodeMatrix.Add(node.Id, node.Position);
                Inputs++;
            }

            for (int i = 1; i < Inputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Input) {
                    Position = new Vector2(0, i),
                };

                InputNeurons[i] = node;
                NodeMatrix.Add(node.Id, node.Position);
            }

            for (int i = 0; i < Outputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Output) {
                    Position = new Vector2(int.MaxValue, i),
                };

                OutputNeurons[i] = node;
                NodeMatrix.Add(node.Id, node.Position);
            }

            InitializeConnections();

            Mutate();
        }

        public NeuralNet(NeuralNet parent)
        {
            Inputs     = parent.Inputs;
            Outputs    = parent.Outputs;
            Innovation = parent.Innovation;

            InputNeurons  = new Node[Inputs];
            OutputNeurons = new Node[Outputs];

            NodeMatrix = new Dictionary<uint, Vector2>();

            Neurons     = new List<Node>();
            Connections = new List<Connection>();

            for (int i = 0; i < parent.InputNeurons.Length; i++) {
                var node = new Node(parent.InputNeurons[i]);

                InputNeurons[i] = node;
                NodeMatrix.Add(node.Id, node.Position);
            }

            for (int i = 0; i < parent.OutputNeurons.Length; i++) {
                var node = new Node(parent.OutputNeurons[i]);

                OutputNeurons[i] = node;
                NodeMatrix.Add(node.Id, node.Position);
            }

            foreach (Node x in parent.Neurons) {
                var nn = new Node(x);

                Neurons.Add(nn);
                NodeMatrix.Add(nn.Id, nn.Position);
            }

            InitializeConnections();
            Mutate();
        }

        public double[] FeedForward(double[] inputValues)
        {


            return null;
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

        protected void InitializeConnections()
        {
            ConnectionTemplate[] conTmpl = new ConnectionTemplate[ Inputs * Outputs ];

            for (int i = 0, c = 0; i < Inputs; i++) {
                for (int o = 0; o < Outputs; o++) {
                    conTmpl[c++] = new ConnectionTemplate(generator.Next, i, o);
                }
            }
 
            SortUtils.Shuffle(conTmpl, _rng);


        }

        protected Node[] GetTwoNodes()
        {
            var rtnv = new Node[2];

            Node input  = Neurons[Random.Range(0, Neurons.Count - (int)Outputs)];
            Node output = Neurons[Random.Range((int)Inputs, Neurons.Count)];

            return rtnv;
        }

        protected Node CreateNewNeuron(Node old)
        {
            var n = new Node(generator.Next, Node.NodeTypes.Hidden) {
                Position = new Vector2(old.Position.x + 1, old.Position.y)
            };

            Neurons.Add(n);

            return n;
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