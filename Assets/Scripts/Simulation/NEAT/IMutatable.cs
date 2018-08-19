namespace NeatFish.Simulation.NEAT
{
    public interface IMutatable
    {
        uint Id { get; }

        void Mutate();
    }
}