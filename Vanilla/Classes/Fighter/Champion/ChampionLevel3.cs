using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion
    {
        public new class Level3 : Fighter.Level3
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