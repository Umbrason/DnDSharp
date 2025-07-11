namespace DnDSharp.Core
{
    public class ClassID(string id)
    {
        public string ID { get; } = id;
        public override string ToString() => ID;
    }
}