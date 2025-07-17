using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion
    {
        [ClassLevel(11)]
        public new class Level11 : Fighter.Level11
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