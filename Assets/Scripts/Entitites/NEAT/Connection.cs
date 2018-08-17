
public class Connection : IMutatable
{
    public Node Input { get; protected set; }

    public Node Output { get; protected set; }

    public float Weight { get; protected set; }

    public float Bias { get; protected set; }

    public bool Enabled { get; set; }

    public uint Innovation { get; protected set; }

    public Connection(Node input, Node output, float weight, float bias)
    {
        if (input.type == Node.NodeTypes.Output || output.type == Node.NodeTypes.Input || output.type == Node.NodeTypes.Bias) {
            throw new System.InvalidOperationException("Input/output node type invalid");
        }

        Input  = input;
        Output = output;
        Weight = weight;
        Bias   = bias;

        Innovation = 0;
    }

    public void Mutate()
    {
        Innovation++;
    }
}
