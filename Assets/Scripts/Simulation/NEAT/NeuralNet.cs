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

        public Node[] InputNeurons { get; protected set; }
        public Node[] OutputNeurons { get; protected set; }

        public List<Node> Neurons { get; protected set; }
        public List<Connection> Connections { get; protected set; }

        protected NodeIDGenerator generator;

        public NeuralNet(uint inputs, uint outputs, bool bias, NodeIDGenerator nodeIDGenerator)
        {
            Inputs     = inputs;
            Outputs    = outputs;
            Innovation = 0;

            generator = nodeIDGenerator;

            InputNeurons  = new Node[inputs + (bias ? 1 : 0) ];
            OutputNeurons = new Node[outputs];

            Neurons     = new List<Node>();
            Connections = new List<Connection>();

            if (bias) {
                var node = new Node(0, Node.NodeTypes.Bias) {
                    Position = new Vector2(0, 0),
                };

                InputNeurons[0] = node;
                Inputs++;
            }

            for (int i = 1; i < Inputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Input) {
                    Position = new Vector2(0, i),
                };

                InputNeurons[i] = node;
            }

            for (int i = 0; i < Outputs; i++) {
                var node = new Node(generator.Next, Node.NodeTypes.Output) {
                    Position = new Vector2(int.MaxValue, i),
                };

                OutputNeurons[i] = node;
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

            Neurons     = new List<Node>();
            Connections = new List<Connection>();

            for (int i = 0; i < parent.InputNeurons.Length; i++) {
                InputNeurons[i] = new Node(parent.InputNeurons[i]);
            }

            for (int i = 0; i < parent.OutputNeurons.Length; i++) {
                OutputNeurons[i] = new Node(parent.OutputNeurons[i]);
            }

            foreach (Node x in parent.Neurons) {
                var nn = new Node(x);

                Neurons.Add(nn);
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
                var r = Random.Range(0f, 1f);

                if (r <= rate) {
                    c.Mutate();

                    Innovation++;
                }
            }

            var rn = Random.Range(0f, 1f);

            if (rn <= rate) {

            }
        }

        protected void InitializeConnections()
        {
            for (int i = 0; i < Inputs; i++) {
                for (int o = (int)Inputs; o < Neurons.Count; o++) {
                    var c = new Connection(Neurons[i], Neurons[o], Random.Range(-1f, 1f), Random.Range(-0.5f, 0.5f));

                    Connections.Add(c);
                }
            }
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

    }
}