using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        [ClassLevel(13)]
        public abstract class Level13 : IClassLevel
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