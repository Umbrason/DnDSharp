using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        [ClassLevel(4)]
        public abstract class Level4 : IClassLevel
        {
            public virtual void OnAdded(Character character)
            {

            }
            public virtual void OnRemoved(Character character)
            {

            }
        }
    }
}