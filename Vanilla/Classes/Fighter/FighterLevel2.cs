using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        public class Level2 : IClassLevel
        {
            public static ClassID ClassID => Class.Fighter;
            public static int LevelID => 2;
            ClassID IClassLevel.ClassID => ClassID;
            int IClassLevel.LevelID => LevelID;
            public void OnAdded(Character character) { }
            public void OnRemoved(Character character) { }
        }
    }
}