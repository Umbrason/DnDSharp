using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        public abstract class Level3 : IClassLevel
        {
            public static ClassID ClassID => Class.Fighter;
            public static int LevelID => 3;
            ClassID IClassLevel.ClassID => ClassID;
            int IClassLevel.LevelID => LevelID;
            public virtual void OnAdded(Character character) { }
            public virtual void OnRemoved(Character character) { }
        }
    }
}