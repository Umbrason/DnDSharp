namespace DnDSharp.Core
{
    public interface IClassLevel
    {
        ClassID ClassID { get; }
        int LevelID { get; }
        void OnAdded(Character character);
        void OnRemoved(Character character);
    }
}