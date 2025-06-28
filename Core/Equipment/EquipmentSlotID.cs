namespace DnDSharp.Core
{
    public class EquipmentSlotID(string identifier)
    {
        public string ID { get; private set; } = identifier;
    }
}