namespace DnDSharp.Core
{
    public interface IClassLevel
    {
        ClassID ClassID => (ClassID)this.GetType().GetProperty(nameof(ClassID), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)?.GetMethod?.Invoke(null, [])!;
        int LevelID => (int)this.GetType().GetProperty(nameof(LevelID), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)?.GetMethod?.Invoke(null, [])!;
        void OnAdded(Character character);
        void OnRemoved(Character character);
    }
}