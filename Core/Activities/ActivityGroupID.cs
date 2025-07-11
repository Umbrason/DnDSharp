namespace DnDSharp.Core
{
    public class ActivityGroupID(string identifier, ActivityGroupID? parentGroup = null)
    {
        public ActivityGroupID? ParentGroup { get; private set; } = parentGroup;
        public string ID { get; private set; } = identifier;
        public override string ToString() => ParentGroup == null ? ID : $"{ParentGroup}.{ID}";
    }
}