namespace DnDSharp.Core
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ChoiceCountAttribute<T> : Attribute
    {
        public int minPicks;
        public int maxPicks;
        /* public Func<T[]> options; */
        public Type optionType;

        public ChoiceCountAttribute(Func<T[]> options) : this(1, options) { }
        public ChoiceCountAttribute(int exactPicks, Func<T[]> options) : this(exactPicks, exactPicks, options) { }
        public ChoiceCountAttribute(int minPicks, int maxPicks, Func<T[]> options) : this(minPicks, maxPicks, options, typeof(T)) { }
        private ChoiceCountAttribute(int minPicks, int maxPicks, Func<T[]> options, Type optionType)
        {
            this.minPicks = minPicks;
            this.maxPicks = maxPicks;
            this.optionType = optionType;
        }

        public static implicit operator ChoiceCountAttribute<object?>(ChoiceCountAttribute<T> attribute)
            => new(attribute.minPicks, attribute.maxPicks, () => Array.ConvertAll(attribute.options?.Invoke() ?? [], i => (object?)i), optionType: attribute.optionType);
    }
}