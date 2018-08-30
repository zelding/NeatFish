using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NeatFish.Simulation.NEAT
{
    public class Node
    {
        public enum NodeTypes { Input, Hidden, Output, Bias };

        public readonly NodeTypes type;

        public UnityEngine.Vector2 Position;

        protected double Value;

        protected double PreviousValue;

        protected uint Identifier;

        protected HashSet<Connection> Outputs;

        public Node(uint id, NodeTypes type)
        {
            Outputs = new HashSet<Connection>();
            Identifier = id;

            this.type = type;
        }

        public Node(Node n)
        {
            Outputs  = n.Outputs;
            Identifier = n.Identifier;
            Position = n.Position;

            type = n.type;
        }

        public void RegisterConnection(Connection c)
        {
            if ( !Outputs.Contains(c) ) {
                Outputs.Add(c);
            }
        }

        public void AddValue(double value)
        {
            Value += value;
        }

        public void Fire()
        {
            // first layer is inputs; they dont get modified
            if (Position.x != 0) {
                Value = Sigmoid(Value);
            }

            foreach (Connection c in Outputs) {
                if (c.Enabled) {
                    c.Output.AddValue(Value * c.Weight + c.Bias);
                }
            }
        }

        public double GetValue()
        {
            return Value;
        }

        public uint Id
        {
            get { return Identifier; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Sigmoid(double x)
        {
            return (2f / (1f + System.Math.Exp(-4.9f * x))) - 1f;
        }

        public bool IsConnectedWith(Node n)
        {
            if ( n.type == NodeTypes.Input ) {
                return false;
            }

            if ( Position.x == n.Position.x ) {
                return false;
            }

            if (Position.x < n.Position.x) {
                foreach (Connection c in n.Outputs) {
                    if (c.Output == this) {
                        return true;
                    }
                }
            }
            else {
                foreach (Connection c in Outputs) {
                    if (c.Output == n) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}