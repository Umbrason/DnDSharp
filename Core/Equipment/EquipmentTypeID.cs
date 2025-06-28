namespace DnDSharp.Core
{
    public class EquipmentTypeID(string identifier)
    {
        public string ID { get; private set; } = identifier;
    }
}