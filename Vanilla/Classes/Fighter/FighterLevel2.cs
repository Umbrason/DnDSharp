using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        [ClassLevel(2)]
        public class Level2 : IClassLevel
        {
            public void OnAdded(Character character) { }
            public void OnRemoved(Character character) { }
        }
    }
}