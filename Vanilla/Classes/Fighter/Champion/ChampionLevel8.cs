using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion
    {
        [ClassLevel(8)]
        public new class Level8 : Fighter.Level8
        {
            public override void OnAdded(Character character)
            {
                base.OnAdded(character);
            }
            public override void OnRemoved(Character character)
            {
                base.OnRemoved(character);
            }
        }
    }
}