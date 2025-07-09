using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        public class Level3 : IClassLevel
        {
            public ClassID ClassID => Class.Fighter;
            public int LevelID => 3;
            public void OnAdded(Character character) { }
            public void OnRemoved(Character character) { }
        }
    }
}