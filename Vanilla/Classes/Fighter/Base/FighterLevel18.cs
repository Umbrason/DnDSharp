using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        [ClassLevel(18)]
        public abstract class Level18 : IClassLevel
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