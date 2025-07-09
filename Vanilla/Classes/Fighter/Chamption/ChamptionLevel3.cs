using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Champion
    {
        public class Level3 : Fighter.Level3
        {
            public void OnAdded(Character character)
            {
                character.Classes[ClassID] = new Champion();
            }
            public void OnRemoved(Character character)
            {
                character.Classes[ClassID] = new Fighter();
            }
        }
    }
}