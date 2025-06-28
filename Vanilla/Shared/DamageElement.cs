using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public static class DamageElement
    {
        public readonly static DamageElementID SLASHING = new(nameof(SLASHING));
        public readonly static DamageElementID PIERCING = new(nameof(PIERCING));
        public readonly static DamageElementID BLUDGEONING = new(nameof(BLUDGEONING));
        public readonly static IReadOnlyList<DamageElementID> PHYSICAL = [SLASHING, PIERCING, BLUDGEONING];

        public readonly static DamageElementID ACID = new(nameof(ACID));
        public readonly static DamageElementID COLD = new(nameof(COLD));
        public readonly static DamageElementID FIRE = new(nameof(FIRE));
        public readonly static DamageElementID LIGHTNING = new(nameof(LIGHTNING));
        public readonly static DamageElementID THUNDER = new(nameof(THUNDER));
        public readonly static DamageElementID POISON = new(nameof(POISON));
        public readonly static IReadOnlyList<DamageElementID> ELEMENTAL = [ACID, COLD, FIRE, LIGHTNING, THUNDER, POISON];

        public readonly static DamageElementID FORCE = new(nameof(FORCE));
        public readonly static DamageElementID PSYCHIC = new(nameof(PSYCHIC));
        public readonly static DamageElementID NECROTIC = new(nameof(NECROTIC));
        public readonly static DamageElementID RADIANT = new(nameof(RADIANT));
        public readonly static IReadOnlyList<DamageElementID> ALL = [.. PHYSICAL, .. ELEMENTAL, FORCE, PSYCHIC, NECROTIC, RADIANT];
    }
}