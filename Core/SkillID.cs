namespace DnDSharp.Core
{
    public class SkillID(string identifier, AbilityID baseAbility)
    {
        public string ID { get; private set; } = identifier;
        public AbilityID baseAbility { get; private set; } = baseAbility;
    }
}