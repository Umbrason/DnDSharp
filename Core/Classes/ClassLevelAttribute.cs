using DnDSharp.Core;

namespace DnDSharp
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ClassLevelAttribute(int level) : Attribute
    {
        public int LevelID { get; } = level;
    }
}