using System.Collections;
using System.Collections.Generic;

namespace NeatFish.Simulation.NEAT
{
    public class Node : IMutatable
    {
        public enum NodeTypes { Input, Hidden, Output, Bias };

        public readonly NodeTypes type;

        public double Value { get; protected set; }

        public double PreviousValue { get; protected set; }

        public uint Innovation { get; protected set; }

        public UnityEngine.Vector2 Position { get; set; }

        protected HashSet<uint> Inputs;
        protected HashSet<uint> Outputs;

        public Node(uint id, NodeTypes type)
        {
            Inputs  = new HashSet<uint>();
            Outputs = new HashSet<uint>();

            Innovation    = id;
            Value         = 0;
            PreviousValue = Value;
            Innovation    = 0;

            this.type = type;
        }

        public Node(Node n)
        {
            Inputs   = new HashSet<uint>(); // Dunno wich
            Outputs  = n.Outputs;

            Value = 0;
            PreviousValue = 0;
            Innovation = n.Innovation;
            Position = n.Position;

            type = n.type;
        }

        public void SetValue(double v)
        {
            PreviousValue = Value;
            Value         = v;
        }

        public void AddInputRef(uint reference)
        {
            Inputs.Add(reference);
        }

        public void AddOutputRef(uint reference)
        {
            Outputs.Add(reference);
        }

        public void Mutate()
        {
            return;
        }

        public uint Id
        {
            get { return Innovation; }
        }
    }
}