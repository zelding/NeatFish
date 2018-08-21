using System.Collections.Generic;
using UnityEngine;
using NeatFish.Simulation.Utilities;

namespace NeatFish.Simulation.NEAT
{
    public class NeuralNet : IMutatable
    {
        public uint Inputs { get; protected set; }
        public uint Outputs { get; protected set; }

        public uint Innovation { get; protected set; }

        public SortedDictionary<uint, Node> Neurons { get; protected set; }
        public List<Connection> Connections { get; protected set; }

        protected NodeIDGenerator generator;

        public NeuralNet(uint inputs, uint outputs, bool bias, NodeIDGenerator nodeIDGenerator)
        {
            Inputs = inputs;
            Outputs = outputs;
            Innovation = 0;

            generator = nodeIDGenerator;

            Neurons     = new SortedDictionary<uint, Node>();
            Connections = new List<Connection>();

            if (bias) {
                var node = new Node(0, Node.NodeTypes.Bias) {
                    Position = new Vector2(-1, 0)
                };

                Neurons.Add(node.Id, node);
                inputs++;
            }

            for (int i = 1; i < inputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Input) {
                    Position = new Vector2(-1, i)
                };

                Neurons.Add(node.Id, node);
            }

            for (int i = 0; i < outputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Output) {
                    Position = new Vector2(int.MaxValue, i)
                };

                Neurons.Add(node.Id, node);
            }

            InitializeConnections();

            Mutate();
        }

        public NeuralNet(NeuralNet parent)
        {
            Inputs = parent.Inputs;
            Outputs = parent.Outputs;
            Innovation = parent.Innovation;

            Neurons = new SortedDictionary<uint, Node>();
            Connections = new List<Connection>();

            foreach (KeyValuePair<uint, Node> x in parent.Neurons) {
                var nn = new Node(x.Value);

                Neurons.Add(nn.Id, nn);
            }

            InitializeConnections();
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
                var r = Random.Range(0f, 1f);

                if (r <= rate) {
                    c.Mutate();
                }
            }

            Innovation++;
        }

        protected void InitializeConnections()
        {
            for (uint i = 0; i < Inputs; i++) {
                for (uint o = Inputs; i < Inputs + Outputs; o++) {
                    var c = new Connection(Neurons[i], Neurons[o], Random.Range(-1f, 1f), Random.Range(-0.5f, 0.5f));

                    Connections.Add(c);
                }
            }
        }

        protected Node CreateNewNeuron()
        {
            var n = new Node(generator.Next, Node.NodeTypes.Hidden);

            Neurons.Add(n.Id, n);

            return n;
        }

    }
}