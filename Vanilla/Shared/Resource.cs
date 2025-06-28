using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public class Resource
    {
        public static readonly ResourceID ACTION = new(nameof(ACTION));
        public static readonly ResourceID BONUS_ACTION = new(nameof(BONUS_ACTION));
        public static readonly ResourceID REACTION = new(nameof(REACTION));
        public static readonly ResourceID MOVEMENT = new(nameof(MOVEMENT));
        public static ResourceID SPELL_SLOT(int level) => new($"{nameof(SPELL_SLOT)}_LEVEL_{level}");
    }
}