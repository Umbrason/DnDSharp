namespace DnDSharp.Core
{
    public class DamageElementID(string identifier)
    {
        public string ID { get; } = identifier;
        public override string ToString() => ID;
    }
}