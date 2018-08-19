using System.Collections.Generic;
using NeatFish.Entities;
using NeatFish.Simulation.NEAT;
using NeatFish.Simulation.Utilities;

namespace NeatFish.Program
{
    public class SimulationManager
    {
        private List<NeuralNet> Brains;

        public NodeIDGenerator NodeIDGenerator { get { return nodeIDGenerator; } }

        private NodeIDGenerator nodeIDGenerator;

        public bool IsRunning = false;

        public SimulationManager(uint population, NodeIDGenerator nodeIDGenerator)
        {
            Brains = new List<NeuralNet>();
            this.nodeIDGenerator = nodeIDGenerator;
        }

        public void AddBrain(NeuralNet brain)
        {
            Brains.Add(brain);
        }
    }
}