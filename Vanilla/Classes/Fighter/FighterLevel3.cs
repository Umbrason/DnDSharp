using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        public abstract class Level3 : IClassLevel
        {
            public ClassID ClassID => Class.Fighter;
            public int LevelID => 3;
            public virtual void OnAdded(Character character) { }
            public virtual void OnRemoved(Character character) { }
        }
    }
}