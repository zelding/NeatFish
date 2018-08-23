namespace NeatFish.Simulation.Utilities
{
    public class NodeIDGenerator
    {
        private uint _next = 1;

        public NodeIDGenerator()
        {
            _next = 1;
        }

        public NodeIDGenerator(uint start)
        {
            _next = start;
        }

        public uint Next
        {
            get {
                if ( _next == uint.MaxValue ) {
                    Reset();
                }

                return _next++;
            }
        }

        public void Reset()
        {
            _next = 1;
        }

        public void Reset(uint start)
        {
            _next = start;
        }
    }
}