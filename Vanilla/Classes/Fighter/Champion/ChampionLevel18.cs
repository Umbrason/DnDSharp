using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion
    {
        [ClassLevel(18)]
        public new class Level18 : Fighter.Level18
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