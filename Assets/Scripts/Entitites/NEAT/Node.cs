namespace NeatFish.Entities.NEAT
{
    public class Node : IMutatable
    {
        public enum NodeTypes { Input, Hidden, Output, Bias };

        public readonly NodeTypes type;

        public float Value { get; protected set; }

        public float PreviousValue { get; protected set; }

        public uint Innovation { get; protected set; }

        public UnityEngine.Vector2 Position { get; set; }

        public Node(uint id, NodeTypes type)
        {
            Innovation = id;
            Value = 0;
            PreviousValue = Value;
            Innovation = 0;

            this.type = type;
        }

        public Node(Node n)
        {
            Value = 0;
            PreviousValue = 0;
            Innovation = n.Innovation;
            Position = n.Position;

            type = n.type;
        }

        public void SetValue(float v)
        {
            PreviousValue = Value;
            Value = v;
        }

        public void Mutate()
        {
            Innovation++;
        }

        public uint Id
        {
            get { return Innovation; }
        }
    }
}