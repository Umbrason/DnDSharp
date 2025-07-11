using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion
    {
        public new class Level3([LevelChoices<Level3.FightingStyle>(1, [Level3.FightingStyle.Duelling, Level3.FightingStyle.Defensive, Level3.FightingStyle.Ranged, Level3.FightingStyle.Twohanded])] Level3.FightingStyle FightingStyle) : Fighter.Level3
        {
            public static int Test => 0;
            public static int Test2 { get; } = 0;
            private readonly FightingStyle chosenFightingStyle = FightingStyle;
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