using System.Collections.Generic;
using UnityEngine;
using NeatFish.Simulation.Utilities;

namespace NeatFish.Entities.NEAT
{
    public class NeuralNet : IMutatable
    {
        public int Inputs { get; protected set; }
        public int Outputs { get; protected set; }

        public uint Innovation { get; protected set; }

        public List<Node> Neurons { get; protected set; }
        public List<Connection> Connections { get; protected set; }

        protected NodeIDGenerator generator;

        public NeuralNet(int inputs, int outputs, bool bias, NodeIDGenerator nodeIDGenerator)
        {
            Inputs = inputs;
            Outputs = outputs;
            Innovation = 0;

            generator = nodeIDGenerator;

            Neurons = new List<Node>();
            Connections = new List<Connection>();

            for (int i = 0; i < inputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Input) {
                    Position = new Vector2(0, i)
                };

                Neurons.Add(node);
            }

            if (bias) {
                var node = new Node(0, Node.NodeTypes.Bias) {
                    Position = new Vector2(0, inputs)
                };

                Neurons.Add(node);
                inputs++;
            }

            for (int i = 0; i < outputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Output) {
                    Position = new Vector2(-1, i)
                };

                Neurons.Add(node);
            }

            InitializeConnections();

            Mutate();
        }

        public NeuralNet(NeuralNet parent)
        {
            Inputs = parent.Inputs;
            Outputs = parent.Outputs;
            Innovation = parent.Innovation;

            Neurons = new List<Node>();
            Connections = new List<Connection>();

            foreach (Node n in parent.Neurons) {
                Neurons.Add(new Node(n));
            }

            InitializeConnections();
        }

        public float[] FeedForward(float[] inputValues)
        {
            var output = new float[Outputs];

            foreach (Connection c in Connections) {
                if (c.Enabled) {

                }
            }

            return output;
        }

        public uint Id
        {
            get { return Innovation; }
        }

        public void Mutate()
        {
            float rate = SimulationManager.mutationRate;

            foreach (Connection c in Connections) {
                var r = Random.Range(0f, 1f);

                if (r <= rate) {
                    c.Mutate();

                    c.Enabled = true;
                }
            }

            Innovation++;
        }

        protected void InitializeConnections()
        {
            foreach (Node i in Neurons) {
                foreach (Node o in Neurons) {
                    if (i.type == Node.NodeTypes.Input || i.type == Node.NodeTypes.Bias || i.type == Node.NodeTypes.Hidden) {
                        if (o.type == Node.NodeTypes.Output || o.type == Node.NodeTypes.Hidden) {
                            if (i != o) {
                                var c = new Connection(i, o, 1f, 0f);

                                Connections.Add(c);
                            }
                        }
                    }
                }
            }
        }

        protected Node CreateNewNeuron()
        {
            var n = new Node(generator.Next, Node.NodeTypes.Hidden);

            Neurons.Add(n);

            return n;
        }

    }
}