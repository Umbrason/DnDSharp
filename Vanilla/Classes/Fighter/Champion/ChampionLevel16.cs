using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion
    {
        [ClassLevel(16)]
        public new class Level16 : Fighter.Level16
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