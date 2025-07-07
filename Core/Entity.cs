namespace DnDSharp.Core
{
    public class Entity
    {
        public CappedValue<int> HitPoints { get; protected set; }
        public ModifyableValue<int> ArmorClassBase { get; protected set; } //usually 10, tho some classes/creatures might modify this
        public ModifyableValue<int> ArmorClass { get; protected set; }
        public Dictionary<DamageElementID, ModifyableValue<int>> DamageMultipliers;
        public IPosition? Position { get; set; }

        public Entity(int baseHitPoints, int baseArmorClass, IReadOnlyDictionary<DamageElementID, int> damageMultipliers, IPosition? position = null)
        {
            HitPoints = new(baseHitPoints, baseHitPoints);
            ArmorClassBase = new(baseArmorClass);
            ArmorClass = ArmorClassBase;
            Position = position;

            DamageMultipliers = [];
            foreach (var damageMult in damageMultipliers)
            {
                DamageMultipliers[damageMult.Key] = new(damageMult.Value);
            }
        }
    }
}