namespace NeatFish.Entities.NEAT
{
    public interface IMutatable
    {
        uint Id { get; }

        void Mutate();
    }
}