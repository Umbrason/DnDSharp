namespace DnDSharp.Core
{
    public struct ActivityParameterDescriptor(Type type, int repeat, Func<object, bool> filter)
    {
        public enum ParameterType
        {
            Character,
            Position,
            Custom
        }
        public Type type = type;
        public int repeat = repeat;
        public Func<object, bool> filter = filter;
    }
}