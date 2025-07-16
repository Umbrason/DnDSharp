using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion
    {
        [ClassLevel(3)]
        public new class Level3 : Fighter.Level3
        {
            public static FightingStyle[] FightingStyleOptions => Enum.GetValues<FightingStyle>();
            private readonly FightingStyle chosenFightingStyle;

            public Level3([Choices(2, nameof(FightingStyleOptions))] FightingStyle[] FightingStyle)
            {
                chosenFightingStyle = FightingStyle[0];
            }

            public enum FightingStyle
            {
                Duelling,
                Defensive,
                Ranged,
                Twohanded
            }

            public override void OnAdded(Character character)
            {
                base.OnAdded(character);
                switch (chosenFightingStyle)
                {
                    case FightingStyle.Duelling:
                        break;
                    case FightingStyle.Defensive:
                        character.ArmorClassBonus.RegisterAdd(1);
                        break;
                };
            }
            public override void OnRemoved(Character character)
            {
                base.OnRemoved(character);
            }
        }
    }
}