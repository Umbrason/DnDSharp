using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        [ClassLevel(7)]
        public abstract class Level7 : IClassLevel
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