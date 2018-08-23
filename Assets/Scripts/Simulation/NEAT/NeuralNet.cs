using System.Collections.Generic;
using UnityEngine;
using NeatFish.Simulation.Utilities;
using System.Linq;

namespace NeatFish.Simulation.NEAT
{
    public class NeuralNet : IMutatable
    {
        public uint Inputs { get; protected set; }
        public uint Outputs { get; protected set; }

        public uint Innovation { get; protected set; }

        public List<Node> Neurons { get; protected set; }
        public List<Connection> Connections { get; protected set; }

        protected NodeIDGenerator generator;

        public NeuralNet(uint inputs, uint outputs, bool bias, NodeIDGenerator nodeIDGenerator)
        {
            Inputs = inputs;
            Outputs = outputs;
            Innovation = 0;

            generator = nodeIDGenerator;

            Neurons     = new List<Node>();
            Connections = new List<Connection>();

            if (bias) {
                var node = new Node(0, Node.NodeTypes.Bias) {
                    Position = new Vector2(-1, 0)
                };

                Neurons.Add(node);
                Inputs++;
            }

            for (int i = 1; i < Inputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Input) {
                    Position = new Vector2(-1, i)
                };

                Neurons.Add( node);
            }

            for (int i = 0; i < Outputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Output) {
                    Position = new Vector2(int.MaxValue, i)
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

            foreach (Node x in parent.Neurons) {
                var nn = new Node(x);

                Neurons.Add(nn);
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
            for (int i = 0; i < Inputs; i++) {
                for (int o = (int)Inputs; o <Inputs + Outputs; o++) {
                    var c = new Connection(Neurons[i], Neurons[o], Random.Range(-1f, 1f), Random.Range(-0.5f, 0.5f));

                    Connections.Add(c);
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