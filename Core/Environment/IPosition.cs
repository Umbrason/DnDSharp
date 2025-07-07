namespace DnDSharp.Core
{
    public interface IPosition
    {
        public TraversalCost[] GetTraversalOptions(IPosition other);
        public readonly struct TraversalCost(ResourceID resource, int amount)
        {
            public readonly ResourceID resource = resource;
            public readonly int amount = amount;
        }
    }
}