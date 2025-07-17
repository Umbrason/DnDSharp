using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        [ClassLevel(6)]
        public abstract class Level6 : IClassLevel
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