using System.Collections.Generic;

public class Node : IMutatable
{
    public enum NodeTypes {Input, Hidden, Output, Bias};

    public readonly NodeTypes type;

    public float Value { get; protected set; }

    public float PreviousValue { get; protected set; }

    public uint Innovation { get; protected set; }

    public Node(NodeTypes type)
    {
        Value         = 0;
        PreviousValue = Value;
        Innovation    = 0;

        this.type = type;
    }

    public Node(Node n)
    {
        Value         = n.Value;
        PreviousValue = 0;
        Innovation    = n.Innovation;

        type = n.type;
    }

    public void SetValue(float v)
    {
        PreviousValue = Value;
        Value         = v;
    }

    public void Mutate()
    {
        Innovation++;
    }
}
