namespace DnDSharp.Core
{
    public class AbilityID(string id)
    {
        public string ID { get; } = id;
        public override string ToString() => ID;
    }
}