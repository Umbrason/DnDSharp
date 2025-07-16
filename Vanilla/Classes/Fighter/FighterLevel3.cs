using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        [ClassLevel(3)]
        public abstract class Level3 : IClassLevel
        {
            public virtual void OnAdded(Character character) { }
            public virtual void OnRemoved(Character character) { }
        }
    }
}