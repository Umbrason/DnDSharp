namespace DnDSharp.Core
{
    public class DamageElementID(string identifier)
    {
        public string ID { get; private set; } = identifier;
    }
}