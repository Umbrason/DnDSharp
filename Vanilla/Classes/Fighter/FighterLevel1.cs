using DnDSharp.Core;

namespace DnDSharp.Vanilla
{
    public partial class Fighter
    {
        public class Level1 : IClassLevel
        {
            public ClassID ClassID => Class.Fighter;
            public int LevelID => 1;
            public ActivityGroupID ActivityGroup => new(nameof(Level1), Fighter.ActivityGroup);
            public IActivity[] Activities = [];
            public void OnAdded(Character character)
            {
                foreach (var activity in Activities)
                    character.Activities.Add(ActivityGroup, activity);
            }
            public void OnRemoved(Character character)
            {
                foreach (var activity in Activities)
                    character.Activities.Remove(ActivityGroup, activity);
            }
        }
    }
}