namespace DnDSharp.Core
{
    public class ResourceID(string identifier)
    {
        public string ID { get; private set; } = identifier;
        public override string ToString() => ID;
    }
}