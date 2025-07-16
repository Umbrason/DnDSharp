using System.Reflection;

namespace DnDSharp.Core
{
    public interface IClassLevel
    {
        ClassID ClassID => this.GetType().DeclaringType!.GetFieldOrPropertyValueOrDefault<ClassID>(nameof(ClassID), BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)!;
        int LevelID => this.GetType().GetCustomAttribute<ClassLevelAttribute>()!.LevelID;
        void OnAdded(Character character);
        void OnRemoved(Character character);
    }
}