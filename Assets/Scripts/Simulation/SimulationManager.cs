using System.Collections.Generic;
using NeatFish.Entities;
using NeatFish.Simulation.NEAT;
using NeatFish.Simulation.Utilities;

namespace NeatFish.Program
{
    public class SimulationManager
    {
        private List<NeuralNet> Brains;

        public NodeIDGenerator NodeIDGenerator { get; }

        public bool IsRunning = false;

        public SimulationManager(uint population, NodeIDGenerator nodeIDGenerator)
        {
            Brains = new List<NeuralNet>();
            NodeIDGenerator = nodeIDGenerator;
        }

        public NeuralNet CreateNewBrain(NeuralNet parent = null)
        {
            NeuralNet brain;

            if (null != parent) {
                brain = new NeuralNet(parent);
            }
            else {
                brain = new NeuralNet(2, 3, NodeIDGenerator);
            }

            brain.GenerateNetwork();

            Brains.Add(brain);

            return brain;
        }

        public void AddBrain(NeuralNet brain)
        {
            Brains.Add(brain);
        }
    }
}