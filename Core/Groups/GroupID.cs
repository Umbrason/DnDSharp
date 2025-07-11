namespace DnDSharp.Core
{
    public class GroupID(string id, List<GroupID>? allied = null, List<GroupID>? opposing = null, GroupID? parentGroup = null)
    {
        public string ID { get; } = id;
        public GroupID? ParentGroup { get; } = parentGroup;
        public List<GroupID> Allied = allied ?? [];
        public List<GroupID> Opposing = opposing ?? [];

        public enum GroupRelationship
        {
            Allied = 1,
            Neutral = 0,
            Opposing = -1,
        }

        public GroupRelationship RelationshipWith(GroupID other)
        {
            var otherGroupIterator = other;
            while (otherGroupIterator != null)
            {
                if (Allied.Contains(otherGroupIterator)) return GroupRelationship.Allied;
                if (Opposing.Contains(otherGroupIterator)) return GroupRelationship.Opposing;
                otherGroupIterator = otherGroupIterator.ParentGroup;
            }
            if (ParentGroup != null) return ParentGroup.RelationshipWith(other);
            return GroupRelationship.Neutral;
        }
        public override string ToString() => ID;
    }
}