using System.Reflection;

namespace DnDSharp.Core
{
    public interface IClassLevel
    {
        ClassID ClassID => this.GetType().GetBaseTypeImplementingInterface<IClassLevel>()!.DeclaringType!.GetFieldOrPropertyValueOrDefault<ClassID>(nameof(ClassID), BindingFlags.Static | BindingFlags.Public)!;
        int LevelID => this.GetType().GetBaseTypeImplementingInterface<IClassLevel>()!.GetCustomAttribute<ClassLevelAttribute>()!.LevelID;
        void OnAdded(Character character);
        void OnRemoved(Character character);
    }
}