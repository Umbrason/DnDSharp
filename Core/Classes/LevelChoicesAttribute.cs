namespace DnDSharp.Core
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class LevelChoicesAttribute<T> : Attribute
    {
        public int minPicks;
        public int maxPicks;
        public T[] options;
        public Type optionType;

        public LevelChoicesAttribute(T[] options) : this(1, options) { }
        public LevelChoicesAttribute(int exactPicks, T[] options) : this(exactPicks, exactPicks, options) { }
        public LevelChoicesAttribute(int minPicks, int maxPicks, T[] options) : this(minPicks, maxPicks, options, typeof(T)) { }
        private LevelChoicesAttribute(int minPicks, int maxPicks, T[] options, Type optionType)
        {
            this.minPicks = minPicks;
            this.maxPicks = maxPicks;
            this.options = options;
            this.optionType = optionType;
        }

        public static implicit operator LevelChoicesAttribute<object?>(LevelChoicesAttribute<T> attribute)
            => new(attribute.minPicks, attribute.maxPicks, Array.ConvertAll(attribute.options, i => (object?)i), optionType: attribute.optionType);
    }
}