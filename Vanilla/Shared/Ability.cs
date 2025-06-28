using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public static class Ability
    {
        public static readonly AbilityID STRENGTH = new(nameof(STRENGTH));
        public static readonly AbilityID DEXTERITY = new(nameof(DEXTERITY));
        public static readonly AbilityID CONSTITUTION = new(nameof(CONSTITUTION));
        public static readonly AbilityID INTELLIGENCE = new(nameof(INTELLIGENCE));
        public static readonly AbilityID WISDOM = new(nameof(WISDOM));
        public static readonly AbilityID CHARISMA = new(nameof(CHARISMA));
    }
}