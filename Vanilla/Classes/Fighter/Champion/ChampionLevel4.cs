using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion
    {
        [ClassLevel(4)]
        public new class Level4 : Fighter.Level4
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