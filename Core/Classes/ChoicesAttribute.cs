namespace DnDSharp.Core
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ChoicesAttribute : Attribute
    {
        public readonly int minPicks;
        public readonly int maxPicks;
        public readonly string optionsFieldName;

        public ChoicesAttribute(int exactPicks, string optionsFieldName) : this(exactPicks, exactPicks, optionsFieldName) { }
        public ChoicesAttribute(int minPicks, int maxPicks, string optionsFieldName)
        {
            this.minPicks = minPicks;
            this.maxPicks = maxPicks;
            this.optionsFieldName = optionsFieldName;
        }
    }
}