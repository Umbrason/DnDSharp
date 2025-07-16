using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        [ClassLevel(1)]
        public class Level1 : IClassLevel
        {
            public ActivityGroupID ActivityGroup => new(nameof(Level1), Fighter.ActivityGroup);
            public IActivity[] Activities = [];
            public void OnAdded(Character character)
            {
                foreach (var activity in Activities)
                    character.Activities[ActivityGroup].Add(activity);
            }
            public void OnRemoved(Character character)
            {
                foreach (var activity in Activities)
                    character.Activities[ActivityGroup].Remove(activity);
            }
        }
    }
}