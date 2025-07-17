using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        [ClassLevel(1)]
        public class Level1 : IClassLevel
        {
            public void OnAdded(Character character)
            {

            }
            public void OnRemoved(Character character)
            {

            }
        }
    }
}